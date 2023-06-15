using System;
using AutoMapper;
using EmployeeService.Dto;
using EmployeeService.Models;

namespace EmployeeService.Mapper
{
	public class ProfileMappper : Profile
	{
		public ProfileMappper()
		{
			// source to target
			CreateMap<Employee, EmployeeDto>().ReverseMap();
			CreateMap<object, EmployeeDto>();
			CreateMap<DetailDto, EmployeeDto>();
			// CreateMap<GenericEventDto, object>().ReverseMap();
		}
	}
}

