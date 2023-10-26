# TestPipelineV5

**This template requires Visual Studio 2022 or later**

## Table Of Contents

* [How to use](#how-to-use)
* [Repository Setup](#how-to-setup-a-repo)
* [What is Included](#what-is-included)
* Individual Components Documentation
  * [Unhandled Exception Middleware](/Markdowns/UnhandledExceptionMiddleware.md)
  * [Request Response Logging](/Markdowns/HttpLogging.md)
  * [Bearer Token Authentication](/Markdowns/BearerTokenAuthentication.md)
  * [Swagger Annotation](/Markdowns/SwaggerAnnotation.md)
  * [Distributed Caching](/Markdowns/DistributedCaching.md)
  * [Terraform Setup](/Markdowns/TerraformSetup.md)

## How to use

* Clone Project down to local repository
  ![Clone Repository From Github](./images/CloneRepositoryFromGitHub.PNG)

  ![Clone into Git Bash](./images/CloneIntoGitBash.PNG)

* Navigate to the cloned project and run dotnet new install .

  ![Install the Template Project](./images/CreateTemplate.PNG)

* This will add the API Template Project as a selection to create in a Visual Studio Project

  ![Installed Template Project](./images/SECURAInsuranceTemplate.PNG)

* To Uninstall Template In the same location where you installed run:

  dotnet new uninstall .

## How to Setup a Repo

<a href="https://securainsurance.sharepoint.com/:w:/r/teams/og-coe-appdev/_layouts/15/Doc.aspx?sourcedoc=%7B0F1FCE3B-B7BC-4133-90E4-E63F874A7AE7%7D&file=Setting%20Up%20a%20GitHub%20Repository.docx&action=default&mobileredirect=true">Here</a> you can find information for setting up a repository from scratch.

## What is Included

* Logging
* Dependency Injection
* Scaffold Project/Folder Structure
* Utilities Endpoints
  * Test Exception Logging
  * Health Check
* [Unhandled Exception Handling](/Markdowns/UnhandledExceptionMiddleware.md)
* [Request Response Logging](/Markdowns/HttpLogging.md)
* [Bearer Token Authentication](/Markdowns/BearerTokenAuthentication.md)
* [Swagger Annotation](/Markdowns/SwaggerAnnotation.md)
* [Compile-time Enforced Nullability Checks](/Markdowns/Nullability.md)
* [Terraform Setup](/Markdowns/TerraformSetup.md)
