using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace AutoMapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creating the source object
            var employees = new List<Employee>()
            {
                new Employee
                {
                Name = "James",
                Salary = 20000,
                Address = "London",
                Department = "IT"
                }
                ,
                new Employee
                {
                    Name = "Bon",
                    Salary = 10000,
                    Address = "LA",
                    Department = "IT"
                }
            };


            
            var mapper = MapperConfig.Instant.CreateMapper();

            var empDtos = employees.Select(emp => mapper.Map<Employee, EmployeeDto>(emp));

            foreach (var empDto in empDtos)
            {
                Console.WriteLine("Name:" + empDto.Name + ", Salary:" + empDto.Salary + ", Address:" + empDto.Address + ", Department:" + empDto.Department);
            }

            Console.ReadLine();
        }
    }

    public class MapperConfig
    {
        private static MapperConfiguration _instant;
        public static MapperConfiguration Instant => _instant ?? CreateConfig();

        private static MapperConfiguration CreateConfig()
        {
            _instant = new MapperConfiguration(cfg =>
            {
                EmployeeConfig.CreateMap(cfg);
                DepartmentConfig.CreateMap(cfg);
            });
            return _instant;
        }
    }

    public class EmployeeConfig
    {
        public static void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Employee, EmployeeDto>();
            cfg.CreateMap<EmployeeDto, Employee>();
        }
    }

    public class DepartmentConfig
    {
        public static void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Department, DepartmentDto>();
            cfg.CreateMap<DepartmentDto, Department>();
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
    }

    public class EmployeeDto
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
    }

    public class Department
    {
        public string DeptName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

    }

    public class DepartmentDto
    {
        public string DeptName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

    }
}
