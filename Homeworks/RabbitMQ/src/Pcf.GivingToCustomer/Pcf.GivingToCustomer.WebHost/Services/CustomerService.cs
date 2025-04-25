using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Services;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;

namespace Pcf.GivingToCustomer.WebHost.Services;

internal class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;

    public CustomerService(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetWhereAsync(Expression<Func<Customer, bool>> predicate)
        => await _customerRepository.GetWhere(predicate);
}
