using System;

namespace Pcf.Administration.Core.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException() : base("Employee not found") { }
}
