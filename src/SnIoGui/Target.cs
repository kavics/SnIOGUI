using System.Diagnostics;

namespace SnIoGui
{
    [DebuggerDisplay("{Name}")]
    public class Target
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? ApiKey { get; set; }
        public string? HealthApiKey { get; set; }
        public string? ImportPath { get; set; }
        public string? DbServer { get; set; }
        public string? DbName { get; set; }
        public string? AdminUrl { get; set; }
    }
}
