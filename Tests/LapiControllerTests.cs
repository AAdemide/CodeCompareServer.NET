using System.Net;
using System.Security.Claims;
using CodeCompareServer.Controllers;
using CodeCompareServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Xunit;

namespace CodeCompareServer.Tests;

public class LapiControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task Daily_ReturnsDailyQuestion()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"data\": { \"activeDailyCodingChallengeQuestion\": { \"link\": \"/some/link\" } } }")
            })
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com") };
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(_ => _.CreateClient("LapiClient")).Returns(httpClient);

        var controller = new LapiController(GetDbContext(), mockFactory.Object);

        var result = await controller.Daily() as OkObjectResult;
        Assert.NotNull(result);
    }
}
