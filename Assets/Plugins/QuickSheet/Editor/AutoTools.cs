using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;

namespace UnityQuickSheet
{
    public class AutoTools
    {
        public static string excelFolderPath = "Assets/Resources/ExcelData";

        [MenuItem("Tools/QuickSheet/Excel/All Excel Data Exporter")]
        public static void ExportAllExcelData()
        {
            string folderFullPath = Path.GetFullPath(excelFolderPath);
            string[] files = Directory.GetFiles(folderFullPath, "*.xlsx", SearchOption.AllDirectories);
            string[] paths = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string SpreadSheetName = Path.GetFileName(files[i]);
                if (SpreadSheetName.EndsWith(".meta"))
                    continue;

                paths[i] = files[i].Substring(files[i].IndexOf("Assets"));
                string excelPath = paths[i];

                string[] sheets = new ExcelQuery(paths[i]).GetSheetNames();
                if (sheets.Length == 0)
                {
                    Debug.LogError("Error: Failed to retrieve the specified excel file.");
                    Debug.LogError("If the excel file is opened, close it then reopen it again.");
                    return;
                }

                for (int j = 0; j < sheets.Length; j++)
                {
                    string sheetName = sheets[j];

                    if (sheetName.StartsWith("~"))
                        continue;

                    if (string.IsNullOrEmpty(excelPath))
                    {
                        Debug.LogError("Failed to create a script from excel.");
                        string msg = "You should specify spreadsheet file first!";
                        EditorUtility.DisplayDialog("Error", msg, "OK");
                        return;
                    }

                    if (!File.Exists(excelPath))
                    {
                        Debug.LogError("Failed to create a script from excel.");
                        string msg = string.Format("File at {0} does not exist.", excelPath);
                        EditorUtility.DisplayDialog("Error", msg, "OK");
                        return;
                    }

                    List<MemberFieldData> fieldDatas = AutoScriptAGenerator.GetFieldData(excelPath, sheetName);

                    AutoScriptAGenerator.CreateScriptableObjectClassScript(sheetName);
                    AutoScriptAGenerator.CreateDataClassScript(sheetName, fieldDatas);
                    AutoScriptAGenerator.CreateScriptableObjectEditorClassScript(sheetName);
                    AutoScriptAGenerator.CreateAssetCreationScript(sheetName, excelPath);
                }
            }
            AssetDatabase.Refresh();
            Debug.Log("Successfully generated!");
        }
    }
}

