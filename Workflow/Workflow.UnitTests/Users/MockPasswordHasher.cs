using Workflow.Core.Users;

namespace Workflow.UnitTests.Users;

public class MockPasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return $"hashed_{password}";
    }

    public bool Verify(string password, string hash)
    {
        return hash == $"hashed_{password}";
    }
}