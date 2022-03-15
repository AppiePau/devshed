# devshed-tools 2.0 beta-1
*Devshed Tooling 2.0 (beta version) (.NET 6.0 Edition)*

## Breaking changes since 1.2 (to 2.0)
 - Devshed.Imaging has been discontinued;
 - Devshed.MVC has been discontinued;
 - Devshed.Webforms has been discontinued;

## Major changes since 1.2 (to 2.0)
 - Conversion to .NET 6.0 for more interoperability;
 - Replaced ClosedXML (0.95) and DocumentFormat.ClosedXML (2.12.0) with DocumentFormat.OpenXml (2.19) only


# Writing with the CsvWriter

The `CsvWriter` is a static class that does all the wiring for you. It requires an object of the `CsvDefintion<T>` type. Where T is the type map to. Creating a `CsvDefintion<UserView>` for example, will allow to pass an array of `UserView` objects to be written as CSV.

CsvWriter definition:
```cs
public static class CsvWriter
{
    public static MemoryStream WriteAsCsv<T>(this CsvDefinition<T> definition, T[] rows) {} // Extensions method
    public static void WriteAsCsv<T>(this CsvDefinition<T> definition, T[] rows, Stream stream) {} // Extensions method

    public static MemoryStream CreateStream<T>(CsvDefinition<T> definition, T[] rows) {} // Creates a stream and writes into it.
    public static void Write<T>(Stream stream, CsvDefinition<T> definition, T[] rows) {} // Writes into the given stream
}
```

## Defining the type mapping
The following example demonstrates the definition of property mappings to the `UserView` object. The `CsvDefinition` constructor accepts an endless amount of columns definitions.

```cs
var definition = new CsvDefinition<UserView>(
    new NumberCsvColumn<UserView>(e => e.Id),
    new TextCsvColumn<UserView>(e => e.Name),
    new BooleanCsvColumn<UserView>(e => e.IsActive));
```

After that, create an array of the `UserView` type, which could be database records as well:

```cs 
var users = new UserView[]
{
	new UserView 
            {
                 Id = 1,
                 Name = “John”,
                 IsActive = true
            },
	new UserView 
            {
                 Id = 2,
                 Name = “Marry”,
                 IsActive = false
            }
}
```

Using a the `CsvWriter` with a StreamWriter you can write directly to a file:

```cs 
using(var stream = new FileStream("C:\\Test.CSV", FileMode.Create))
{
    CsvWriter.Write(stream, definition, users);
}
```

Extension methods allow using the definition directly, like this:

```cs 
using(var stream = new FileStream("C:\\Test.CSV", FileMode.Create))
{
    definition.WriteStream(stream, users);
}
```

## Reading CSV with the CsvReader
Like the CsvWriter, the CsvReader follows the same principle by using a definition and a source stream; it reads and materializes the contents to an array:

```cs 
var definition = new CsvDefinition<UserView>(
    new NumberCsvColumn<UserView>(e => e.Id),
    new TextCsvColumn<UserView>(e => e.Name),
    new BooleanCsvColumn<UserView>(e => e.IsActive));
```
Use as stream for content and present both to the reader:

```cs 
using (var reader = new FileStream("C:\\Test.CSV", FileMode.Open))
{
    var users = CsvReader.Read<UserView>(stream, definition);
}
```

Like that, `users` now contains an array of `UserView` objects. A reading extension is also available on the definition: 

```cs 
using (var reader = new FileStream("C:\\Test.CSV", FileMode.Open))
{
    var users = definition.ReadStream<UserView>(stream);
}
```

## Additional CsvDefinition<> options
The `CsvDefintion<TRow>` has the following three additional properties:

### FirstRowContainsHeaders (default false)
This option is for both writing and reading. The first line will contain the header names of the defined columns. By default the names will be the reflected property names, which can be overridden. 

When a custom header name is required, it can be overruled with the HeaderName property. More on this in the 'CsvColumn definitions' section.

When reading a file with this option on, the reader expects the first line to be filled with header names as quoted strings. The names must match either the reflected name or the custom name. If no header names are specified, the reader works index based.

### RemoveNewLineCharacters (default false)
When enabled all written CSV text fields will be stripped of new line characters. For writing CSV only. NOTE; header names will be stripped from newline characters anyway.

### ElementDelimiter (default “;”)
The character that separates the CSV elements for reading and writing CSV content.
 
