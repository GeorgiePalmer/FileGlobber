
# RecursiveFileGlob

A C# library for recursive file globbing and filtering with Unix-style patterns and streaming support—designed to feel just like the built-in `Directory.EnumerateDirectories()` and `EnumerateFiles()` methods.

## Features
- `**` recursive glob patterns (e.g., `**/*.cs`).
- Support for multiple include and exclude patterns.
- **Synchronous** lazy enumeration via `EnumerateFiles()` and `EnumerateDirectories()`.
- **Asynchronous** streaming via `IAsyncEnumerable<string>` for high-throughput scenarios.
- Optional settings (max recursion depth, case sensitivity, hidden items).
- Transparent Windows long-path support (`\\?\\` prefix) and reserved-name handling.

## Installation

```shell
dotnet add package RecursiveFileGlob --version 1.0.0
```

## Usage

### Synchronous Enumeration

```csharp
var options = new GlobOptions
{
    RootPath = @"C:\Projects",
    Patterns = new[] { "**/*.cs", "**/*.js" },
    ExcludePatterns = new[] { "**/bin/**", "**/obj/**" },
    MaxDepth = 5,
    CaseSensitive = false,
    IncludeHidden = false
};

var globber = new Globber(options);

// Files
foreach (var file in globber.EnumerateFiles())
{
    Console.WriteLine(file);
}

// Directories
foreach (var dir in globber.EnumerateDirectories())
{
    Console.WriteLine(dir);
}
```

### Asynchronous Enumeration

```csharp
await foreach (var file in globber.EnumerateFilesAsync())
{
    Console.WriteLine(file);
}
```

### Fluent API

```csharp
// Chain options and enumerate
await foreach (var file in Glob.Create(@"C:\Projects")
    .Include("**/*.md")
    .Exclude("**/temp/**")
    .WithMaxDepth(3)
    .WithCaseSensitivity(true)
    .IncludeHidden()
    .EnumerateFilesAsync())
{
    Console.WriteLine(file);
}
```

## API Reference

### `class GlobOptions`
- `string RootPath` — Base directory to start the search.
- `string[] Patterns` — Include patterns.
- `string[] ExcludePatterns` — Exclude patterns.
- `int? MaxDepth` — Maximum recursion depth.
- `bool CaseSensitive` — Toggle case sensitivity.
- `bool IncludeHidden` — Include hidden files and directories.
- `EnvironmentOptions EnvOptions` — OS-specific behavior (e.g. long-path, name validation).

### `class EnvironmentOptions`
- `int MaxPathLength` — Maximum allowed path length on the current OS.
- `Func<string, bool> IsValidName` — Custom delegate to validate file or directory names.

### `class Globber`
- `Globber(GlobOptions options)` — Initialize with options.
- `IEnumerable<string> EnumerateFiles()` — Sync, lazy file enumeration.
- `IEnumerable<string> EnumerateDirectories()` — Sync, lazy directory enumeration.
- `IAsyncEnumerable<string> EnumerateFilesAsync()` — Async file enumeration.
- `IAsyncEnumerable<string> EnumerateDirectoriesAsync()` — Async directory enumeration.

### `static class Glob`
- `Glob Create(string rootPath)` — Start a fluent query.
- `Glob Include(params string[] patterns)` — Add include patterns.
- `Glob Exclude(params string[] patterns)` — Add exclude patterns.
- `Glob WithMaxDepth(int depth)` — Limit recursion depth.
- `Glob WithCaseSensitivity(bool caseSensitive)` — Set sensitivity.
- `Glob IncludeHidden()` — Include hidden items.
- `IEnumerable<string> EnumerateFiles()` / `EnumerateDirectories()` — Sync enumeration.
- `IAsyncEnumerable<string> EnumerateFilesAsync()` / `EnumerateDirectoriesAsync()` — Async enumeration.

## Project Structure

```
/src
/RecursiveFileGlob
Globber.cs
GlobOptions.cs
EnvironmentOptions.cs
Glob.cs
/RecursiveFileGlob.Tests
GlobberTests.cs
RecursiveFileGlob.sln
RecursiveFileGlob.nuspec
```
