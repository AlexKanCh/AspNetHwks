using System.Threading.Tasks;
using System;
using Pcf.GivingToCustomer.Core.Services;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;

namespace Pcf.GivingToCustomer.WebHost.Services;


internal class PreferenceService : IPreferenceService
{
    private readonly IRepository<Preference> _preferencesRepository;

    public PreferenceService(IRepository<Preference> preferencesRepository)
    {
        _preferencesRepository = preferencesRepository;
    }

    public async Task<Preference> GetByIdAsync(Guid id)
        => await _preferencesRepository.GetByIdAsync(id);
}