using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;
using Game.Hotfix.TableData;

///
/// !!! Machine generated code !!!
///
public class SceneDataAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Resources/ExcelData/Scene.xlsx";
    private static readonly string assetFilePath = "Assets/Bundle/Configs/SceneData.asset";
    private static readonly string sheetName = "SceneData";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            SceneData data = (SceneData)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(SceneData));
            if (data == null) {
                data = ScriptableObject.CreateInstance<SceneData> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<SceneDataData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.DeserializeDic<SceneDataData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