## Column definitions
In order to configure your CSV definition, several column types are available. All columns expose a Format and HeaderName property in order to overwrite the default behavior. Note that some of the values passing through the Format expression can be null. For example the DecimalCsvColumn.
The following examples are written with their default values.

## TextCsvColumn<TSource>
Accepts strings and can remove new line characters.

```cs 
new TextCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => value + "_ADD_THIS",
    HeaderName = "Overruled Name",
    ForceExcelTextCell = true
}
```
### ForceExcelTextCell
Writes the text value like =”VALUE” instead of “VALUE” to force Excel to parse it as text otherwise textual numbers will still be treated as number.

### CurrencyCsvColumn<TSource>
Accepts nullable decimal and formats them to the current culture currency of the thread.

```cs 
new CurrencyCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format("{0:c2}", value ?? 0), 
    HeaderName = “Overruled Name”
}
```

### NumberCsvColumn<TSource>
Accepts nullable int and formats them to the current culture currency of the thread.

```cs 
new NumberCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format("{0:0.00}", value ?? 0), 
    HeaderName = “Overruled Name”
}
```
### DecimalCsvColumn<TSource>
Accepts nullable decimal values and formats them to the current culture currency of the thread. 

```cs 
new CurrencyCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format("{0:c4}", value ?? 0), 
    HeaderName = “Overruled Name”,
    Format = (number, formatter) => number != null
                      ? number.Value.ToString(formatter)
                : string.Empty,
}
``` 
### DateCsvColumn<TSource>
Accepts nullable DateTime values and formats them to a short date format.

```cs 
new DateCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format("{0:c4}", value ?? 0), 
    HeaderName = “Overruled Name”,
    Format = e => e != null
         ? e.Value.ToShortDateString()
         : string.Empty
}
```
### TimeCsvColumn<TSource>
Accepts nullable decimal values and formats them to the current culture currency of the thread.

```cs 
new TimeCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format(“{0:c4}”, value ?? 0), 
    HeaderName = “Overruled Name” ”,
    Format = e => e != null
               ? string.Format("{0:00}:{1:00}", e.Value.Hours, e.Value.Minutes)
         : string.Empty
}
```
### BooleanCsvColumn<TSource>
Accepts boolean values and formats them to string.

```cs 
new TimeCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format(“{0:c4}”, value ?? 0), 
    HeaderName = “Overruled Name”,
    Format = e => e.ToString()
}
```

## Special columns
### ObjectCsvColumn<TSource>
Accepts any type of value and formatting can be handled as you please.

```cs 
new ObjectCsvColumn<TSource>(e => e.TProperty)
{
    Format = value => string.Format(“{0:c4}”, value ?? 0), 
    HeaderName = “Overruled Name”,
    Format = e => e.ToString()
}
```

### ArrayCsvColumn<TSource, TArray>
This column type allows to write the content of an array be written in a single element. It accepts a mapping to a property of an array type `TArray`.

```cs 
new ArrayCsvColumn<TSource, int>(e => e.TProperty)
{
    Format = value => string.Format(“{0:c4}”, value ?? 0), 
    HeaderName = “Overruled Name”,
    Format = e => e.ToString()
}
```
The Format expression is executed with each element in the array. In the CSV the values will be written comma separated like `1,2,3`. For example:

```csv 
"John";1,2,3;"Street"
```

This does not allow strings to be written either newlines or delimiter characters like `;`.

### CompositeCsvColumn<TSource, TValue>
This column was introduced to solve problem of dynamic fields, when generating financial exports that require for example tax groups that cannot be predefined in the compiled definition.

Unavoidably this requires the use of a `CompositeColumnValue<,>` object from the assembly. A `KeyValuePair<string, string>` that was used in prior versions was considered as too unspecific for the purpose.

```cs 
new CompositeCsvColumn<TSource, string>(
    e => e.Collection, “Header1”, “Header2”)
{
    Format = value => string.Format(“{0:c4}”, value ?? 0), 
    HeaderName = “Overruled Name”,
    Format = e => e.ToString()
}

var rows = new [] {
    new Model {
        Name = “John”,
        Collection = new [] 
        {
            new CompositeColumnValue <string>(“Header 1”, “Value 1”), 
            new CompositeColumnValue <string>(“Header 2”, “Value 2”)  
        }
    }  
}
```
In the CSV the values will be written as separate elements, each with their own header name, like this:

```csv
"Name","Header1","Header2"
"John";"Value 1";"Value 2"
```

Note that the values of the composition will be treated as strings.