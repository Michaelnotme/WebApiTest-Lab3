using NUnit.Framework;
using Reqnroll;
using RestSharp;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiTest.StepDefinitions
{
    [Binding]
    public class BookerStepDefinitions
    {
        private readonly RestClient _client = new RestClient("http://restful-booker.herokuapp.com");
        private RestResponse _response;
        private string _token;
        private int _bookingId;


        [Given(@"the Booker API is available")]
        public void GivenTheBookerAPIIsAvailable()
        {
            Assert.That(_client, Is.Not.Null);
        }

        [Given(@"I am authenticated as an administrator")]
        public async Task GivenIAmAuthenticatedAsAnAdministrator()
        {
            var request = new RestRequest("auth", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(new { username = "admin", password = "password123" });

            var response = await _client.ExecuteAsync(request);
            using JsonDocument doc = JsonDocument.Parse(response.Content);
            _token = doc.RootElement.GetProperty("token").GetString();
        }

        [Given(@"a booking exists for ""(.*)""")]
        public async Task GivenABookingExistsFor(string name)
        {
            var request = new RestRequest("booking", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(new
            {
                firstname = name,
                lastname = "Test",
                totalprice = 100,
                depositpaid = true,
                bookingdates = new { checkin = "2026-12-01", checkout = "2026-12-02" }
            });

            var response = await _client.ExecuteAsync(request);
            using JsonDocument doc = JsonDocument.Parse(response.Content);
            _bookingId = doc.RootElement.GetProperty("bookingid").GetInt32();
        }


        [When(@"I create a new booking for ""(.*)""")]
        public async Task WhenICreateANewBookingFor(string name)
        {
            var request = new RestRequest("booking", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(new
            {
                firstname = name,
                lastname = "Student",
                totalprice = 100,
                depositpaid = true,
                bookingdates = new { checkin = "2026-12-01", checkout = "2026-12-02" },
                additionalneeds = "Breakfast"
            });

            _response = await _client.ExecuteAsync(request);
            if (_response.IsSuccessStatusCode)
            {
                using JsonDocument doc = JsonDocument.Parse(_response.Content);
                _bookingId = doc.RootElement.GetProperty("bookingid").GetInt32();
            }
        }

        [When(@"I request the details of this booking")]
        public async Task WhenIRequestTheDetailsOfThisBooking()
        {
            var request = new RestRequest($"booking/{_bookingId}", Method.Get);
            request.AddHeader("Accept", "application/json");
            _response = await _client.ExecuteAsync(request);
        }

        [When(@"I fully update the booking")]
        public async Task WhenIFullyUpdateTheBookingLastnameTo()
        {
            var request = new RestRequest($"booking/{_bookingId}", Method.Put);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cookie", $"token={_token}");
            request.AddJsonBody(new
            {
                firstname = "Mihajlo",
                lastname = "NewLastName",
                totalprice = 150,
                depositpaid = false,
                bookingdates = new { checkin = "2026-12-01", checkout = "2026-12-02" },
                additionalneeds = "Lunch"
            });
            _response = await _client.ExecuteAsync(request);
        }

        [When(@"I update only the firstname to ""(.*)""")]
        public async Task WhenIUpdateOnlyTheFirstnameTo(string newFirstname)
        {
            var request = new RestRequest($"booking/{_bookingId}", Method.Patch);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cookie", $"token={_token}");
            request.AddJsonBody(new { firstname = newFirstname });
            _response = await _client.ExecuteAsync(request);
        }

        [When(@"I delete this booking")]
        public async Task WhenIDeleteThisBooking()
        {
            var request = new RestRequest($"booking/{_bookingId}", Method.Delete);
            request.AddHeader("Cookie", $"token={_token}");
            _response = await _client.ExecuteAsync(request);
        }


        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedCode)
        {
            Assert.That((int)_response.StatusCode, Is.EqualTo(expectedCode));
        }

        [Then(@"a valid booking ID should be returned")]
        public void ThenAValidBookingIDShouldBeReturned()
        {
            Assert.That(_bookingId, Is.Not.Null);
        }
    }
}