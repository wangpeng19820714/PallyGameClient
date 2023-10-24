using Codice.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityQuickSheet
{
    public class AutoPathUtility
    {
        public static string classPath = "Assets/Main/Scripts/Hotfix/Game/TableData";

        public static string editorPath = "Assets/Editor/TableData";

        public static string tempPath = "Plugins/QuickSheet/ExcelPlugin/Templates";
        public static string TargetPathForClassScript(string worksheetName)
        {
            return Path.Combine(classPath, worksheetName + "." + "cs");
        }

        public static string TargetPathForData(string worksheetName)
        {
            return Path.Combine(classPath, worksheetName + "Data" + "." + "cs");
        }

        public static string TargetPathForEditorScript(string worksheetName)
        {
            return Path.Combine(editorPath, worksheetName + "Editor" + "." + "cs");
        }

        public static string TargetPathForAssetPostProcessorFile(string worksheetName)
        {
            return Path.Combine(editorPath, worksheetName + "AssetPostProcessor" + "." + "cs");
        }

        public static string GetAbsoluteCustomTemplatePath()
        {
            return Path.Combine(Application.dataPath, tempPath);
        }

        public static string GetAbsoluteBuiltinTemplatePath()
        {
            return Path.Combine(EditorApplication.applicationContentsPath, tempPath);
        }

    }
}

