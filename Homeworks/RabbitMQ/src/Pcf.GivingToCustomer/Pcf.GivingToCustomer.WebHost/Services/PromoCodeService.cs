using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Exceptions;
using Pcf.GivingToCustomer.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Services;

internal class PromoCodeService : IPromoCodeService
{
    private readonly IRepository<PromoCode> _promoCodesRepository;
    private readonly IPreferenceService _preferenceService;
    private readonly ICustomerService _customerService;

    public PromoCodeService(IRepository<PromoCode> promoCodesRepository,
        IPreferenceService preferenceService,
        ICustomerService customerService)
    {
        _promoCodesRepository = promoCodesRepository;
        _preferenceService = preferenceService;
        _customerService = customerService;
    }

    public async Task<IEnumerable<PromoCode>> GetAllAsync()
        => await _promoCodesRepository.GetAllAsync();

    public async Task GivePromoCodeToCustomersWithPreferenceAsync(PromoCode promoCode)
    {
        var preference = await _preferenceService.GetByIdAsync(promoCode.PreferenceId) ?? throw new PreferenceNotFoundException();

        var customers = await _customerService.GetWhereAsync(d => d.Preferences.Any(x => x.Preference.Id == preference.Id));

        promoCode.Preference = preference;

        promoCode.Customers = customers.Select(c =>
            new PromoCodeCustomer
            {
                CustomerId = c.Id,
                Customer = c,
                PromoCodeId = promoCode.Id,
                PromoCode = promoCode
            }
        ).ToList();

        await _promoCodesRepository.AddAsync(promoCode);
    }
}
