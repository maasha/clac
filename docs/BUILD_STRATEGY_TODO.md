# TODO - Cross-Platform CI/CD Implementation

This document outlines the step-by-step tasks required to implement the cross-platform build and release pipeline as described in `BUILD_STRATEGY.md`.

The implementation is organized into milestones:
- **Milestone 1: macOS Pipeline** (Current focus)
- **Milestone 2: Windows Pipeline** (Future)
- **Milestone 3: Linux Pipeline** (Future)

## Milestone 1: macOS Pipeline

Goal: Establish a working CI/CD pipeline that builds and releases macOS binaries.

### Prerequisites ✅ COMPLETED

#### 1. Update Project Configuration ✅ COMPLETED
**File:** `src/Clac.UI/Clac.UI.csproj`

**What:** Change `OutputType` from `WinExe` to `Exe`

**Why:** 
- `WinExe` is Windows-specific and prevents the project from building on macOS
- `Exe` works on all platforms (Windows, macOS, Linux)
- This change is required for GitHub Actions macOS runners to successfully build the project

**Action:**
```xml
<PropertyGroup>
  <OutputType>Exe</OutputType>  <!-- Changed from WinExe -->
  <!-- ... rest of properties ... -->
</PropertyGroup>
```

**Status:** ✅ Completed - Changed `OutputType` from `WinExe` to `Exe` in `Clac.UI.csproj`

---

#### 2. Create GitHub Actions Directory Structure ✅ COMPLETED
**File:** `.github/workflows/` (new directory)

**What:** Create the `.github/workflows/` directory in the project root

**Why:**
- GitHub Actions automatically looks for workflow files in this directory
- This is the standard location for all CI/CD workflows
- Without this directory, GitHub won't recognize workflow files

**Action:**
```bash
mkdir -p .github/workflows
```

**Status:** ✅ Completed - Created `.github/workflows/` directory structure

---

### Milestone 1 Tasks

#### 3. Determine and Configure Branch Strategy ✅ COMPLETED
**What:** Decide on branch naming and set up branch protection

**Why:**
- Need to determine if using `main` or `master` as the default branch
- Branch protection is required for open source projects
- Ensures all changes go through PRs and tests pass before merge
- Enables fork-based contributions

**Action:**
1. ✅ Check current default branch name: Was `master`
2. ✅ Rename branch to `main`:
   ```bash
   git branch -m master main
   git push origin -u main
   ```
3. ✅ Delete `master` branch from GitHub:
   ```bash
   git push origin --delete master
   ```
4. ⚠️ **Remaining Step** (Manual in GitHub UI):
   - Go to GitHub repository → Settings → Branches
   - Add branch protection rule for `main`
   - Enable: Require PR, Require status checks, Require approvals (allow self-approval for solo maintainer)

**Status:** ✅ Branch renamed to `main`, `master` deleted from GitHub, default branch is now `main`. ⚠️ **Action Required**: Configure branch protection in GitHub Settings → Branches.

---

#### 4. Create PR Validation Workflow ✅ COMPLETED
**File:** `.github/workflows/build-pr.yml`

**What:** Create workflow that validates PRs before merge

**Why:**
- Ensures all PRs (including from forks) are tested before merge
- Provides feedback to contributors
- Works with fork-based open source contributions
- Required for branch protection status checks

**Action:**
- ✅ Created `build-pr.yml` based on Workflow 1 example in `BUILD_STRATEGY.md`
- ✅ Configured to trigger on pull requests to `main`
- ✅ Includes build and test steps

**Status:** ✅ Completed - PR validation workflow created and ready to use

---

#### 5. Create Main Branch CI Workflow ✅ COMPLETED
**File:** `.github/workflows/build-main.yml`

**What:** Create workflow that builds and uploads artifacts when code is merged to main

**Why:**
- Validates merged code on main branch
- Creates build artifacts for testing
- Ensures main branch always has working builds
- Artifacts available for 90 days for manual testing

