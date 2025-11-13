# CI/CD Pipeline Diagram

This document provides a visual overview of the entire CI/CD process for the Clac project.


## Workflow Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                    CI/CD Pipeline Architecture                   │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                    Reusable Workflow                             │
│                                                                   │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  pre-build.yml (Reusable)                               │   │
│  │  ┌───────────────────────────────────────────────────┐  │   │
│  │  │  pre-build job                                    │  │   │
│  │  │  1. Checkout code                                  │  │   │
│  │  │  2. Setup .NET SDK                                 │  │   │
│  │  │  3. Restore dependencies                           │  │   │
│  │  │  4. Run linter (dotnet format)                     │  │   │
│  │  │  5. Run tests                                      │  │   │
│  │  └───────────────────────────────────────────────────┘  │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

## Workflow 1: PR Validation (`build-pr.yml`)

**Trigger:** Pull Request opened/updated to `main`

```
┌─────────────────────────────────────────────────────────────┐
│  Pull Request Created/Updated                                │
└─────────────────────────────────────────────────────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  build-pr.yml workflow         │
        └───────────────────────────────┘
                        │
        ┌───────────────┴───────────────┐
        │                               │
        ▼                               ▼
┌───────────────┐              ┌───────────────┐
│  pre-build    │              │    build      │
│  (reusable)   │──────────────▶│   (waits)    │
│               │  depends on  │               │
└───────────────┘              └───────────────┘
        │                               │
        │  ✅ Lint passes               │  ✅ Build succeeds
        │  ✅ Tests pass                │
        │                               │
        └───────────────┬───────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  PR Status: ✅ Ready to Merge  │
        └───────────────────────────────┘
```

**Jobs:**
- `pre-build`: Calls `pre-build.yml` (linting + testing)
- `build`: Validation build (depends on `pre-build`)

**Purpose:** Validate PRs before merge

---

## Workflow 2: Main Branch CI (`build-main.yml`)

**Trigger:** Push to `main` branch (after PR merge)

```
┌─────────────────────────────────────────────────────────────┐
│  Code Merged to main                                         │
└─────────────────────────────────────────────────────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  build-main.yml workflow       │
        └───────────────────────────────┘
                        │
        ┌───────────────┴───────────────┐
        │                               │
        ▼                               ▼
┌───────────────┐              ┌───────────────────┐
│  pre-build    │              │  build-and-upload  │
│  (reusable)   │──────────────▶│   (waits)         │
│               │  depends on  │                    │
└───────────────┘              └───────────────────┘
        │                               │
        │  ✅ Lint passes               │  ┌─────────────────┐
        │  ✅ Tests pass                │  │ Matrix Strategy │
        │                               │  │                 │
        │                               │  │ ┌─────────────┐ │
        │                               │  │ │ osx-x64     │ │
        │                               │  │ │ (parallel)  │ │
        │                               │  │ └─────────────┘ │
        │                               │  │                 │
        │                               │  │ ┌─────────────┐ │
        │                               │  │ │ osx-arm64   │ │
        │                               │  │ │ (parallel)  │ │
        │                               │  │ └─────────────┘ │
        │                               │  └─────────────────┘
        │                               │         │
        │                               │         ▼
        │                               │  ┌─────────────────┐
        │                               │  │ Build binaries  │
        │                               │  │ Create ZIPs     │
        │                               │  │ Upload artifacts│
        │                               │  └─────────────────┘
        │                               │
        └───────────────┬───────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  Artifacts Available:          │
        │  • clac-macos-osx-x64.zip      │
        │  • clac-macos-osx-arm64.zip    │
        │  (Available for 90 days)      │
        └───────────────────────────────┘
```

**Jobs:**
- `pre-build`: Calls `pre-build.yml` (linting + testing)
- `build-and-upload`: Builds binaries for both architectures (depends on `pre-build`)

**Purpose:** Build and upload artifacts after merge to main

---

## Workflow 3: Release (`release.yml`)

**Trigger:** Tag push (`v*`) or manual dispatch

