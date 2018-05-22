# Infrastructure services

There are two types of services we are going to use in the Inventory App: *domain* and *infrastructure* services. The *infrastructure* ones are services that typically talk to external resources and are not part of the primary problem domain, but at the same time they are also essential services to make the app works as expected.

| Service | Description |
| ------- | ----------- |
| `IContextService` | This service is the one responsable of identify the context we are working with. Due our application could be executed in multiple windows, this kind of service is necessary |
| `IDialogService` | Abstraction for displaying alerts and confirmation messages to the user |
| `IFilePickerService` | Service that allows the app to access to the file system |
| `ILogService` | Service to write the logs of the activity as well as possible exceptions |
| [`IMessageService`](message-service.md#Message-Service) | Communication service between diffrent layers of the app |
| [`INavigationService`](navigation-service.md#Navigation-Service) | Service in charge of the navigation of the app | 
| `ISettingsService` | This service stores and provides the configurations and settings needed by the app |


