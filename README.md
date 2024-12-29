# TeamUpWebScraper

WinForms App to parse a TeamUp Calendar and return data in Excel spreadsheet report format.

## Keep in mind the environmental variables specifieed in UI project
(They are in project properties > Debug/General > Open Debug Launch Profiles UI > Environment Variables)
(Or in Solution Explorer > Properties > launchSettings.json)
* BUILD_NUMBER = DEV_ENV
* DOTNET_ENVIRONMENT = Development


#### To Do List

- ✅ Dependency Injection
- ✅ Serilog Logger + Rotating File
- ✅ Unit tests (xUnit, FluentAssertions, NSubstitute, bogus)
- ✅ Add versionning with build and environment variables
- ✅ write tests with _teamUpAPIService mocked with n substitute using provided json file
- ✅ write logic that transforms the data recieved from API into a model that will go to Excel
- ✅ Model for Excel Table
- 🔲 Excel spreadsheet report provider
