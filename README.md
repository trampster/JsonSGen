# ![alt text](https://github.com/trampster/JsonSrcGen/blob/master/JsonSrcGen/icon.png "JsonSrcGen Logo") JsonSrcGen
Json library that uses .NET 5 c# Source Generators

Note: Requires the .Net 5 Preview to run. Mono platforms do not have support for the .Net 5 Source Generators yet.

# Supported Types

**Classes**
Class serializers can be generated by defining a Json attribute on the class
```csharp
[Json]
public class MyType
{
    [JsonName("my_name")]
    public string MyProperty {get;set}

    [JsonIgnore]
    public string IgnoredProperty {get;set;}
}

var converter = new JsonConverter();

ReadOnlySpan<char> json = convert.ToJson(new MyType(){MyProperty = "Some value"});

var myType = new MyType();
convert.FromJson("{\"MyProperty\:\"Some value\"}", myType);
```
Properties with the following types are supported:

Integer Types | Others
------|--------
int | float?
int? | double
uint | double?
uint? | boolean
ushort | boolean?
ushort? | string 
short | DateTime
short? | DateTime?
byte | DateTimeOffset
byte? | DateTimeOffset?
long | Guid
long? | Arrays
float | List<T>
    
**Arrays**

Arrays are generated by defining a JsonArray attribute at the assembly level.

```csharp
[assembly: JsonArray(typeof(bool))]
```

**List**

Lists are generated by defining a JsonList attribute at the assembly level.

```csharp
[assembly: JsonList(typeof(bool))]
```

**Dictionary**

Dictionaries are generated by defining a JsonList attribute at the assembly level. Only Dictionaries with string keys are supported currently.

```csharp
[assembly: JsonDictionary(typeof(string), typeof(int))]
```

**Values**

Simple json values are generated by defining a JsonValue attribute at the assembly level.

```csharp
[assembly: JsonValue(typeof(int))]
```

**Custom Converters**

You can customize the conversion for specific types by providing your own converter. You must implement ICustomConverter<T> and
put the CustomConverter attribute on your class.

```csharp
[CustomConverter(typeof(int))]
public class CustomCaseStringConverter : ICustomConverter<int>
{
    public void ToJson(IJsonBuilder builder, int value)
    {
        // Write your json to the builder here
    }

    public ReadOnlySpan<char> FromJson(ReadOnlySpan<char> json, ref int value)
    {
        // Read the Json from the json span here
    }
}
```

**Leave out null properties**

You can instruct JsonSrcGen to skip serializing null property values by adding the following attribute:

```csharp
[JsonIgnoreNull]
public class MyJsonType
{
    int? MyProperty {get;set;}
}
```

**Set Property to default if missing**

By default JsonSrcGen doesn't set properties to there default value if they are missing in the JSON. If you alwasy give FromJson a new instance this isn't a problem. However if you reused objects (which is a big performance boost) then the property wont get set unless present in the Json. If you want JsonSrcGen to set missing properties to default then you can specify this using the JsonOptionalAttribute

```csharp
public class MyJsonType
{
    [JsonOptional]
    string MyProperty{get;set;}
}
```

**Nuget Packages**

JsonSrcGen is available as a nuget package:
https://www.nuget.org/packages/JsonSrcGen/
