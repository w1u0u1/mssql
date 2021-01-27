# mssql
mssql client

# basic
vs2019 + net3.5

# usage
mssql.exe connectString [queryString [logFile]]  
mssql.exe "Server=localhost;Database=test;User Id=user;Password=pass;"  
mssql.exe "Server=localhost;Database=test;Trusted_Connection=True;" "select * from test"  
mssql.exe "Server=localhost;Database=test;Trusted_Connection=True;" "select * from test" c:\log.csv
