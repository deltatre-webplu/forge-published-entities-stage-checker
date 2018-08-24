# Forge Published Entities Stage Checker

Run this console application to check your forge backoffice database and discover inconsistencies inside published entities projections.  
Projections for both builtin entities and custom entities are checked. The entities considered not consistent are the ones having Stage equals to *reviewed* or *unpublished*.

## Invoke from command line

To invoke the application from command line and pass command line arguments, type the following command in your command line:
```
DuplicatedSlugAnalyzer.exe --guishellBaseUrl="YOUR GUISHELL BASE URL" --applicationName="NAME OF FORGE APPLICATION REGISTERED UNDER GUISHELL" --guishellSecret="YOUR GUISHELLL ADMIN API SECRET"
```

