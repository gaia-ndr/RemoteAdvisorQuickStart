# Overview #
The remote advisor sample is a simple way to integrate Skype for Business Video into your website.
The pattern in the solution is geared around business-to-consumer scenarios where anonymous join would typically be used.

### The included solution will use: ###
* Azure Website
* Azure SQL Database
* Azure AD Application for Anonymous Join

Key Views used in the application
<br/>
>### Session Queue ###
><img src="https://remote-advisor.azurewebsites.net/media/image1.jpg?v=1" width=600/>

>### Create Session ###
><img src="https://remote-advisor.azurewebsites.net/media/image2.jpg?v=1" width=600/>

>### Join with Edge ###
><img src="https://remote-advisor.azurewebsites.net/media/image3.jpg?v=1" width=600/>

## Configuration Video on YouTube ##
[![Getting Started Video](https://remote-advisor.azurewebsites.net/media/image4.jpg?v=1)](http://www.youtube.com/watch?v=rHc5U9VpehU)

## Configuration Notes ##
To Install and get running follow the below steps:
1) Right-Click on solution, and choose restore, Nuget Packages
2) Create an Azure SQL Database and get the connection string added to the web.config
3) Create an Azure Website and download the publish profile 
4) Right-Click on solution, Publish to an Azure Web Application
5) Create Azure Active Directory for your Office 365 tenant
	Setup Endpoint (see sdk)
6) Modify Web Config.... with app information
