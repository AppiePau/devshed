# devshed-tools
*Devshed Tooling*

Devshed Tooling (currently for web) contains helper classes that simplify common scenarios that I come across during my developing days. Not much is documented at the moment, but will soon commence.

*Devshed.Csv*

Powerfull yet simplistic CSV processing library without any compromise to your code. See the draft documentation for the current 1.2 release!

*Devshed.Mvc*

Currently a few helper methods, but soon to be expanded.

*Devshed.IO*

Stream extensions, a FileContainer and file-type support to pack file downloads from service layers.

*Devshed.Web(Forms)*

Handy helpers for strong typed request variables, object-to-url serialization and deserializataion, generate urls based on webforms page classes.

*Devshed.Shared*

Shared functionality across the Devshed and useful on its own. Expression name resolver, GetBytes() and GetBytesWithoutBom() bits from a stream. String to byte array extensions with various encodings.

*PageUrlBuilder example (Devshed.Web)*

The PageUrlBuilder can be a very powerful tool when you have kept your Webforms pages correctly namepaced as your folder structure. With reflection it wil generate a predictable path to the 'file' (class). Using the URL serialization API it is even easier to add parameters and getting them back in your detination page.

*Uage:*

//// Register the root namespace once in the Application_Start event (Global.asax).
PageUrlBuilder.RegisterRootNamespace("Company.Application.MySite");
In your page, create an URL: 
//// Create a UrlBuilder instance with the PageUrlBuilder helper class and add some parameters:
PageUrlBuilder.Builder.For<Details>(new { UserId = 1 }); 
Contact

NuGet - Twitter - Blog
