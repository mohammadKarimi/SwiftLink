## 📐 SwiftLink - Modern Shortener Link Project

The SwiftLink project is a .NET 8 application designed to streamline the process of link shortening. It provides a comprehensive solution for creating shorter, more manageable URLs, which is particularly useful for sharing links on platforms where space is limited or for tracking and managing links more efficiently. The project includes instructions for setting up the environment, including database migration and the use of a Redis container for caching, indicating a focus on performance and scalability.

## Explain about shorten use-case

https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Commands/GenerateShortCode/GenerateShortCodeCommand.cs
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Commands/GenerateShortCode/GenerateShortCodeCommandHandler.cs
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Commands/GenerateShortCode/GenerateShortCodeValidator.cs

The "Shorten" use case in the SwiftLink project involves generating a shortened version of a given URL. This process is encapsulated within the GenerateShortCodeCommand and its corresponding handler, GenerateShortCodeCommandHandler. Here's a breakdown of how this use case works:

### GenerateShortCodeCommand
This command is a record that acts as a data transfer object (DTO) for the necessary information to generate a shortened link. It includes properties such as:

- GroupName: Optional grouping for the link.
- Url: The original URL to be shortened.
- Description: A description of the link.
- Title: The title of the link.
- ExpirationDate: When the link should expire and become invalid.
- Password: An optional password to protect the link.
- BackHalf: A custom back half for the shortened URL.
- Tags: A list of tags associated with the link.

### GenerateShortCodeCommandHandler
This class handles the logic for generating a shortened link. It involves several steps:

Validation: It first ensures that the provided URL and other parameters meet specific criteria through the GenerateShortCodeValidator.
Short Code Generation: If no custom back half is provided, it generates a unique short code for the URL using the IShortCodeGenerator service.
Link Creation: It creates a new Link entity with the provided details and the generated or provided short code.
Database Insertion: The new link entity is added to the database.
Caching: The newly created link is cached using the ICacheProvider service, with the expiration date set accordingly.
Response: It returns a Result<LinksDto> object, which includes details of the created link, such as the short code, original URL, and expiration date.
Execution Flow in the Controller
The LinkController class exposes an endpoint that handles the "Shorten" use case. The Shorten method in the controller:

Accepts a GenerateShortCodeCommand object from the request body.
Passes this command to the MediatR library, which in turn finds and executes the appropriate handler (GenerateShortCodeCommandHandler).
Returns an HTTP response with the result, which includes the shortened link information if the operation was successful.
This use case is a core feature of the SwiftLink project, allowing users to create shorter, more manageable links that can be shared easily, tracked, and managed.

