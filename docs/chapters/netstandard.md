# .Net Standard

The project **VanArsdel.Data** of our example app is declared as a .Net Standard library. This is an important concept that we should know in order to develop cross-platform apps. 

## What is .Net Standard

The .Net Standard is a formal specification of .NET APIs that are intended to be available on all .NET implementations. The motivation behind the .NET Standard is establishing greater uniformity in the .NET ecosystem.

.Net Standard is the evolution of the *Portable Class Libraries* and its main purpose is to share code between different platforms.

## Why .Net Standard

The old *Portable Class Libraries (PCL)* were based on profiles, i. e. different Microsoft platforms. We hadf different profiles depending on the target platform selected and that was causing fragmentation of the *crossplatform libraries*.

The .Net Standard is not based on the target platform of the library, it is based in a standardized set of APIs making our libraries agnostic to the platform.

Let's compare both solutions in detail.

### Comparation with PCLs

Similarities:

- Defines APIs that can be used for binary code sharing.

Differences:

- .NET Standard is a curated set of APIs, while PCL profiles are defined by intersections of existing platforms.
- .NET Standard linearly versions, while PCL profiles do not.
PCL profiles represents Microsoft platforms while the .NET Standard is agnostic to platform.


