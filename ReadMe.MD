# Overview #
The remote advisor sample is a simple way to integrate Skype for Business Video into your website.
The pattern in the solution is geared around business-to-consumer scenarios where anonymous join would typically be used.

### The included solution will use: ###
* Azure Website
* Azure SQL Database
* Azure AD Application for Anonymous Join

Key Views used in the application
<br/>
### Session Queue ###
<img src="https://raw.githubusercontent.com/devwheel/RemoteAdvisorQuickStart/master/RemoteAdvisor/Media/Image1.jpg" width=600/>

### Create Session ###
<img src="https://raw.githubusercontent.com/devwheel/RemoteAdvisorQuickStart/master/RemoteAdvisor/Media/image2.jpg" width=600/>

### Join with Edge ###
<img src="https://raw.githubusercontent.com/devwheel/RemoteAdvisorQuickStart/master/RemoteAdvisor/Media/image3.jpg" width=600/>

## Configuration Video on YouTube ##
[![Getting Started Video](http://img.youtube.com/vi/rHc5U9VpehU/0.jpg)](http://www.youtube.com/watch?v=rHc5U9VpehU)

To Install and get running follow the below steps:
1) Right-Click on solution, and choose restore, Nuget Packages
2) Right-Click on solution, Publish to an Azure Web Application
3) Create Azure Active Directory for your Office 365 tenant
	Setup Endpoint (see sdk)
4) Modify Web Config.... with app information
