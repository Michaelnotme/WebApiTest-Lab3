Feature: Random Fox API
  As user
  I want to get a random picture of a fox
  So that I can see a fox and test the public API

  Scenario: I successfully retrieve a random fox picture
    Given the Random Fox API is online and ready
    When I ask for a random fox picture
    Then the server should return a successful response with status 200

  Scenario: I verify the response contains an image link
    Given the Random Fox API is online and ready
    When I ask for a random fox picture
    Then the response must contain a valid image URL