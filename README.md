# devshed-tools 2.0 beta-1
*Devshed Tooling 2.0 (beta version) (.NET 6.0 Edition)*

Devshed Tooling (currently for web) contains helper classes that simplify common scenarios that I come across during my developing days. Not much is documented at the moment, but will soon commence.

*Devshed.Csv*

Powerfull and descriptive CSV processing library without any compromise to your code. See the draft documentation for the current 1.2 release!

With the use of ClosedXML you can generate or read XLSX files!

Create a definition of the import/export model:
```cs
var definition = new CsvDefinition<UserExportModel>(
     new TextCsvColumn<UserExportModel>(user => user.UserId),
     new TextCsvColumn<UserExportModel>(user => user.Email) { HeaderName = Resources.Users.Email },
     new TextCsvColumn<UserExportModel>(user => user.Fullname) { HeaderName = Resources.Users.Fullname });
```
Get users from the database as an array of UserExportModel and render it into a stream:
```cs
  var users = this.GetUsersFromDatabase();

  var stream = CsvWriter.CreateStream(parameters.Definition, users);

  return new FileContainer(this.fileTypes.GetMimeType(".CSV"), stream);
```
For more example, [see documentation](Documentation/Documentation.md).

*Devshed.IO*

Stream extensions, a FileContainer and file-type support to pack file downloads from service layers.

*Devshed.Shared*

Shared functionality across the Devshed and useful on its own. Expression name resolver, GetBytes() and GetBytesWithoutBom() bits from a stream. String to byte array extensions with various encodings.

*Devshed.Mvc*

No longer supported or maintained.

*Devshed.Web(Forms)*

No longer supported or maintained.

*Devshed.Imaging*

No longer supported or maintained.