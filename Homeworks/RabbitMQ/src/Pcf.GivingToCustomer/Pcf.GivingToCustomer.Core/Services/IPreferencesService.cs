using System.Threading.Tasks;
using System;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.Core.Services;

public interface IPreferenceService
{
    Task<Preference> GetByIdAsync(Guid id);
}