**Action:**
- ✅ Created `build-main.yml` based on Workflow 2 example in `BUILD_STRATEGY.md`
- ✅ Configured to trigger on push to `main`
- ✅ Includes build, test, and artifact upload steps
- ✅ Builds for both macOS architectures (osx-x64 and osx-arm64)

**Status:** ✅ Completed - Main branch CI workflow created and ready to use

---

#### 6. Create Release Workflow ✅ COMPLETED
**File:** `.github/workflows/release.yml`

**What:** Create workflow that creates GitHub releases with binaries

**Why:**
- Controlled release process (manual via tags)
- Creates GitHub releases with attached binaries
- Supports frequent releases
- Separate from main branch builds

**Action:**
- ✅ Created `release.yml` based on Workflow 3 example in `BUILD_STRATEGY.md`
- ✅ Configured to trigger on tag push (`v*`) and manual dispatch
- ✅ Includes build, test, and release creation steps
- ✅ Builds for both macOS architectures and creates GitHub release with binaries

**Status:** ✅ Completed - Release workflow created and ready to use

---

#### 7. Test Build Commands Locally (Optional but Recommended)
**What:** Verify the build commands work on your local macOS machine

**Why:**
- Catch configuration issues before pushing to GitHub
- Understand what the workflow will do
- Faster iteration than waiting for GitHub Actions

**Action:**
```bash
# Test restore
dotnet restore

# Test build
dotnet build -c Release

# Test publish for one architecture
dotnet publish src/Clac.UI/Clac.UI.csproj \
  -c Release \
  -r osx-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -o publish/test-osx-x64
```

---

#### 8. Commit and Push Changes
**What:** Commit the project configuration change and workflow file, then push to GitHub

**Why:**
- GitHub Actions only runs on code that's in the repository
- Pushing triggers the workflow automatically (if configured for push events)
- This validates that the workflow file is correctly formatted and recognized

**Action:**
```bash
git add src/Clac.UI/Clac.UI.csproj
git add .github/workflows/build-pr.yml
git add .github/workflows/build-main.yml
git add .github/workflows/release.yml
git commit -m "Add macOS CI/CD pipeline workflows"
git push origin main  # or master, depending on your default branch
```

---

#### 9. Verify Workflow Execution
**What:** Check the GitHub Actions tab to ensure the workflow runs successfully

**Why:**
- Confirms the workflow is properly configured
- Identifies any issues with the build process
- Validates that tests pass and binaries are created

**Action:**
1. Go to GitHub repository
2. Click on "Actions" tab
3. Test PR workflow:
   - Create a test PR or check existing PR
   - Verify "Build and Test PR" workflow runs
   - Check that tests pass
4. Test main branch workflow:
   - Merge a PR to main (or push directly if protection not yet enabled)
   - Verify "Build Main Branch" workflow runs
   - Check that artifacts are uploaded
5. Verify all steps complete successfully (green checkmarks)

---

#### 10. Download and Test Artifacts
**What:** Download the built macOS binaries from GitHub Actions artifacts and test them locally

**Why:**
- Ensures the binaries actually work on macOS
- Validates that the build process produces usable executables
- Catches runtime issues that might not appear during build

**Action:**
1. In GitHub Actions, find the completed workflow run
2. Scroll to "Artifacts" section
3. Download `clac-macos-osx-x64` and/or `clac-macos-osx-arm64`
4. Extract the ZIP file
5. Run the executable on macOS
6. Verify the calculator application launches and functions correctly

---

#### 11. Test Release Creation
**What:** Create a test release by pushing a version tag

**Why:**
- Validates that the release creation step works correctly
- Ensures binaries are properly attached to releases
- Tests the complete end-to-end release process

**Action:**
```bash
# Create a test tag
git tag v0.1.0-test
git push origin v0.1.0-test

# Check GitHub Releases page for the new release
# Verify both macOS binaries are attached
# Delete the test release if desired
```

