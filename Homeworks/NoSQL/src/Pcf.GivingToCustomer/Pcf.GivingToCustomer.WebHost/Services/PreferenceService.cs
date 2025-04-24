using Pcf.GivingToCustomer.Core.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Services;

public class PreferenceService
{
    private readonly HttpClient _httpClient;

    public PreferenceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<Preference>> GetPreferencesAsync(List<string> шds)
    {
        var queryString = string.Join(",", шds);
        var response = await _httpClient.GetAsync($"by-ids?ids={queryString}");

        response.EnsureSuccessStatusCode();

        var preferences = await response.Content.ReadFromJsonAsync<IList<Preference>>();
        return preferences ?? new List<Preference>();
    }
}
