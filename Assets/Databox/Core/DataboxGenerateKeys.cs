using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

namespace Databox.Ed
{
	// Looks through a databox object and generates static strings
	// from tables, entries and values
	public static class DataboxGenerateKeys
	{
		private static string result = "// ------ GENERATED STATIC KEYS FOR DATABOX OBJECT ------";
		
		public static string GenerateStaticStrings(DataboxObject _databox)
		{
			result = "// ------ GENERATED STATIC KEYS FOR DATABOX OBJECT ------";
			
			var _databoxName = _databox.name;
			_databoxName = _databoxName.Replace(" ", "_");
			
			result += "\n" + "public static class " + _databoxName + "_KEYS" + "\n{";
			
			foreach(var table in _databox.DB.Keys)
			{
				
				var _tableName = table;
				
				int n;
				if (int.TryParse(_tableName, out n))
				{
					// Add underscore if table name is numeric
					_tableName = _tableName.Insert(0, "_");
				}
				
				_tableName = _tableName.Replace(" " , "_");
				
				result += "\n\t" + "public static class " + _tableName + "\n\t{";
				
				result += "\n\t\t" + "public static string " + "TableName" + " = " + '"' + table + '"' + "; \n";
				
				foreach(var entry in _databox.DB[table].entries.Keys)
				{
					
					var _entryName = entry;
					
					int en;
					if (int.TryParse(_entryName, out en))
					{
						// Add underscore if entry name is numeric
						_entryName = _entryName.Insert(0, "_");
					}
					
					_entryName = _entryName.Replace(" ", "_");
					
					result += "\n\t\t" + "public static class " + _entryName + "\n\t\t{";
					
					result += "\n\t\t\t" + "public static string " + "EntryName" + " = " + '"' + entry + '"' + ";";
					
					foreach(var value in _databox.DB[table].entries[entry].data.Keys)
					{
						var _valueName = value;
						_valueName = _valueName.Replace(" ", "_");
						
						var _typeName = "";
						foreach(var type in _databox.DB[table].entries[entry].data[value].Keys)
						{
							_typeName = type.ToString();
						}
						// Add Summary description
						result += "\n\t\t\t" + "/// <summary>";
						result += "\n\t\t\t" + "///" + " Type of: " + _typeName;
						result += "\n\t\t\t" + "/// </summary>";
						
						result += "\n\t\t\t" + "public static string " + "_" + _valueName + " = " + '"' + value + '"' + ";";
					}
					
					result += "\n\t\t" + "}";
				}
				
				result += "\n\t" + "}";
			}
			
			result += "\n" + "}";
			
			
			return result;
		}
	}
}