---

#### 12. Update README with Download Instructions
**File:** `README.md`

**What:** Add a section explaining how users can download macOS builds

**Why:**
- Makes it easy for users to find and download the application
- Documents the release process for end users
- Improves project accessibility

**Action:**
Add a section like:
```markdown
## Downloads

macOS binaries are available in the [Releases](https://github.com/yourusername/clac/releases) section.

- **Intel Macs (x64):** Download `clac-macos-osx-x64.zip`
- **Apple Silicon Macs (arm64):** Download `clac-macos-osx-arm64.zip`

Extract the ZIP file and run the executable.
```

---

#### 13. Clean Up Test Releases (Optional)
**What:** Remove any test releases created during testing

**Why:**
- Keeps the releases page clean
- Avoids confusion with test versions
- Only keep actual release versions

**Action:**
1. Go to GitHub Releases page
2. Find test releases (e.g., `v0.1.0-test`)
3. Click "Edit" → "Delete release" → "Delete this release"

---

### Milestone 1 Summary Checklist

**Prerequisites:**
- [x] Update `Clac.UI.csproj` OutputType to `Exe`
- [x] Create `.github/workflows/` directory

**Milestone 1 Tasks:**
- [x] Determine and configure branch strategy (renamed to main, master deleted, branch protection pending GitHub UI setup)
- [x] Create `build-pr.yml` workflow file (PR validation)
- [x] Create `build-main.yml` workflow file (main branch CI)
- [x] Create `release.yml` workflow file (release creation)
- [ ] Test build commands locally (optional)
- [ ] Commit and push changes
- [ ] Verify PR workflow runs successfully
- [ ] Verify main branch workflow runs successfully
- [ ] Download and test macOS binaries from artifacts
- [ ] Test release creation with a tag
- [ ] Update README with download instructions
- [ ] Clean up test releases (optional)

**Milestone 1 Success Criteria:**
- ✅ All workflows run successfully
- ✅ macOS binaries build for both x64 and arm64
- ✅ Artifacts are uploaded and downloadable
- ✅ Releases are created with attached binaries
- ✅ README documents download process

---

## Milestone 2: Windows Pipeline (Future)

Goal: Add Windows build support to the existing CI/CD pipeline.

### Planned Tasks
- [ ] Add Windows runners to `build-pr.yml` workflow
- [ ] Add Windows builds to `build-main.yml` workflow (win-x64, win-x86)
- [ ] Update `release.yml` workflow to include Windows binaries
- [ ] Test Windows builds locally (optional)
- [ ] Verify Windows binaries work on Windows systems
- [ ] Update README with Windows download instructions
- [ ] Consider Windows code signing (optional)

**Target Runtimes:**
- `win-x64` - 64-bit Windows
- `win-x86` - 32-bit Windows
- `win-arm64` - ARM64 Windows (optional)

---

## Milestone 3: Linux Pipeline (Future)

Goal: Add Linux build support to the existing CI/CD pipeline.

### Planned Tasks
- [ ] Add Linux runners to `build-pr.yml` workflow
- [ ] Add Linux builds to `build-main.yml` workflow (linux-x64, linux-arm64)
- [ ] Update `release.yml` workflow to include Linux binaries
- [ ] Test Linux builds locally (optional)
- [ ] Verify Linux binaries work on Linux systems
- [ ] Consider Linux packaging formats (AppImage, DEB, RPM)
- [ ] Update README with Linux download instructions

**Target Runtimes:**
- `linux-x64` - 64-bit Linux
- `linux-arm64` - ARM64 Linux (optional)

---

## Optional Enhancements (All Phases)

- Code signing for macOS and Windows
- macOS notarization for Gatekeeper compatibility
- Linux package formats (AppImage, DEB, RPM)
- Automated versioning with GitVersion
- Multi-arch support (ARM64 for all platforms)

