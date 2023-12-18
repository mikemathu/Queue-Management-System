# Queue Management System

## Technology Stack

- ASP.NET Core 6.0
- PostgreSQL
- FastReport.Net
- Npgsql

## Description

This system has the following pages:

1. Check-In page. This page allows customers to select the kind of service they're receiving and support kiosk mode with the following which has:
   - Descriptive text.
   - Large enough buttons.
   - TODO: Ability to print tickets. Designed using FastReport.Net.
2. Waiting page. This page display the called customer's ticket number and the service point they should head to.
3. Service point. This page allows the service provider to:
   - Authenticate and select their service point.
   - Get next number.
   - Recall number.
   - Mark number as no show.
   - Mark number as finished.
   - Transfer number.
   - View their queue.
4. Admin dashboard. This page have the following:
   - Ability to configure service points.
   - Ability to configure service providers.
   - TODO: Ability to generate an analytical report, using **FastReport.Net**, displaying the following information:
     - Number of customers served.
     - Average waiting time per service point/provider.
     - Average service time per service point/provider.

## Database modelling

- PostgreSQL is the database used.
- The database tables are properly mapped to the project's POCO models.
- **NO** existing ORMs is used i.e. Entity Framework is not used for modelling. Instead, I have writen my own CRUD methods.

## How to Run

1. Clone repository to local computer
2. Create new postgres database 
3. Import sql file located at "Data" folder of this repository 
4. Set up User Secrets
- right-click the project and choose "Manage User Secrets"
![App Screenshot](https://github.com/mikemathu/Queue-Management-System/blob/main/Queue%20Management%20System/wwwroot/Screenshots/manageUserSecrets.PNG)
- This opens an editor for a secrets.json file in which you can store your configuration
![App Screenshot](https://github.com/mikemathu/Queue-Management-System/blob/main/Queue%20Management%20System/wwwroot/Screenshots/secretJson.PNG)

5. When completed, run the website in Visual Studio
- Login as Admin using Name: Admin | Password Admin@123
- Login as service provider using Name as Nurse1, Nurse2, Nurse3, Nurse4 or Nurse5 and Password as Nurse1@123, Nurse2@123, Nurse3@123, Nurse4@123 or Nurse5@123 respectively.


## Screenshots
1. Check-In page
![App Screenshot](https://github.com/mikemathu/Queue-Management-System/blob/main/Queue%20Management%20System/wwwroot/Screenshots/Check-In%20page.PNG)


2. Waiting page
![App Screenshot](https://github.com/mikemathu/Queue-Management-System/blob/main/Queue%20Management%20System/wwwroot/Screenshots/Waiting%20Page.PNG)

3. Service point
![App Screenshot](https://github.com/mikemathu/Queue-Management-System/blob/main/Queue%20Management%20System/wwwroot/Screenshots/Service%20point.PNG)

4. Admin dashboard
![App Screenshot](https://github.com/mikemathu/Queue-Management-System/blob/main/Queue%20Management%20System/wwwroot/Screenshots/Admin%20Page.PNG)
