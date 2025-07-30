using Microsoft.Data.SqlClient;
using System.Net.Http;
using System.Text.Json;

namespace SnIoGui.Services
{
    public class HealthService : IHealthService
    {
        private readonly HttpClient _httpClient;

        public HealthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(Target target)
        {
            var result = new HealthCheckResult
            {
                TargetName = target?.Name ?? "Unknown",
                CheckedAt = DateTime.Now
            };

            if (target == null)
            {
                result.ErrorMessage = "Target is null";
                return result;
            }

            var details = new List<string>();

            // Check URL accessibility
            try
            {
                result.UrlAccessible = await CheckUrlAccessibilityAsync(target);
                details.Add($"URL ({target.Url}): {(result.UrlAccessible ? "✅ Accessible" : "❌ Not accessible")}");
            }
            catch (Exception ex)
            {
                result.UrlAccessible = false;
                details.Add($"URL ({target.Url}): ❌ Error - {ex.Message}");
            }

            // Check database connectivity
            try
            {
                result.DatabaseAccessible = await CheckDatabaseConnectivityAsync(target);
                details.Add($"Database ({target.DbServer}/{target.DbName}): {(result.DatabaseAccessible ? "✅ Connected" : "❌ Not connected")}");
            }
            catch (Exception ex)
            {
                result.DatabaseAccessible = false;
                details.Add($"Database ({target.DbServer}/{target.DbName}): ❌ Error - {ex.Message}");
            }

            // Check import path accessibility
            try
            {
                result.ImportPathAccessible = CheckImportPathAccessibility(target);
                details.Add($"Import Path ({target.ImportPath}): {(result.ImportPathAccessible ? "✅ Accessible" : "❌ Not accessible")}");
            }
            catch (Exception ex)
            {
                result.ImportPathAccessible = false;
                details.Add($"Import Path ({target.ImportPath}): ❌ Error - {ex.Message}");
            }

            result.Details = details;
            result.IsHealthy = result.UrlAccessible && result.DatabaseAccessible && result.ImportPathAccessible;

            return result;
        }

        public async Task<RepositoryHealthResult?> QueryRepositoryHealthAsync(Target target)
        {
            if (target == null || string.IsNullOrWhiteSpace(target.Url) || string.IsNullOrWhiteSpace(target.HealthApiKey))
                return null;

            try
            {
                var healthUrl = $"{target.Url.TrimEnd('/')}/health";
                var request = new HttpRequestMessage(HttpMethod.Get, healthUrl);
                
                // Add ApiKey header
                request.Headers.Add("apiKey", target.HealthApiKey);
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var response = await _httpClient.SendAsync(request, cts.Token);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return JsonSerializer.Deserialize<RepositoryHealthResult>(json, options);
                }
            }
            catch (Exception)
            {
                // Swallow exceptions - we'll return null
            }

            return null;
        }

        public async Task<bool> CheckUrlAccessibilityAsync(Target target)
        {
            if (string.IsNullOrWhiteSpace(target?.Url))
                return false;

            try
            {
                // Set a reasonable timeout
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                
                // Try to make a GET request first. Sometimes HEAD is not supported.
                var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, target.Url), cts.Token);
                
                // Consider 2xx and 3xx status codes as successful
                return response.IsSuccessStatusCode || 
                       (int)response.StatusCode >= 300 && (int)response.StatusCode < 400;
            }
            catch (HttpRequestException)
            {
                // Try with GET request as fallback
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                    var response = await _httpClient.GetAsync(target.Url, cts.Token);
                    return response.IsSuccessStatusCode || 
                           (int)response.StatusCode >= 300 && (int)response.StatusCode < 400;
                }
                catch
                {
                    return false;
                }
            }
            catch (TaskCanceledException)
            {
                // Timeout
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckDatabaseConnectivityAsync(Target target)
        {
            if (string.IsNullOrWhiteSpace(target?.DbServer) || string.IsNullOrWhiteSpace(target?.DbName))
                return false;

            string connStrTemplate = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True;Connection Timeout=10;";
            string connectionString = string.Format(connStrTemplate, target.DbServer, target.DbName);

            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return connection.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckImportPathAccessibility(Target target)
        {
            if (string.IsNullOrWhiteSpace(target?.ImportPath))
                return false;

            try
            {
                return Directory.Exists(target.ImportPath) && 
                       HasReadAccess(target.ImportPath);
            }
            catch
            {
                return false;
            }
        }

        private static bool HasReadAccess(string path)
        {
            try
            {
                // Try to enumerate the directory to check read access
                Directory.EnumerateFileSystemEntries(path).Take(1).ToList();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}