# Student Management Console Application

This is a .NET console application for managing student information. The application provides functionalities to add, update, delete, and retrieve student records.

## Project Structure

```
StudentManagementConsoleApplication
├── .github
│   └── workflows
│       └── Student-Management-CI_CD.yml
├── StudentManagementConsoleApplication
│   ├── Program.cs
│   └── StudentManagementConsoleApplication.csproj
└── README.md
```

## Getting Started

To get started with the Student Management Console Application, follow these steps:

1. **Clone the repository**:
   ```
   git clone https://github.com/yourusername/StudentManagementConsoleApplication.git
   ```

2. **Navigate to the project directory**:
   ```
   cd StudentManagementConsoleApplication
   ```

3. **Restore dependencies**:
   ```
   dotnet restore
   ```

4. **Build the application**:
   ```
   dotnet build
   ```

5. **Run the application**:
   ```
   dotnet run --project StudentManagementConsoleApplication/StudentManagementConsoleApplication.csproj
   ```

## Features

- Add new student records
- Update existing student information
- Delete student records
- Retrieve student details

## CI/CD Workflow

This project includes a GitHub Actions CI/CD workflow defined in `.github/workflows/Student-Management-CI_CD.yml`. The workflow automates the build and deployment process, ensuring that the application is built and tested on every push and pull request to the master branch.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or features you would like to add.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.