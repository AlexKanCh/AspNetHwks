using Pcf.GivingToCustomer.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Services;

public interface IPromoCodeService
{
    Task<IEnumerable<PromoCode>> GetAllAsync();

    Task GivePromoCodeToCustomersWithPreferenceAsync(PromoCode promoCode);
}
