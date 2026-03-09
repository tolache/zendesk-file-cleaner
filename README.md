# Zendesk Ticket Cleaner

A simple command line tool that removes ticket directories from the disk if the ticket was closed earlier than a certain
threshold (60 days).

## Run from sources

Requires [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0).

```
dotnet run --project src/ZendeskFileCleaner/ZendeskFileCleaner.csproj --help
```

### Examples

#### macOS/Linux/Shell

```shell
dotnet run --project src/ZendeskFileCleaner/ZendeskFileCleaner.csproj ~/Downloads/ \
  --subdomain "company" \
  --email "user@company.com" \
  --token "XYZ123" \
  --zd-user-id 111122223333
```

#### Windows/PowerShell

```powershell
dotnet run --project src\ZendeskFileCleaner\ZendeskFileCleaner.csproj ~/Downloads/ `
  --subdomain "company" `
  --email "user@company.com" `
  --token "XYZ123" `
  --zd-user-id 111122223333
```
