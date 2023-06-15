using System;
using AutoMapper;
using SalaryService.Dto;
using SalaryService.Models;


namespace SalaryService.Mapper
{
	public class ProfileMappper : Profile
	{
		public ProfileMappper()
		{
			// source to target
			CreateMap<Salary, SalaryDto>().ReverseMap();
			CreateMap<Salary, ComputeDto>().ReverseMap();

        }
    }
}

