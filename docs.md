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

### What exactly happen when a user click on shorten url?
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Presentation/Controllers/V1/LinkController.cs
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Queries/VisitShortenLink
https://github.com/mohammadkarimi/swiftlink/blob/main/src/SwiftLink.Application/UseCases/Links/Queries/VisitShortenLink/VisitShortenLinkQueryHandler.cs

When a user clicks on a shortened URL, the following process occurs, as handled by the VisitShortenLinkQueryHandler:
Short Code Resolution: The system receives the short code from the URL and attempts to resolve it to the original URL. This is initiated by the Visit action in the LinkController, which captures the short code and any provided password from the request.
Cache Check: The handler first checks if there's a cached result for the given short code. If a cached result exists, it deserializes this result into a Link object. This step optimizes performance by reducing database lookups for frequently accessed links.
Database Query: If no cached result is found, the handler queries the database to find a Link object matching the short code. If no matching link is found, it returns a failure result indicating that the link does not exist.
Banned Link Check: The handler checks if the link is marked as banned. If it is, it returns a failure result indicating that the link is banned and cannot be visited.
Expiration Check: It checks if the link has expired (i.e., the current date is beyond the link's expiration date). If the link is expired, it returns a failure result indicating that the link is no longer valid.
Password Protection Check: If the link is protected by a password, the handler verifies if the provided password matches the stored password for the link. If the passwords do not match, it returns a failure result indicating that the password is invalid.
Visit Notification: Upon successfully passing the above checks, the handler publishes a VisitLinkNotification with the link's ID and client metadata. This step is typically used for analytics or logging purposes to track link visits.
Success Response: Finally, if all checks pass, the handler returns a success result with the original URL of the link. The Visit action in the LinkController then redirects the user to this original URL.
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