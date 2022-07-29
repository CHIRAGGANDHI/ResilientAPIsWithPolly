using Moq;
using Moq.Protected;

namespace ResilientAPIsWithPolly.Tests
{
    public class ReqresApiClientUnitTests
    {
        [Fact]
        public async Task GetUserShouldReturnASuccessStatusCodeAfterAnInitialUnsuccessfulRequest()
        {
            //Arrange
            var userId = 1;
            Mock<HttpMessageHandler> handlerMock = BuildMockHttpHandlerFailOnFirstRequestThenSucceedOnSecond();

            var httpClient = new HttpClient(handlerMock.Object);
            var resilientApisWithPolly = new ReqresAPIClient(httpClient);

            // Act
            var httpResponseMessage = await resilientApisWithPolly.GetUser(userId);

            // Assert
            Assert.True(httpResponseMessage.IsSuccessStatusCode);

        }


        private static Mock<HttpMessageHandler> BuildMockHttpHandlerFailOnFirstRequestThenSucceedOnSecond()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var initialFailureResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };

            var retrySecondRequestResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.Accepted
            };

            handlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(initialFailureResponse)
                .ReturnsAsync(retrySecondRequestResponse);

            return handlerMock;

        }
    }
}
