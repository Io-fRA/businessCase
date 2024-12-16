using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace businessCase;

class FindOccurences
{
    
    static string githubApiToken = "GITHUB_API_TOKEN";
    static string baseRepoUrl = "https://api.github.com/repos/lodash/lodash/";
    private static string gettingTreeShaPath = "git/refs/heads/main";
    static List<string> pathOfJsAndTsFiles = new List<string>();
    private static HttpClient client;
    private static Dictionary<char, int> letterOccurrences = new Dictionary<char, int>();
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("Begin Find Occurences");
        CreateHttpClient();
        Console.WriteLine("HttpClient created");
        
        try
        {
            // Using tree endpoint uses less API calls
            await RetrievingAllFilesUsingTree();
            //await RetrievingAllFilesRecursively(baseRepoUrl + "contents/");
            Console.WriteLine($"There are {pathOfJsAndTsFiles.Count} .js/.ts files");
            
            // Parsing each js/ts file and counting occurrences of each letter
            await ParsingJsAndTsFilesToCountLetter();
            
            // Sorting letter count
            var sortedDictionary = letterOccurrences
                .OrderByDescending(pair => pair.Value)
                .ToList();
            
            foreach (var pair in sortedDictionary)
            {
                Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
            }

        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
        
    }

    private static async Task ParsingJsAndTsFilesToCountLetter()
    {
        foreach (var jsOrTsPath in pathOfJsAndTsFiles)
        {
            //Console.WriteLine($"{jsOrTsPath}");
            GitHubContent fileInfo = await GetPathContent(jsOrTsPath, new GitHubContent());
            var decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(fileInfo.content));
            foreach (char c in decodedContent)
            {
                if (char.IsLetter(c))
                {
                    if (letterOccurrences.ContainsKey(char.ToLower(c)))
                    {
                        letterOccurrences[char.ToLower(c)]++;
                    }
                    else
                    {
                        letterOccurrences[c] = 1;
                    }
                }
            }
        }
    }

    private static async Task RetrievingAllFilesUsingTree()
    {
        // retrieving the sha of the github repository tree to minimize calls to the API
        GitHubRef gitHubRef = await GetPathContent(baseRepoUrl + gettingTreeShaPath, new GitHubRef());
        string treeSha = gitHubRef.GitHubObject.Sha;
            
        GitHubTree tree = await GetPathContent(baseRepoUrl + "git/trees/" + treeSha + "?recursive=1", new GitHubTree());
        foreach (GitHubTreeObject treeObject in tree.tree)
        {
            if (treeObject.type == "blob" &&
                (treeObject.path.EndsWith(".js") || treeObject.path.EndsWith(".ts")))
            {
                pathOfJsAndTsFiles.Add(baseRepoUrl + "contents/" + treeObject.path);
            }
        }
    }
    
    private static void CreateHttpClient()
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));    
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", githubApiToken);
        client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

        // GitHub requires a User-Agent header in API requests
        client.DefaultRequestHeaders.UserAgent.ParseAdd("FindOccurences/1.0.0");
    } 

    private static async Task<T> GetPathContent<T>(string path, T returnedType)
    {
        // Make the GET request
        //Console.WriteLine("Getting path content for path: " + path);
        HttpResponseMessage response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        // Mapping of the response (type, path, content)
        return JsonSerializer.Deserialize<T>(json);
    }
    
    private static void SavePathForJsAndTsFile(GitHubContent content)
    {
        if (content.name.EndsWith(".js") || content.name.EndsWith(".ts"))
        {
            pathOfJsAndTsFiles.Add(content.url);
        }
    }

    private static async Task RetrievingAllFilesRecursively(string url)
    {
        List<GitHubContent> gitHubContents = await GetPathContent(url, new List<GitHubContent>() );
            
        foreach (var content in gitHubContents)
        {
            if (content.type == "dir")
            {
                // It is a directory, recursive loop to check for js/ts files
                await RetrievingAllFilesRecursively(content.url);
            }
            else if (content.type == "file")
            {
                SavePathForJsAndTsFile(content);
            }
        }
    }

}
