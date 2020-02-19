Feature: AdminUpdateTour
	In order to verify a tour updating
	As an administrator
	I want to go through the processes and get back to the initial page

@mytag
Scenario: Update Tour
	Given I have gone to the first tour editing page "Tour Type"
	And I have clicked next button to "Overview" Page
	And I have clicked next button to "Booking Details" Page
	And I have clicked next button to "Schedule & Price" Page
	And I have clicked next button to "Pictures" Page
	When I press next
	Then the url should be changed to /Admin/Console/Tour
