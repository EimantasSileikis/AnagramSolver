using AnagramSolver.WebApp.Controllers.Api;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.Tests.ControllersTests
{
    public class DownloadsControllerTests
    {
        [Test]
        public async Task DownloadFile_Always_ReturnsFileContentResult()
        {
            var controller = new DownloadsController();

            var result = await controller.DownloadFile();

            Assert.That(result, Is.InstanceOf<FileContentResult>());
        }
    }
}
