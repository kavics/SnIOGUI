namespace SnIoGui.Services
{
    public interface IHealthService
    {
        /// <summary>
        /// Performs a health check on the specified target
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>Health check result information</returns>
        Task<HealthCheckResult> CheckHealthAsync(Target target);

        /// <summary>
        /// Queries the repository health endpoint and returns detailed health information
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>Repository health data</returns>
        Task<RepositoryHealthResult?> QueryRepositoryHealthAsync(Target target);

        /// <summary>
        /// Checks if the target URL is accessible
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>URL accessibility result</returns>
        Task<bool> CheckUrlAccessibilityAsync(Target target);

        /// <summary>
        /// Checks database connectivity
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>Database connectivity result</returns>
        Task<bool> CheckDatabaseConnectivityAsync(Target target);

        /// <summary>
        /// Checks if the import path exists and is accessible
        /// </summary>
        /// <param name="target">The target to check</param>
        /// <returns>Import path accessibility result</returns>
        bool CheckImportPathAccessibility(Target target);
    }

    public class HealthCheckResult
    {
        public bool IsHealthy { get; set; }
        public string TargetName { get; set; } = string.Empty;
        public bool UrlAccessible { get; set; }
        public bool DatabaseAccessible { get; set; }
        public bool ImportPathAccessible { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string> Details { get; set; } = new List<string>();
        public DateTime CheckedAt { get; set; } = DateTime.Now;
        public RepositoryHealthResult? RepositoryHealth { get; set; }
    }

    public class RepositoryHealthResult
    {
        public RepositoryStatus? Repository_Status { get; set; }
        public HealthInfo? Health { get; set; }
        public HealthDetails? Details { get; set; }
    }

    public class RepositoryStatus
    {
        public bool Running { get; set; }
        public string? Status { get; set; }
    }

    public class HealthInfo
    {
        public string? Color { get; set; }
        public ComponentHealth? Database { get; set; }
        public ComponentHealth? BlobStorage { get; set; }
        public ComponentHealth? Search { get; set; }
        public ComponentHealth? Identity { get; set; }
    }

    public class ComponentHealth
    {
        public string? Color { get; set; }
        public string? ResponseTime { get; set; }
        public string? Method { get; set; }
    }

    public class HealthDetails
    {
        public string? HealthServiceStatus { get; set; }
        public List<string>? Repository_StatusHistory { get; set; }
    }
}