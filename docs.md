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

- Validation: It first ensures that the provided URL and other parameters meet specific criteria through the GenerateShortCodeValidator.
- Short Code Generation: If no custom back half is provided, it generates a unique short code for the URL using the IShortCodeGenerator service.
- Link Creation: It creates a new Link entity with the provided details and the generated or provided short code.
- Database Insertion: The new link entity is added to the database.
- Caching: The newly created link is cached using the ICacheProvider service, with the expiration date set accordingly.
- Response: It returns a Result<LinksDto> object, which includes details of the created link, such as the short code, original URL, and expiration date.
Execution Flow in the Controller
The LinkController class exposes an endpoint that handles the "Shorten" use case. The Shorten method in the controller:

Accepts a GenerateShortCodeCommand object from the request body.
Passes this command to the MediatR library, which in turn finds and executes the appropriate handler (GenerateShortCodeCommandHandler).
Returns an HTTP response with the result, which includes the shortened link information if the operation was successful.
This use case is a core feature of the SwiftLink project, allowing users to create shorter, more manageable links that can be shared easily, tracked, and managed.

### What exactly happen when a user click on shorten url?
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Presentation/Controllers/V1/LinkController.cs
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Queries/VisitShortenLink
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Queries/VisitShortenLink/VisitShortenLinkQueryHandler.cs

When a user clicks on a shortened URL, the following process occurs, as handled by the VisitShortenLinkQueryHandler:

