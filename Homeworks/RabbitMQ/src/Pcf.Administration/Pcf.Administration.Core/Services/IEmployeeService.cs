using Pcf.Administration.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Services;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();

    Task<Employee> GetByIdAsync(Guid id);

    Task UpdateAppliedPromocodesAsync(Guid id);
}
