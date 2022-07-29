namespace ResilientAPIsWithPolly.Tests
{
    public class ReqresApiClientIntegrationTests
    {
        private static HttpClient _httpClient = new HttpClient();

        [Fact]
        public async Task GetUserShouldReturnASuccessStatusCode()
        {
            //Arrange
            var userId = 1;
            var resilientApisWithPolly = new ReqresAPIClient(_httpClient);

            //Act
            var httpResponseMessage = await resilientApisWithPolly.GetUser(userId);

            //Assert
            Assert.True(httpResponseMessage.IsSuccessStatusCode);
        }
    }
}