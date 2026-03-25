# Custom DI Container in .NET

## Overview

This project implements a lightweight Dependency Injection (DI) container using C# and .NET. It demonstrates core DI concepts such as constructor injection, service lifetimes, and abstraction-to-implementation mapping.

The implementation focuses on simplicity while correctly handling dependency resolution and service lifetimes.

---

## Features

* Constructor Injection
* Singleton and Transient lifetimes
* Interface to Implementation mapping
* Nested dependency resolution
* Exception handling for unregistered services

---

## Design Approach

The container maintains a registry of services using a `ServiceDescriptor`. Each registration defines:

* Service type (interface)
* Implementation type (concrete class)
* Lifetime (Singleton or Transient)

During resolution:

* The container uses reflection to identify constructor dependencies
* Dependencies are resolved recursively
* Singleton instances are created once and reused
* Transient instances are created on each request

---

## Example Usage

```csharp
var container = new DIContainer();

container.Register<IEmailSender, AzureEmailSender>(ServiceLifetime.Transient);
container.Register<INotificationService, NotificationService>(ServiceLifetime.Transient);

var service = container.Resolve<INotificationService>();
```

---

## Unit Tests

The solution includes unit tests to validate the core behaviors:

* Transient lifetime creates a new instance on each resolve call
* Singleton lifetime returns the same instance
* Nested dependencies are resolved correctly using constructor injection
* Correct implementation is returned for a given interface (mapping validation)
* Exception is thrown when resolving an unregistered service

---

## Technologies Used

* C#
* .NET 10
* xUnit

---

## Project Structure

```
CustomDIContainer
│
├── CustomDIContainer
│   ├── DIContainer.cs
│   ├── ServiceDescriptor.cs
│   ├── ServiceLifetime.cs
│   ├── Services/
│
├── CustomDIContainer.Tests
│   ├── ContainerTests.cs
│
├── README.md
├── .gitignore
├── CustomDIContainer.sln
```

---

## Conclusion

This project demonstrates a clean and minimal implementation of a Dependency Injection container, covering essential concepts such as service lifetimes, constructor injection, and dependency resolution as required by the task.
