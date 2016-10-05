using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestDataSheet : ScriptableObject
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
		
		public string enemy;
		public int killAmount;
		public int enemyLV;
		public int mob;
		public float time;
		public string memo;
		public string name;
	}
}

