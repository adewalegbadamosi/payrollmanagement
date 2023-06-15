using System;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Dto
{
	public class UserDto
	{
		public UserDto()
		{
           
		}

        public string? UserName { get; set; }

        [EmailAddress]
        
        public string? Email { get; set; }       
        
        public string? Password { get; set; }        
        public string? ConfirmPassword { get; set; }

    }
}

