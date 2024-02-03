namespace SwiftLink.Shared.Tests;

public class StringHasherExtensionTests
{
    [Fact]
    public void Hash_ReturnsCorrectHash()
    {
        // Arrange
        string value = "password";
        string salt = "somesalt";

        // Act
        string hashedResult = value.Hash(salt);

        // Assert
        Assert.NotNull(hashedResult);
        Assert.NotEmpty(hashedResult);
        Assert.Equal("a8621T1RoRw73nfoyv4fFSeCxeUqE+UU2hKp41sMK8s=", hashedResult);
    }
}
