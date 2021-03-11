using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take_home_test.Models;
using Take_home_test.Constants;

namespace Take_home_test.Service
{
	public class StringMatchService : IStringMatchService
	{
		public ResponseModel InsertData()
		{
			try
			{
				Stopwatch sw = new Stopwatch();
				sw.Start();
				var (isSuccess, errorMessage) = GeneralCSVFile(GeneralData(), $"{Constants.Constants.FileName}");
				sw.Stop();
				return new ResponseModel
				{
					IsSuccess = isSuccess,
					Message = errorMessage
				};
			}
			catch(Exception e)
			{
				return new ResponseModel
				{
					IsSuccess = false,
					Message = e.Message
				};
			}			
		}

		private DataTable GeneralData()
		{
			var TblData = new DataTable();
			TblData.Columns.Add("String ID", typeof(string));
			TblData.Columns.Add("String Content", typeof(string));

			for (int i = 0; i < 100000; i++)
			{
				TblData.Rows.Add(Guid.NewGuid().ToString(), GeneralStringRandom(1024, 2048)); 
			}
			return TblData;
		}


		//min = 1024b (1k)-->
		//max = 2048b (2k)
		private string GeneralStringRandom(int minLength, int maxLength)
		{
			Random random = new Random();
			return new string(Enumerable.Repeat(Constants.Constants.RandomCharacters, random.Next(minLength, maxLength))
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public (bool, string) GeneralCSVFile(DataTable table, string strFilePath)
		{
			try
			{
				//hearder with 2 columns
				StringBuilder csvBuilder = new StringBuilder(Constants.Constants.Capacity);
				csvBuilder.AppendLine(string.Join(Constants.Constants.SplitCharacter,
					table.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

				// geneeal the CSV file, then write all from StringBuilder
				using var sw = new StreamWriter(strFilePath, false);
				foreach (DataRow dr in table.Rows)
				{
					if (csvBuilder.Capacity >= Constants.Constants.MaxCapacity)
					{
						sw.Write(csvBuilder.ToString());
						csvBuilder = new StringBuilder(Constants.Constants.Capacity);
					}
					csvBuilder.Append(String.Join(Constants.Constants.SplitCharacter, dr.ItemArray));
					csvBuilder.Append("\n");
				}
				sw.Write(csvBuilder.ToString());
				return (true, null);
			}
			catch(Exception e)
			{
				return (false, e.Message);
			}
		}

		public ResponseModel Search(string searchValue, int pageIndex, int pageSize)
		{
			try
			{
				Dictionary<String, StringMatchModel> Externals = File
				.ReadLines($"{Constants.Constants.FileName}")
				.Select(line => line.Split(','))
				.Where(x => Guid.TryParse(x[0], out _))
				.ToDictionary(
				  items => items[0],
				  items => CalculateStringMatch(items, searchValue)
				);
				var recordTotal = Externals.Count();
				var takeValue = pageSize == 0 ? 1 : pageSize;
				return new ResponseModel
				{
					IsSuccess = true,
					Message = "success",
					Paging = new PagingModel
					{
						PageIndex = pageIndex,
						PageSize = pageSize,
						PageTotal = (int)Math.Ceiling((float)recordTotal / takeValue),
						RecordTotal = recordTotal
					},
					Data = Externals.Select(x => x.Value).Skip((pageIndex - 1) * takeValue).Take(takeValue)
				};
			}
			catch(Exception e)
			{
				return new ResponseModel
				{
					IsSuccess = false,
					Message = e.Message
				};
			}
		}

		private StringMatchModel CalculateStringMatch(string[] items, string searchValue)
		{
			return new StringMatchModel
			{
				ID = items[0],
				Content = items[1],
				ExistTime = ExistTime(searchValue, items[1])
			};
		}

		//calculate how many time exist of a string into a parent string
		private int ExistTime(string searchValue, string parentString)
		{
			try
			{
				int searchValueLength = searchValue.Length;
				int parentLength = parentString.Length;
				int res = 0;

				for (int i = 0; i <= parentLength - searchValueLength; i++)
				{
					// check if pattern match
					int j;
					for (j = 0; j < searchValueLength; j++)
					{
						if (parentString[i + j] != searchValue[j])
						{
							break;
						}
					}

					if (j == searchValueLength)
					{
						res++;
					}
				}
				return res;
			}
			catch(Exception e)
			{
				return 0;
			}
		}
	}
}
