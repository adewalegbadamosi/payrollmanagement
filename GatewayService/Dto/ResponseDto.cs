using System;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Dto
{
	public class ResponseDto
	{
		public ResponseDto()
		{
           
		}

        public object Result { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSuccessfullyCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsFaulted { get; set; }
        public object AsyncState { get; set; }
        public int CreationOptions { get; set; } = 0;
        public object Exception { get; set; } 

    }
}

