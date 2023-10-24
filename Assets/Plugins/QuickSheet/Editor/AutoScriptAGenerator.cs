using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityQuickSheet
{
    public class AutoScriptAGenerator
    {
        public static string bundlePath = "Assets/Bundle/Configs/";

        static readonly string NoTemplateString = "No Template file is found";

        public static void CreateScriptableObjectClassScript(string sheetName)
        {
            ScriptPrescription sp = new ScriptPrescription();
            sp.className = sheetName;
            sp.dataClassName = sheetName + "Data";
            sp.template = GetTemplate("ScriptableObjectClass");

            string fullPath = AutoPathUtility.TargetPathForClassScript(sheetName);
            string folderPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folderPath))
            {
                EditorUtility.DisplayDialog(
                    "Warning",
                    "The folder for runtime script files does not exist. Check the path " + folderPath + " exists.",
                    "OK"
                );
                return;
            }

            StreamWriter writer = null;
            try
            {
                // write a script to the given folder.		
                writer = new StreamWriter(fullPath);
                writer.Write(new ScriptGenerator(sp).ToString());
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public static void CreateScriptableObjectEditorClassScript(string sheetName)
        {
            ScriptPrescription sp = new ScriptPrescription();
            sp.className = sheetName + "Editor";
            sp.worksheetClassName = sheetName;
            sp.dataClassName = sheetName + "Data";
            sp.template = GetTemplate("ScriptableObjectEditorClass");

            // check the directory path exists
            string fullPath = AutoPathUtility.TargetPathForEditorScript(sp.worksheetClassName);
            string folderPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folderPath))
            {
                EditorUtility.DisplayDialog(
                    "Warning",
                    "The folder for editor script files does not exist. Check the path " + folderPath + " exists.",
                    "OK"
                    );
                return;
            }

            StreamWriter writer = null;
            try
            {
                // write a script to the given folder.		
                writer = new StreamWriter(fullPath);
                writer.Write(new ScriptGenerator(sp).ToString());
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
        public static void CreateDataClassScript(string sheetName, List<MemberFieldData> fieldList)
        {
            // check the directory path exists
            string fullPath = AutoPathUtility.TargetPathForData(sheetName);
            string folderPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folderPath))
            {
                EditorUtility.DisplayDialog(
                    "Warning",
                    "The folder for runtime script files does not exist. Check the path " + folderPath + " exists.",
                    "OK"
                    );
                return;
            }

            ScriptPrescription sp = new ScriptPrescription();
            sp.className = sheetName + "Data";
            sp.template = GetTemplate("DataClass");

            sp.memberFields = fieldList.ToArray();

            // write a script to the given folder.		
            using (var writer = new StreamWriter(fullPath))
            {
                writer.Write(new ScriptGenerator(sp).ToString());
                writer.Close();
            }
        }

        public static void CreateAssetCreationScript(string sheetName, string filePath)
        {
            ScriptPrescription sp = new ScriptPrescription();
            sp.className = sheetName;
            sp.dataClassName = sheetName + "Data";
            sp.worksheetClassName = sheetName;

            // where the imported excel file is.
            sp.importedFilePath = filePath.Replace('\\', '/');

            // path where the .asset file will be created.
            string path = Path.GetDirectoryName(bundlePath);
            path += "/" + sheetName + ".asset";
            sp.assetFilepath = path.Replace('\\', '/');
            sp.assetPostprocessorClass = sheetName + "AssetPostprocessor";
            sp.template = GetTemplate("PostProcessor");

            // write a script to the given folder.
            using (var writer = new StreamWriter(AutoPathUtility.TargetPathForAssetPostProcessorFile(sheetName)))
            {
                writer.Write(new ScriptGenerator(sp).ToString());
                writer.Close();
            }
        }

        static string GetTemplate(string nameWithoutExtension)
        {
            string path = Path.Combine(AutoPathUtility.GetAbsoluteCustomTemplatePath(), nameWithoutExtension + ".txt");
            if (File.Exists(path))
                return File.ReadAllText(path);

            path = Path.Combine(AutoPathUtility.GetAbsoluteBuiltinTemplatePath(), nameWithoutExtension + ".txt");
            if (File.Exists(path))
                return File.ReadAllText(path);

            return NoTemplateString;
        }

        public static List<MemberFieldData> GetFieldData(string excelPath, string sheetName)
        {
            List<MemberFieldData> fieldList = new List<MemberFieldData>();

            int nameIndex = 0;
            string error = string.Empty;
            var titles = new ExcelQuery(excelPath, sheetName).GetTitle(nameIndex, ref error);
            var types = new ExcelQuery(excelPath, sheetName).GetTitle(nameIndex + 1, ref error);
            if (titles == null || types == null || !string.IsNullOrEmpty(error))
            {
                EditorUtility.DisplayDialog("Error", error, "OK");
                return null;
            }
            else
            {
                foreach (string column in titles)
                {
                    if (!IsValidHeader(column))
                    {
                        error = string.Format(@"Invalid column header name {0}. Any c# keyword should not be used for column header. Note it is not case sensitive.", column);
                        EditorUtility.DisplayDialog("Error", error, "OK");
                        return null;
                    }
                }

                for (int i = 0; i < titles.Length; i++)
                {
                    MemberFieldData member = new MemberFieldData();
                    member.Name = titles[i];
                    member.type = Enum.Parse<CellType>(types[i]);
                    if (types[i].EndsWith("[]"))
                    {
                        member.IsArrayType = true;
                    }
                    else
                    {
                        member.IsArrayType = false;
                    }
                    fieldList.Add(member);
                }
                return fieldList;
            }
        }

        static bool IsValidHeader(string s)
        {
            // no case sensitive!
            string comp = s.ToLower();

            string found = Array.Find(Util.Keywords, x => x == comp);
            if (string.IsNullOrEmpty(found))
                return true;

            return false;
        }
    }
}

