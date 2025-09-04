# Volunteer Management System
[Link to project video:](https://drive.google.com/file/d/1og2zaUf6eYw_3JS2ojj9qU8EpMx-Y7fN/view)
## Overview
VolunteerManagement is a C# WPF-based system designed to manage volunteers, calls, and assignments in an organization. 

The system provides functionality for administrators to manage volunteers, track calls, and assign tasks. Volunteers can view available calls, accept them, and update their status.

## Features

### For Administrators / Managers
- **Volunteer Management:** Add, update, and manage volunteers, including roles, contact information, and availability.  
- **Call Management:** Create, update, or delete calls with details such as address, type, description, and deadlines.  
- **Assignment Management:** Assign calls to volunteers, monitor progress, and generate reports.  
- **Observers & Notifications:** Real-time updates for volunteers and calls to reflect current statuses.

### For Volunteers
- **View Open Calls:** Browse open calls within their available range.  
- **Accept Calls:** Choose a call for handling based on availability and eligibility.  
- **Update Call Status:** End treatment or cancel assigned calls with proper authorization.  
- **Track Assignments:** Monitor ongoing and completed assignments.

## Technology Stack
- **Language:** C#  
- **Frameworks:** WPF for UI  
- **Architecture:** Three-layer architecture (PL, BL, DAL)  
- **Data:** XML files and in-memory lists  
- **Design Patterns & Principles:** Singleton (for DAL), Observer (for UI updates), Dependency Injection (DI)

## Architecture
The system is structured in three layers:

- **Data Access Layer (DAL):** Handles all data storage, retrieval, and CRUD operations.  
- **Business Logic Layer (BL):** Contains the application logic, validation, and business rules.  
- **Presentation Layer (PL):** WPF-based UI for administrators and volunteers, with full data binding.

## Usage
1. Clone the repository.  
2. Open the solution in Visual Studio.  
3. Build the project.  
4. Run the application.  
5. Log in as a volunteer or manager to start managing calls and assignments.

### Administrator login details
- **Username:** Eli Amar0556726282  
- **Password:** 327773271

### Volunteer login details
- **Username:** yaeli shushan0527575821  
- **Password:** 328216437
