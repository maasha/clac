# CLAC

A reverse-Polish notation calculator written in C#

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


## Tech Stack

- dotnet
- Avalonia UI
- xunit

## Downloads

macOS, Windows, and Linux binaries are available in the [Releases](https://github.com/maasha/clac/releases) section.

### macOS Installation

1. **Download the appropriate version:**
   - **Intel Macs (x64):** Download `clac-macos-osx-x64.zip`
   - **Apple Silicon Macs (arm64):** Download `clac-macos-osx-arm64.zip`

2. **Extract the ZIP file:**
   - Double-click the downloaded ZIP file to extract `Clac.app`

3. **Run the application:**
   - macOS Gatekeeper will block the app initially (it's ad-hoc signed, not notarized)
   - **Option 1 (Recommended):** - Follow these steps in order:
     1. Right-click `Clac.app` → Select "Open"
     2. Go to System Settings → Privacy & Security
     3. Look for a message about "Clac.app was blocked" and click "Open Anyway"
     4. Right-click `Clac.app` → Select "Open" again (the 
   - **Option 2:** Remove quarantine attribute:
     ```bash
     xattr -d com.apple.quarantine /path/to/Clac.app
     ```
     Then double-click `Clac.app` normally

4. **Move to Applications (optional):**
   - Drag `Clac.app` to your Applications folder for easy access

**Note:** The app is ad-hoc signed (not notarized) because we don't have an Apple Developer certificate. This is common for open-source projects. After the first launch using the right-click method, macOS will remember your choice and allow normal double-clicking.

### Windows Installation

1. **Download the appropriate version:**
   - **64-bit Windows:** Download `clac-windows-win-x64.zip`
   - **32-bit Windows:** Download `clac-windows-win-x86.zip`

2. **Extract the ZIP file:**
   - Right-click the downloaded ZIP file → "Extract All"
   - Choose a destination folder (e.g., `C:\Program Files\Clac`)

3. **Run the application:**
   - Navigate to the extracted folder
   - Double-click `Clac.exe` to launch the application

4. **Create a shortcut (optional):**
   - Right-click `Clac.exe` → "Create shortcut"
   - Move the shortcut to your Desktop or Start Menu for easy access

**Note:** Windows binaries are self-contained and don't require .NET to be installed. Windows Defender may show a warning on first launch since the app isn't code-signed. Click "More info" → "Run anyway" if this occurs.

### Linux Installation

1. **Download the appropriate version:**
   - **64-bit Linux (x64):** Download `clac-linux-linux-x64.tar.gz`
   - **ARM64 Linux:** Download `clac-linux-linux-arm64.tar.gz`

2. **Extract the tarball:**
   ```bash
   tar -xzf clac-linux-linux-x64.tar.gz
   ```

3. **Run the application:**
   ```bash
   ./Clac
   ```

4. **Optional: Install system-wide:**
   ```bash
   # Move to a system directory (requires sudo)
   sudo mv Clac /usr/local/bin/
   
   # Or create a symlink
   sudo ln -s $(pwd)/Clac /usr/local/bin/clac
   ```

**Note:** Linux binaries are self-contained and don't require .NET to be installed. The executable may need execute permissions: `chmod +x Clac`

## Creating a Release

To create a new release with macOS, Windows, and Linux binaries:

1. **Ensure your code is ready:**
   - Make sure all changes are committed and pushed to `main`
   - Verify tests pass and the build works

2. **Create and push a version tag:**
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```
   The tag must start with `v` (e.g., `v1.0.0`, `v1.2.3`) to trigger the release workflow.

3. **Wait for the workflow to complete:**
   - Check the GitHub Actions tab for the "Release" workflow
   - The workflow will automatically:
     - Build binaries for macOS (osx-x64 and osx-arm64)
     - Build binaries for Windows (win-x64 and win-x86)
     - Build binaries for Linux (linux-x64 and linux-arm64)
     - Create a GitHub Release
     - Attach all platform binaries to the release

4. **Verify the release:**
   - Go to the [Releases](https://github.com/maasha/clac/releases) page
   - Confirm the release appears with all platform binaries attached

**Note:** You can also trigger the workflow manually via GitHub Actions → "Release" → "Run workflow", but this won't automatically create a GitHub Release (it will only build and upload artifacts).

## Boiler plate

Commands for setting up boiler plate project

```bash
brew install dotnet-sdk
dotnet --version
cd ~/src/clac
dotnet new install Avalonia.Templates
dotnet new classlib -n Clac.Core -o src/Clac.Core
dotnet new avalonia.app -n Clac.UI -o src/Clac.UI
dotnet new sln -n Clac
dotnet sln add src/Clac.Core/Clac.Core.csproj
dotnet sln add src/Clac.UI/Clac.UI.csproj
dotnet add src/Clac.UI/Clac.UI.csproj reference src/Clac.Core/Clac.Core.csproj
mkdir tests
cd tests
dotnet new xunit -n Clac.Tests
dotnet sln ../Clac.sln add Clac.Tests/Clac.Tests.csproj
dotnet add Clac.Tests/Clac.Tests.csproj reference ../src/Clac.Core/Clac.Core.csproj
cd ..
dotnet run --project src/Clac.UI
dotnet test
```


###

For Result<T> instead of throw:

```bash
dotnet add package DotNext
```
