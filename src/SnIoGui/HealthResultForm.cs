using SnIoGui.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SnIoGui
{
    public partial class HealthResultForm : Form
    {
        private readonly Target _target;
        private readonly IHealthService _healthService;
        private readonly List<HealthCheckItem> _healthItems = new();

        private class HealthCheckItem
        {
            public string Name { get; set; } = string.Empty;
            public string Status { get; set; } = "Checking...";
            public Color LampColor { get; set; } = Color.Gray;
            public bool IsCompleted { get; set; } = false;
        }

        public HealthResultForm(Target target)
        {
            _target = target;
            _healthService = Program.ServiceProvider.GetRequiredService<IHealthService>();
            
            InitializeComponent();
            
            // Set form title and initial status
            this.Text = $"Health Check - {_target.Name}";
            lblStatus.Text = "🔄 WORKING...";
            lblStatus.ForeColor = Color.Blue;
            
            // Set target name and check time
            lblTargetName.Text = _target.Name ?? "Unknown";
            lblCheckTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // Setup grid and show empty items first
            SetupDataGrid();
            InitializeHealthItems();
            PopulateEmptyGrid();
            
            // Set ESC key to close the form
            this.KeyPreview = true;
            this.KeyDown += HealthResultForm_KeyDown;
            
            // Start sequential health checks
            _ = PerformHealthChecksAsync();
        }

        private void HealthResultForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void InitializeHealthItems()
        {
            _healthItems.Clear();
            _healthItems.Add(new HealthCheckItem { Name = $"URL ({_target.Url ?? "Not configured"})" });
            _healthItems.Add(new HealthCheckItem { Name = $"Database ({_target.DbServer}/{_target.DbName})" });
            _healthItems.Add(new HealthCheckItem { Name = $"Import Path ({_target.ImportPath ?? "Not configured"})" });
        }

        private void PopulateEmptyGrid()
        {
            dgvDetails.Rows.Clear();
            
            foreach (var item in _healthItems)
            {
                var row = dgvDetails.Rows[dgvDetails.Rows.Add()];
                row.Cells["LampColumn"].Value = "●";
                row.Cells["LampColumn"].Style.ForeColor = item.LampColor;
                row.Cells["StatusColumn"].Value = item.Status;
                row.Cells["DescriptionColumn"].Value = item.Name;
            }
            
            // Clear any selection
            dgvDetails.ClearSelection();
            dgvDetails.CurrentCell = null;
        }

        private async Task PerformHealthChecksAsync()
        {
            try
            {
                // Run all basic checks in parallel
                var urlTask = CheckUrlAsync();
                var databaseTask = CheckDatabaseAsync();
                var importPathTask = CheckImportPathAsync();
                
                // Wait for all basic checks to complete
                await Task.WhenAll(urlTask, databaseTask, importPathTask);
                
                // If all basic checks are successful, query repository health
                bool allBasicChecksGreen = _healthItems.All(item => item.LampColor == Color.Green);
                if (allBasicChecksGreen)
                {
                    await QueryRepositoryHealthAsync();
                }
                
                // Update final status
                UpdateFinalStatus();
            }
            catch (Exception ex)
            {
                Invoke(() =>
                {
                    AddErrorRow($"Health check failed: {ex.Message}");
                    lblStatus.Text = "❌ UNHEALTHY";
                    lblStatus.ForeColor = Color.Red;
                });
            }
        }

        private async Task CheckUrlAsync()
        {
            try
            {
                bool isAccessible = await _healthService.CheckUrlAccessibilityAsync(_target);
                Invoke(() => UpdateHealthItem(0, isAccessible ? Color.Green : Color.Red, 
                    isAccessible ? "Accessible" : "Not Accessible"));
            }
            catch (Exception ex)
            {
                Invoke(() => UpdateHealthItem(0, Color.Red, $"Error: {ex.Message}"));
            }
        }

        private async Task CheckDatabaseAsync()
        {
            try
            {
                bool isConnected = await _healthService.CheckDatabaseConnectivityAsync(_target);
                Invoke(() => UpdateHealthItem(1, isConnected ? Color.Green : Color.Red, 
                    isConnected ? "Connected" : "Not Connected"));
            }
            catch (Exception ex)
            {
                Invoke(() => UpdateHealthItem(1, Color.Red, $"Error: {ex.Message}"));
            }
        }

        private async Task CheckImportPathAsync()
        {
            try
            {
                bool isAccessible = await Task.Run(() => 
                    _healthService.CheckImportPathAccessibility(_target));
                
                Invoke(() => UpdateHealthItem(2, isAccessible ? Color.Green : Color.Red, 
                    isAccessible ? "Accessible" : "Not Accessible"));
            }
            catch (Exception ex)
            {
                Invoke(() => UpdateHealthItem(2, Color.Red, $"Error: {ex.Message}"));
            }
        }

        private async Task QueryRepositoryHealthAsync()
        {
            try
            {
                var repositoryHealth = await _healthService.QueryRepositoryHealthAsync(_target);
                if (repositoryHealth != null)
                {
                    Invoke(() => AddRepositoryHealthItems(repositoryHealth));
                }
            }
            catch (Exception ex)
            {
                Invoke(() => AddErrorRow($"Repository Health Query Failed: {ex.Message}"));
            }
        }

        private void UpdateHealthItem(int index, Color lampColor, string status)
        {
            if (index < _healthItems.Count && index < dgvDetails.Rows.Count)
            {
                _healthItems[index].LampColor = lampColor;
                _healthItems[index].Status = status;
                _healthItems[index].IsCompleted = true;
                
                var row = dgvDetails.Rows[index];
                row.Cells["LampColumn"].Style.ForeColor = lampColor;
                row.Cells["StatusColumn"].Value = status;
            }
        }

        private void AddRepositoryHealthItems(RepositoryHealthResult repositoryHealth)
        {
            // Add repository status
            if (repositoryHealth.Repository_Status != null)
            {
                bool isRunning = repositoryHealth.Repository_Status.Running;
                string status = repositoryHealth.Repository_Status.Status ?? "Unknown";
                string color = isRunning ? "Green" : "Red";
                AddRepositoryHealthRowWithColor(color, "Running", $"Repository Status: {status}");
            }

            // Add health components
            if (repositoryHealth.Health != null)
            {
                if (repositoryHealth.Health.Database != null)
                {
                    string responseTime = repositoryHealth.Health.Database.ResponseTime ?? "N/A";
                    AddRepositoryHealthRowWithColor(repositoryHealth.Health.Database.Color, "Healthy", $"Repository Database (Response Time: {responseTime})");
                }

                if (repositoryHealth.Health.BlobStorage != null)
                {
                    string responseTime = repositoryHealth.Health.BlobStorage.ResponseTime ?? "N/A";
                    AddRepositoryHealthRowWithColor(repositoryHealth.Health.BlobStorage.Color, "Healthy", $"Repository Blob Storage (Response Time: {responseTime})");
                }

                if (repositoryHealth.Health.Search != null)
                {
                    string responseTime = repositoryHealth.Health.Search.ResponseTime ?? "N/A";
                    AddRepositoryHealthRowWithColor(repositoryHealth.Health.Search.Color, "Healthy", $"Repository Search (Response Time: {responseTime})");
                }

                if (repositoryHealth.Health.Identity != null)
                {
                    string responseTime = repositoryHealth.Health.Identity.ResponseTime ?? "N/A";
                    AddRepositoryHealthRowWithColor(repositoryHealth.Health.Identity.Color, "Healthy", $"Repository Identity (Response Time: {responseTime})");
                }
            }
        }

        private void UpdateFinalStatus()
        {
            // Count colors from all rows
            int greenCount = 0;
            int yellowCount = 0;
            int redCount = 0;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["LampColumn"].Style.ForeColor == Color.Green)
                    greenCount++;
                else if (row.Cells["LampColumn"].Style.ForeColor == Color.Gold)
                    yellowCount++;
                else if (row.Cells["LampColumn"].Style.ForeColor == Color.Red)
                    redCount++;
            }

            // Determine final status based on priority: Red > Yellow > Green
            if (redCount > 0)
            {
                lblStatus.Text = "❌ UNHEALTHY";
                lblStatus.ForeColor = Color.Red;
            }
            else if (yellowCount > 0)
            {
                lblStatus.Text = "⚠️ WARNING";
                lblStatus.ForeColor = Color.Orange;
            }
            else if (greenCount > 0)
            {
                lblStatus.Text = "✅ HEALTHY";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "❓ UNKNOWN";
                lblStatus.ForeColor = Color.Gray;
            }
        }

        private enum HealthStatus
        {
            Healthy,
            Warning,
            Unhealthy
        }

        private HealthStatus GetHealthStatusFromColor(string? color)
        {
            return color?.ToLowerInvariant() switch
            {
                "green" => HealthStatus.Healthy,
                "yellow" => HealthStatus.Warning,
                "red" => HealthStatus.Unhealthy,
                _ => HealthStatus.Unhealthy
            };
        }

        private void AddRepositoryHealthRowWithColor(string? healthColor, string statusText, string description)
        {
            var row = dgvDetails.Rows[dgvDetails.Rows.Add()];
            
            // Set lamp symbol
            row.Cells["LampColumn"].Value = "●";
            
            // Set lamp color and status text based on health color
            var healthStatus = GetHealthStatusFromColor(healthColor);
            switch (healthStatus)
            {
                case HealthStatus.Healthy:
                    row.Cells["LampColumn"].Style.ForeColor = Color.Green;
                    row.Cells["StatusColumn"].Value = statusText;
                    break;
                case HealthStatus.Warning:
                    row.Cells["LampColumn"].Style.ForeColor = Color.Gold;
                    row.Cells["StatusColumn"].Value = $"Partial {statusText}";
                    break;
                case HealthStatus.Unhealthy:
                default:
                    row.Cells["LampColumn"].Style.ForeColor = Color.Red;
                    row.Cells["StatusColumn"].Value = $"Not {statusText}";
                    break;
            }
            
            row.Cells["DescriptionColumn"].Value = description;
        }

        private void AddErrorRow(string errorMessage)
        {
            var row = dgvDetails.Rows[dgvDetails.Rows.Add()];
            row.Cells["LampColumn"].Value = "●";
            row.Cells["LampColumn"].Style.ForeColor = Color.Red;
            row.Cells["StatusColumn"].Value = "Error";
            row.Cells["DescriptionColumn"].Value = errorMessage;
        }

        private void SetupDataGrid()
        {
            dgvDetails.AutoGenerateColumns = false;
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AllowUserToResizeRows = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.MultiSelect = false;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.EnableHeadersVisualStyles = false;
            dgvDetails.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dgvDetails.ColumnHeadersDefaultCellStyle.Font = new Font(dgvDetails.Font, FontStyle.Bold);
            
            // Add columns
            var lampColumn = new DataGridViewTextBoxColumn
            {
                Name = "LampColumn",
                HeaderText = "",
                Width = 30,
                Resizable = DataGridViewTriState.False,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 12F) }
            };
            
            var statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "StatusColumn",
                HeaderText = "Status",
                Width = 120,
                Resizable = DataGridViewTriState.True
            };
            
            var descriptionColumn = new DataGridViewTextBoxColumn
            {
                Name = "DescriptionColumn",
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Resizable = DataGridViewTriState.True
            };
            
            dgvDetails.Columns.Add(lampColumn);
            dgvDetails.Columns.Add(statusColumn);
            dgvDetails.Columns.Add(descriptionColumn);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}