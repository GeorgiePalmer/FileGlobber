# WindowsTestArchive.ps1

# --- Configurable variables ---
$root = "WindowsTestArchive"
$zip  = "$root.zip"

# --- Cleanup ---
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
if (Test-Path $zip)  { Remove-Item $zip -Force }

# --- Create directory tree ---
$dirs = @(
  "server\logs",
  "server\logs\archive",
  "src\foo\tests",
  "src\foo\Implementation",
  "SRC\FOO\TESTS",
  "builds",
  "config",
  "images",
  "a\b\c\d\e\f\g",
  "windows",
  "path with spaces",
  "unicode\路径",
  "reserved",
  "dotfiles\.env",
  "links",
  "pipes",
  "relative\.\server\logs",
  "relative\..\src\foo\tests",
  "dash",
  "control",
  "emoji"
)
foreach ($d in $dirs) {
  New-Item -ItemType Directory -Path (Join-Path $root $d) -Force | Out-Null
}

# --- Helper: write random bytes to a file ---
function Write-RandomFile {
  param($FullPath, $Length = 128)
  # Generate cryptographically-strong random bytes
  $bytes = New-Object byte[] $Length
  [System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
  [System.IO.File]::WriteAllBytes($FullPath, $bytes)
}

# --- Create each placeholder with random content ---
$files = @(
  "server\logs\error-2025-05-27.log",
  "server\logs\archive\error-2025-01-01.log",
  "src\foo\tests\MyTest.cs",
  "src\foo\tests\MixedSep.cs",
  "src\foo\Implementation\TodoHandler.cs",
  "SRC\FOO\TESTS\mytest.CS",
  "builds\MyApp-v2.3.1.zip",
  "builds\MyApp-v2.3-beta.zip",
  "builds\MyApp-v2.3-rc1.zip",
  "config\app.prod.local.json",
  "images\.DS_Store",
  "a\b\c\d\e\f\g\deepFile.txt",
  "path with spaces\My File Name.txt",
  "unicode\路径\文件😀.txt",
  "reserved\CON.txt",
  "dotfiles\.gitignore",
  "dotfiles\.env\.secret",
  "dash\-important-file.txt",
  "control\ctrl`u0001char.txt",
  "emoji\👍.md"
)
foreach ($rel in $files) {
  $full = Join-Path $root $rel
  # Ensure file exists, then write random bytes
  New-Item -ItemType File -Path $full -Force | Out-Null
  Write-RandomFile -FullPath $full -Length 128
}

# --- Trailing‐space filename ---
$cwd       = (Get-Location).ProviderPath
$tsLiteral = "\\?\$cwd\$root\trailingSpace.txt "
New-Item -LiteralPath $tsLiteral -ItemType File -Force | Out-Null
Write-RandomFile -FullPath $tsLiteral -Length 64

# --- Symlinks (requires Admin or Developer Mode) ---
# If these fail, you can ignore them—they won't block the zip creation.
Try {
  New-Item -ItemType SymbolicLink `
           -Path (Join-Path $root "links\symlinkToTodoHandler.cs") `
           -Target "..\src\foo\Implementation\TodoHandler.cs" | Out-Null
  New-Item -ItemType SymbolicLink `
           -Path (Join-Path $root "links\brokenSymlink") `
           -Target "nonexistent.target" | Out-Null
} Catch { Write-Warning "Symlink creation failed: $_" }

# --- Named‐pipe placeholder (just a file) ---
$pipePath = Join-Path $root "pipes\myfifo"
New-Item -ItemType File -Path $pipePath -Force | Out-Null
Write-RandomFile -FullPath $pipePath -Length 32

# --- Zip it up ---
Compress-Archive -Path "$root\*" -DestinationPath $zip -Force

Write-Host "✅ Created $zip with Windows test archive (files filled with random bytes)"
