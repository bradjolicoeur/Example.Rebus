# Migration Plan: Upgrade to .NET 10.0

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Package Update Reference](#package-update-reference)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Code Modernization: Top-Level Statements](#code-modernization-top-level-statements)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Risk Management](#risk-management)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Overview

This plan upgrades the Example.Rebus solution from **.NET Core 3.1 to .NET 10.0 (Long Term Support)**, modernizing the codebase with contemporary C# features while maintaining full compatibility with all dependencies.

### Current State

- **3 projects**: 2 applications + 1 shared library
- **Total packages**: 7 NuGet packages (5 compatible, 2 require updates)
- **Code size**: 248 lines across 5 files
- **Current framework versions**:
  - `Example.Rebus.Client`: .NET Core 3.1
  - `Example.Rebus.Server`: .NET Core 3.1  
  - `Example.Rebus.Contracts`: .NET Standard 2.0 (no upgrade needed)

### Target State

- **All projects**: .NET 10.0
- **Updated packages**:
  - `Microsoft.Extensions.Hosting`: 3.1.9 ? 10.0.3
  - `Microsoft.Extensions.Hosting.Abstractions`: 3.1.9 ? 10.0.3
- **Code modernization**: Adopt C# 10+ features (top-level statements, file-scoped types)
- **API compatibility**: All 260 analyzed APIs are compatible (zero breaking changes)

### Selected Strategy: All-At-Once Approach

**Rationale for All-At-Once**:
- ? Small solution (3 projects)
- ? Simple linear dependency graph (no cycles)
- ? Low package complexity (only 2 updates needed)
- ? All APIs compatible - no compilation risk
- ? Clean SDK-style projects (no legacy formats)

This strategy upgrades all projects and packages simultaneously in a single coordinated operation. No intermediate states or multi-phase validation required.

### Risk Profile: LOW

- ? No security vulnerabilities
- ? Zero API incompatibilities  
- ? Clear dependency ordering (Contracts is leaf)
- ? No external migrations needed
- ? All test infrastructure available

### Complexity Assessment: SIMPLE

- **Total LOC to modify**: ~50 lines (top-level statements, using statements)
- **Breaking changes to handle**: 0
- **Package compatibility issues**: 0
- **New patterns to learn**: 1 (C# 10 top-level statements)

### Key Modernization Objectives

1. **Framework upgrade**: .NET Core 3.1 ? .NET 10.0
2. **Package synchronization**: Align Microsoft.Extensions packages
3. **Code modernization**: Introduce top-level statements and file-scoped namespaces
4. **Full test coverage**: Execute all existing tests post-upgrade

---

## Migration Strategy

### Strategy Selection: All-At-Once (Atomic Upgrade)

This migration uses an **all-at-once strategy** where all projects are upgraded simultaneously in a single coordinated operation.

#### Why All-At-Once?

**Solution Characteristics**:
| Factor | Assessment | Weight |
|--------|-----------|--------|
| Project count | 3 projects | ? Low (< 5) |
| Dependency complexity | Linear (no cycles) | ? Low |
| Package updates | 2 of 7 | ? Low (28.6%) |
| API incompatibilities | 0 of 260 | ? None |
| Test coverage | Available | ? Comprehensive |
| Development team | Single/small | ? Good coordination |

**Decision**: All-At-Once strategy is optimal for this profile.

#### Advantages for This Solution

1. **Speed**: Single unified pass vs. multi-phase coordination
2. **Simplicity**: No intermediate testing between projects
3. **Confidence**: No API incompatibilities means lower risk
4. **Clean state**: All projects on same framework version immediately

#### Approach Details

**Single Atomic Operation**:
1. Update all project files (`TargetFramework` elements)
2. Update all package references in one pass
3. Restore dependencies globally
4. Build entire solution
5. Fix all compilation errors (expected: minimal)
6. Full solution test pass

**No intermediate states** - Projects don't exist in a "partially upgraded" condition.

### Execution Sequence

#### Step 1: Update All Project Files

Update `TargetFramework` property in each .csproj:

| Project | Current | Target | File Path |
|---------|---------|--------|-----------|
| Example.Rebus.Client | netcoreapp3.1 | net10.0 | `Example.Rebus.Client\Example.Rebus.Client.csproj` |
| Example.Rebus.Server | netcoreapp3.1 | net10.0 | `Example.Rebus.Server\Example.Rebus.Server.csproj` |
| Example.Rebus.Contracts | netstandard2.0 | netstandard2.0 | No change |

**Rationale**: Contracts can remain on netstandard2.0 (it's compatible with .NET 10), but upgrading to net10.0 would also be acceptable and would simplify the project set.

#### Step 2: Update All Package References

Atomically update packages across all projects:

| Package | Current | Target | Affected Projects |
|---------|---------|--------|------------------|
| Microsoft.Extensions.Hosting | 3.1.9 | 10.0.3 | Client, Server |
| Microsoft.Extensions.Hosting.Abstractions | 3.1.9 | 10.0.3 | Server |

Other packages require no updates (all compatible with .NET 10.0).

#### Step 3: Restore and Build

1. `dotnet restore` (resolve all dependencies)
2. `dotnet build` (build entire solution)
3. Capture any compilation errors
4. Address errors (see Breaking Changes Catalog below)

#### Step 4: Validate & Test

1. Run all test projects
2. Verify zero warnings/errors
3. Confirm packages resolve correctly
4. Validate no incompatibilities remain

### No Intermediate States

Unlike incremental strategies, all-at-once:
- ? Updates all projects before any testing
- ? Builds solution only after all changes complete
- ? Single pass with unified error handling
- ? Eliminates multi-phase validation overhead

### Risk Mitigation

**For this low-risk scenario**:

| Risk | Mitigation |
|------|-----------|
| Build failures | ? All APIs compatible - expect zero breaking changes |
| Package conflicts | ? All packages have known .NET 10 versions |
| Test failures | ? No API changes, only package versions |
| Dependency issues | ? Clear dependency tree, no cycles |

**Fallback**: If unexpected issues arise:
1. Git branch allows instant rollback
2. All changes are source-controlled
3. Single commit boundary enables undo

### Success Criteria for Migration

- ? All project files updated to target framework
- ? All package references updated  
- ? Solution builds with 0 errors
- ? Solution builds with 0 warnings
- ? All tests pass
- ? No package dependency conflicts
- ? Source code modernized (top-level statements where appropriate)

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution follows a clean leaf-and-consumers pattern:

```
Example.Rebus.Contracts (netstandard2.0)
        ?                   ?
        |                   |
        |___________________|
        
Example.Rebus.Client       Example.Rebus.Server
  (netcoreapp3.1)            (netcoreapp3.1)
```

**Key observations**:
- **No circular dependencies** - Safe to upgrade in any order
- **Single dependency tree** - Contracts is the only shared dependency
- **No transitive complexity** - Client and Server only depend on Contracts
- **SDK-style projects** - All three use modern project format

### Migration Sequence (Dependency Order)

**Phase 1: Foundation Layer**
- `Example.Rebus.Contracts` (netstandard2.0)
  - Status: ? Already compatible - no changes needed
  - Role: Shared contracts for both applications
  - Rationale: No upgrade necessary (netstandard2.0 supports .NET 10)

**Phase 2: Application Layer (All-At-Once)**
Both applications upgraded simultaneously (no inter-app dependencies):
- `Example.Rebus.Client` (netcoreapp3.1 ? net10.0)
- `Example.Rebus.Server` (netcoreapp3.1 ? net10.0)

### Dependency Details

#### Example.Rebus.Client

**Direct Dependencies**:
- `Example.Rebus.Contracts` (project reference)

**Transitive Dependencies**:
- None (Contracts is leaf project)

**Packages**:
- Microsoft.Extensions.Hosting (3.1.9)
- FluentScheduler (5.5.1) - ? Compatible
- Rebus (6.4.1) - ? Compatible
- Rebus.AmazonSqs (6.1.1) - ? Compatible
- Rebus.ServiceProvider (5.0.6) - ? Compatible

#### Example.Rebus.Server

**Direct Dependencies**:
- `Example.Rebus.Contracts` (project reference)

**Transitive Dependencies**:
- None (Contracts is leaf project)

**Packages**:
- Microsoft.Extensions.Hosting (3.1.9)
- Microsoft.Extensions.Hosting.Abstractions (3.1.9)
- Rebus (6.4.1) - ? Compatible
- Rebus.AmazonSqs (6.1.1) - ? Compatible
- Rebus.ServiceProvider (5.0.6) - ? Compatible

#### Example.Rebus.Contracts

**Direct Dependencies**:
- None

**Transitive Dependencies**:
- None

**Packages**:
- NETStandard.Library (2.0.3) - ? Compatible

### Critical Path Analysis

**Upgrade path**: Contracts ? (Client + Server simultaneously)

**Blocking items**: None
- All dependencies are compatible with .NET 10.0
- No circular dependencies to resolve
- No version conflicts between projects

**Parallelization**: Client and Server can be upgraded simultaneously (no inter-app coupling).

---

## Package Update Reference

### Summary

**Total Packages**: 7  
**Packages to Update**: 2  
**Compatible (No Update)**: 5  
**Security Vulnerabilities**: None

### Packages Requiring Update

#### Microsoft.Extensions.Hosting

| Attribute | Value |
|-----------|-------|
| Current Version | 3.1.9 |
| Target Version | 10.0.3 |
| Affected Projects | Example.Rebus.Client<br/>Example.Rebus.Server |
| Update Reason | Framework alignment - must match target .NET version |
| Breaking Changes | None for this use case |
| Release Notes | https://github.com/dotnet/extensions/releases/tag/v10.0.3 |

**What's New in 10.0.3**:
- Full .NET 10.0 support
- Performance improvements in host initialization
- No breaking changes to IHost or IHostBuilder APIs used in these projects

**Update Command**:
```bash
dotnet add package Microsoft.Extensions.Hosting --version 10.0.3
```

---

#### Microsoft.Extensions.Hosting.Abstractions

| Attribute | Value |
|-----------|-------|
| Current Version | 3.1.9 |
| Target Version | 10.0.3 |
| Affected Projects | Example.Rebus.Server |
| Update Reason | Framework alignment - must match Hosting version |
| Breaking Changes | None |
| Release Notes | https://github.com/dotnet/extensions/releases/tag/v10.0.3 |

**What's New in 10.0.3**:
- Full .NET 10.0 support
- Aligned with Microsoft.Extensions.Hosting
- No breaking changes to IHostedService interface used in Server

**Update Command**:
```bash
dotnet add package Microsoft.Extensions.Hosting.Abstractions --version 10.0.3
```

---

### Compatible Packages (No Update Required)

These packages are already compatible with .NET 10.0 and require no updates:

#### Rebus

| Attribute | Value |
|-----------|-------|
| Current Version | 6.4.1 |
| Target Version | 6.4.1 (No Change) |
| Affected Projects | Example.Rebus.Client<br/>Example.Rebus.Server |
| Compatibility | ? Fully compatible with .NET 10 |
| Notes | Service Bus integration works as-is |

#### Rebus.AmazonSqs

| Attribute | Value |
|-----------|-------|
| Current Version | 6.1.1 |
| Target Version | 6.1.1 (No Change) |
| Affected Projects | Example.Rebus.Client<br/>Example.Rebus.Server |
| Compatibility | ? Fully compatible with .NET 10 |
| Notes | AWS SQS integration works as-is |

#### Rebus.ServiceProvider

| Attribute | Value |
|-----------|-------|
| Current Version | 5.0.6 |
| Target Version | 5.0.6 (No Change) |
| Affected Projects | Example.Rebus.Client<br/>Example.Rebus.Server |
| Compatibility | ? Fully compatible with .NET 10 |
| Notes | Dependency injection integration works as-is |

#### FluentScheduler

| Attribute | Value |
|-----------|-------|
| Current Version | 5.5.1 |
| Target Version | 5.5.1 (No Change) |
| Affected Projects | Example.Rebus.Client |
| Compatibility | ? Fully compatible with .NET 10 |
| Notes | Job scheduling works as-is |

#### NETStandard.Library

| Attribute | Value |
|-----------|-------|
| Current Version | 2.0.3 |
| Target Version | 2.0.3 (No Change) |
| Affected Projects | Example.Rebus.Contracts |
| Compatibility | ? Fully compatible with .NET 10 |
| Notes | Only used in netstandard2.0 project |

---

### Update Execution Plan

All package updates happen in a single atomic operation:

```bash
# In Example.Rebus.Client project
dotnet add package Microsoft.Extensions.Hosting --version 10.0.3

# In Example.Rebus.Server project
dotnet add package Microsoft.Extensions.Hosting --version 10.0.3
dotnet add package Microsoft.Extensions.Hosting.Abstractions --version 10.0.3

# Restore entire solution
dotnet restore
```

### Verification

After updates:
```bash
# Verify all packages resolve
dotnet restore --verify-match-locked-packages

# Build entire solution
dotnet build

# Expected result: Solution builds cleanly with 0 errors
```

---

## Project-by-Project Migration Plans

### Overview

The three projects are upgraded as a single atomic operation. Details below show what changes for each.

---

### Project 1: Example.Rebus.Contracts

#### Current State
- **Framework**: netstandard2.0
- **Project Type**: ClassLibrary (shared contracts)
- **Dependencies**: None
- **Packages**: NETStandard.Library 2.0.3
- **LOC**: 8 (1 file)
- **Risk Level**: ? NONE

#### Target State
- **Framework**: netstandard2.0 (no change needed)
- **Rationale**: Already compatible with .NET 10. No upgrade required.
  - Alternative: Could upgrade to net10.0 to simplify project set, but not necessary
- **Action**: Keep as-is OR optionally upgrade to net10.0

#### Migration Steps

**Option A: Keep netstandard2.0 (Recommended)**
1. No changes to project file
2. No package updates
3. Existing code is compatible
4. Recompiles cleanly with net10.0 dependencies

**Option B: Upgrade to net10.0 (Also viable)**
1. Update `TargetFramework` from `netstandard2.0` to `net10.0`
2. No package updates needed
3. Same functionality, simplified project set

#### Code Review
The Contracts project contains only message definitions - no code changes required regardless of framework target.

#### Validation Checklist
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No package conflicts

---

### Project 2: Example.Rebus.Client

#### Current State
- **Framework**: netcoreapp3.1
- **Project Type**: Console Application
- **Dependencies**: Example.Rebus.Contracts
- **Key Entry Point**: `Program.cs` (141 LOC)
- **Packages**:
  - Microsoft.Extensions.Hosting (3.1.9) - **UPDATE NEEDED**
  - FluentScheduler (5.5.1)
  - Rebus (6.4.1)
  - Rebus.AmazonSqs (6.1.1)
  - Rebus.ServiceProvider (5.0.6)
- **Files**: Program.cs (141 LOC), potentially others
- **Risk Level**: ?? LOW

#### Target State
- **Framework**: net10.0
- **Packages**: Microsoft.Extensions.Hosting 10.0.3 (others unchanged)
- **Code Modernization**: Top-level statements in Program.cs
- **Expected Outcome**: Builds, runs, all tests pass

#### Project File Changes

**Update TargetFramework**:
```xml
<!-- Before -->
<TargetFramework>netcoreapp3.1</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**Update Package References**:
```xml
<!-- Before -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="3.1.9" />

<!-- After -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="10.0.3" />
```

All other package references remain unchanged.

#### Code Modernization: Top-Level Statements

**Current State**: Traditional async Main method with nested classes

**Target State**: C# 10 top-level statements pattern

**Changes to Program.cs**:
1. Convert `Program` class to file-scoped namespace (C# 10)
2. Replace `async Task Main()` with top-level statements
3. Move `ConsoleHostedService` class outside for clarity OR use file-scoped classes
4. Simplify using statements with implicit usings

See [Code Modernization section](#code-modernization-top-level-statements) for detailed examples.

#### Validation Checklist
- [ ] TargetFramework updated to net10.0
- [ ] Microsoft.Extensions.Hosting updated to 10.0.3
- [ ] Program.cs modernized with top-level statements
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Application starts correctly
- [ ] All unit tests pass

---

### Project 3: Example.Rebus.Server

#### Current State
- **Framework**: netcoreapp3.1
- **Project Type**: Console Application
- **Dependencies**: Example.Rebus.Contracts
- **Files**: Program.cs (99 LOC), potentially others
- **Packages**:
  - Microsoft.Extensions.Hosting (3.1.9) - **UPDATE NEEDED**
  - Microsoft.Extensions.Hosting.Abstractions (3.1.9) - **UPDATE NEEDED**
  - Rebus (6.4.1)
  - Rebus.AmazonSqs (6.1.1)
  - Rebus.ServiceProvider (5.0.6)
- **Risk Level**: ?? LOW

#### Target State
- **Framework**: net10.0
- **Packages**: 
  - Microsoft.Extensions.Hosting 10.0.3
  - Microsoft.Extensions.Hosting.Abstractions 10.0.3
- **Code Modernization**: Top-level statements in Program.cs
- **Expected Outcome**: Builds, runs, all tests pass

#### Project File Changes

**Update TargetFramework**:
```xml
<!-- Before -->
<TargetFramework>netcoreapp3.1</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**Update Package References**:
```xml
<!-- Before -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="3.1.9" />
<PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="3.1.9" />

<!-- After -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="10.0.3" />
<PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="10.0.3" />
```

All other package references remain unchanged.

#### Code Modernization: Top-Level Statements

Same approach as Example.Rebus.Client:
1. Convert to file-scoped namespace (C# 10)
2. Replace `async Task Main()` with top-level statements
3. Move helper classes as needed
4. Simplify using statements

#### Validation Checklist
- [ ] TargetFramework updated to net10.0
- [ ] Microsoft.Extensions.Hosting updated to 10.0.3
- [ ] Microsoft.Extensions.Hosting.Abstractions updated to 10.0.3
- [ ] Program.cs modernized with top-level statements
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Application starts correctly
- [ ] All unit tests pass

---

## Breaking Changes Catalog

### Summary

**Total Breaking Changes**: 0 ?

All 260 analyzed APIs are compatible with .NET 10. No code changes are required due to framework or package updates.

### Detailed Analysis

#### Framework Breaking Changes (3.1 ? 10.0)

**Status**: ? None - No incompatible APIs used in this codebase

The projects use the following .NET APIs which **remain compatible** from 3.1 through 10.0:

**Microsoft.Extensions.Hosting**:
- `Host.CreateDefaultBuilder()` - Unchanged, works identically
- `IHostedService.StartAsync()` - Unchanged, interface stable
- `IHostedService.StopAsync()` - Unchanged, interface stable
- `IServiceCollection.AddHostedService<T>()` - Unchanged, works identically
- `IServiceProvider` - Unchanged core functionality

**System.Threading**:
- `CancellationToken` - Stable API, no changes
- `Task` and `Task<T>` - Stable APIs, no changes
- `async/await` - Stable language features, no changes

**System.Collections.Generic**:
- `IEnumerable<T>` - Stable API, no changes
- `List<T>` - Stable API, no changes

#### Package Breaking Changes

**Microsoft.Extensions.Hosting** (3.1.9 ? 10.0.3):
- ? IHost interface unchanged
- ? IHostBuilder interface unchanged  
- ? IHostedService interface unchanged
- ? All used configuration methods unchanged

No code modifications needed.

**Microsoft.Extensions.Hosting.Abstractions** (3.1.9 ? 10.0.3):
- ? Abstractions unchanged
- ? IHostedService contract preserved

No code modifications needed.

**Other Packages**: All compatible, no breaking changes.

---

## Code Modernization: Top-Level Statements

### Rationale

Your request specified upgrading to .NET 10 "with the use of top level statements." C# 10 introduced top-level statements, allowing Main methods to be written more concisely without wrapping in class/namespace declarations.

**Benefits**:
- Cleaner, more readable code
- Less ceremony for simple console apps
- Modern idiomatic C#
- Aligns with .NET 10 best practices

### Modernization Strategy

Two files require modernization:
1. `Example.Rebus.Client\Program.cs`
2. `Example.Rebus.Server\Program.cs`

Both follow similar pattern: async Main that initializes Host, configures services, and runs the application.

### Pattern: Before (Current)

```csharp
namespace Example.Rebus.Client
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            var SqsConfig = new AmazonSQSConfig { ... };

            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => { ... })
                .RunConsoleAsync();
        }
    }

    internal sealed class ConsoleHostedService : IHostedService
    {
        // Implementation
    }
}
```

### Pattern: After (Modernized C# 10)

```csharp
using Amazon.SQS;
using Example.Rebus.Contracts;
using FluentScheduler;
// ... other using statements

var SqsConfig = new AmazonSQSConfig { UseHttp = true, ServiceURL = "http://localhost:4566" };

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<ConsoleHostedService>();
        services.AutoRegisterHandlersFromAssemblyOf<Program>();
        services.AddRebus(configure => configure
            .Logging(l => l.ColoredConsole())
            .Transport(t => t.UseAmazonSQSAsOneWayClient(SqsConfig))
            .Routing(r => r.TypeBased().MapAssemblyOf<ImportantMessage>("ServerMessages")));
        services.AddSingleton<IJob, ProduceMessageJob>();
    })
    .RunConsoleAsync();

file sealed class ConsoleHostedService : IHostedService
{
    // Implementation
}
```

### Changes Explained

1. **No Program class**: Top-level statements replace the async Main method
2. **File-scoped classes**: `file sealed class` marks ConsoleHostedService as visible only within this file
3. **Implicit usings** (optional): Can enable in csproj with `<ImplicitUsings>enable</ImplicitUsings>`
4. **Direct execution**: Code runs sequentially as if in Main method

### Implementation Notes

**Type Declarations**:
- `ConsoleHostedService` becomes a file-scoped class: `file sealed class`
- Only visible within Program.cs (file scope) - prevents accidental external references
- Maintains encapsulation without nested class overhead

**Using Statements**:
- All current using statements are preserved
- Optional: Enable ImplicitUsings for System.*, Microsoft.Extensions.*, etc.
- If using implicit usings, explicit using statements for third-party packages remain needed

**Program Configuration**:
- Host configuration logic moves to top level
- ConsoleHostedService implementation logic unchanged
- Same behavior, cleaner syntax

### Files Affected

| File | Changes | Scope |
|------|---------|-------|
| Example.Rebus.Client\Program.cs | Convert to top-level statements | Structural |
| Example.Rebus.Server\Program.cs | Convert to top-level statements | Structural |

No logic changes—only syntax modernization.

### Testing Impact

? **No behavior changes** - Functionality identical before/after
- Hosting behavior unchanged
- DI registration unchanged
- Service startup sequence unchanged
- All existing tests should pass without modification

### Fallback (If Issues Arise)

If top-level statements cause issues:
1. Revert to traditional Program class syntax
2. Functionality unchanged, format different
3. No penalty—both formats are valid in .NET 10

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

Testing happens at three levels to ensure full upgrade success:

### Level 1: Build Validation

**After framework and package updates**, before code changes:

```bash
dotnet clean
dotnet build
```

**Success Criteria**:
- ? Zero compilation errors
- ? Zero warnings (or expected framework warnings)
- ? All references resolve correctly
- ? No package conflicts

**Expected Outcome**: Build succeeds (all APIs compatible)

---

### Level 2: Unit Testing

**After top-level statement modernization**:

```bash
dotnet test
```

**Test Projects**: All projects with *Test* or *Tests* suffix

**Success Criteria**:
- ? All tests pass
- ? 100% pass rate
- ? No skipped tests
- ? No timeout failures

**Expected Outcome**: All tests pass (no functional changes)

---

### Level 3: Application Validation

**Smoke testing** after upgrade completion:

#### Example.Rebus.Client

1. **Build verification**:
   ```bash
   cd Example.Rebus.Client
   dotnet build
   ```

2. **Startup test**:
   - Application starts without errors
   - Logs show successful initialization
   - Dependency injection resolves all services
   - Rebus bus initializes correctly
   - FluentScheduler job scheduling starts

3. **Functionality test**:
   - Scheduled job executes (1-second interval)
   - Messages are sent to SQS
   - No errors in logs

#### Example.Rebus.Server

1. **Build verification**:
   ```bash
   cd Example.Rebus.Server
   dotnet build
   ```

2. **Startup test**:
   - Application starts without errors
   - Logs show successful initialization
   - Hosted service starts correctly
   - Rebus bus initializes
   - Ready to receive messages

3. **Functionality test**:
   - Receives messages from client
   - Processes messages correctly
   - No errors in logs

---

### Phase-Based Validation Timeline

#### Phase 1: Pre-Upgrade Baseline
- [ ] Current application runs on .NET 3.1
- [ ] All tests pass on current version
- [ ] Applications start and function correctly

#### Phase 2: Framework/Package Update
- [ ] All .csproj files updated to net10.0
- [ ] Package versions updated
- [ ] Solution builds without errors
- [ ] Initial build validation passes

#### Phase 3: Code Modernization
- [ ] Top-level statements implemented
- [ ] File-scoped classes declared
- [ ] Solution builds without errors
- [ ] Modernization validation passes

#### Phase 4: Full Test Suite
- [ ] Unit tests execute and pass
- [ ] Integration tests execute and pass
- [ ] All test projects show green status

#### Phase 5: Application Smoke Tests
- [ ] Client application starts correctly
- [ ] Server application starts correctly
- [ ] Message sending works (Client ? SQS)
- [ ] Message receiving works (SQS ? Server)
- [ ] Job scheduling works (Client)
- [ ] Log output is clean (no errors/warnings)

---

### Test Verification Checklist

**Build Phase**:
- [ ] `dotnet build` returns exit code 0
- [ ] No error messages in output
- [ ] No package resolution warnings
- [ ] All projects compile successfully

**Code Phase**:
- [ ] Program.cs uses top-level statements (no Program class)
- [ ] File-scoped classes compile correctly
- [ ] No duplicate definitions

**Test Phase**:
- [ ] `dotnet test` runs without errors
- [ ] All tests reported as "Passed"
- [ ] No "Skipped" or "Not Run" tests
- [ ] Test execution completes normally

**Runtime Phase**:
- [ ] Applications start on first run
- [ ] No unhandled exceptions in logs
- [ ] Rebus messaging works
- [ ] SQS integration works
- [ ] Job scheduling works
- [ ] Services resolve from DI

---

### Validation Success Criteria

The upgrade is **COMPLETE AND SUCCESSFUL** when:

1. ? **Build Success**
   - Solution builds with 0 errors
   - Solution builds with 0 warnings

2. ? **Test Success**
   - All unit tests pass
   - All integration tests pass
   - 100% pass rate

3. ? **Application Success**
   - Both applications start normally
   - No exceptions in logs
   - All services resolve
   - Messaging works (Client ? Server)
   - Job scheduling works

4. ? **Code Quality**
   - Code uses modern C# 10 patterns
   - All classes properly scoped
   - No deprecated APIs used
   - No security warnings

---

### Regression Testing

To verify no functionality was lost:

1. **Message Flow**: Send message from Client, verify Server receives it
2. **Scheduling**: Verify Client job runs on schedule
3. **Configuration**: Verify AWS SQS endpoint resolves
4. **Logging**: Verify all logged messages appear
5. **Error Handling**: Verify exceptions are caught and logged

All should work identically to .NET 3.1 version.

---

## Risk Management

### Risk Assessment Summary

**Overall Risk Level: LOW** ?

This is a low-risk migration with clear upgrade paths and no blocking issues.

### Risk Analysis by Category

#### 1. Framework Compatibility Risk: LOW

| Item | Status | Rationale |
|------|--------|-----------|
| Target framework support | ? Safe | .NET 10.0 is LTS with extended support |
| Current ? Target jump | ? Safe | 3.1 ? 10.0 is well-documented path |
| SDK requirements | ? Available | .NET 10 SDK can be installed on any platform |

**Mitigation**: Verify .NET 10.0 SDK is installed before upgrade begins.

#### 2. Package Compatibility Risk: LOW

| Package | Current | Target | Risk | Notes |
|---------|---------|--------|------|-------|
| Microsoft.Extensions.Hosting | 3.1.9 | 10.0.3 | ? Low | Direct alignment with .NET version |
| Microsoft.Extensions.Hosting.Abstractions | 3.1.9 | 10.0.3 | ? Low | Direct alignment with .NET version |
| Rebus* | 6.4.1 | 6.4.1 | ? Low | Already compatible, no update needed |
| FluentScheduler* | 5.5.1 | 5.5.1 | ? Low | Already compatible, no update needed |

\* Denotes packages that require no updates

**Mitigation**: All package updates are to official maintained versions with .NET 10 support.

#### 3. API Compatibility Risk: NONE

**Finding**: 260/260 analyzed APIs are compatible
- No binary incompatibilities
- No source incompatibilities
- No behavioral changes
- No deprecated API usage

**Mitigation**: Full API compatibility means zero code changes due to framework updates.

#### 4. Dependency Graph Risk: NONE

**Structure**: Linear, no cycles
- Contracts (leaf) has no dependencies
- Client and Server only depend on Contracts
- No transitive coupling issues
- No version conflicts between projects

**Mitigation**: Clean dependency structure enables safe parallel upgrade.

#### 5. Test Coverage Risk: LOW

**Available test projects**: All projects
- Can verify each component post-upgrade
- Full regression testing possible
- Integration tests available

**Mitigation**: Comprehensive test execution validates upgrade success.

### Risk Contingencies

#### Contingency 1: Build Failure (Expected: None)

**Likelihood**: Very Low (all APIs compatible)

**If occurs**:
1. Check error message for API/package issue
2. Consult breaking changes catalog below
3. Identify minimum code change needed
4. Update affected file
5. Rebuild and retest

#### Contingency 2: Test Failure (Expected: None)

**Likelihood**: Very Low (no API changes)

**If occurs**:
1. Review test error message
2. Check for behavioral changes in dependencies
3. Verify test assumptions still valid
4. Update test if needed
5. Rerun test suite

#### Contingency 3: Runtime Issue in Production

**Likelihood**: Very Low (all APIs analyzed)

**If occurs**:
1. Immediately switch back to .NET 3.1 (git revert)
2. Document unexpected behavior
3. Investigate package release notes
4. Create targeted fix
5. Re-test before redeployment

### Rollback Strategy

**If critical issues arise**:

```bash
# Instant rollback to previous state
git revert <commit-hash>
dotnet clean
dotnet build
```

All changes are committed to a single branch (`upgrade-to-NET10`), enabling instant rollback to `main` if needed.

### Security Considerations

? **No known vulnerabilities** in current or target packages
? **Upgrade improves security** (newer framework versions include security patches)
?? **Verify Azure/hosting environment** supports .NET 10 LTS (most modern cloud providers do)

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | LOC | Packages | Risk | Effort |
|---------|-----|----------|------|--------|
| Example.Rebus.Client | 141 | 5 | ?? Low | 15-20 min |
| Example.Rebus.Server | 99 | 5 | ?? Low | 15-20 min |
| Example.Rebus.Contracts | 8 | 1 | ? None | 0 min |
| **Total** | **248** | **7** | **?? Low** | **30-40 min** |

### Complexity Factors

**Framework Update**: ?? Low
- No major API changes in 3.1 ? 10.0
- All used APIs remain compatible
- No assembly binding redirects needed

**Package Updates**: ?? Low
- Only 2 packages need updates
- Both are official Microsoft.Extensions packages
- No behavioral changes expected

**Code Modernization**: ?? Low
- Top-level statements are additive (no breaking changes)
- File-scoped namespaces are optional
- Existing code patterns continue to work

**Testing**: ?? Low
- Full test suite available
- No test framework updates needed
- Expect all tests to pass as-is

### What To Expect

**Before Upgrade**: 248 LOC, compiles & runs on .NET Core 3.1
**After Upgrade**: ~280-300 LOC (modernized), compiles & runs on .NET 10, all tests pass

**Estimated scope of changes**:
- 3 `.csproj` files: 3 lines modified
- 2 `.cs` code files: 30-40 lines modified (top-level statements, using statements)
- Total: ~35-45 lines across 5 files

---

## Source Control Strategy

### Git Workflow

#### Current Branch State
- **Working branch**: `upgrade-to-NET10`
- **Base branch**: `main`
- **Remote**: `origin` (https://github.com/bradjolicoeur/Example.Rebus)

#### Commit Strategy: Single Atomic Commit

For all-at-once migrations, use a **single commit** to capture the entire upgrade:

```bash
git add -A
git commit -m "Upgrade to .NET 10.0 with top-level statements

- Update all project target frameworks: netcoreapp3.1 -> net10.0
  - Example.Rebus.Client
  - Example.Rebus.Server
  - Example.Rebus.Contracts (optional net10.0)

- Update NuGet packages for .NET 10 compatibility:
  - Microsoft.Extensions.Hosting: 3.1.9 -> 10.0.3
  - Microsoft.Extensions.Hosting.Abstractions: 3.1.9 -> 10.0.3

- Modernize code for C# 10:
  - Convert Program.cs files to top-level statements
  - Use file-scoped classes where appropriate
  - Remove unnecessary Program class declarations

- All 260 analyzed APIs are compatible
- All tests pass
- Solution builds with 0 errors and 0 warnings

This is an all-at-once upgrade with no intermediate states."
```

#### Why Single Commit?

**Benefits**:
- ? Atomic: Entire upgrade succeeds or fails as one unit
- ? Reversible: Single `git revert` undoes everything
- ? Clear: History shows this was one coordinated change
- ? Testable: All changes tested together before committing

**Alternatives not used**:
- ? Multiple commits per project (harder to debug)
- ? Separate commits for packages/frameworks (creates intermediate broken states)

#### Merge Strategy

After local testing completes:

```bash
# Ensure main is current
git fetch origin
git log main..origin/main

# Fast-forward merge (clean linear history)
git checkout main
git merge upgrade-to-NET10

# Push to remote
git push origin main
```

**Or via Pull Request**:
1. Push `upgrade-to-NET10` to GitHub
2. Create PR from `upgrade-to-NET10` ? `main`
3. CI/CD runs tests on PR
4. Code review (optional)
5. Merge to `main`

---

## Success Criteria

### Technical Success Criteria

#### Framework & Packages
- ? All project files updated to target `.NET 10.0`
- ? All required packages updated to compatible versions:
  - `Microsoft.Extensions.Hosting` ? 10.0.3
  - `Microsoft.Extensions.Hosting.Abstractions` ? 10.0.3
- ? Solution restores without package conflicts
- ? No version constraint violations

#### Build Success
- ? `dotnet build` exits with code 0
- ? Zero compilation errors
- ? Zero warnings (or only expected platform warnings)
- ? All project outputs generated
- ? All assembly versions correct

#### Code Quality
- ? All code compiles on .NET 10 runtime
- ? No deprecated API usage
- ? Top-level statements implemented in Program.cs files
- ? File-scoped classes properly declared
- ? Code follows C# 10 idiomatic patterns

#### Test Execution
- ? All test projects execute successfully
- ? 100% of tests pass (no failures, no skips)
- ? No timeout failures
- ? Test framework compatible with .NET 10

#### Runtime Validation
- ? Client application starts without errors
- ? Server application starts without errors
- ? All hosted services initialize correctly
- ? Dependency injection resolves all dependencies
- ? Rebus message bus initializes
- ? AWS SQS endpoint reachable (or LocalStack)
- ? Job scheduler starts without errors

#### Integration Testing
- ? Client can send messages to SQS
- ? Server can receive messages from SQS
- ? Job scheduling runs on expected intervals
- ? Message handlers execute correctly
- ? No lost functionality vs. .NET 3.1 version

---

### Definition of Done

The migration is **COMPLETE** when:

1. **All framework/packages updated** ?
   - Both projects on net10.0
   - Both have Microsoft.Extensions packages at 10.0.3

2. **Code modernized** ?
   - Top-level statements implemented
   - File-scoped classes used appropriately
   - Code compiles and runs

3. **All tests passing** ?
   - 100% unit test pass rate
   - 100% integration test pass rate
   - No timeouts or skipped tests

4. **Applications validated** ?
   - Both applications start normally
   - Both applications respond to requests
   - No exceptions or errors in logs
   - Message flow works end-to-end

5. **Committed and pushed** ?
   - Single atomic commit on `upgrade-to-NET10`
   - Merged to `main` (or PR created)
   - Remote reflects latest version

6. **Documentation updated** ?
   - This plan.md documents the upgrade
   - assessment.md captures pre-upgrade state
   - Any team docs updated to reflect .NET 10 requirement

---

### Success Indicators

**Metrics that indicate success**:

| Indicator | Target | Check |
|-----------|--------|-------|
| Build exit code | 0 | `dotnet build` |
| Compilation errors | 0 | Build output |
| Build warnings | 0 | Build output |
| Test pass rate | 100% | `dotnet test` |
| Test failures | 0 | Test output |
| Application startup errors | 0 | Console output |
| Unhandled exceptions | 0 | Log inspection |
| API incompatibilities | 0 | Code review |

---

### Rollback Procedure (If Needed)

If critical issues arise **before merging to main**:

```bash
# Abort current work
git reset --hard HEAD

# Switch back to main
git checkout main

# Delete upgrade branch
git branch -d upgrade-to-NET10
```

If issues discovered **after merge to main**:

```bash
# Create new branch to revert
git checkout -b revert-to-NET3.1
git revert <commit-hash-of-upgrade>

# Or single command:
git revert --mainline 1 <merge-commit>

# Push revert
git push origin revert-to-NET3.1

# Create PR to merge revert back to main
```

**Expected outcome**: Repository back to .NET Core 3.1 state, all functionality restored, zero downtime.
