using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take_home_test.Models;

namespace Take_home_test.Service
{
	public interface IStringMatchService
	{
		ResponseModel InsertData();
		ResponseModel Search(string searchValue, int pageIndex, int pageSize);
	}
}
