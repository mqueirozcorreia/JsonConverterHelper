Implementations of JsonConverter of Newtonsoft.Json packages

# Sample
## Create Classes
~~~~
public class ClassWithId
{
    public int Id { get; set; }
}

[JsonConverter(typeof(KeysAsListJsonConverter<ClassWithId, KeysAsList>))]
public class KeysAsList : List<ClassWithId>
{
}
~~~~

## Deserialize
~~~~
var json = @"{ ""1"" : { ""id"": 1 }, 
""2"" : { ""id"": 2 }}";

var result = JsonConvert.DeserializeObject<KeysAsList>(json);

Console.WriteLine(result.Count);
Console.WriteLine(result[0].Id);
Console.WriteLine(result[1].Id);
~~~~