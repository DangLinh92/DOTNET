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
            var department = new Department()
            {
                DeptName = "D1",
                Address = "Duy tan",
                Description = "Phong IT"
            };
            //Creating the source object
            var employees = new List<Employee>()
            {
                new Employee
                {
                FullName = "James",
                Salary = 20000,
                Address = "London",
                Department = "IT",
                Description = "aaa",
                DeptName = "bbb",
                DeptAddress = "HN"
                }
                ,
                new Employee
                {
                    FullName = "Bon",
                    Salary = 10000,
                    Address = "LA",
                    Department = "IT",
                    Description = "aaa",
                    DeptName = "bbb",
                    DeptAddress = "HN"
                }
            };

            var config = new MapperConfiguration(cfg =>
            {
               cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();

            var empDtos = employees.Select(emp => mapper.Map<Employee, EmployeeDto>(emp));

            foreach (var empDto in empDtos)
            {
                Console.WriteLine("Name:" + empDto.Name + ", Salary:" + empDto.Salary + 
                                  ", Address:" + empDto.Address + ", Department:" + empDto.Department+
                                  ", Department Desciption:"+empDto.DepartmentDto.DescriptionDto);
            }

            Console.ReadLine();
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    des => des.Name,
                    act => act.MapFrom(src => src.FullName))
                .ForMember(des => des.DepartmentDto,
                    act => act.MapFrom(src => new DepartmentDto
                    {
                    DeptNameDto = src.DeptName,
                    AddressDto = src.DeptAddress,
                    DescriptionDto = src.Description
                }));
              

            CreateMap<Department, DepartmentDto>().
                ForMember(des=>des.DeptNameDto,
                    act=>act.MapFrom(src=>src.DeptName))
                .ForMember(des => des.AddressDto,
                act => act.MapFrom(src => src.Address))
                .ForMember(des => des.DescriptionDto,
                    act => act.MapFrom(src => src.Description));
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
        public string FullName { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string DeptName { get; set; }
        public string DeptAddress { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeDto
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public DepartmentDto DepartmentDto { get; set; }
    }

    public class Department
    {
        public string DeptName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

    }

    public class DepartmentDto
    {
        public string DeptNameDto { get; set; }
        public string AddressDto { get; set; }
        public string DescriptionDto { get; set; }

    }
}
