 ## Contributing to the Sample

 - [Questions](#question)
 - [Issues or Bugs](#issue)
 - [Submitting a pull request](#pr)
 - [Quality assurance for pull requests for XAML controls](#xaml)
 - [General rules](#rules)
 - [Naming conventions](#naming)
 - [Documentation](#documentation)
 - [Files and folders](#files)


## <a name="question"></a> Questions
Please do not open issues for general support questions and keep our GitHub issues for bug reports and feature requests. 

## <a name="issue"></a> Found a Bug?
If you find a bug, you can help us by
[submitting an issue](https://github.com/Microsoft/InventorySample/issues). Even better, you can
[submit a Pull Request](#pr) with a fix.

## <a name="pr"></a> Submitting a pull request
For every contribution, you must:

* test your code with the [supported SDKs](README.md#supported)
* follow the [quality guidance](#xaml), [general rules](#rules) and [naming convention](#naming)
* target master branch (or an appropriate release branch if appropriate for a bug fix)

PR has to be validated by at least two core members before being merged.

## <a name="xaml"></a> Quality assurance for pull requests
We encourage developers to follow the following guidances when submitting pull requests:
 * Your UI must be usable and efficient with keyboard only
  * Tab order must be logical
  * Action must be triggered when hitting Enter key
 * Do not use custom colors but instead rely on theme colors so high contrasts themes can be used with your control
 * Add AutomationProperties.Name on all controls to define what the controls purpose (Name is minimum, but there are some other things too that can really help the screen reader). 
  * Don't use the same Name on two different elements unless they have different control types
 * Use Narrator Dev mode (Launch Narrator [WinKey+Enter], then CTRL+F12) to test the screen reader experience. Is the information sufficient, meaningful and helps the user navigate and understand your UI
 * Ensure that you have run your xaml file changes through Xaml Styler (version 2.3+), which can be downloaded from [here](https://visualstudiogallery.msdn.microsoft.com/3de2a3c6-def5-42c4-924d-cc13a29ff5b7). Do not worry about the settings for this as they are set at the project level (settings.xamlstyler).

You can find more information about these topics [here](https://blogs.msdn.microsoft.com/winuiautomation/2015/07/14/building-accessible-windows-universal-apps-introduction)

This is to help as part of our effort to build an accessible toolkit (starting with 1.2)

## <a name="rules"></a> General rules

* DO NOT require that users perform any extensive initialization before they can start programming basic scenarios.
* DO provide good defaults for all values associated with parameters, options, etc.
* DO ensure that APIs are intuitive and can be successfully used in basic scenarios without referring to the reference documentation.
* DO communicate incorrect usage of APIs as soon as possible. 
* DO design an API by writing code samples for the main scenarios. Only then, you define the object model that supports those code samples.
* DO declare static dependency properties at the top of their file.
* DO NOT seal controls.
* DO use extension methods over static methods where possible.
*
## <a name="naming"></a> Naming conventions
* We are following the coding guidelines of [.NET Core Foundational libraries](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md). 

## <a name="documentation"></a> Documentation
* DO NOT expect that your API is so well designed that it needs no documentation. No API is that intuitive.
* DO provide great documentation with all APIs. 
* DO use readable and self-documenting identifier names. 
* DO use consistent naming and terminology.
* DO provide strongly typed APIs.
* DO use verbose identifier names.

## <a name="files"></a> Files and folders
* DO associate no more than one class per file.
* DO use folders to group classes based on features.