- Short Code Resolution: The system receives the short code from the URL and attempts to resolve it to the original URL. This is initiated by the Visit action in the LinkController, which captures the short code and any provided password from the request.
- Cache Check: The handler first checks if there's a cached result for the given short code. If a cached result exists, it deserializes this result into a Link object. This step optimizes performance by reducing database lookups for frequently accessed links.
- Database Query: If no cached result is found, the handler queries the database to find a Link object matching the short code. If no matching link is found, it returns a failure result indicating that the link does not exist.
- Banned Link Check: The handler checks if the link is marked as banned. If it is, it returns a failure result indicating that the link is banned and cannot be visited.
- Expiration Check: It checks if the link has expired (i.e., the current date is beyond the link's expiration date). If the link is expired, it returns a failure result indicating that the link is no longer valid.
- Password Protection Check: If the link is protected by a password, the handler verifies if the provided password matches the stored password for the link. If the passwords do not match, it returns a failure result indicating that the password is invalid.
- Visit Notification: Upon successfully passing the above checks, the handler publishes a VisitLinkNotification with the link's ID and client metadata. This step is typically used for analytics or logging purposes to track link visits.
- Success Response: Finally, if all checks pass, the handler returns a success result with the original URL of the link. The Visit action in the LinkController then redirects the user to this original URL.
This process ensures that only valid, non-expired, and non-banned links are successfully resolved and visited, while also providing mechanisms for analytics and security through password protection.

## Describe why this code uses polly?
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Infrastructure/CacheProvider/RedisCacheProvider.cs

The code uses Polly, a resilience and transient-fault-handling library, to implement robustness in the operations related to caching with Redis. Specifically, Polly is utilized here for its Circuit Breaker pattern, which is a design pattern used to detect failures and encapsulate the logic of preventing a failure from constantly recurring during maintenance, temporary external system failure, or unexpected system difficulties.

Here's why Polly is used in the context of RedisCacheProvider:

### Handling Redis Connection Failures:
Redis operations, like setting, getting, or removing cache entries, can fail due to transient network issues or Redis server problems. Polly's Circuit Breaker policy allows the application to temporarily halt operations to the cache when a predefined threshold of failures is reached, thus preventing a cascade of failures when the Redis server is unavailable or experiencing issues.

### Resiliency:
By using the Circuit Breaker pattern, the system becomes more resilient. It avoids making calls to a system that is likely to fail, which can help in maintaining the stability of the application. Once the circuit is open, it prevents the application from performing the operation that is likely to fail, until a specified amount of time has passed and the circuit closes again, allowing the operation to be attempted once more.

### Fail Fast:
The Circuit Breaker policy allows the application to fail fast. When the circuit is open, it immediately returns a failure response without attempting the operation, which can help in reducing the load on the failing system and improve the response time for the user or calling service.

### Recovery and Self-Healing:
The Circuit Breaker automatically attempts to close the circuit after a specified "break duration," allowing for a retry of the operation. This self-healing behavior ensures that the application can recover from transient faults without requiring manual intervention.

In the RedisCacheProvider class, Polly's Circuit Breaker policies are configured for different cache operations (Set, Remove, Get). These policies define the conditions under which the circuit should open (e.g., on a RedisConnectionException) and the duration for which the circuit remains open before allowing another attempt. This approach enhances the reliability and stability of cache operations within the SwiftLink application, especially in scenarios where the Redis cache might be temporarily inaccessible or unreliable.

## Endpoints
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Presentation/Controllers/V1/LinkController.cs
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Presentation/Controllers/V1/SubscriberController.cs

Based on the provided information from the LinkController and SubscriberController within the SwiftLink.Presentation/Controllers/V1 directory, here is a list of the endpoints in the SwiftLink project and a description of each:

### LinkController Endpoints
### POST /Shorten
Description: This endpoint is used to create a shortened URL. It accepts a GenerateShortCodeCommand object in the request body, which likely includes the original URL and possibly other metadata for generating the short code.
Use Case: When a user wants to shorten a URL.

### GET /{shortCode}
Description: This endpoint is responsible for redirecting a user to the original URL associated with the given short code. It may also accept a password query parameter if the link is protected.
Use Case: When a user visits a shortened URL.

### GET /List
Description: Lists all the shortened links based on the criteria provided in the ListOfLinksQuery object.
Use Case: To retrieve a list of shortened links, possibly for administrative or user dashboard purposes.

### GET /Count
Description: Returns a count of visits to shortened links based on the CountVisitShortenLinkQuery object.
Use Case: To get analytics or metrics on the usage of shortened links.

### PUT /Update
Description: Updates an existing shortened link. It accepts an UpdateLinkCommand object, which likely includes the ID of the link to update and the new details.
Use Case: When a user needs to modify the details of an existing shortened link.

### DELETE /Disable/{id}
Description: Disables a shortened link based on the provided ID.
Use Case: To disable or "soft delete" a shortened link, making it inaccessible to users.

### GET /GetByGroupName
Description: Retrieves shortened links by a group name specified in the GetLinkByGroupNameQuery object.
Use Case: Useful for organizing and retrieving links that are grouped under a specific category or tag.

### GET /Inquery
Description: Inquires about the availability or status of a back half (the part of the URL after the domain) based on the InquiryBackHalfQuery object.
Use Case: Before creating a custom back half for a shortened link, this can check if the desired back half is available.

SubscriberController Endpoints
### POST /Add
Description: Adds a new subscriber to the system. It accepts an AddSubscriberCommand object, which likely includes subscriber details such as email.
Use Case: When a new user subscribes to the service, possibly for updates or newsletters.

Each endpoint serves a specific function within the SwiftLink application, ranging from URL shortening and redirection to subscriber management and analytics.


## Architecture

The SwiftLink project employs Clean Architecture, which organizes the application into distinct layers with specific responsibilities. This architecture aims to create systems that are independent of UI, databases, frameworks, and external agencies, making them more maintainable, scalable, and adaptable to change. Here's a breakdown of each layer as observed in the SwiftLink project:

### 1. **Domain Layer (`SwiftLink.Domain`)**

- **Responsibilities**: This is the innermost layer and represents the business and application domain. It contains enterprise logic and types, including entities, enums, exceptions, interfaces, and domain events. The domain layer defines the business rules and how business objects interact with one another but is independent of any application logic, database, or external concerns.
  
- **Implementation**: In SwiftLink, the domain layer includes interfaces like `IEntity` and attributes like `EntityAttribute` for marking and identifying domain entities. These entities are the core business objects manipulated by the application.

### 2. **Application Layer (`SwiftLink.Application`)**

- **Responsibilities**: This layer contains application logic and defines the jobs the software is supposed to do. It orchestrates the flow of data to and from the domain layer, and directs the expressive domain models to work out business problems. This layer is where the use cases of the application are implemented. It's also responsible for validation, authorization, and running business rules on the data that flows between the UI and the domain layer.
  
- **Implementation**: The SwiftLink application layer implements functionalities like logging, validation, and handling of business commands and queries using MediatR. It also includes DTOs (Data Transfer Objects) for transferring data between processes, and configurations for mapping domain entities to DTOs.

### 3. **Infrastructure Layer (`SwiftLink.Infrastructure`)**

- **Responsibilities**: This layer supports the application and domain layers with technical capabilities such as database access, file system manipulation, network calls, and so on. It implements interfaces defined in the application layer, providing concrete implementations that are specific to the technologies chosen (e.g., Entity Framework Core for ORM, Redis for caching).
  
- **Implementation**: In SwiftLink, the infrastructure layer includes the `RedisCacheService` for caching, configurations for entity models, and the application's DbContext for database operations. It also contains services for initializing and seeding the database, and extensions for registering infrastructure services like DbContext and RedisCacheService in the application's dependency injection container.

### 4. **Presentation Layer (`SwiftLink.Presentation`)**

- **Responsibilities**: This outermost layer is where the application interacts with the external world. It handles HTTP requests, translates them into commands or queries, and returns the response to the client. This layer is concerned with presenting information to the user and interpreting user commands. It's also where the application's configuration settings, such as caching, logging, and other infrastructure concerns, are defined.
  
- **Implementation**: The SwiftLink presentation layer likely includes controllers, view models, and views (in the case of a web application). It defines endpoints for the application's REST API, handling requests to shorten URLs, redirect, and manage subscribers. It also includes application configuration settings for aspects like caching, logging, and allowed hosts.

### Summary

Clean Architecture in SwiftLink ensures that the application is easy to maintain and adapt to changes, whether those changes are in the form of new business rules, new UI requirements, or changes in external dependencies like databases or web services. Each layer has clear responsibilities and dependencies that flow inwards, meaning inner layers are not dependent on outer layers. This setup allows for greater flexibility and easier testing.

## What is the mechanism of authentication for subscriber?

The authentication mechanism for subscribers in the SwiftLink project involves token validation, as implemented in the `SubscriberAuthorizationBehavior<TRequest, TResponse>` class. Here's a step-by-step breakdown of how it works:

- **Token Check**: The behavior first checks if the incoming request is marked as `IAnonymousRequest`. If so, it bypasses the authentication check, allowing the request to proceed without a token. This is useful for operations that do not require authentication.

- **Token Validation**: If the request requires authentication (i.e., not an `IAnonymousRequest`), the behavior checks if the `IUser` instance (injected into the behavior) has a non-null token. If the token is null, it throws a `SubscriberUnAuthorizedException`, indicating that the request is unauthorized due to the absence of a valid token.

- **Subscriber Verification**: The behavior then queries the database for a `Subscriber` entity that matches the token provided by the `IUser` instance and is marked as active (`IsActive`). This is done using the `_dbContext` to access the `Subscriber` entities. If no matching active subscriber is found, it throws a `SubscriberUnAuthorizedException`.

- **Context Setting**: Upon successful verification, the behavior sets the subscriber's ID and Name in the `_sharedContext`, making these details available for subsequent operations within the same request processing pipeline.

- **Proceeding with Request**: Finally, if the subscriber is successfully authenticated, the behavior invokes the next delegate in the request processing pipeline, allowing the request to proceed to its intended handler.

This authentication mechanism ensures that only requests with a valid and active subscriber token can access certain operations within the SwiftLink application. Unauthorized requests are effectively blocked, enhancing the security of the application by ensuring that only registered and active subscribers can perform actions that require authentication.