```
┌─────────────────────────────────────────────────────────────┐
│  Tag Pushed: v1.0.0                                          │
│  OR Manual Workflow Dispatch                                 │
└─────────────────────────────────────────────────────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  release.yml workflow          │
        └───────────────────────────────┘
                        │
        ┌───────────────┴───────────────┐
        │                               │
        ▼                               ▼
┌───────────────┐              ┌───────────────────┐
│  pre-build    │              │     release       │
│  (reusable)   │──────────────▶│   (waits)         │
│               │  depends on  │                    │
└───────────────┘              └───────────────────┘
        │                               │
        │  ✅ Lint passes               │  ┌─────────────────┐
        │  ✅ Tests pass                │  │ Matrix Strategy │
        │                               │  │                 │
        │                               │  │ ┌─────────────┐ │
        │                               │  │ │ osx-x64     │ │
        │                               │  │ │ (parallel)  │ │
        │                               │  │ └─────────────┘ │
        │                               │  │                 │
        │                               │  │ ┌─────────────┐ │
        │                               │  │ │ osx-arm64   │ │
        │                               │  │ │ (parallel)  │ │
        │                               │  │ └─────────────┘ │
        │                               │  └─────────────────┘
        │                               │         │
        │                               │         ▼
        │                               │  ┌─────────────────┐
        │                               │  │ Build binaries  │
        │                               │  │ Create ZIPs     │
        │                               │  │ Upload artifacts│
        │                               │  └─────────────────┘
        │                               │
        └───────────────┬───────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  create-release job            │
        │  (depends on pre-build +       │
        │   release)                     │
        └───────────────────────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  1. Download artifacts         │
        │  2. Create GitHub Release      │
        │  3. Attach binaries to release │
        └───────────────────────────────┘
                        │
                        ▼
        ┌───────────────────────────────┐
        │  ✅ GitHub Release Created     │
        │  • v1.0.0                      │
        │  • clac-macos-osx-x64.zip      │
        │  • clac-macos-osx-arm64.zip    │
        └───────────────────────────────┘
```

**Jobs:**
- `pre-build`: Calls `pre-build.yml` (linting + testing)
- `release`: Builds binaries for both architectures (depends on `pre-build`)
- `create-release`: Creates GitHub release with binaries (depends on `pre-build` + `release`)

**Purpose:** Create GitHub releases with attached binaries

---

## Complete CI/CD Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    Complete CI/CD Flow                          │
└─────────────────────────────────────────────────────────────────┘

Developer Workflow:
───────────────────

1. Create Feature Branch
   │
   ├─▶ Push to GitHub
   │
   ├─▶ Open Pull Request to main
   │
   └─▶ ┌─────────────────────────────────────┐
       │  build-pr.yml triggers               │
       │  ┌─────────────┐                     │
       │  │ pre-build   │──▶ Lint + Test     │
       │  └─────────────┘                     │
       │         │                             │
       │         ▼                             │
       │  ┌─────────────┐                     │
       │  │ build       │──▶ Validation Build │
       │  └─────────────┘                     │
       │                                       │
       └─▶ ✅ PR Status: Ready to Merge
           │
           ▼
2. Merge PR to main
   │
   └─▶ ┌─────────────────────────────────────┐
       │  build-main.yml triggers             │
       │  ┌─────────────┐                     │
       │  │ pre-build   │──▶ Lint + Test     │
       │  └─────────────┘                     │
       │         │                             │
       │         ▼                             │
       │  ┌───────────────────┐               │
       │  │ build-and-upload  │──▶ Build     │
       │  │ (matrix: 2x)       │    Binaries  │
       │  └───────────────────┘               │
       │                                       │
       └─▶ ✅ Artifacts Available for Testing
           │
           ▼
3. Create Release Tag
   │
   ├─▶ git tag v1.0.0
   │
   ├─▶ git push origin v1.0.0
   │
   └─▶ ┌─────────────────────────────────────┐
       │  release.yml triggers                │
       │  ┌─────────────┐                     │
       │  │ pre-build   │──▶ Lint + Test     │
       │  └─────────────┘                     │
       │         │                             │
       │         ▼                             │
       │  ┌─────────────┐                     │
       │  │ release      │──▶ Build Binaries   │
       │  │ (matrix: 2x) │    (2 architectures)│
       │  └─────────────┘                     │
       │         │                             │
       │         ▼                             │
       │  ┌───────────────────┐               │
       │  │ create-release    │──▶ GitHub    │
       │  │                   │    Release   │
       │  └───────────────────┘               │
       │                                       │
       └─▶ ✅ GitHub Release Created with Binaries
```

## Workflow Comparison

| Feature | PR Workflow | Main Workflow | Release Workflow |
|---------|-------------|---------------|------------------|
| **Trigger** | Pull Request | Push to main | Tag push / Manual |
| **Pre-Build** | ✅ Yes | ✅ Yes | ✅ Yes |
| **Linting** | ✅ Yes | ✅ Yes | ✅ Yes |
| **Testing** | ✅ Yes | ✅ Yes | ✅ Yes |
| **Build** | Validation only | Full publish | Full publish |
| **Matrix** | No | Yes (2 arch) | Yes (2 arch) |
| **Artifacts** | None | Uploaded | Uploaded + Release |
| **Release** | No | No | Yes |

## Key Principles

1. **DRY (Don't Repeat Yourself)**: All workflows use `pre-build.yml` for validation
2. **Consistency**: Same linting and testing across all workflows
3. **Quality Gates**: Tests must pass before builds
4. **Parallel Execution**: Matrix builds run in parallel for speed
5. **Dependency Management**: Jobs depend on each other in correct order
6. **Separation of Concerns**: Each workflow has a specific purpose

## Workflow Files

- `.github/workflows/pre-build.yml` - Reusable validation workflow
- `.github/workflows/build-pr.yml` - PR validation
- `.github/workflows/build-main.yml` - Main branch CI
- `.github/workflows/release.yml` - Release creation

