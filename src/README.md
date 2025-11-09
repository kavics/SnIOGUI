# SnIOGUI

A Windows Forms application for importing and exporting content to/from sensenet repositories using SnIO.

## Overview

**SnIOGUI** (sensenet Import/Export GUI) is a .NET Windows Forms application that provides a graphical user interface for managing sensenet content import and export operations. It integrates with the SnIO command-line tool and provides features for browsing repository content, managing multiple targets, and monitoring system health.

## Features

### Import
- Browse local file system directories for content to import
- Tree view navigation of local content structure
- File content preview and editing
- Import content to sensenet repositories
- Real-time content search within the tree view
- Support for multiple target repositories

### Export
- Browse sensenet repository content structure
- Lazy loading of repository tree for performance
- JSON content preview with caching
- Export content from sensenet repositories to local file system
- Clean exported directories using integrated cleaner tool
- Real-time content search within the repository tree

### Common Features
- **Multi-Target Support**: Configure and manage multiple sensenet repositories
- **Health Check**: Check repository health status via health API endpoints
- **Settings Management**: Configure targets, paths, and tools through a dedicated settings editor
- **API Key Management**: Quick clipboard access to API keys
- **Admin UI Access**: Direct browser access to sensenet admin interface
- **Log Viewing**: Quick access to the latest log files

## Requirements

- .NET 8.0 Windows Runtime
- Windows Forms support
- SnIO command-line tool (for import/export operations)
- Optional: Cleaner tool (for cleaning export directories)

## Configuration

The application uses the `[user-directory]\AppData\Local\SnIoGuisettings.json` file for configuration, which has a structure similar to this:

```json
{
  "SnIO": "path/to/SnIO.exe",
  "Cleaner": "path/to/Cleaner.exe",
  "Targets": [
    {
      "Name": "Target Name",
      "Url": "https://example.com",
      "ApiKey": "your-api-key",
      "HealthApiKey": "your-health-api-key",
      "ImportPath": "C:\\path\\to\\import",
      "ExportPath": "C:\\path\\to\\export",
      "DbServer": "database-server",
      "DbName": "database-name",
      "AdminUrl": "https://admin.example.com"
    }
  ]
}
```

### Configuration Properties

- **SnIO**: Path to the SnIO executable for import/export operations
- **Cleaner**: Path to the cleaner tool executable
- **Targets**: Array of target repository configurations with this data per item:
  - **Name**: Display name for the target
  - **Url**: sensenet repository URL
  - **ApiKey**: API key for repository access
  - **HealthApiKey**: API key for health check endpoint
  - **ImportPath**: Default local path for importing content
  - **ExportPath**: Default local path for exporting content
  - **DbServer**: Database server name (optional)
  - **DbName**: Database name (optional)
  - **AdminUrl**: Admin interface URL (optional)


## Usage

### Importing Content

1. Launch the application (defaults to Import form)
2. Select a target repository from the dropdown
3. Browse the local file system to find content for import
4. Select the content node to import (you can only import if the selected item is under a "Root")
5. Click the Import button to configure and execute the import

### Exporting Content

1. Switch to Export form using the "Export" button
2. Select a target repository from the dropdown
3. Browse the repository tree to find content to export
4. Select the content node to export
5. Click the Export button to configure and execute the export

### Managing Settings

1. Click the Settings button (??) on either form
2. Edit global settings (SnIO path, Cleaner path)
3. Add, edit, or remove target configurations
4. Save changes to update the application configuration

### Health Check

1. Select a target from the dropdown
2. Click the Health button
3. Wait and view health check results in a dedicated dialog
