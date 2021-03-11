using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Take_home_test.Models
{
	public class ResponseModel
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public IEnumerable<StringMatchModel> Data { get; set; }
		public PagingModel Paging { get; internal set; }
	}
}
