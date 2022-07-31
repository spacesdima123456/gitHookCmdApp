using System.Text.Json.Serialization;

namespace GitHookCmdApp.Models
{
    public class Tags
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("$type")]
        public string Type { get; set; }
    }
}
