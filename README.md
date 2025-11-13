# CLAC

A reverse-Polish notation calculator written in C#


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
   - **Option 1 (Recommended):** Right-click `Clac.app` → Select "Open" → Click "Open" in the dialog
   - **Option 2:** Remove quarantine attribute:
     ```bash
     xattr -d com.apple.quarantine /path/to/Clac.app
     ```
     Then double-click `Clac.app` normally

4. **Move to Applications (optional):**
   - Drag `Clac.app` to your Applications folder for easy access

**Note:** The app is ad-hoc signed (not notarized) because we don't have an Apple Developer certificate. This is common for open-source projects. After the first launch using the right-click method, macOS will remember your choice and allow normal double-clicking.

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
