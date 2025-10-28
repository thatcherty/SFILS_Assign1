# How to Build the Project
First things first, clone the whole repository

## MySQL Database
 - There is a file to restore the database and all its contents
 - After this, run the create_user.sql to add the required user to the DB
 - Verify that mySQL is running on localhost:7297 as this is required for the app connection
 - Verify that the database is called sfils as this is required for the app connection

## App
 - This app requires Visual Studio to be installed
 - In the app folder, open SFILS.sln
 - Run the app by selecting https at the top

# What the app includes
 - The app allows you to view the contents of the DB
 - You can view individual records by selecting Details on the right side
 - You can apply various filters by using the selections below each column header
 - You can advance through pages of records to view the next records in the result of your search filters
 - The app does not support editing or creating new records
