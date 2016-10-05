using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ListTest_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/ListTest.xlsx";
	private static readonly string exportPath = "Assets/Resources/Data/ListTest.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			AbilityList data = (AbilityList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(AbilityList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<AbilityList> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					AbilityList.Sheet s = new AbilityList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						AbilityList.Param p = new AbilityList.Param ();
						
					cell = row.GetCell(0); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Attack = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.Sp = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.benefit = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.value = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.element = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.ability = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.motion = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.memo = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.range = (float)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
