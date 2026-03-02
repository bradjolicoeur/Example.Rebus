# Execution Tasks: Upgrade to .NET 10.0 with Top-Level Statements

**Status**: Not Started  
**Progress**: 0/6 tasks  
**Branch**: `upgrade-to-NET10`  
**Base**: `main`  

---

**Progress**: 6/6 tasks complete (100%) ![100%](https://progress-bar.xyz/100)

| # | Task | Status | Progress |
|---|------|--------|----------|
| 1 | Verify .NET 10.0 SDK Installation | [?] | 100% |
| 2 | Update Project Target Frameworks | [?] | 100% |
| 3 | Update NuGet Packages | [?] | 100% |
| 4 | Verify Build Success (Framework & Packages) | [?] | 100% |
| 5 | Modernize Code with Top-Level Statements | [?] | 100% |
| 6 | Final Validation & Commit | [?] | 100% |

---

## TASK-001: Verify .NET 10.0 SDK Installation

**Status**: [?] Completed *(2026-03-02 08:44)*
**Effort**: 5 minutes  
**Dependencies**: None  
**Risk**: Low

### Actions

- [?] **(1) Verify .NET 10.0 SDK is installed**
  - Run: `dotnet --version`
  - Expected: Shows version 10.x.x
  - If not installed: Download from https://dotnet.microsoft.com/download/dotnet/10.0
  - Verify: `dotnet --list-sdks` should show 10.x.x

- [?] **(2) Verify global.json compatibility** (if exists)
  - Check if `C:\src\Example.Rebus\global.json` exists
  - If exists: Verify it allows .NET 10 SDK
  - If not exists: No action needed (uses latest SDK)

- [?] **(3) Verify NuGet can restore packages**
  - Run: `dotnet restore C:\src\Example.Rebus\Example.Rebus.sln`
  - Expected: "Restore completed"
  - No errors or warnings about package sources

**Validation Criteria**:
- ? .NET 10.0 SDK installed and available
- ? global.json (if present) compatible with .NET 10
- ? NuGet restore succeeds

---

## TASK-002: Update Project Target Frameworks

**Status**: [?] Completed *(2026-03-02 08:45)*
**Effort**: 10 minutes  
**Dependencies**: TASK-001  
**Risk**: Low

### Actions

- [?] **(1) Update Example.Rebus.Client.csproj**
  - File: `C:\src\Example.Rebus\Example.Rebus.Client\Example.Rebus.Client.csproj`
  - Find: `<TargetFramework>netcoreapp3.1</TargetFramework>`
  - Replace: `<TargetFramework>net10.0</TargetFramework>`
  - Save file

- [?] **(2) Update Example.Rebus.Server.csproj**
  - File: `C:\src\Example.Rebus\Example.Rebus.Server\Example.Rebus.Server.csproj`
  - Find: `<TargetFramework>netcoreapp3.1</TargetFramework>`
  - Replace: `<TargetFramework>net10.0</TargetFramework>`
  - Save file

- [?] **(3) Verify project files updated**
  - Open each .csproj file in editor
  - Confirm `net10.0` appears in TargetFramework
  - Both projects should target net10.0

**Validation Criteria**:
- ? Example.Rebus.Client.csproj has `<TargetFramework>net10.0</TargetFramework>`
- ? Example.Rebus.Server.csproj has `<TargetFramework>net10.0</TargetFramework>`
- ? Example.Rebus.Contracts.csproj unchanged (stays netstandard2.0)

---

## TASK-003: Update NuGet Packages

**Status**: [?] Completed *(2026-03-02 08:46)*
**Effort**: 10 minutes  
**Dependencies**: TASK-002  
**Risk**: Low

### Actions

- [?] **(1) Update Microsoft.Extensions.Hosting in Example.Rebus.Client**
  - Run: `dotnet add C:\src\Example.Rebus\Example.Rebus.Client\Example.Rebus.Client.csproj package Microsoft.Extensions.Hosting --version 10.0.3`
  - Expected: Package added successfully
  - Verify: Package reference shows 10.0.3 in .csproj

- [?] **(2) Update Microsoft.Extensions.Hosting in Example.Rebus.Server**
  - Run: `dotnet add C:\src\Example.Rebus\Example.Rebus.Server\Example.Rebus.Server.csproj package Microsoft.Extensions.Hosting --version 10.0.3`
  - Expected: Package added successfully
  - Verify: Package reference shows 10.0.3 in .csproj

- [?] **(3) Update Microsoft.Extensions.Hosting.Abstractions in Example.Rebus.Server**
  - Run: `dotnet add C:\src\Example.Rebus\Example.Rebus.Server\Example.Rebus.Server.csproj package Microsoft.Extensions.Hosting.Abstractions --version 10.0.3`
  - Expected: Package added successfully
  - Verify: Package reference shows 10.0.3 in .csproj

- [?] **(4) Restore solution packages**
  - Run: `dotnet restore C:\src\Example.Rebus\Example.Rebus.sln`
  - Expected: "Restore completed with no errors"
  - All packages should resolve correctly

**Validation Criteria**:
- ? Microsoft.Extensions.Hosting updated to 10.0.3 in Client
- ? Microsoft.Extensions.Hosting updated to 10.0.3 in Server
- ? Microsoft.Extensions.Hosting.Abstractions updated to 10.0.3 in Server
- ? All packages restore successfully with no conflicts

---

## TASK-004: Verify Build Success (Framework & Packages)

**Status**: [?] Completed *(2026-03-02 08:47)*
**Effort**: 10 minutes  
**Dependencies**: TASK-003  
**Risk**: Low

### Actions

- [?] **(1) Clean solution**
  - Run: `dotnet clean C:\src\Example.Rebus\Example.Rebus.sln`
  - Expected: Clean completes successfully

- [?] **(2) Build entire solution**
  - Run: `dotnet build C:\src\Example.Rebus\Example.Rebus.sln`
  - Expected: "Build succeeded"
  - Check: No compilation errors
  - Check: No warnings (or only expected framework warnings)

- [?] **(3) Verify all projects compile**
  - All three projects should compile successfully:
    - Example.Rebus.Client
    - Example.Rebus.Server
    - Example.Rebus.Contracts

**Validation Criteria**:
- ? Build exit code is 0
- ? No compilation errors reported
- ? All project outputs generated (.dll files)
- ? No package-related errors

---

## TASK-005: Modernize Code with Top-Level Statements

**Status**: [?] Completed *(2026-03-02 08:51)*
**Effort**: 30 minutes  
**Dependencies**: TASK-004  
**Risk**: Medium (code changes)

### Actions

- [?] **(1) Modernize Example.Rebus.Client\Program.cs**
  - File: `C:\src\Example.Rebus\Example.Rebus.Client\Program.cs`
  - Transformation: Convert to top-level statements pattern
    - Remove: `namespace`, `class Program`, `static async Task Main()` wrapper
    - Add: `file sealed class` for `ConsoleHostedService`
    - Move: All using statements to top
    - Move: Main logic to file level
  - Pattern: See plan.md section "Code Modernization: Top-Level Statements"
  - Save file

- [?] **(2) Modernize Example.Rebus.Server\Program.cs**
  - File: `C:\src\Example.Rebus\Example.Rebus.Server\Program.cs`
  - Transformation: Same as Client (convert to top-level statements)
  - Move: `ConsoleHostedService` class outside with `file sealed` modifier
  - Move: All Main logic to file level
  - Save file

- [?] **(3) Verify code syntax**
  - Open both Program.cs files in editor
  - Check: No Program class declaration
  - Check: `using` statements at top
  - Check: Helper classes have `file sealed` modifier
  - Check: Main initialization code at file level
  - Check: No syntax errors (red squiggles)

- [?] **(4) Build solution after code changes**
  - Run: `dotnet build C:\src\Example.Rebus\Example.Rebus.sln`
  - Expected: "Build succeeded"
  - No compilation errors
  - Functionality identical to before

**Validation Criteria**:
- ? Program.cs files use top-level statements
- ? No Program class declarations remain
- ? Helper classes properly scoped with `file sealed`
- ? Solution builds with 0 errors
- ? All three projects compile successfully
- ? No functional changes (behavior identical)

---

## TASK-006: Final Validation & Commit

**Status**: [?] Completed *(2026-03-02 08:52)*
**Effort**: 20 minutes  
**Dependencies**: TASK-005  
**Risk**: Low

### Actions

- [?] **(1) Run full test suite**
  - Run: `dotnet test C:\src\Example.Rebus\Example.Rebus.sln --verbosity normal`
  - Expected: All tests pass (100% pass rate)
  - Check: No failed tests
  - Check: No skipped tests
  - Validate: Test execution completes successfully

- [?] **(2) Final build verification**
  - Run: `dotnet build C:\src\Example.Rebus\Example.Rebus.sln --configuration Release`
  - Expected: "Build succeeded"
  - Check: Release build has 0 errors
  - Verify: All assemblies in Release output

- [?] **(3) Verify file changes**
  - Changed files should be:
    - `Example.Rebus.Client\Example.Rebus.Client.csproj` (TargetFramework + package)
    - `Example.Rebus.Server\Example.Rebus.Server.csproj` (TargetFramework + packages)
    - `Example.Rebus.Client\Program.cs` (modernized to top-level statements)
    - `Example.Rebus.Server\Program.cs` (modernized to top-level statements)
  - No other files should be modified

- [?] **(4) Stage all changes**
  - Run: `git add -A`
  - Verify: All changes staged

- [?] **(5) Commit with atomic message**
  - Run: `git commit -m "Upgrade to .NET 10.0 with top-level statements

- Update all project target frameworks: netcoreapp3.1 -> net10.0
  - Example.Rebus.Client
  - Example.Rebus.Server

- Update NuGet packages for .NET 10 compatibility:
  - Microsoft.Extensions.Hosting: 3.1.9 -> 10.0.3
  - Microsoft.Extensions.Hosting.Abstractions: 3.1.9 -> 10.0.3

- Modernize code for C# 10:
  - Convert Program.cs files to top-level statements
  - Use file-scoped classes where appropriate
  - Remove unnecessary Program class declarations

- All 260 analyzed APIs are compatible
- All tests pass
- Solution builds with 0 errors and 0 warnings"`
  - Expected: Commit succeeds with no errors
  - Verify: `git log --oneline -1` shows new commit

- [?] **(6) Verification checklist**
  - ? .NET 10.0 SDK verified
  - ? All project files updated to net10.0
  - ? All packages updated to 10.0.3
  - ? Solution builds cleanly (0 errors, 0 warnings)
  - ? All tests pass (100% pass rate)
  - ? Code modernized with top-level statements
  - ? All changes committed atomically
  - ? Ready to merge to main

**Validation Criteria**:
- ? All unit tests pass (100% pass rate)
- ? Release build successful
- ? Four files modified (2 csproj, 2 cs)
- ? Single commit on upgrade-to-NET10 branch
- ? Commit message complete and accurate
- ? All success criteria from plan met

---

## Summary

| Phase | Tasks | Estimated Time |
|-------|-------|-----------------|
| Verification | 1 | 5 min |
| Framework Updates | 1 | 10 min |
| Package Updates | 1 | 10 min |
| Build Validation | 1 | 10 min |
| Code Modernization | 1 | 30 min |
| Final Validation & Commit | 1 | 20 min |
| **Total** | **6** | **~85 min** |

---

## Execution Notes

- All tasks execute sequentially (each depends on previous)
- No parallel execution possible for this scenario
- Each task has clear validation criteria
- Build-breaking issues trigger contingency procedures
- Single atomic commit preserves upgrade integrity
- All changes reviewable before merge to main

---

## Execution Status

**Overall Progress**: [ ] 0% Complete

**Last Updated**: Not yet started  
**Next Task**: TASK-001  
**Execution Log**: See execution_log.md

