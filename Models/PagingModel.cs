using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Take_home_test.Models
{
	public class PagingModel
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int RecordTotal { get; set; }
		public int PageTotal { get; set; }
	}
}
