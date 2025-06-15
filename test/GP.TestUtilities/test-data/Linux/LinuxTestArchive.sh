#!/usr/bin/env bash
set -euo pipefail

root="LinuxTestArchive"
zipfile="$root.zip"

# --- Cleanup any existing artifacts ---
rm -rf "$root" "$zipfile"

# --- 1) Create directories ---
dirs=(
  server/logs
  server/logs/archive
  src/foo/tests
  src/foo/Implementation
  SRC/FOO/TESTS
  builds
  config
  images
  a/b/c/d/e/f/g
  dotfiles/.env
  links
  pipes
  dash
  unicode/路径
  emoji
)
for d in "${dirs[@]}"; do
  mkdir -p "$root/$d"
done

# --- 2) Helper: write 128 random bytes into a file ---
write_random() {
  dd if=/dev/urandom of="$1" bs=1 count=128 status=none
}

# --- 3) Create each test file with random content ---
files=(
  server/logs/error-2025-05-27.log
  server/logs/archive/error-2025-01-01.log
  src/foo/tests/MyTest.cs
  src/foo/tests/MixedSep.cs
  src/foo/Implementation/TodoHandler.cs
  SRC/FOO/TESTS/mytest.CS
  builds/MyApp-v2.3.1.zip
  builds/MyApp-v2.3-beta.zip
  builds/MyApp-v2.3-rc1.zip
  config/app.prod.local.json
  images/.DS_Store
  a/b/c/d/e/f/g/deepFile.txt
  dotfiles/.gitignore
  dotfiles/.env/.secret
  dash/-important-file.txt
  unicode/路径/文件😀.txt
  emoji/👍.md
)
for f in "${files[@]}"; do
  target="$root/$f"
  write_random "$target"
done

# --- 4) Create symlinks ---
ln -s ../src/foo/Implementation/TodoHandler.cs "$root/links/symlinkToTodoHandler.cs"
ln -s nonexistent.target                     "$root/links/brokenSymlink"

# --- 5) Create a named pipe (FIFO) ---
mkfifo "$root/pipes/myfifo"

# --- 6) Zip it up, preserving symlinks ---
(
  cd "$root"
  zip -r -y "../$zipfile" .
)

echo "✅ Created $zipfile containing Linux test archive."
#!/usr/bin/env bash
set -euo pipefail

root="LinuxTestArchive"
zipfile="$root.zip"

# --- Cleanup any existing artifacts ---
rm -rf "$root" "$zipfile"

# --- 1) Create directories ---
dirs=(
  server/logs
  server/logs/archive
  src/foo/tests
  src/foo/Implementation
  SRC/FOO/TESTS
  builds
  config
  images
  a/b/c/d/e/f/g
  dotfiles/.env
  links
  pipes
  dash
  unicode/路径
  emoji
)
for d in "${dirs[@]}"; do
  mkdir -p "$root/$d"
done

# --- 2) Helper: write 128 random bytes into a file ---
write_random() {
  dd if=/dev/urandom of="$1" bs=1 count=128 status=none
}

# --- 3) Create each test file with random content ---
files=(
  server/logs/error-2025-05-27.log
  server/logs/archive/error-2025-01-01.log
  src/foo/tests/MyTest.cs
  src/foo/tests/MixedSep.cs
  src/foo/Implementation/TodoHandler.cs
  SRC/FOO/TESTS/mytest.CS
  builds/MyApp-v2.3.1.zip
  builds/MyApp-v2.3-beta.zip
  builds/MyApp-v2.3-rc1.zip
  config/app.prod.local.json
  images/.DS_Store
  a/b/c/d/e/f/g/deepFile.txt
  dotfiles/.gitignore
  dotfiles/.env/.secret
  dash/-important-file.txt
  unicode/路径/文件😀.txt
  emoji/👍.md
)
for f in "${files[@]}"; do
  target="$root/$f"
  write_random "$target"
done

# --- 4) Create symlinks (if supported) ---
ln -s ../src/foo/Implementation/TodoHandler.cs "$root/links/symlinkToTodoHandler.cs" 2>/dev/null || echo "⚠️  Skipped symlinkToTodoHandler.cs"
ln -s nonexistent.target                     "$root/links/brokenSymlink"         2>/dev/null || echo "⚠️  Skipped brokenSymlink"

# --- 5) Create a named pipe (FIFO) or fallback to regular file ---
fifo="$root/pipes/myfifo"
if ! mkfifo "$fifo" 2>/dev/null; then
  echo "⚠️  mkfifo not supported—creating regular file instead"
  write_random "$fifo"
fi

# --- 6) Zip it up, preserving symlinks ---
(
  cd "$root"
  zip -r -y "../$zipfile" . 
)

echo "✅ Created $zipfile containing Linux test archive."
#!/usr/bin/env bash
set -euo pipefail

root="LinuxTestArchive"
zipfile="$root.zip"

# --- Cleanup any existing artifacts ---
rm -rf "$root" "$zipfile"

# --- 1) Create directories ---
dirs=(
  server/logs
  server/logs/archive
  src/foo/tests
  src/foo/Implementation
  SRC/FOO/TESTS
  builds
  config
  images
  a/b/c/d/e/f/g
  dotfiles/.env
  links
  pipes
  dash
  unicode/路径
  emoji
)
for d in "${dirs[@]}"; do
  mkdir -p "$root/$d"
done

# --- 2) Helper: write 128 random bytes into a file ---
write_random() {
  dd if=/dev/urandom of="$1" bs=1 count=128 status=none
}

# --- 3) Create each test file with random content ---
files=(
  server/logs/error-2025-05-27.log
  server/logs/archive/error-2025-01-01.log
  src/foo/tests/MyTest.cs
  src/foo/tests/MixedSep.cs
  src/foo/Implementation/TodoHandler.cs
  SRC/FOO/TESTS/mytest.CS
  builds/MyApp-v2.3.1.zip
  builds/MyApp-v2.3-beta.zip
  builds/MyApp-v2.3-rc1.zip
  config/app.prod.local.json
  images/.DS_Store
  a/b/c/d/e/f/g/deepFile.txt
  dotfiles/.gitignore
  dotfiles/.env/.secret
  dash/-important-file.txt
  unicode/路径/文件😀.txt
  emoji/👍.md
)
for f in "${files[@]}"; do
  target="$root/$f"
  write_random "$target"
done

# --- 4) Create symlinks (errors suppressed) ---
ln -s ../src/foo/Implementation/TodoHandler.cs "$root/links/symlinkToTodoHandler.cs" 2>/dev/null || echo "⚠️  Skipped symlinkToTodoHandler.cs"
ln -s nonexistent.target                     "$root/links/brokenSymlink"         2>/dev/null || echo "⚠️  Skipped brokenSymlink"

# --- 5) Create pipes/myfifo as a plain file ---
write_random "$root/pipes/myfifo"

# --- 6) Zip it up, preserving symlinks ---
(
  cd "$root"
  zip -r -y "../$zipfile" .
)

echo "✅ Created $zipfile containing Linux test archive (no FIFO)." 
