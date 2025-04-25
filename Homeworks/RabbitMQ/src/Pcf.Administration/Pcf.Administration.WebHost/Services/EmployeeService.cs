using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Administration.Core.Exceptions;
using Pcf.Administration.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepository<Employee> _employeeRepository;

    public EmployeeService(IRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _employeeRepository.GetAllAsync();

    public async Task<Employee> GetByIdAsync(Guid id)
        => await _employeeRepository.GetByIdAsync(id);

    public async Task UpdateAppliedPromocodesAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id) ?? throw new EmployeeNotFoundException();

        employee.AppliedPromocodesCount++;

        await _employeeRepository.UpdateAsync(employee);
    }
}
