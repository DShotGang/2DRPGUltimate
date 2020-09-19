using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

public class DataboxCSVConverter
{
	static string ignoreKey = "DB_IGNORE";
	static string fieldNameKey = "DB_FIELD_NAMES";
	static string fieldTypeKey = "DB_FIELD_TYPES";
	
	public static bool firstTimeReplace = true;
	
	[System.Serializable]
	public class Entry
	{
		public string entryName = "";
		public List<string> fields = new List<string>();
		public List<string> types = new List<string>();
		public List<string> values = new List<string>();
		
		public Entry(string _entryName)
		{
			entryName = _entryName;
		}
	}

	
	public static void ConvertCSV(string _input, out List<Entry> entries)
	{

		entries = new List<Entry>();
		
		var _output = DataboxCSVReader.SplitCsvGrid(_input); 
		var _fieldTypesIndex = 0;
		var _fieldNamesIndex = 0;
		
		for (int x = 0; x < _output.GetLength(0); x ++)
		{
		
			
			for (int y = 0; y < _output.GetLength(1); y ++)
			{
				
				//Debug.Log(_output[x, y]);
				
				if (_output[x, y] == fieldTypeKey)
				{
					_fieldTypesIndex = y;
				}
				
				if (_output[x, y] == fieldNameKey)
				{
					_fieldNamesIndex = y;
				}
				
				//// Entry names
				if (x == 0 && y != _fieldNamesIndex && y != _fieldTypesIndex)
				{
					//Debug.Log("Entry: " + _output[x, y]);
					
					var _entryYIndex = y;
					
					if (!string.IsNullOrEmpty(_output[x,y]) && _output[x,y] != ignoreKey)
					{
						entries.Add(new Entry(_output[x, y]));
						
					
						for (int x1 = 0; x1 < _output.GetLength(0); x1 ++)
						{
							for (int y1 = 0; y1 < _output.GetLength(1); y1 ++)
							{
								// field names
								if (x1 > 0 && y1 == _fieldNamesIndex)
								{
									if (!string.IsNullOrEmpty(_output[x1,y1]))
									{
										entries[entries.Count-1].fields.Add(_output[x1,y1]);
									}
								}
								// field types
								if (x1 > 0 && y1 == _fieldTypesIndex)
								{
									if (!string.IsNullOrEmpty(_output[x1,y1]))
									{
										entries[entries.Count-1].types.Add(_output[x1,y1]);
									}
								}
								// values
								if (x1 > 0 && y1 == _entryYIndex && y1 != _fieldTypesIndex && y1 != _fieldNamesIndex)
								{
									entries[entries.Count - 1].values.Add(_output[x1, y1]);									
								}
							}
						}
						
					}
				}
			}
		}
		
		// cleanup
		for (int e = 0; e < entries.Count; e ++)
		{
			for (int f = 0; f < entries[e].fields.Count; f ++)
			{
				if (entries[e].fields[f] == ignoreKey)
				{
					entries[e].types[f] = ignoreKey;
					entries[e].values[f] = ignoreKey;
					entries[e].fields[f] = ignoreKey;
				}
			}
			
			for (int v = 0; v < entries[e].values.Count; v ++)
			{
				if (string.IsNullOrEmpty(entries[e].values[v]))
				{
					if (v < entries[e].types.Count)
					{
						entries[e].types[v] = ignoreKey;
					}

					if (v < entries[e].fields.Count)
					{
						entries[e].fields[v] = ignoreKey;
					}
					
					if (v < entries[e].values.Count)
					{
						entries[e].values[v] = ignoreKey;
					}
				}
			}
		}
		
		// remove all entries which contains the ignore key
		for (int e = 0; e < entries.Count; e ++)
		{
			for (int v = entries[e].values.Count - 1; v >= 0; v --)
			{
				if (entries[e].values[v].Contains(ignoreKey))
				{
					entries[e].values.RemoveAt(v);
				}
			}

			for (int t = entries[e].types.Count - 1; t >= 0 ; t --)
			{
				if (entries[e].types[t].Contains(ignoreKey))
				{
				
					entries[e].types.RemoveAt(t);
				}
			}

			for (int f = entries[e].fields.Count - 1; f >= 0; f --)
			{
				if (entries[e].fields[f].Contains(ignoreKey))
				{
					entries[e].fields.RemoveAt(f);
				}
			}
		}
	}
		
		
	public static void AppendToDB(DataboxObject _database, string _tableName, List<Entry> _entries)
	{
		// Add entries
		for (int e = 0; e < _entries.Count; e ++)
		{
			for (int a = 0; a < _entries[e].values.Count; a ++)
			{

				var _type = System.Type.GetType(_entries[e].types[a]);
				var _instance = System.Activator.CreateInstance(_type) as DataboxType;
				
				// Convert csv string value to DataboxType value
				_instance.Convert(_entries[e].values[a]);
				
				_database.AddData(_tableName, _entries[e].entryName, _entries[e].fields[a], _instance as DataboxType);
			}
		}
	}	
	
	public static void ReplaceDB(DataboxObject _database, string _tableName, List<Entry> _entries)
	{
		if (firstTimeReplace)
		{
			_database.DB = new Databox.Dictionary.OrderedDictionary<string, DataboxObject.Database>();
			firstTimeReplace = false;
		}
		
		// Add entries
		for (int e = 0; e < _entries.Count; e ++)
		{
			for (int a = 0; a < _entries[e].values.Count; a ++)
			{

				var _type = System.Type.GetType(_entries[e].types[a]);
				var _instance = System.Activator.CreateInstance(_type) as DataboxType;
				
				// Convert csv string value to DataboxType value
				_instance.Convert(_entries[e].values[a]);
				
				_database.AddData(_tableName, _entries[e].entryName, _entries[e].fields[a], _instance as DataboxType);
			}
		}

	}
}
