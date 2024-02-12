namespace SwiftLink.Application.Common;

public static class CommonMessages
{
    public static class Database
    {
        public static readonly Error InsertFailed = Error.Failure("Database.Failed", "Insert into db is failed :(");
        public static readonly Error UpdateFailed = Error.Failure("Database.Failed", "Update record in db is failed :(");
    }
}
