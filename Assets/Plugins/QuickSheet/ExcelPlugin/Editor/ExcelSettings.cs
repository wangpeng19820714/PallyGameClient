///////////////////////////////////////////////////////////////////////////////
///
/// ExcelSettings.cs
/// 
/// (c)2015 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UnityQuickSheet
{
    /// <summary>
    /// A class manages excel setting.
    /// </summary>
    [CreateAssetMenu(menuName = "QuickSheet/Setting/Excel Setting")]
    public class ExcelSettings : SingletonScriptableObject<ExcelSettings>
    {
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

        /// <summary>
        /// Select currently exist account setting asset file.
        /// </summary>
        [MenuItem("Tools/QuickSheet/Setting/Select Excel Setting")]
        public static void Edit()
        {
            Selection.activeObject = Instance;
            if (Selection.activeObject == null)
            {
                Debug.LogError(@"No ExcelSetting.asset file is found. Create setting file first. See the menu at 'Create/QuickSheet/Setting/Excel Setting'.");
            }
        }
    }
}
