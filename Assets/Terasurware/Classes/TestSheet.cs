using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSheet : ScriptableObject
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
		
		public string Name;
		public int id;
		public float Attack;
		public float Sp;
		public float benefit;
		public float value;
		public int element;
		public float ability;
		public int motion;
		public string memo;
	}
}

