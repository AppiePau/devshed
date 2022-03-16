#rm .\dist\* -r
#dotnet pack --configuration release --output .\dist
Get-ChildItem -Path .\dist |
Foreach-Object {
    dotnet nuget push  $_.FullName --api-key oy2nezaxpvixft4exnhah6cedrjaxfdubhomh6izrbt7sa --source https://api.nuget.org/v3/index.json
}
