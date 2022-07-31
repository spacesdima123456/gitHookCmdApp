using System.Text.Json.Serialization;

namespace GitHookCmdApp.Models
{
    public class Issues
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
}
