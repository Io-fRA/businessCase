using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace businessCase;

class FindOccurences
{
    
    static string githubApiToken = "API_KEY_GITHUB";
    static string baseRepoUrl = "https://api.github.com/repos/lodash/lodash/contents";
    static List<string> pathOfJsAndTsFiles = new List<string>();
    private static HttpClient client;
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("Begin Find Occurences");
        CreateHttpClient();
        Console.WriteLine("HttpClient created");
        
        try
        {
            await AssessContentUnderUrl(baseRepoUrl);
            Console.WriteLine($"There are {pathOfJsAndTsFiles.Count} .js/.ts files");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
        
    }

    private static void PrintObject(object obj)
    {
        foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(obj);
            Console.WriteLine("{0}={1}", name, value);
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

    private static async Task<List<GitHubContent>> GetPathContent(string path)
    {
        // Make the GET request
        Console.WriteLine("Getting path content for path: " + path);
        HttpResponseMessage response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        // Mapping of the response (type, path, content)
        return JsonSerializer.Deserialize<List<GitHubContent>>(json);
    }

    private static void SavePathForJsAndTsFile(GitHubContent content)
    {
        if (content.name.EndsWith(".js") || content.name.EndsWith(".ts"))
        {
            pathOfJsAndTsFiles.Add(content.url);
        }
    }

    private static async Task AssessContentUnderUrl(string url)
    {
        List<GitHubContent> gitHubContents = await GetPathContent(url);
            
        foreach (var content in gitHubContents)
        {
            if (content.type == "dir")
            {
                // It is a directory, recursive loop to check for js/ts files
                await AssessContentUnderUrl(content.url);
            }
            else if (content.type == "file")
            {
                SavePathForJsAndTsFile(content);
            }
        }
    }

}
