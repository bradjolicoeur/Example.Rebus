# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [Example.Rebus.Client\Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj)
  - [Example.Rebus.Contracts\Example.Rebus.Contracts.csproj](#examplerebuscontractsexamplerebuscontractscsproj)
  - [Example.Rebus.Server\Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 3 | 2 require upgrade |
| Total NuGet Packages | 7 | 2 need upgrade |
| Total Code Files | 5 |  |
| Total Code Files with Incidents | 2 |  |
| Total Lines of Code | 248 |  |
| Total Number of Issues | 5 |  |
| Estimated LOC to modify | 0+ | at least 0.0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [Example.Rebus.Client\Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj) | netcoreapp3.1 | 🟢 Low | 1 | 0 |  | DotNetCoreApp, Sdk Style = True |
| [Example.Rebus.Contracts\Example.Rebus.Contracts.csproj](#examplerebuscontractsexamplerebuscontractscsproj) | netstandard2.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [Example.Rebus.Server\Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj) | netcoreapp3.1 | 🟢 Low | 2 | 0 |  | DotNetCoreApp, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 5 | 71.4% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 2 | 28.6% |
| ***Total NuGet Packages*** | ***7*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 260 |  |
| ***Total APIs Analyzed*** | ***260*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| FluentScheduler | 5.5.1 |  | [Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj) | ✅Compatible |
| Microsoft.Extensions.Hosting | 3.1.9 | 10.0.3 | [Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj)<br/>[Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Hosting.Abstractions | 3.1.9 | 10.0.3 | [Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj) | NuGet package upgrade is recommended |
| NETStandard.Library | 2.0.3 |  | [Example.Rebus.Contracts.csproj](#examplerebuscontractsexamplerebuscontractscsproj) | ✅Compatible |
| Rebus | 6.4.1 |  | [Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj)<br/>[Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj) | ✅Compatible |
| Rebus.AmazonSqs | 6.1.1 |  | [Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj)<br/>[Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj) | ✅Compatible |
| Rebus.ServiceProvider | 5.0.6 |  | [Example.Rebus.Client.csproj](#examplerebusclientexamplerebusclientcsproj)<br/>[Example.Rebus.Server.csproj](#examplerebusserverexamplerebusservercsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;Example.Rebus.Server.csproj</b><br/><small>netcoreapp3.1</small>"]
    P2["<b>📦&nbsp;Example.Rebus.Client.csproj</b><br/><small>netcoreapp3.1</small>"]
    P3["<b>📦&nbsp;Example.Rebus.Contracts.csproj</b><br/><small>netstandard2.0</small>"]
    P1 --> P3
    P2 --> P3
    click P1 "#examplerebusserverexamplerebusservercsproj"
    click P2 "#examplerebusclientexamplerebusclientcsproj"
    click P3 "#examplerebuscontractsexamplerebuscontractscsproj"

```

## Project Details

<a id="examplerebusclientexamplerebusclientcsproj"></a>
### Example.Rebus.Client\Example.Rebus.Client.csproj

#### Project Info

- **Current Target Framework:** netcoreapp3.1
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 2
- **Number of Files with Incidents**: 1
- **Lines of Code**: 141
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Example.Rebus.Client.csproj"]
        MAIN["<b>📦&nbsp;Example.Rebus.Client.csproj</b><br/><small>netcoreapp3.1</small>"]
        click MAIN "#examplerebusclientexamplerebusclientcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P3["<b>📦&nbsp;Example.Rebus.Contracts.csproj</b><br/><small>netstandard2.0</small>"]
        click P3 "#examplerebuscontractsexamplerebuscontractscsproj"
    end
    MAIN --> P3

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 165 |  |
| ***Total APIs Analyzed*** | ***165*** |  |

<a id="examplerebuscontractsexamplerebuscontractscsproj"></a>
### Example.Rebus.Contracts\Example.Rebus.Contracts.csproj

#### Project Info

- **Current Target Framework:** netstandard2.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 2
- **Number of Files**: 1
- **Lines of Code**: 8
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;Example.Rebus.Server.csproj</b><br/><small>netcoreapp3.1</small>"]
        P2["<b>📦&nbsp;Example.Rebus.Client.csproj</b><br/><small>netcoreapp3.1</small>"]
        click P1 "#examplerebusserverexamplerebusservercsproj"
        click P2 "#examplerebusclientexamplerebusclientcsproj"
    end
    subgraph current["Example.Rebus.Contracts.csproj"]
        MAIN["<b>📦&nbsp;Example.Rebus.Contracts.csproj</b><br/><small>netstandard2.0</small>"]
        click MAIN "#examplerebuscontractsexamplerebuscontractscsproj"
    end
    P1 --> MAIN
    P2 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="examplerebusserverexamplerebusservercsproj"></a>
### Example.Rebus.Server\Example.Rebus.Server.csproj

#### Project Info

- **Current Target Framework:** netcoreapp3.1
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 2
- **Number of Files with Incidents**: 1
- **Lines of Code**: 99
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Example.Rebus.Server.csproj"]
        MAIN["<b>📦&nbsp;Example.Rebus.Server.csproj</b><br/><small>netcoreapp3.1</small>"]
        click MAIN "#examplerebusserverexamplerebusservercsproj"
    end
    subgraph downstream["Dependencies (1"]
        P3["<b>📦&nbsp;Example.Rebus.Contracts.csproj</b><br/><small>netstandard2.0</small>"]
        click P3 "#examplerebuscontractsexamplerebuscontractscsproj"
    end
    MAIN --> P3

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 95 |  |
| ***Total APIs Analyzed*** | ***95*** |  |

