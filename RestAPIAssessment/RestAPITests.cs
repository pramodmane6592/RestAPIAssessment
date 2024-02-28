namespace RestAPIAssessment;
using NUnit.Framework;
using RestSharp;
using System;
using System.Text;
using System.Text.Json;

[TestFixture]
public class RestAPITests
{
    //private RestClient client;
    //private string baseUrl = "http://your-api-url.com"; // Update this with API base URL
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        //client = new RestClient(baseUrl);
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://your-api-url/");
    }  

    [Test]
    public async Task TestGetNextBirthday_ValidDateOfBirth()
    {
        var dateOfBirth = new DateTime(1990, 6, 15);
        var input = new { DateOfBirth = dateOfBirth };

        var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/GetNextBirthday", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<NextBirthdayResponse>(responseContent);

        Assert.IsNotNull(output);
        Assert.IsNotNull(output.NextBirthday);
        Assert.IsTrue(output.NextBirthday > dateOfBirth);
    }

    [Test]
    public async Task GetNextBirthday_InvalidInput_ReturnsBadRequest()
    {
        var invalidDateOfBirth = "InvalidDate";

        var input = new { DateOfBirth = invalidDateOfBirth };
        var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/GetNextBirthday", content);

        Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
    }
}

public class NextBirthdayResponse
{
    public DateTime NextBirthday { get; set; }
}

