namespace SwiftLink.Application;

internal static class Constants
{
    public static class Database
    {
        public static Error InsertFailed = Error.Failure("Database.Failed", "Insert into db is failed :(");
    }


    public static class Subscriber
    {
        public static Error TokenMustBeSent = Error.Validation("UnAuthorized", "Token must be sent!");

        public static Error InvalidToken = Error.NotFound("UnAuthorized", "Invalid User!");
    }
}
