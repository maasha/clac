# Build Strategy - Cross-Platform Binary and GitHub Release

## Overview
This document outlines the CI/CD steps required to compile the Clac application into binaries for Windows, macOS, and Linux, and release them as GitHub releases.

## Implementation Phases

The build strategy is implemented in phases:

- **Phase 1: macOS** (Current focus) - Build and release macOS binaries
- **Phase 2: Windows** (Future) - Add Windows builds (`win-x64`, `win-x86`, `win-arm64`)
- **Phase 3: Linux** (Future) - Add Linux builds (`linux-x64`, `linux-arm64`)

Each phase follows the same workflow architecture, building on the previous phase's foundation.

## Branching Strategy (GitHub Flow for Open Source)

This project uses **GitHub Flow** optimized for open source collaboration:

- **Main branch** (`main` or `master`): Protected branch, production-ready code
- **Feature branches**: Created by maintainers or contributors
- **Fork-based contributions**: External contributors fork the repo and open PRs

### Workflow Overview

1. **PR Validation**: All PRs trigger build/test (works with forks)
2. **Main Branch CI**: Merges to main trigger build/test and artifact upload
3. **Releases**: Tag-based releases create GitHub releases with binaries

This strategy ensures:
- Security: Protected main branch, PR reviews required
- Quality: Tests run on PRs and main branch
- Collaboration: Works seamlessly with fork-based contributions
- Control: Manual releases via tags

## Prerequisites

### Project Configuration
- The project uses .NET 9.0 and Avalonia UI
- `Clac.UI.csproj` must have `OutputType` set to `Exe` (not `WinExe`) to enable cross-platform builds
- Project structure: `Clac.Core` (library) + `Clac.UI` (executable)
- .NET 9.0 supports Windows, macOS, and Linux builds

### GitHub Setup
- Repository exists on GitHub
- GitHub Actions enabled (enabled by default)
- Appropriate permissions for creating releases (if using GitHub token)

## CI/CD Pipeline Steps

### Workflow Architecture

We use three separate workflows for different purposes:

1. **`build-pr.yml`** - PR Validation
   - Triggers: PR opened/updated to main
   - Purpose: Validate PRs before merge (works with forks)
   - Actions: Build + test only

2. **`build-main.yml`** - Main Branch CI
   - Triggers: Push to main (after PR merge)
   - Purpose: Validate merged code, upload artifacts
   - Actions: Build + test + upload artifacts

3. **`release.yml`** - Release Creation
   - Triggers: Tag push (`v*`) or manual dispatch
   - Purpose: Create GitHub releases with binaries
   - Actions: Build + test + create release

### 1. Create PR Validation Workflow

Create `.github/workflows/build-pr.yml`:

**Workflow Triggers:**
- Pull requests to `main`/`master` branch
- Works with forks (external contributors)

**Purpose:**
- Validate all PRs before merge
- Ensure code quality and tests pass
- Provide feedback to contributors

### 2. Create Main Branch CI Workflow

Create `.github/workflows/build-main.yml`:

**Workflow Triggers:**
- Push to `main`/`master` branch (after PR merge)

**Purpose:**
- Validate merged code
- Upload build artifacts for testing
- Ensure main branch always has working builds

### 3. Create Release Workflow

Create `.github/workflows/release.yml`:

**Workflow Triggers:**
- Tag push (`v*` pattern, e.g., `v1.0.0`)
- Manual workflow dispatch (optional)

**Purpose:**
- Create GitHub releases with attached binaries
- Controlled release process
- Supports frequent releases

### 4. Detailed Workflow Steps

#### Step 1: Checkout Code
```yaml
- uses: actions/checkout@v4
```

#### Step 2: Setup .NET SDK
```yaml
- name: Setup .NET
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '9.0.x'
```

#### Step 3: Restore Dependencies
```yaml
- name: Restore dependencies
  run: dotnet restore
```

#### Step 4: Run Tests
```yaml
- name: Run tests
  run: dotnet test --no-restore --verbosity normal
```

#### Step 5: Build for macOS Architectures

Use a matrix strategy to build for both Intel (x64) and Apple Silicon (arm64):

```yaml
strategy:
  matrix:
    runtime: [osx-x64, osx-arm64]
```

Build command for each runtime:
```bash
dotnet publish src/Clac.UI/Clac.UI.csproj \
  -c Release \
  -r ${{ matrix.runtime }} \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -o publish/${{ matrix.runtime }}
```

#### Step 6: Create Artifacts

Create ZIP archives for each architecture:

