using System.Text.Json.Serialization;

namespace businessCase;

public class GitHubContent
{
    public string type  { get; set; }
    public string path  { get; set; }
    public string name  { get; set; }
    public string url  { get; set; }
    public string content  { get; set; }
}

public class GitHubRef
{
    [JsonPropertyName("object")]
    public GitHubObject GitHubObject { get; set; }
}

public class GitHubObject
{
    [JsonPropertyName("sha")]
    public string Sha { get; set; }
}

public class GitHubTree
{
    public List<GitHubTreeObject> tree  { get; set; }
}

public class GitHubTreeObject
{
    public string path  { get; set; }
    public string type  { get; set; }
}