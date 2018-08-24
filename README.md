# Forge Published Entities Stage Checker

Run this console application to check your forge backoffice database and discover inconsistencies inside published entities projections (*wcm.AlbumsPublished*, *wcm.DocumentsPublished*, ecc...).

Projections for both built-in entities and custom entities are checked; the entities considered inconsistent are the ones having **Stage** equals to *reviewed* or *unpublished*. You will get a JSON report file containing all the inconsistent entities.

## Configuration

There are two mandatories configurations: 
 - MongoConnString: connection string of your Forge backoffice database. You can find it inside Forge backoffice under *Administration* -> *Configuration* -> *Back End Store*
 - ReportFilePath: absolute path of the JSON report file produced by the application. The application will automatically create the file directory if it doesn't exist.

## Invoke from command line

Command line invokation command for Windows environment:
```
ForgePublishedEntitiesStageChecker.exe --MongoConnString="YOUR MONGO CONN STRING" --ReportFilePath="C:\temp\myDir\report.json"
```

Command line invokation command for MacOS environment:
```
./ForgePublishedEntitiesStageChecker --MongoConnString="YOUR MONGO CONN STRING" --ReportFilePath="YOUR ABSOLUTE PATH TO JSON REPORT FILE"
```

