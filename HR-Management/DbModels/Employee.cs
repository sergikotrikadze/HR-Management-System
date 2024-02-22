using System;
using System.Collections.Generic;

namespace HR_Management.DbModels;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public int? Age { get; set; }

    public string Email { get; set; } = null!;

    public decimal? Salary { get; set; }

    public string? Department { get; set; }

    public string FullName { get; set; } = null!;
}
