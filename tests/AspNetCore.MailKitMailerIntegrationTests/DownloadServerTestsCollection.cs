using Xunit;

namespace AspNetCore.MailKitMailerIntegrationTests
{
    /// <summary>
    /// Collection definition for tests that use the download server.
    /// Tests in this collection run sequentially to avoid port conflicts.
    /// </summary>
    [CollectionDefinition("DownloadServerTests", DisableParallelization = true)]
    public class DownloadServerTestsCollection : ICollectionFixture<object>
    {
    }
}
