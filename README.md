# Welcome to the NCEA Classifier Microservice Repository

This is the code repository for the NCEA Classifier Microservice codebase.

[Wiki](https://dev.azure.com/defragovuk/DEFRA-NCEA/_wiki/wikis/NCEA-BETA%20Team.Wiki/25361/NCEA-Classifiers)

# Prerequisites

Before proceeding, ensure you have the following installed:

- .NET 8 SDK: You can download and install it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0).

# Configuration
1. ConnectionStrings : DefaultConnection

	- Server=devncedbssq1401.postgres.database.azure.com;Database=ncea-classifier;Port=5432;Username=xxx@Defra.onmicrosoft.com;Ssl Mode=Require
	- Defra.onmicrosoft.com account needed to be used to get access on Azure PostGreSQL

2. KeyVaultUri : https://devnceinfkvt1401.vault.azure.net/