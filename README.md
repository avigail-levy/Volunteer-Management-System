**Volunteer Management System**
**Overview**
VolunteerManagement is a C# WPF-based system designed to manage volunteers, calls, and assignments in an organization.
The system provides functionality for administrators to manage volunteers, track calls, and assign tasks. 
Volunteers can view available calls, accept them, and update their status.

**Features**
For Administrators / Managers:
Volunteer Management: Add, update, and manage volunteers, including roles, contact information, and availability.
Call Management: Create, update, or delete calls with details such as address, type, description, and deadlines.
Assignment Management: Assign calls to volunteers, monitor progress, and generate reports.
Observers & Notifications: Real-time updates for volunteers and calls to reflect current statuses.

**For Volunteers:**
View Open Calls: Browse open calls within their available range.
Accept Calls: Choose a call for handling based on availability and eligibility.
Update Call Status: End treatment or cancel assigned calls with proper authorization.
Track Assignments: Monitor ongoing and completed assignments.

**Technology Stack**

Language: C#
Frameworks: WPF for UI
Architecture: Three-layer architecture (PL, BL, DAL)
Data: XML files and in-memory lists
Design Patterns: Singleton (for DAL), Observer pattern (for UI updates),Dependency Injection

**Architecture**
The system is structured in three layers:
Data Access Layer (DAL): Handles all data storage, retrieval, and CRUD operations.
Business Logic Layer (BL): Contains the application logic, validation, and business rules.
Presentation Layer (PL): WPF-based UI for administrators and volunteers, with full data binding.

**Usage**
Clone the repository.
Open the solution in Visual Studio.
Build the project.
Run the application.
Log in as a volunteer or manager to start managing calls and assignments
Administrator login details: username:Eli Amar0556726282
                             password:327773271

Volunteer login details:  username: yaeli shushan0527575821
                          password:328216437




