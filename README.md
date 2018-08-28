# Forge Published Entities Stage Checker

Run this console application to check your forge backoffice database and discover inconsistencies inside published entities projections (*wcm.AlbumsPublished*, *wcm.DocumentsPublished*, ecc...).

Projections for both built-in entities and custom entities are checked; the entities considered inconsistent are the ones having **Stage** equals to *reviewed* or *unpublished*. You will get a JSON report file containing all the inconsistent entities.

## Configuration

There are two mandatories configurations: 
 - *ConfigFilePath*: absolute path to the JSON configuration file (see above for more info). **This file must exist and be accessible to the application**.
 - *ReportDirectoryPath*: absolute path to the directory under which the application will write its output (several JSON report files). The application will automatically create the directory if it doesn't exist.

You can **optionally** provide the following configurations:
- LogFilePath: absolute path to the log file of the application. If you don't provide this configuration, the application automatically writes a log file called **logs.txt** at the same level of the executable file

## JSON configuration file

**You must provide a valid JSON configuration file in order to run the application**.

Inside this file you must write a JSON array containing objects representing the tenants for which you want to run the stage check: there will be one object for each tenant.  

For each tenant you must provide a name and the connection string of the Forge backoffice database: you can find it inside Forge backoffice under *Administration* -> *Configuration* -> *Back End Store*

**Please do not use special characters or spaces inside the tenant name, use only letters underscore (\_) or dash (-)**

This is an example of a valid JSON configuration file: 
```
[
	{
		"name": "tenant1",
		"connString": "CONN STRING FOR TENANT1 DATABASE"
	},
	{
		"name": "tenant2",
		"connString": "CONN STRING FOR TENANT2 DATABASE"
	}
]
```

## Invoke from command line

Command line invokation command for Windows environment:
```
ForgePublishedEntitiesStageChecker.exe --ConfigFilePath="C:\foo\bar\config.json" --ReportDirectoryPath="C:\myDir\myReports"
```

Command line invokation command for MacOS environment:
```
./ForgePublishedEntitiesStageChecker --ConfigFilePath="/Foo/Bar/config.json" --ReportDirectoryPath="/MyDir/MyReports/"
```

