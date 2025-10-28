# How to Build the Project
First things first, clone the whole repository with the following command
```bash
git clone https://github.com/thatcherty/SFILS_Assign1.git
```

## MySQL Database
 - This assumes that you have a MySQL server running and know your root password
 - Unzip the [DB Dump](https://github.com/thatcherty/SFILS_Assign1/tree/main/scripts/database%20backup)
 - In the folder you unzipped it to, run the commands below:
   - ```bash
      mysql -u root -p -e "CREATE DATABASE sfils_prod CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;"
     ```
   - ```bash
     type sfils_full.sql | mysql -u root -p sfils_prod
     ```
 - These commands may take about 1min (no more in my experience) to run
 - Run the [Create New User](https://github.com/thatcherty/SFILS_Assign1/tree/main/scripts/user%20creation) file
   - You can do this by opening MySQL and running the script, or opening MySQL in the command line and running the same commands
  
### Some Checks 
 - Verify that MySQL is running on port 3306, as this is required for the app connection
 - Verify that the database is called sfils_prod, as this is required for the app connection
 - Verify a user has been created called sfils_reader with password ReadOnlyPass! that includes select access to the DB

## App
 - This app requires Visual Studio and .NET SDK 8.0 or later
 - In the app folder, open SFILS.sln
 - Run the app by selecting https at the top
![HTTPS start](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/Start_App_Screenshot.png)
 - This should automatically open a webpage at https://localhost:7297/
   - If not, click the link above, and it should take you there

> Note: If you do not have Visual Studio but do have .NET SDK 8.0, you can navigate to the project directory [here](https://github.com/thatcherty/SFILS_Assign1/tree/main/app/SFILS/SFILS).
> 
> In a command line, enter `dotnet run`
>
> You will need to find the exact port in the CLI output, for me it was http://localhost:5078/
