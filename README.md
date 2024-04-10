![image 138](https://github.com/mohammadKarimi/SwiftLink/assets/5300102/9720e942-4853-4f7f-a426-f0f7a9fefeca)



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<a name="readme-top"></a>

## 📐 SwiftLink - Modern Shortener Link Project
![dotnet-version]

### Introduction and Architecture
Welcome to the SwiftLink project, a .NET 8 application designed to streamline link shortening. This README provides comprehensive instructions on how to clone, restore, build, and run the project. Additionally, it covers database migration and the usage of a Redis container via Docker.

**For More Information, Please Read <a href="https://github.com/mohammadKarimi/SwiftLink/blob/main/docs.md">Our Documentation</a>**

![Architecture](https://github.com/mohammadKarimi/SwiftLink/blob/main/docs/architecture.png) 

## Core Features
**Link Shortening:** Allows users to generate shortened URLs from longer ones, making them easier to share and manage.

**Customizable Shortened Paths:** Through the **Back-Half Option feature**, users can customize the path or identifier that follows the base URL of a shortened link.

**Tagging for Shortened Links**
The tagging feature in SwiftLink allows users to add multiple tags to a shortened link for better identification and organization. This feature enhances the usability of the system by enabling users to categorize and manage their links more effectively. Tags are simple yet powerful identifiers that help users classify their shortened links based on various criteria such as project names, link types, urgency levels, or any other categorization scheme the user finds useful. By attaching tags to links, users can easily filter and locate their links within the SwiftLink dashboard or through API queries.
- Benefits
Enhanced Organization: Users can categorize their links in a way that makes sense for their personal or organizational needs.
Improved Searchability: Tags make it easier to search for and filter links, especially when dealing with a large number of shortened URLs.
Customization: The ability to define custom tags provides flexibility, allowing users to tailor the system to their specific use cases.

**Password Protection for Shortened Links**
The password protection feature in SwiftLink allows users to set a password for a shortened link. This ensures that only individuals with the password can access the original URL linked to the shortened link. This feature is particularly useful for securing direct access to files or sensitive content.
- Benefits
Enhanced Security: Password protection adds an extra layer of security, ensuring that only authorized users can access the content linked to the shortened URL.
Flexibility: By allowing the password to be nullable, the system offers flexibility in how links are secured, catering to both sensitive and non-sensitive content.

**Link Resolution and Visiting:** The application handles the resolution of shortened URLs to their original URLs and tracks visits.

**Analytics:** Tracks and provides insights into the number of users who click on a shortened link via the LinkVisit entity.

**Resilience and Transient-Fault-Handling:** Utilizes Polly for resilience strategies, ensuring the application can handle and recover from transient faults effectively.

**Authentication for Subscribers:** Implements an authentication mechanism for subscribers, allowing for the creation and management of subscriber accounts.

**Link Management:** Users can create, disable, enable, and manage links, including setting expiration dates and adding tags for organization.

**Subscriber Management:** Allows for the creation and management of subscriber accounts, including activation and deactivation of accounts.

## Technical Features
**Clean Architecture:** The project is structured using Clean Architecture principles, separating concerns and making the codebase more maintainable.

**Domain-Driven Design (DDD):** Implements DDD principles in the domain layer, organizing the code around the business domain and its logic.

**Caching with Redis:** Utilizes Redis for caching, enhancing performance and scalability.

**Resilience with Polly:** Integrates Polly for advanced resilience policies, including retries and circuit breakers.

**API Documentation:** Provides Swagger integration for API documentation and testing.

**Global Exception Handling:** Implements global exception handling for centralized error management.

**Validation:** Uses FluentValidation for input validation, ensuring data integrity and providing meaningful error messages.

**Logging and Monitoring:** Incorporates logging and potentially monitoring mechanisms to track the application's health and usage.

**Dependency Injection:** Utilizes dependency injection for managing dependencies, improving code modularity and testability.

**Entity Framework Core:** Leverages EF Core for data access and management, including support for migrations.

**Docker Support:** Includes instructions for setting up a Redis container via Docker, indicating support for containerization.


## Development and Deployment Features
.NET 8 Application: Built with .NET 8, ensuring modern features and performance optimizations.

Database Migration Support: Offers guidance on database migration, facilitating deployment and updates.

Docker Integration: Provides instructions for using Docker, particularly for Redis, which suggests the application is designed with containerization in mind.

This summary encapsulates the features and capabilities of the SwiftLink application as inferred from the provided documentation and code structure.

## 💾 Getting Started

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
For Adding Your Connection String(in Development phase) please add a user-sercret file with visual-studio and locate your connection string there.
For Adding Your Connection String(In Production phase) please add your connection string in appsettings.Production.json, locate the connection string and update it accordingly. This connection string is crucial for updating the database during migrations.

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
OR
* PMC (package manager console)
```
Update-Database
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

Access the application by navigating to https://localhost:44314 in your web browser.

That's it! You've successfully cloned, restored, and run the SwiftLink project with database migration and a Redis container. If you encounter any issues, refer to the project documentation or seek help from the community. Happy coding!

Contributions and bug reports are welcome.

<!-- CONTRIBUTING -->
## 🌈 Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## 🌟 Support this project
If you believe this project has potential, feel free to star this repo just like many <a href="https://github.com/mohammadKarimi/SwiftLink/stargazers">amazing people</a> have.

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-url]: https://github.com/mohammadKarimi/SwiftLink/graphs/contributors
[stars-url]: https://github.com/mohammadKarimi/SwiftLink/stargazers
[forks-url]: https://github.com/mohammadKarimi/SwiftLink/network/members
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/mha-karimi/
[contributors-shield]: https://img.shields.io/github/contributors/mohammadKarimi/SwiftLink.svg?style=for-the-badge
[forks-shield]: https://img.shields.io/github/forks/mohammadKarimi/SwiftLink.svg?style=for-the-badge
[stars-shield]: https://img.shields.io/github/stars/mohammadKarimi/SwiftLink.svg?style=for-the-badge
[issues-shield]: https://img.shields.io/github/issues/mohammadKarimi/SwiftLink.svg?style=for-the-badge
[issues-url]: https://github.com/mohammadKarimi/SwiftLink/issues
[license-shield]: https://img.shields.io/github/license/mohammadKarimi/SwiftLink.svg?style=for-the-badge
[license-url]: https://github.com/mohammadKarimi/SwiftLink/blob/main/LICENSE.txt
[dotnet-version]: https://img.shields.io/badge/dotnet%20version-net8.0-blue
