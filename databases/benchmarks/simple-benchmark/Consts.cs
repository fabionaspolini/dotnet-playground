namespace SimpleBenchmarkPlayground;

public class Consts
{
#if MARIADB
    public const string DatabaseEngine = "MariaDb";
#elif MYSQL
    public const string DatabaseEngine = "MySql";
#elif POSTGRESQL
    public const string DatabaseEngine = "PostgreSql";
#elif SQLSERVER
    public const string DatabaseEngine = "SqlServer";
#else
    public const string DatabaseEngine = "Indefined";
#endif
}