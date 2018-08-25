# Forge Published Entities Stage Checker

Run this console application to check your forge backoffice database and discover inconsistencies inside published entities projections (*wcm.AlbumsPublished*, *wcm.DocumentsPublished*, ecc...).

Projections for both built-in entities and custom entities are checked; the entities considered inconsistent are the ones having **Stage** equals to *reviewed* or *unpublished*. You will get a JSON report file containing all the inconsistent entities.

## Configuration

There are two mandatories configurations: 
 - ConfigFilePath: absolute path to the JSON configuration file (see above for more info). This file must exist and be accessible to the application.
 - ReportDirectoryPath: absolute path to the directory under which the application will write its output (several JSON report files). The application will automatically create the directory if it doesn't exist.

## Invoke from command line

Command line invokation command for Windows environment:
```
ForgePublishedEntitiesStageChecker.exe --ConfigFilePath="C:\foo\bar\config.json" --ReportDirectoryPath="C:\myDir\myReports"
```

Command line invokation command for MacOS environment:
```
./ForgePublishedEntitiesStageChecker --ConfigFilePath="/Foo/Bar/config.json" --ReportDirectoryPath="/MyDir/MyReports/"
```

