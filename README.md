# SnIoGui

A Windows Forms GUI for sensenet import operations, providing a user-friendly interface for browsing, editing, and importing content, as well as viewing logs.

## Features

- **Target selection:** Choose from configured import targets.
- **Directory tree:** Browse and select files or folders for import.
- **Content viewer/editor:** View and edit file contents directly in the app.
- **Import:** Launch import operations using the configured SnIO executable.
- **Log viewer:** Open the latest log file with the default application (e.g., Notepad).
- **Script execution:** Import scripts are executed in a new PowerShell window that closes after execution.

## Usage

1. **Configure targets:**
   - Edit `appsettings.json` to define import targets and the path to the SnIO executable.
2. **Start the application:**
   - Build and run the solution (`SnIoGui.sln`) in Visual Studio or with `dotnet run`.
3. **Select a target:**
   - Use the dropdown to choose an import target.
4. **Browse and edit:**
   - Navigate the directory tree, view or edit file contents, and save changes.
5. **Import:**
   - Select a folder and click "Import Selected" to run the import script.
6. **View logs:**
   - Click "Open Log" to open the latest log file in the default associated application.

## Requirements

- .NET 8.0 (or compatible)
- Windows OS (tested)

## Configuration

Edit `src/SnIoGui/appsettings.json`:

```
{
  "SnIO": "C:/path/to/SnIO.exe",
  "Targets": [
    {
      "Name": "SnWebApp Sql.TokenAuth",
      "Url": "https://localhost:44362",
      "ApiKey": "pc9Q_blah_blah_3mJl",
      "ImportPath": "D:\\dev\\import\\cars import\\Root"
    },
    {
      "Name": "SnCloud Demo",
      "Url": "https://cloud.example.com",
      "ApiKey": "demo_cloud_api_key_123",
      "ImportPath": "D:\\dev\\import\\cloud demo\\Root"
    },
    {
      "Name": "SnLocal Test",
      "Url": "http://localhost:8080",
      "ApiKey": "local_test_api_key_456",
      "ImportPath": "D:\\dev\\import\\local test\\Root"
    }
  ]
}
```

- `SnIO`: Path to the SnIO executable.
- `Targets`: List of import targets with display names, URLs, API keys, and import paths.

## Building

Open the solution in Visual Studio and build, or use the .NET CLI:

```
dotnet build src/SnIoGui/SnIoGui.sln
```

## Notes

- The log viewer always opens the latest file from the `logs` directory next to the running application's executable.

## License

See [LICENSE](LICENSE).