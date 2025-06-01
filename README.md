# BETA | Glob Pattern File Enumeration Utility

A lightweight C# utility for recursive file and directory enumeration using flexible glob and regex patterns. It lets you combine inclusion and exclusion rules, control recursion depth, handle hidden files, and choose between synchronous and asynchronous APIs.

---

## Table of Contents

- [Installation](#installation)
- [Overview](#overview)
- [GlobOptions Model](#globoptions-model)
- [Basic Usage](#basic-usage)
- [API Usage](#api-usage)
- [Examples](#examples)
- [License](#license)

---

## Installation

TODO: NuGet package or source code instructions.

---

## Overview

- **`GlobOptions`**: Holds configuration for root path, include/exclude patterns, recursion depth, and flags.
- **`Glob.Create(...)`**: Static factory to validate a root path and obtain a `Globber`.
- **`Globber`**: Service that performs enumeration:
  - Recursion up to `MaxDepth`
  - Includes/excludes via regex-converted patterns
  - Options for case sensitivity and hidden files
  - Synchronous and asynchronous enumeration

---

## GlobOptions Model

| Property           | Type                | Default     | Description                                                                                 |
|--------------------|---------------------|-------------|---------------------------------------------------------------------------------------------|
| `RootPath`         | `string`            | `""`      | Fully qualified base directory for enumeration                                              |
| `MatchPatterns`    | `IList<string>`     | `[]`        | Glob/regex patterns to include (converted to regex internally)                              |
| `ExcludePatterns`  | `IList<string>`     | `[]`        | Glob/regex patterns to exclude                                                              |
| `MaxDepth`         | `uint`              | `50`        | Maximum recursion depth (0 = current folder only)                                           |
| `IgnoreCase`       | `bool`              | `false`     | Case-insensitive matching                                                                   |
| `IncludeHidden`    | `bool`              | `false`     | Whether to include hidden files/directories                                                 |
| `NormalizedRoot`   | `string` (read-only)| —           | `RootPath` with a trailing directory separator                                              |
| `PrefixLength`     | `int` (read-only)   | —           | Length of `NormalizedRoot` (used to trim full paths to relative paths)                      |

---

## Basic Usage

Demonstrates how to obtain a `Globber` instance.

### Factory Method

```csharp
using YourNamespace.Utilities;

var root = @"C:\Projects\MyApp";
var globber = Glob.Create(root);
```

### Manual Instantiation

```csharp
using YourNamespace.Utilities;
using System.Collections.Generic;

var options = new GlobOptions
{
    RootPath = @"C:\Projects\MyApp",
    MatchPatterns = new List<string> { @".*\\.cs$" },
    ExcludePatterns = new List<string> { @".*\\.g\\.cs$" },
    MaxDepth = 3,
    IgnoreCase = true,
    IncludeHidden = true
};
var globber = new Globber(options);
```

---

## API Usage

Configure behavior and perform enumeration.

### Configuration

Chain methods to set match/exclude rules, depth, and flags.

```csharp
globber
    .CaseInsensitive()      // ignore case
    .IncludeHidden()        // include hidden files
    .Match(@".*\\.cs$")  // include .cs files
    .Exclude(@".*\\.g\\.cs$") // exclude generated
    .MaxDepth(5);           // set max depth
```

### Enumeration

#### Directories

```csharp
var dirs = globber.EnumerateDirectories();
// or
var customDirs = globber.EnumerateDirectories(@"D:\Data");
```

#### Files

```csharp
var files = globber.EnumerateFiles();
// or
var customFiles = globber.EnumerateFiles(@"D:\Logs");
```

#### Asynchronous

```csharp
var asyncDirs  = await globber.EnumerateDirectoriesAsync();
var asyncFiles = await globber.EnumerateFilesAsync();
```

---

## Examples

```csharp
// 1) Find all .log files, exclude archives
var logs = Glob.Create("C:\Logs")
    .Match(@".*\\.log$")
    .Exclude(@"archive/.*")
    .EnumerateFiles();

// 2) Enumerate test folders up to 2 levels deep
var tests = Glob.Create("C:\Projects")
    .CaseInsensitive()
    .Match(@"src/.+/tests$")
    .MaxDepth(2)
    .EnumerateDirectories();

// 3) Async scan of /data
var dataFiles = await Glob.Create("/data")
    .IncludeHidden()
    .EnumerateFilesAsync();
```

---

## License

TODO: Add license information here.
