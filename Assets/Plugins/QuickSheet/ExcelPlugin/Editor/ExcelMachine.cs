///////////////////////////////////////////////////////////////////////////////
///
/// ExcelMachine.cs
///
/// (c)2014 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using static UnityEngine.GraphicsBuffer;

namespace UnityQuickSheet
{
    /// <summary>
    /// A class for various setting to read excel file and generated related script files.
    /// </summary>
    internal class ExcelMachine : BaseMachine
    {
        /// <summary>
        /// where the .xls or .xlsx file is. The path should start with "Assets/".
        /// </summary>
        public string excelFilePath;

        public static string excelFolderPath = "Assets/Resources/ExcelData";

        public static string classPath = "Assets/Main/Scripts/Hotfix/Game/TableData";

        public static string editorPath = "Assets/Editor/TableData";

        public static string bundlePath = "Assets/Bundle/Configs/";

        public static string TempPath = "Plugins/QuickSheet/ExcelPlugin/Templates";

        static readonly string NoTemplateString = "No Template file is found";
        /// <summary>
        /// A default path where .txt template files are.
        /// </summary>
        public string TemplatePath = "Plugins/QuickSheet/ExcelPlugin/Templates";

        /// <summary>
        /// A path where generated ScriptableObject derived class and its data class script files are to be put.
        /// </summary>
        public string RuntimePath = "Main/Hotfix/Game/TableData";

        /// <summary>
        /// A path where generated editor script files are to be put.
        /// </summary>
        public string EditorPath = "Editor/TableData";

        // both are needed for popup editor control.
        public string[] SheetNames = { "" };
        public int CurrentSheetIndex
        {
            get { return currentSelectedSheet; }
            set { currentSelectedSheet = value;}
        }

        [SerializeField]
        protected int currentSelectedSheet = 0;


        /// <summary>
        /// Note: Called when the asset file is created.
        /// </summary>

        private void Awake() {
            if (ExcelSettings.Instance != null)
            {
                // excel and google plugin have its own template files,
                // so we need to set the different path when the asset file is created.
                TemplatePath = ExcelSettings.Instance.TemplatePath;
            }
        }

        /// <summary>
        /// A menu item which create a 'ExcelMachine' asset file.
        /// </summary>
        [MenuItem("Tools/QuickSheet/Excel Asset Import")]
        public static void CreateScriptMachineAsset()
        {
            ExcelMachine inst = ScriptableObject.CreateInstance<ExcelMachine>();
            string path = CustomAssetUtility.GetUniqueAssetPathNameOrFallback(ImportSettingFilename);
            AssetDatabase.CreateAsset(inst, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = inst;
        }

        [MenuItem("Tools/QuickSheet/All Excel Asset Export")]
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

                string[] sheets = new ExcelQuery(paths[i]).GetSheetNames();
                if (sheets.Length == 0)
                {
                    Debug.LogError("Error: Failed to retrieve the specified excel file.");
                    Debug.LogError("If the excel file is opened, close it then reopen it again.");
                    return;
                }
                for (int j = 0; j < sheets.Length; j++)
                {
                    string excelPath = paths[i];
                    string sheetName = sheets[j];

                    if (sheetName.StartsWith("~"))
                        continue;

                    if (string.IsNullOrEmpty(excelPath))
                    {
                        string msg = "You should specify spreadsheet file first!";
                        EditorUtility.DisplayDialog("Error", msg, "OK");
                        return;
                    }

                    if (!File.Exists(excelPath))
                    {
                        string msg = string.Format("File at {0} does not exist.", excelPath);
                        EditorUtility.DisplayDialog("Error", msg, "OK");
                        return;
                    }

                    List<MemberFieldData> fieldDatas = GetFieldData(excelPath, sheetName);

                    CreateScriptableObjectClassScript(sheetName);
                    CreateDataClassScript(sheetName, fieldDatas);
                    CreateScriptableObjectEditorClassScript(sheetName);
                    CreateAssetCreationScript(sheetName, excelPath);
                }
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

        static void CreateScriptableObjectClassScript(string sheetName)
        {
            ScriptPrescription sp = new ScriptPrescription();
            sp.className = sheetName;
            sp.dataClassName = sheetName + "Data";
            sp.template = GetTemplate("ScriptableObjectClass");

            string fullPath = TargetPathForClassScript(sheetName);
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

        static void CreateScriptableObjectEditorClassScript(string sheetName)
        {
            ScriptPrescription sp = new ScriptPrescription();
            sp.className = sheetName + "Editor";
            sp.worksheetClassName = sheetName;
            sp.dataClassName = sheetName + "Data";
            sp.template = GetTemplate("ScriptableObjectEditorClass");

            // check the directory path exists
            string fullPath = TargetPathForEditorScript(sp.worksheetClassName);
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

        static List<MemberFieldData> GetFieldData(string excelPath, string sheetName)
        {
            List<MemberFieldData> fieldList = new List<MemberFieldData>(); 

           int nameIndex = 0;
            string error = string.Empty;
            var titles = new ExcelQuery(excelPath, sheetName).GetTitle(nameIndex, ref error);
            var types = new ExcelQuery(excelPath, sheetName).GetValue(nameIndex + 1, ref error);
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

                for(int i = 0; i< titles.Length; i++)
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

        static void CreateDataClassScript(string sheetName, List<MemberFieldData> fieldList)
        {
            // check the directory path exists
            string fullPath = TargetPathForData(sheetName);
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

        static void CreateAssetCreationScript(string sheetName, string filePath)
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
            using (var writer = new StreamWriter(TargetPathForAssetPostProcessorFile(sheetName)))
            {
                writer.Write(new ScriptGenerator(sp).ToString());
                writer.Close();
            }
        }

        static string TargetPathForClassScript(string worksheetName)
        {
            return Path.Combine(classPath, worksheetName + "." + "cs");
        }

        static string TargetPathForData(string worksheetName)
        {
            return Path.Combine(classPath, worksheetName + "Data" + "." + "cs");
        }

        static string TargetPathForEditorScript(string worksheetName)
        {
            return Path.Combine(editorPath, worksheetName + "Editor" + "." + "cs");
        }

        static string TargetPathForAssetPostProcessorFile(string worksheetName)
        {
            return Path.Combine(editorPath, worksheetName + "AssetPostProcessor" + "." + "cs");
        }

        static string GetTemplate(string nameWithoutExtension)
        {
            string path = Path.Combine(GetAbsoluteCustomTemplatePath(), nameWithoutExtension + ".txt");
            if (File.Exists(path))
                return File.ReadAllText(path);

            path = Path.Combine(GetAbsoluteBuiltinTemplatePath(), nameWithoutExtension + ".txt");
            if (File.Exists(path))
                return File.ReadAllText(path);

            return NoTemplateString;
        }

        static string GetAbsoluteCustomTemplatePath()
        {
            return Path.Combine(Application.dataPath, TempPath);
        }

        static string GetAbsoluteBuiltinTemplatePath()
        {
            return Path.Combine(EditorApplication.applicationContentsPath, TempPath);
        }
    }
}