![image 138](https://github.com/mohammadKarimi/SwiftLink/assets/5300102/9720e942-4853-4f7f-a426-f0f7a9fefeca) 


[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]


SwiftLink - .NET 8 Project
Introduction
Welcome to the SwiftLink project, a .NET 8 application designed to streamline link shortening. This README provides comprehensive instructions on how to clone, restore, build, and run the project. Additionally, it covers database migration and the usage of a Redis container via Docker.

## Getting Started

### Prerequisites
Make sure you have the following tools installed on your machine:

.NET 8 SDK
Git
Docker
Cloning the Repository
Open your terminal or command prompt and run the following command:

* bash
```
git clone https://github.com/mohammadKarimi/SwiftLink.git
```

This will create a local copy of the SwiftLink repository on your machine.

### Restoring Dependencies
Navigate to the project directory:

* bash
```
cd SwiftLink
```

Run the following command to restore project dependencies:

* bash
```
dotnet restore
```

### Updating Database and Connection String
In the appSettings.json file, locate the connection string and update it accordingly. This connection string is crucial for updating the database during migrations.

* json
```
{
  "ConnectionStrings": {
    "DefaultConnection": "YourUpdatedConnectionString"
  },
  // Other configurations...
}
```

Next, apply the database migrations using the following command:

* bash
```
dotnet ef database update
```

### Redis Container
This project utilizes a Redis container to manage caching. Ensure that Docker is running and execute the following command to pull the Redis image:

* bash
```
docker pull redis
```

Once the image is downloaded, start a Redis container:

* bash
```
docker run -d -p 6379:6379 --name swiftlink-redis redis
```

### Building and Running the Project
Now that everything is set up, build and run the SwiftLink project using the following commands:

* bash

```
dotnet build
dotnet run
```

Access the application by navigating to https://localhost:5001 in your web browser.

That's it! You've successfully cloned, restored, and run the SwiftLink project with database migration and a Redis container. If you encounter any issues, refer to the project documentation or seek help from the community. Happy coding!

Contributions and bug reports are welcome.

