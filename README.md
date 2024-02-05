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

## SwiftLink - Modern Shortener Link Project
![dotnet-version]

### Introduction
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

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



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