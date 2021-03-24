using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppS3.DTOs;
using AppS3.Models;
using AutoMapper;

namespace AppS3.Profiles
{
    public class AppS3Profile : Profile
    {
        public AppS3Profile()
        {
            CreateMap<Employee, EmployeeDTO>();
        }
    }
}
