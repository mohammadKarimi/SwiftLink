namespace SwiftLink.Application;

internal static class Constants
{
    public static class Database
    {
        public static Error InsertFailed = new("Database.Failed", "Insert into db is failed :(");
    }

    public static class Link
    {
        public static Error UrlMustBeSent = new("InValidFormat", "Url must be sent!");

        public static Error InvalidUrlFormat = new("InValidFormat", "Invalid Url Format!");
    }

    public static class Subscriber
    {
        public static Error TokenMustBeSent = new("UnAuthorized", "Token must be sent!");

        public static Error InvalidToken = new("UnAuthorized", "Invalid User!");
    }
}
