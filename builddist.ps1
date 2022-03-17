dotnet test
rm .\dist\* -r
dotnet pack --configuration release --output .\dist