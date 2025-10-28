# How to Build the Project
First things first, clone the whole repository with the following command
```bash
git clone https://github.com/thatcherty/SFILS_Assign1.git
```

## MySQL Database
 - Unzip the [DB Dump](https://github.com/thatcherty/SFILS_Assign1/tree/main/scripts/database%20backup)
 - In the folder you unzipped it to, run the below commands:
 - ```bash
   mysql -u root -p -e "CREATE DATABASE sfils_prod CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;"
   mysql -u root -p sfils_prod < sfils_full.sql
   ```
 - These commands may take about 1min (no more in my experience) to run
 - Run the [Create New User](https://github.com/thatcherty/SFILS_Assign1/tree/main/scripts/user%20creation) file
   - You can do this by opening mySQL and running the script, or opening mySQL in the command line and running the same commands
  
### Some Checks 
 - Verify that mySQL is running on port 3306 as this is required for the app connection
 - Verify that the database is called sfils_prod as this is required for the app connection

## App
 - This app requires Visual Studio to be installed
 - In the app folder, open SFILS.sln
 - Run the app by selecting https at the top
![HTTPS start](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/Start_App_Screenshot.png)
 - This should automatically open a webpage at https://localhost:7297/
   - If not, just click the link above and it should take you there

# What the app includes
 - The app allows you to view the contents of the DB
 - You can view individual records by selecting Details on the right side
 - You can apply various filters by using the selections below each column header
 - You can advance through pages of records to view the next records in the result of your search filters
 - The app does not support editing or creating new records

Once the app loads, you should see something similar to the below:

![App UI](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/App_UI.png)

If you select Details on a Patron record, you will see the below page:

![Patron Details](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/Patron_Details.png)

