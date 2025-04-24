using Pcf.ReceivingFromPartner.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.WebHost.Services;

public class PreferenceService
{
    private readonly HttpClient _httpClient;

    public PreferenceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<Preference>> GetPreferences(List<string> ids)
    {
        var queryString = string.Join(",", ids);
        var response = await _httpClient.GetAsync($"by-ids?ids={queryString}");

        response.EnsureSuccessStatusCode();

        var preferences = await response.Content.ReadFromJsonAsync<IList<Preference>>();
        return preferences ?? new List<Preference>();
    }
    public async Task<Preference> GetPreference(string id)
    {
        var response = await _httpClient.GetAsync($"by-ids?ids={id}");

        response.EnsureSuccessStatusCode();

        var preferences = await response.Content.ReadFromJsonAsync<IList<Preference>>();
        return preferences.FirstOrDefault();
    }
}
