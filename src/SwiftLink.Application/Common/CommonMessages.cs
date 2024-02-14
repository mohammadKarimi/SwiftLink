namespace SwiftLink.Application.Common;

public static class CommonMessages
{
    public static class Database
    {
        public static readonly Error InsertFailed = Error.Exception("Database.Failed", "Insert into db is failed :(");
        public static readonly Error UpdateFailed = Error.Exception("Database.Failed", "Update record in db is failed :(");
    }
}
