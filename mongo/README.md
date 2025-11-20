# How to Build the Project
First things first, clone the whole repository with the following command
```bash
git clone https://github.com/thatcherty/SFILS_Assign1.git
```

## MongoDB Database
 - This assumes that you have a MongoDB server running
   - If not, you can download it [Here](https://www.mongodb.com/try/download/community)
  
### Some Checks 
 - Verify that MongoDB is running on port 27017, as this is required for the app connection

## Unzip Patrons
 - The patrons json file is too large to store on GitHub without zipping, so you will need to unzip this file before attempting to seed the database.
 - You can find the file [here](https://github.com/thatcherty/SFILS_Assign1/tree/main/mongo/SFILS/SFILS/seed_data/mongo/data) called patrons.zip
 - Unzip it into the data folder

## App
 - This app requires Visual Studio and .NET SDK 8.0
   - You can check your .NET version in the CLI with `dotnet --version`
     - Or even better, check the control panel for your installed programs. `dotnet --version` shows the latest install.
   - [Here](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.415-windows-x64-installer) is a link to Microsoft .NET SDK 8 downloads
 - In the [SFILS folder](https://github.com/thatcherty/SFILS_Assign1/tree/main/mongo/SFILS), open SFILS.sln
 - Run the app by selecting https at the top
![HTTPS start](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/Start_App_Screenshot.png)
 - This should automatically open a webpage at https://localhost:7297/
   - If not, click the link above, and it should take you there

> Note: If you do not have Visual Studio but do have .NET SDK 8.0, you can navigate to the project directory [located here](https://github.com/thatcherty/SFILS_Assign1/tree/main/mongo/SFILS/SFILS) on your local file explorer.
> 
> In a command line, enter `dotnet run`
>
> You will need to find the exact port in the CLI output, for me it was http://localhost:5078/

## Seed MongoDB
 - Rather than a dump file (like done in MySQL), I have seed files that use the C# integration to create a database, collections, and insert the data.
 - When you start the app, you will see this initially:
![No Found Patrons](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/No_Patrons_Found.png)
 - Select **Seed** at the top
 - From here, click on **Run Seed**
   - This took around 3 minutes to complete the initialization
 - You will see the message below once it has successfully seeded the database:
![Mongo_Init](https://raw.githubusercontent.com/thatcherty/SFILS_Assign1/main/docs/photos/Mongo_Init.png) 
 - Select **Mongo** at the top to return to the home page and view the data

# Some Things to Note
 - In the Home Library filter, you will see the same name appear more than once. This is because the filter looks at the Home Library Code as the unique value. The code is different for each value shown, but the name of the library is the same.
