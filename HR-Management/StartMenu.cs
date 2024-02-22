using System;
using System.Linq;
using HR_Management.DbModels;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace HR_Management
{
    public class StartMenu
    {
        static void Main(string[] args)
        {
            using (var context = new HRManagementContext())
            {
                while (true)
                {
                    Console.WriteLine("Welcome to HR Management System!");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("3. Exit");

                    Console.Write("Enter your choice: ");
                   
                    var choice = Console.ReadLine();
                    Console.WriteLine("-----------------");

                    switch (choice)
                    {
                        case "1":
                            Login(context);
                            break;
                        case "2":
                            Register(context);
                            break;
                        case "3":
                            Console.WriteLine("Exiting...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            Console.WriteLine("-----------------");
                            break;
                    }
                }
            }
        }

        static void Login(HRManagementContext context)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var employee = context.Employees.FirstOrDefault(e => e.Username == username);
            if (employee != null && VerifyPassword(password, employee.PasswordHash, employee.Salt))
            {
                Console.WriteLine("Login successful!");
                Console.WriteLine("-----------------");

                AfterLogin.SuccessfulLogin(employee,context);
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                Console.WriteLine("-----------------");
            }
        }

        static void Register(HRManagementContext context)
        {
            Console.WriteLine("Register a new employee:");

            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            // Generate salt and hash for password
            byte[] salt = GenerateSalt();
            string passwordHash = ComputeHash(password, salt);

            Console.Write("Enter full name: ");
            var fullName = Console.ReadLine();

            Console.Write("Enter age: ");
            var age = int.Parse(Console.ReadLine());

            Console.Write("Enter email: ");
            var email = Console.ReadLine();

            Console.Write("Enter salary: ");
            var salary = decimal.Parse(Console.ReadLine());

            Console.Write("Enter department: ");
            var department = Console.ReadLine();

            // Create new employee
            var newEmployee = new Employee
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt,
                FullName = fullName,
                Age = age,
                Email = email,
                Salary = salary,
                Department = department
            };

            context.Employees.Add(newEmployee);
            context.SaveChanges();

            Console.WriteLine("Employee registered successfully!");
            Console.WriteLine("-----------------");
        }

        static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        static string ComputeHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes =
                    sha256.ComputeHash(salt.Concat(System.Text.Encoding.UTF8.GetBytes(password)).ToArray());
                return Convert.ToBase64String(hashedBytes);
            }
        }

        static bool VerifyPassword(string password, string passwordHash, byte[] salt)
        {
            return passwordHash == ComputeHash(password, salt);
        }
    }
}