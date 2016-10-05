using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Code_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/Code.xlsx";
	private static readonly string exportPath = "Assets/Resources/Data/Code.asset";
	private static readonly string[] sheetNames = { "Code", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			MatrixCode data = (MatrixCode)AssetDatabase.LoadAssetAtPath (exportPath, typeof(MatrixCode));
			if (data == null) {
				data = ScriptableObject.CreateInstance<MatrixCode> ();
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

					MatrixCode.Sheet s = new MatrixCode.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						MatrixCode.Param p = new MatrixCode.Param ();
						
					cell = row.GetCell(0); p.col0 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.col1 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.col2 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.col3 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.col4 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.col5 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.col6 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.col7 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.col8 = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.col9 = (cell == null ? 0.0 : cell.NumericCellValue);
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
