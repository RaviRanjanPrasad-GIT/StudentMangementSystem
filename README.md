# Student Management System

A .NET 10 console application intended to help manage student records through a straightforward, menu-driven interface. The repository is currently at its initial scaffold stage; student-management workflows are planned and are not yet implemented.

## Project Overview

The Student Management System is a lightweight application for maintaining student information from a command-line environment. It is designed as a clear starting point for an academic administration tool, with a focus on core student-record operations and room to grow into broader enrollment and reporting workflows.

## Business Objectives

- Centralize essential student details in one simple application.
- Make common record-management tasks consistent and easy to perform.
- Reduce errors through input validation and predictable workflows.
- Establish a maintainable foundation for future academic administration features.

## Technology Stack

- **Language:** C#
- **Framework:** .NET 10
- **Application type:** Console application
- **Project format:** SDK-style .NET project
- **External dependencies:** None currently declared

## Application Features

The following capabilities describe the intended initial feature set; they are not yet implemented in the current scaffold:

- Menu-driven navigation for student-management tasks.
- Create and view student records.
- Update and delete existing records.
- Find students using identifying details.
- Validate user input and handle invalid menu selections gracefully.

## Project Structure

```text
StudentManagementConsoleApplication/
|-- Program.cs
|-- StudentManagementConsoleApplication.csproj
|-- README.md
|-- docs/
|   |-- ARCHITECTURE.md
|   |-- DEVELOPMENT-PLAN.md
|   |-- REQUIREMENTS.md
|   `-- TEST-PLAN.md
|-- bin/   (generated build output)
`-- obj/   (generated build and restore files)
```

## Prerequisites

- .NET 10 SDK installed and available on your `PATH`.
- A terminal or command prompt.
- Windows, macOS, or Linux.

Confirm the SDK installation with:

```sh
dotnet --version
```

The reported SDK version should be `10.0` or later.

## Build the Application

From the repository root, restore dependencies and compile the project with:

```sh
dotnet build StudentManagementConsoleApplication.csproj
```

## Run the Application

Run the project from the repository root with:

```sh
dotnet run --project StudentManagementConsoleApplication.csproj
```

At this scaffold stage, running the application displays the default starter output. The planned student-management interface will be added in a later development phase.

## Future Enhancements

- Persist student records to a local file or relational database.
- Support course enrollment and academic records.
- Add sorting, filtering, and summary reports.
- Introduce role-based access and audit history where appropriate.
- Add automated tests for validation and student-record workflows.
