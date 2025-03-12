using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;


namespace InfrastructureTest;
public class MssqlTestContainer
{
    private readonly MsSqlContainer _container;

    public MssqlTestContainer()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithPassword("SuperStrong@Passw0rd")
            .WithPortBinding(1433, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();
    }

    public async Task StartAsync()
    {
        await _container.StartAsync();
    }

    public async Task StopAsync()
    {
        await _container.StopAsync();
    }

    public string GetConnectionString()
    {
        return _container.GetConnectionString() + ";Database=TestDb;Encrypt=false;";
    }

}