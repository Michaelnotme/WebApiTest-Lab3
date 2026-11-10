using NUnit.Framework;
using Reqnroll;
using RestSharp;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiTest.StepDefinitions 
{
    [Binding]
    public class RandomFoxStepDefinitions
    {
        private readonly RestClient _client = new RestClient("https://randomfox.ca");
        private RestResponse _response;

        [Given(@"the Random Fox API is online and ready")]
        public void GivenTheRandomFoxAPIIsOnlineAndReady()
        {
            Assert.That(_client, Is.Not.Null);
        }

        [When(@"I ask for a random fox picture")]
        public async Task WhenIAskForARandomFoxPicture()
        {
            var request = new RestRequest("floof/", Method.Get);
            request.AddHeader("Accept", "application/json");
            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"the server should return a successful response with status (.*)")]
        public void ThenTheServerShouldReturnASuccessfulResponseWithStatus(int expectedCode)
        {
            Assert.That((int)_response.StatusCode, Is.EqualTo(expectedCode));
        }

        [Then(@"the response must contain a valid image URL")]
        public void ThenTheResponseMustContainAValidImageURL()
        {
            using JsonDocument doc = JsonDocument.Parse(_response.Content);
            var root = doc.RootElement;

            bool hasImageProperty = root.TryGetProperty("image", out JsonElement imageElement);
            Assert.That(hasImageProperty, Is.True, "JSON response does not contain 'image' property");

            string imageUrl = imageElement.GetString();
            Assert.That(imageUrl, Is.Not.Empty, "The image URL is empty");
            Assert.That(imageUrl.StartsWith("http"), Is.True, "The returned string is not a valid web link");
        }
    }
}