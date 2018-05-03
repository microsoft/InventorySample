# Introduction

During the development of an enterprise app, we as developers, must face several challenges as:
- App requirements that can change over time.
- New business opportunities and challenges.
- Ongoing feedback during development that can significantly affect the scope and requirements of the app.

With these in mind, it's important to build apps that can be easily modified or extended over time. Designing for such adaptability can be difficult as it requires an architecture that allows individual parts of the app to be independently developed and tested in isolation without affecting the rest of the app.

An effective remedy for these challenges is to partition an app into discrete, loosely coupled components that can be easily integrated together into an app. Such an approach offers several benefits:
- It allows individual functionality to be developed, tested, extended, and maintained by different individuals or teams.
- It promotes reuse and a clean separation of concerns between the app's horizontal capabilities, such as authentication and data access, and the vertical capabilities, such as app specific business functionality. This allows the dependencies and interactions between app components to be more easily managed.
- It helps maintain a separation of roles by allowing different individuals, or teams, to focus on a specific task or piece of functionality according to their expertise. In particular, it provides a cleaner separation between the user interface and the app's business logic.

Before proceeding to decouple the app in different components, it's important to choose the design patterns that will help us to do it properly. These are the patterns we have decided to choose for the Inventory App:

- [Model-View-ViewModel (MVVM)](mvvm.md): Windows 10 enterprise apps are specially design to apply this pattern to decouple the business logic, the presentation logic and the UI views.
- [Dependency Injection](architecture/app-initial-setup.md#Dependency-Injection): Dependency injection containers reduce the dependency coupling between objects by providing a facility to construct instances of classes with their dependencies injected, and manage their lifetime based on the configuration of the container.
- [Communication between loosely coupled components](architecture/message-service.md): Message-based communication between components that are inconvenient to link by object and type references.
- [Navigation](navigation-service.md#Navigation-Service): Define how the Navigation will work, and where the Navigation logic will reside.
- [Data Access](dataaccess.md): Define how to connect with data sources and the technology to use for that purpose.

This guide not only explain in detail each of the patterns used, it also shows you how are being applied in the Inventory Sample app.

## The Inventory Sample app

The purpose of the Invetory Sample app is to provide an example of Windows 10 enterprise app implementation. This app manages the Van Arsdel store, including customers, products, orders, etc. 

 ![solution](img/solution.png)

 App's solution has been divided in three decoupled projects, and each of them represents an important role in the app:

 | Project | Description |
 | ------- | ----------- |
 | Inventory.Data | [.Net Standard](netstandard.md) project with the data access logic of the app |
 | Inventory.ViewModels | [.Net Standard](netstandard.md) project representing the business and the presentation logic of the app |
 | Inventory.App | This is the Windows 10 app, where *User Interface Views* |

 ### The Inventory.Data project

 .Net Standard library which purpose is to interact with a database through *Entity Framework Core*. It also contains the definitions of the DTOs (*data transfer objects*) of the app. We will explain this project in detail [here](dataaccess.md).

 ### The Inventory.ViewModels project

The main project of the app is the **Inventory.ViewModels**. This project, as we mentioned before, is a .Net Standard library, and it's agnostic of the platform that is going to consume it. A special attention is required for its folder structure:

 ![solution](img/inventory-viewmodels.png)

 | Folder | Description |
 | ------ | ----------- |
 | Infrastructure | This folder will contain the [infrastructure services](architecture/infrastructure-services.md#Infrastructure-services) as well as the base logic of the ViewModels |
 | Models | Application Domain models |
 | Services | Application Domain Services |
 | ViewModels | ViewModels classes with the presentation logic of the app |

### The Inventory.App project

This is the UWP app. It contains platform specific implementations like services and the UI. The last advance features of Windows 10 development has been applied to this project as well.

## Summary

After this introduction, the user should be familiarized with the structure of the solution and the purpose of each of the projects.
We will address now the patterns applied in the Inventory Sample App as well as their implementation by example. 


