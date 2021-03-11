using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take_home_test.Models
{
	public class RequestSearch
	{
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string SearchValue { get; set; }
		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 200;
	}
}
