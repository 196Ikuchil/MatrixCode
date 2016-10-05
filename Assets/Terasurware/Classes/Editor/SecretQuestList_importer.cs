using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class SecretQuestList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/SecretQuestList.xlsx";
	private static readonly string exportPath = "Assets/Resources/Data/SecretQuestList.asset";
	private static readonly string[] sheetNames = { "QuestData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			QuestDataSheet data = (QuestDataSheet)AssetDatabase.LoadAssetAtPath (exportPath, typeof(QuestDataSheet));
			if (data == null) {
				data = ScriptableObject.CreateInstance<QuestDataSheet> ();
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

					QuestDataSheet.Sheet s = new QuestDataSheet.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						QuestDataSheet.Param p = new QuestDataSheet.Param ();
						
					cell = row.GetCell(0); p.enemy = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.killAmount = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.enemyLV = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.mob = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.time = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.memo = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.name = (cell == null ? "" : cell.StringCellValue);
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
