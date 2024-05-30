using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests;

public abstract class BaseTests : WebApplicationFactory<Program>
{
    protected readonly HttpClient _client;

    protected BaseTests()
    {
        _client = CreateClient();
    }
}