```yaml
- name: Create artifacts
  run: |
    cd publish/${{ matrix.runtime }}
    zip -r ../../clac-macos-${{ matrix.runtime }}.zip .
    cd ../..
```

#### Step 7: Upload Artifacts

```yaml
- name: Upload artifacts
  uses: actions/upload-artifact@v4
  with:
    name: clac-macos-${{ matrix.runtime }}
    path: clac-macos-${{ matrix.runtime }}.zip
```

#### Step 8: Create GitHub Release (on tag)

```yaml
- name: Create Release
  if: startsWith(github.ref, 'refs/tags/')
  uses: softprops/action-gh-release@v1
  with:
    files: |
      clac-macos-osx-x64.zip
      clac-macos-osx-arm64.zip
    draft: false
    prerelease: false
  env:
    GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

## Complete Workflow Examples

### Workflow 1: PR Validation (`build-pr.yml`)

```yaml
name: Build and Test PR

on:
  pull_request:
    branches: [ main, master ]

jobs:
  build-and-test:
    runs-on: macos-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Run tests
        run: dotnet test --no-restore --verbosity normal

      - name: Build (validation)
        run: dotnet build -c Release --no-restore
```

### Workflow 2: Main Branch CI (`build-main.yml`)

```yaml
name: Build Main Branch

on:
  push:
    branches: [ main, master ]

jobs:
  build-and-upload:
    runs-on: macos-latest
    strategy:
      matrix:
        runtime: [osx-x64, osx-arm64]
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Run tests
        run: dotnet test --no-restore --verbosity normal

      - name: Build macOS binary
        run: |
          dotnet publish src/Clac.UI/Clac.UI.csproj \
            -c Release \
            -r ${{ matrix.runtime }} \
            --self-contained true \
            -p:PublishSingleFile=true \
            -p:IncludeNativeLibrariesForSelfExtract=true \
            -o publish/${{ matrix.runtime }}

      - name: Create artifact
        run: |
          cd publish/${{ matrix.runtime }}
          zip -r ../../clac-macos-${{ matrix.runtime }}.zip .
          cd ../..

      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: clac-macos-${{ matrix.runtime }}
          path: clac-macos-${{ matrix.runtime }}.zip
```

### Workflow 3: Release (`release.yml`)

```yaml
name: Release macOS

on:
  push:
    tags: [ 'v*' ]
  workflow_dispatch:

jobs:
  release:
    runs-on: macos-latest
    strategy:
      matrix:
        runtime: [osx-x64, osx-arm64]
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Run tests
        run: dotnet test --no-restore --verbosity normal

      - name: Build macOS binary
        run: |
          dotnet publish src/Clac.UI/Clac.UI.csproj \
            -c Release \
            -r ${{ matrix.runtime }} \
            --self-contained true \
            -p:PublishSingleFile=true \
            -p:IncludeNativeLibrariesForSelfExtract=true \
            -o publish/${{ matrix.runtime }}

      - name: Create artifact
        run: |
          cd publish/${{ matrix.runtime }}
          zip -r ../../clac-macos-${{ matrix.runtime }}.zip .
          cd ../..

      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: clac-macos-${{ matrix.runtime }}
          path: clac-macos-${{ matrix.runtime }}.zip
          retention-days: 90

  create-release:
    needs: release
    runs-on: ubuntu-latest
    steps:
      - name: Download artifacts
        uses: actions/download-artifact@v4
        with:
          path: artifacts

      - name: Create Release
        uses: softprops/action-gh-release@v1
        with:
          files: |
            artifacts/clac-macos-osx-x64/clac-macos-osx-x64.zip
            artifacts/clac-macos-osx-arm64/clac-macos-osx-arm64.zip
          draft: false
          prerelease: false
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

## Release Process

### Creating a Release

1. **Tag the release:**
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. **Release workflow automatically:**
   - Builds for both macOS architectures
   - Runs tests
   - Creates a GitHub release
   - Attaches both binary ZIP files to the release

### Manual Release

1. Go to GitHub Actions tab
2. Select "Release macOS" workflow
3. Click "Run workflow"
4. Click "Run workflow" button
5. Workflow will build and create release

### Typical Development Workflow

1. **Create feature branch:**
   ```bash
   git checkout -b feature/new-feature
   ```

2. **Make changes and push:**
   ```bash
   git push origin feature/new-feature
   ```

3. **Open PR to main:**
   - PR triggers `build-pr.yml` workflow
   - Tests run automatically
   - Review and merge when ready

4. **Merge to main:**
   - Triggers `build-main.yml` workflow
   - Builds binaries and uploads artifacts
   - Artifacts available for testing

