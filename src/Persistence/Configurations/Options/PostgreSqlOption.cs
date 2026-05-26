namespace Persistence.Configurations.Options;

public class PostgreSqlOption
{
    public const string Key = "PostgreSQL";

    public string Database { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Port { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string ServerReader { get; init; } = string.Empty;
    public string ServerWriter { get; init; } = string.Empty;

    public string GetConnectionString(bool isReader = true)
    {
        var server = isReader ? ServerReader : ServerWriter;
        return $"Host={server};Port={Port};Pooling=true;Database={Database};User Id={User};Password={Password}";
    }
}