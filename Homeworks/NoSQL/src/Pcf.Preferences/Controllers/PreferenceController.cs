using Microsoft.AspNetCore.Mvc;
using Redis.OM.Searching;
using Pcf.Preferences.Model;
using Redis.OM;

namespace Pcf.Preferences.Controllers;

[ApiController]
[Route("[controller]")]
public class PreferenceController : ControllerBase
{
    private readonly RedisCollection<Preference> _preference;
    private readonly RedisConnectionProvider _provider;
    public PreferenceController(RedisConnectionProvider provider)
    {
        _provider = provider;
        _preference = (RedisCollection<Preference>)provider.RedisCollection<Preference>();
    }

    /// <summary>
    /// Creates an indexed Preference object in Redis
    /// </summary>
    /// <param name="preference"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Preference> AddPreference([FromBody] Preference preference)
    {
        await _preference.InsertAsync(preference);
        return preference;
    }

    /// <summary>
    /// Get all preferences
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    public IList<Preference> GetAll()
    {
        return _preference.ToList();
    }

    ///// <summary>
    ///// Filters Preferences by their  name
    ///// </summary>
    ///// <param name="firstName"></param>
    ///// <param name="lastName"></param>
    ///// <returns></returns>
    //[HttpGet("filterName")]
    //public IList<Preference> FilterByName([FromQuery] string name)
    //{
    //    return _preference.Where(x => x.Name == name ).ToList();
    //}

    /// <summary>
    /// Updates a Preference at Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public IActionResult UpdateName([FromRoute] string id, [FromBody] string newName)
    {
        foreach (var pref in _preference.Where(x => x.Id == id))
        {
            pref.Name = newName;
        }
        _preference.Save();
        return Accepted();
    }

    /// <summary>
    /// Deletes a Preference at the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public IActionResult DeletePreference([FromRoute] string id)
    {
        _provider.Connection.Unlink($"Preference:{id}");
        return NoContent();
    }
}