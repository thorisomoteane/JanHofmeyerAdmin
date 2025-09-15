## **README.md for JanHofmeyerAdmin (Admin Portal)**
```markdown
# Jan Hofmeyer Community Services - Admin Portal

Administrative portal for managing the Jan Hofmeyer Community Services website content, built with ASP.NET Core MVC (.NET 8).

## Features

### Admin Capabilities
- **Dashboard** - Overview statistics and quick access
- **Projects Management** - Create, edit, delete community projects
- **Events Management** - Manage events and view registrations
- **Gallery Management** - Upload and manage photos/videos
- **Review Moderation** - Approve, hide, or delete reviews
- **Message Center** - View contact messages and volunteer applications
- **Secure Authentication** - Username/password login system

## Technology Stack

- **Framework**: ASP.NET Core MVC (.NET 8)
- **Database**: Azure SQL Database (shared with public site)
- **Storage**: Azure Blob Storage
- **Authentication**: Cookie-based with BCrypt password hashing
- **Frontend**: Bootstrap 5, Font Awesome
- **ORM**: Entity Framework Core
- **Hosting**: Azure App Services

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022
- Azure subscription
- Access to shared database with public website

## Installation

1. Clone the repository
```bash
git clone [repository-url]
cd JanHofmeyerAdmin

2. Install NuGet packages
```bash
dotnet restore

3. Configure appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Your-Connection-String"
  },
  "AzureStorage": {
    "ConnectionString": "Your-Storage-Connection-String",
    "ContainerName": "janhofmeyer-media"
  }
}

4.Build and run:
```bash
dotnet build
dotnet run

5. Create Admin User
```sql

-- Password: Admin123!
INSERT INTO AdminUsers (Username, PasswordHash, CreatedDate, FullName, Email, Role, IsActive)
VALUES ('admin', '$2a$11$rBvQgYKzSsSmH3h568gbnmB6XuQh6yJvJpGrJh4fHrYp8PeVUS1EGKqOK', GETDATE(), 
        'Administrator', 'admin@janhofmeyer.org', 'Admin', 1);



