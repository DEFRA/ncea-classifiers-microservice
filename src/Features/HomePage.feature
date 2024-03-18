Feature: HomePage
		User should be presented with Design features
		Behaviours Quick Search Object
		User performs onward journeys

#Verify User cannot able to perform Quick Search from Home Page when enters less than 4 characters of Input value
@Chrome
Scenario: Perform Quick Search enter less than 4 characters
	Given User navigate to homepage by launching "SandBoxURL"
	When user enters search term less than four characters "NCE" excluding separators
	Then user observes the dormant behavior of search button
