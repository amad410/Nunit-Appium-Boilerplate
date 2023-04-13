Feature: AppiumWebView

@mytag
Scenario: Search on Google
	Given we navigate to home
	And and enter "C#, Appium and Visual Studio"
	When hitting search
	Then the result should be displayed