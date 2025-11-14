# CLAC

A reverse-Polish notation calculator written in C#

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


## Tech Stack

- dotnet
- Avalonia UI
- xunit

## Downloads

macOS binaries are available in the [Releases](https://github.com/maasha/clac/releases) section.

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

## Creating a Release

To create a new release with macOS binaries:

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
   - Check the GitHub Actions tab for the "Release macOS" workflow
   - The workflow will automatically:
     - Build binaries for both architectures (osx-x64 and osx-arm64)
     - Create a GitHub Release
     - Attach the ZIP files to the release

4. **Verify the release:**
   - Go to the [Releases](https://github.com/maasha/clac/releases) page
   - Confirm the release appears with both macOS ZIP files attached

**Note:** You can also trigger the workflow manually via GitHub Actions → "Release macOS" → "Run workflow", but this won't automatically create a GitHub Release (it will only build and upload artifacts).

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
