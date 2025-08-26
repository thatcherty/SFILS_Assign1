# SFILS: San Francisco Integrated Library System.

SFILS is an integrated library system for the San Francisco Public Library.

This project uses the [library usage data set](https://data.sfgov.org/Culture-and-Recreation/Library-Usage/qzz6-2jup/about_data) to build a complete database of the San Francisco Public Library.

There could be missing data (some of them intentional - to protect the identity of the patrons). However, we do not complement the data with other data sources. The only data set that is used in this project is [SFPL_DataSF_library-usage_Jan 2023.xlsx](https://data.sfgov.org/api/views/qzz6-2jup/files/ecfd0051-fe40-43c3-a17d-caeff0f1503f?download=true&filename=SFPL_DataSF_library-usage_Jan%202023.xlsx), which is also linked from the library usage data set page linked above.

The goal of this project is to carefully store this Excel sheet in a database with several tables. Eventually, we find interesting insights into the San Francisco public library through this database, and provide an app that allows for updates and edits.