5. **Create release:**
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```
   - Triggers `release.yml` workflow
   - Creates GitHub release with binaries

## Artifact Details

### Binary Types
- **Self-contained**: Includes .NET runtime (~50-100MB per binary)
- **Single file**: All dependencies bundled into one executable
- **Native libraries**: Included for self-extraction

### Output Files by Platform

#### Phase 1: macOS (Current)
- `clac-macos-osx-x64.zip` - Intel Macs (x64)
- `clac-macos-osx-arm64.zip` - Apple Silicon Macs (arm64)

#### Phase 2: Windows (Future)
- `clac-windows-win-x64.zip` - 64-bit Windows
- `clac-windows-win-x86.zip` - 32-bit Windows
- `clac-windows-win-arm64.zip` - ARM64 Windows (optional)

#### Phase 3: Linux (Future)
- `clac-linux-linux-x64.zip` - 64-bit Linux
- `clac-linux-linux-arm64.zip` - ARM64 Linux (optional)

### Binary Location
After extraction, the executable will be in the root of the ZIP file:
- **macOS/Linux**: `Clac.UI` (executable)
- **Windows**: `Clac.UI.exe`

## Considerations

### Build Time
- Estimated 5-8 minutes per architecture
- Both architectures build in parallel using matrix strategy
- Total workflow time: ~10-15 minutes

### Storage
- GitHub Actions artifacts stored for 90 days
- GitHub Releases provide permanent storage
- Release attachments are part of the repository

### Code Signing (Optional)
For distribution outside GitHub, consider:
- macOS code signing with Apple Developer certificate
- Notarization for macOS Gatekeeper compatibility
- Requires additional setup and certificates

### Version Management
- Use semantic versioning (e.g., `v1.0.0`)
- Extract version from tag in workflow if needed
- Consider using GitVersion or similar for automatic versioning

## Future Phases

### Phase 2: Windows Builds
Once macOS builds are working, add Windows support:
- Add Windows runners (`windows-latest`) to workflows
- Build for `win-x64`, `win-x86`, and optionally `win-arm64`
- Consider code signing for Windows (optional)
- Update release workflow to include Windows binaries
- Expected build time: ~5-8 minutes per architecture

### Phase 3: Linux Builds
After Windows is working, add Linux support:
- Add Linux runners (`ubuntu-latest`) to workflows
- Build for `linux-x64` and optionally `linux-arm64`
- Consider packaging formats (AppImage, DEB, RPM) for easier distribution
- Update release workflow to include Linux binaries
- Expected build time: ~5-8 minutes per architecture

### Optional Enhancements
- **Code Signing**: macOS and Windows code signing for trusted distribution
- **macOS Notarization**: Required for Gatekeeper compatibility
- **Linux Packages**: Create DEB/RPM packages or AppImage for easier installation
- **Automated Versioning**: Use GitVersion or similar for automatic version management
- **Multi-arch Support**: Add ARM64 builds for all platforms

## GitHub Branch Protection Setup

To enable this workflow strategy, configure branch protection for your main branch:

1. Go to GitHub repository → Settings → Branches
2. Add branch protection rule for `main` (or `master`)
3. Enable:
   - ✅ Require a pull request before merging
   - ✅ Require approvals (1 approval, or allow self-approval for solo maintainer)
   - ✅ Require status checks to pass before merging
   - ✅ Require branches to be up to date before merging
   - ✅ Do not allow force pushes
   - ✅ Do not allow deletions

This ensures:
- All changes go through PRs
- Tests must pass before merge
- Main branch is protected
- Works with fork-based contributions

## Testing the Workflow

1. **Test PR workflow:**
   - Create a feature branch
   - Open PR to main
   - Verify `build-pr.yml` runs and tests pass

2. **Test main branch workflow:**
   - Merge PR to main
   - Verify `build-main.yml` runs
   - Download artifacts and test locally

3. **Test release workflow:**
   - Push a tag: `git tag v0.1.0-test && git push origin v0.1.0-test`
   - Verify `release.yml` runs
   - Check GitHub Releases page for new release
   - Delete test release if desired

## Troubleshooting

### Build Fails
- Check .NET SDK version matches project requirements
- Verify `OutputType` is set to `Exe` in `Clac.UI.csproj`
- Check test failures in workflow logs

### Release Not Created
- Verify tag format matches trigger pattern (`v*`)
- Check `GITHUB_TOKEN` permissions
- Ensure workflow has permission to create releases

### Binary Not Executable
- macOS may require `chmod +x` on extracted binary
- Check file permissions in ZIP archive
- Verify `PublishSingleFile` is enabled

