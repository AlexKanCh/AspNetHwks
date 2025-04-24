using Redis.OM.Modeling;

namespace Pcf.Preferences.Model;

[Document(StorageType = StorageType.Json, Prefixes = new []{"Preference"})]
public class Preference
{
    // Id Field, also indexed, marked as nullable to pass validation
    [RedisIdField] [Indexed]public string? Id { get; set; } = Guid.NewGuid().ToString();

    // Indexed for exact text matching
    [Indexed] public string? Name { get; set; }
    
}