Feature: Booker API CRUD Operations
  As a user
  I want to manage hotel bookings
  So that I can create, view, update, and delete reservations

  Scenario: I create a new booking 
	Given the Booker API is available
    When I create a new booking for "Mihajlo Student"
    Then the response status code should be 200
    And a valid booking ID should be returned

  Scenario: I read an existing booking 
    Given the Booker API is available
    And a booking exists for "Mihajlo Student"
    When I request the details of this booking
    Then the response status code should be 200

  Scenario: I fully update a booking 
    Given the Booker API is available
    And I am authenticated as an administrator
    And a booking exists for "Mihajlo Student"
    When I fully update the booking
    Then the response status code should be 200

  Scenario: I partially update a booking
    Given the Booker API is available
    And I am authenticated as an administrator
    And a booking exists for "Mihajlo Student"
    When I update only the firstname to "Mihajlo Patch"
    Then the response status code should be 200

  Scenario: I delete a booking 
    Given the Booker API is available
    And I am authenticated as an administrator
    And a booking exists for "Mihajlo Student"
    When I delete this booking
    Then the response status code should be 201