using System;
namespace TimeoutSample.DataLayer
{
	public class TableConfig(string tableName)
	{
		public string TableName => tableName;
	}
}

