# FccuOpsLite

## Deployed App

1. https://fccuopslite-prod-h2arhjh6b5ddhdcj.ukwest-01.azurewebsites.net/

## Overview

FccuOpsLite is a full stack ASP.NET Core MVC web application that simulates an internal loan operations system for a financial institution.

The app allows authenticated users to manage members, create and review loan applications, track application status, and access data through both a web interface and API endpoints. It was built to demonstrate practical experience with ASP.NET Core MVC, MySQL, role based access control, Web API development, and Azure deployment in a way that reflects real business software.

## Features

1. Secure authentication with ASP.NET Core Identity

2. Role based access control for Admin, LoanOfficer, and Viewer users

3. Member management through an MVC interface

4. Loan application creation, review, and status updates

5. Status workflow for Submitted, UnderReview, Approved, and Rejected applications

6. API endpoints that return loan data in JSON and XML

7. Reporting support for operational visibility

8. MySQL integration through Entity Framework Core

9. Azure ready deployment setup

10. GitHub version control for source management

## Architecture

FccuOpsLite follows a layered architecture to keep responsibilities clear and maintainable.

1. Controllers handle incoming HTTP requests and return views or API responses

2. Services contain business logic and workflow rules

3. Repositories manage data access and database interaction

4. Domain models represent core business entities

5. View models shape data for forms and Razor views

6. ApplicationDbContext connects the app to MySQL through Entity Framework Core

This structure keeps presentation logic, business logic, and data access logic separated and easier to maintain.

## Tech Stack

1. ASP.NET Core MVC

2. C#

3. Entity Framework Core

4. MySQL

5. ASP.NET Core Identity

6. Razor Views

7. Web API with JSON and XML support

8. Azure App Service

9. GitHub

## Core Entities

### Member

Represents a financial institution member and stores core contact and identification details.

### LoanApplication

Represents a loan request tied to a member and includes amount, type, notes, and workflow status.

### ApplicationUser

Extends the default Identity user for authentication and role based authorization.

## Request Flow

A typical request moves through the application in this order:

1. The user submits a request from the browser

2. The controller receives the request

3. The controller calls the appropriate service

4. The service applies business rules

5. The repository interacts with the database through Entity Framework Core

6. The result is returned as a Razor view or API response

## Security

Security is built into the application design.

1. Authentication is required for protected areas

2. Authorization is enforced through roles

3. Sensitive actions are limited to approved users

4. Server side validation helps protect data integrity

## API

The application includes API endpoints for integration scenarios.

Example endpoints include:

1. `/api/loanapplications`

2. `/api/loanapplications/{id}`

3. `/api/loanapplications/export/{id}`

The API supports both JSON and XML responses.
