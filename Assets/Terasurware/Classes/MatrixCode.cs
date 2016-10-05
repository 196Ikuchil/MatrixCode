using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatrixCode : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public double col0;
		public double col1;
		public double col2;
		public double col3;
		public double col4;
		public double col5;
		public double col6;
		public double col7;
		public double col8;
		public double col9;
	}
}

