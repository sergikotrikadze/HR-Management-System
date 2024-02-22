using System;
using System.Linq;
using HR_Management.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HR_Management
{
    public static class AfterLogin
    {
        public static void SuccessfulLogin(Employee employee, HRManagementContext context)
        {
            if (employee.Department == "HR")
            {
                // Logic for HR department employees
                while (true)
                {
                    Console.WriteLine("HR Department Menu:");
                    Console.WriteLine("1. View your information");
                    Console.WriteLine("2. Resign");
                    Console.WriteLine("3. See all employee's emails");
                    Console.WriteLine("4. Go to employee's page view/update info");
                    Console.WriteLine("5. Back");

                    Console.Write("Enter your choice: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ViewOwnInformation(employee);
                            break;
                        case "2":
                            Resign(context, employee);
                            break;
                        case "3":
                            ViewAllEmployeeEmails(context);
                            break;
                        case "4":
                            ViewUpdateEmployeeInformation(context);
                            break;
                        case "5":
                            Console.WriteLine("Backing out...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                // Logic for regular employees
                while (true)
                {
                    Console.WriteLine("Employee Menu:");
                    Console.WriteLine("1. View your information");
                    Console.WriteLine("2. Resign");
                    Console.WriteLine("3. back");

                    Console.Write("Enter your choice: ");
                    var choice = Console.ReadLine();
                    Console.WriteLine("-----------------");

                    switch (choice)
                    {
                        case "1":
                            ViewOwnInformation(employee);
                            break;
                        case "2":
                            Resign(context, employee);
                            return;
                        case "3":
                            Console.WriteLine("Backing out...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }

        // this is for non hr and if also hr wants to see their own info
        private static void ViewOwnInformation(Employee employee)
        {
            Console.WriteLine($"Employee ID: {employee.EmployeeId}");
            Console.WriteLine($"Username: {employee.Username}");
            Console.WriteLine($"Age: {employee.Age}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Salary: {employee.Salary}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine("-----------------");
        }

        private static void ViewAllEmployeeEmails(HRManagementContext context)
        {
            Console.WriteLine("Employee emails");

            //es varianti ar mushaobs sworad ragac await is gamoyeneba mchirdeba mgoni da ver vxvdebi
            
            // context.Employees.ForEachAsync(e => Console.WriteLine(e.Email));

            var employees = context.Employees.ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.Email);
            }
        }

        private static void ViewUpdateEmployeeInformation(HRManagementContext context)
        {
            while (true)
            {
                Console.WriteLine("Enter employee email");
                var employeeEmail = Console.ReadLine();
                Console.WriteLine("-----------------");
                var fetchedEmployee = context.Employees.FirstOrDefault(e => e.Email == employeeEmail);
                if (fetchedEmployee != null)
                {
                    ViewEmployeeInformation(fetchedEmployee);
                    Console.WriteLine("----");
                    Console.WriteLine("Employee information update menu");
                    Console.WriteLine("1. Change employee's salary");
                    Console.WriteLine("2. Change employee's department");
                    Console.WriteLine("3. Fire employee");
                    Console.WriteLine("4. Back");
                    Console.Write("Enter your choice: ");

                    var choice = Console.ReadLine();


                    switch (choice)
                    {
                        case "1":
                            ChangeEmployeeSalary(context, fetchedEmployee);
                            break;
                        case "2":
                            ChangeEmployeeDepartment(context, fetchedEmployee);
                            break;
                        case "3":
                            FireEmployee(context, fetchedEmployee);
                            break;
                        case "4":
                            Console.WriteLine("Backing out ...");
                            return;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                }

                else
                {
                    Console.WriteLine("Employee not found.");
                    Console.WriteLine("-----------------");
                }
            }
        }

        private static void ViewEmployeeInformation(Employee fetchedEmployee)
        {
            Console.WriteLine($"Employee ID: {fetchedEmployee.EmployeeId}");
            Console.WriteLine($"Username: {fetchedEmployee.Username}");
            Console.WriteLine($"Age: {fetchedEmployee.Age}");
            Console.WriteLine($"Email: {fetchedEmployee.Email}");
            Console.WriteLine($"Salary: {fetchedEmployee.Salary}");
            Console.WriteLine($"Department: {fetchedEmployee.Department}");
            Console.WriteLine("-----------------");
        }

        private static void ChangeEmployeeSalary(HRManagementContext context, Employee fetchedEmployee)
        {
            Console.Write("Enter new salary: ");
            var newSalary = decimal.Parse(Console.ReadLine());

            fetchedEmployee.Salary = newSalary;
            context.SaveChanges();

            Console.WriteLine("Salary updated successfully.");
            Console.WriteLine("-----------------");
        }

        private static void ChangeEmployeeDepartment(HRManagementContext context, Employee fetchedEmployee)
        {
            Console.Write("Enter new department: ");
            var newDepartment = Console.ReadLine();

            fetchedEmployee.Department = newDepartment;
            context.SaveChanges();

            Console.WriteLine("Department updated successfully.");
            Console.WriteLine("-----------------");
        }

        private static void FireEmployee(HRManagementContext context, Employee fetchedEmployee)
        {
            context.Employees.Remove(fetchedEmployee);
            context.SaveChanges();

            Console.WriteLine("Employee fired successfully.");
            Console.WriteLine("-----------------");
        }

        private static void Resign(HRManagementContext context, Employee employee)
        {
            context.Employees.Remove(employee);
            context.SaveChanges();

            Console.WriteLine("Resignation successful. You have been removed from the system.");
            Console.WriteLine("-----------------");
        }
    }
}