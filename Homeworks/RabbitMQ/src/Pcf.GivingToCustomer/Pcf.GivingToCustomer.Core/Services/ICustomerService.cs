using Pcf.GivingToCustomer.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace Pcf.GivingToCustomer.Core.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetWhereAsync(Expression<Func<Customer, bool>> predicate);
}
