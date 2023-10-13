using HybridCLR.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using GameFramework;

public class BuildPackCommand
{
    [MenuItem("HybridCLR/Build Pack/Clean And Build Content", priority = 1)]
    public static void ReBuildAddress()
    {
        ClearAllAddressBuild();
        BuildAssetsCommand.BuildSceneAssetBundleActiveBuildTarget();
        AddressableAssetSettings.BuildPlayerContent();
    }

    [MenuItem("HybridCLR/Build Pack/Build Update Content", priority = 2)]
    public static void BuildUpdate()
    {
        BuildAssetsCommand.BuildSceneAssetBundleActiveBuildTargetExcludeAOT();

        //对比更新列表
        CheckForUpdateContent();

        var path = ContentUpdateScript.GetContentStateDataPath(false);
        var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressablesPlayerBuildResult result = ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
        Log.Debug("BuildFinish path = " + m_Settings.RemoteCatalogBuildPath.GetValue(m_Settings));
    }

    [MenuItem("HybridCLR/Build Pack/Asset Group Settings", priority = 3)]
    public static void SetAddresablesAssets()
    {
        string settingPath = "Assets/Resources/Settings/Addressable/AssetGroupSettingData.asset";
        AssetGroupConfigScriptableObject AssetsGroupSetting = AssetDatabase.LoadAssetAtPath<AssetGroupConfigScriptableObject>(settingPath);
        AddressableAssetGroup group = null;
        foreach (AssetGroupData data in AssetsGroupSetting.AssetGroupConfig)
        {
            string[] assets = GetAssets("Assets/" + data.FolderName, data.Filter);
            group = CreateGroup(data.GroupName);

            foreach (var assetPath in assets)
            {
                string address = assetPath;
                AddAssetEntry(group, assetPath, address);
            }
            EditorUtility.SetDirty(Settings);
            AssetDatabase.Refresh();

            Log.Debug($"Reset group finished, group: {data.GroupName}, asset folder: {data.FolderName}, filter: {data.Filter}, count: {assets.Length}");
        }
    }

    [MenuItem("HybridCLR/Build Pack/Print Path", priority = 4)]
    public static void PrintBuildPath()
    {
        Log.Debug("BuildPath = " + Addressables.BuildPath);
        Log.Debug("PlayerBuildDataPath = " + Addressables.PlayerBuildDataPath);
        Log.Debug("RemoteCatalogBuildPath = " + AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings));
    }

    public static void ClearAllAddressBuild()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetSettings.CleanPlayerContent(settings.ActivePlayerDataBuilder);
        var serverDataPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
        Log.Debug("clear serverdata " + serverDataPath);
        if (System.IO.Directory.Exists(serverDataPath))
        {
            System.IO.Directory.Delete(serverDataPath, true);
        }
    }

    public static void CheckForUpdateContent()
    {
        string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
        var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
        List<AddressableAssetEntry> entrys = ContentUpdateScript.GatherModifiedEntries(m_Settings, buildPath);
        if (entrys.Count == 0)
        {
            Log.Debug("No updated resources");
            return;
        }
        StringBuilder sbuider = new StringBuilder();
        sbuider.AppendLine("Need Update Assets:");
        foreach (var entry in entrys)
        {
            sbuider.AppendLine(entry.address);
        }
        Log.Debug(sbuider.ToString());
        var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
        ContentUpdateScript.CreateContentUpdateGroup(m_Settings, entrys, groupName);
    }
    public static string GetServerDataPath()
    {
        var path = Application.dataPath.Replace("Assets", "ServerData");
        path = path.Replace('\\', '/');
        path = path.Replace("//", "/");
        return path;
    }

     // 创建分组
     static AddressableAssetGroup CreateGroup(string groupName)
     {
         AddressableAssetGroup group = Settings.FindGroup(groupName);
         if (group == null)
        {
            group = Settings.CreateGroup(groupName, false, false, false, null);
        }
        Settings.AddLabel(groupName, false);

        ContentUpdateGroupSchema cugSchema = group.GetSchema<ContentUpdateGroupSchema>();
        if (cugSchema == null)
        {
            cugSchema = group.AddSchema<ContentUpdateGroupSchema>();
        }
        cugSchema.StaticContent = false;

        BundledAssetGroupSchema bagSchema = group.GetSchema<BundledAssetGroupSchema>();
        var defultBAGSchema = Settings.DefaultGroup.GetSchema<BundledAssetGroupSchema>();
        if (bagSchema == null)
        {
            bagSchema = group.AddSchema<BundledAssetGroupSchema>();
        }
        bagSchema.BuildPath.SetVariableByName(Settings, AddressableAssetSettings.kLocalBuildPath);
        bagSchema.LoadPath.SetVariableByName(Settings, AddressableAssetSettings.kLocalLoadPath);

         return group;
     }

    // 给某分组添加资源
    static AddressableAssetEntry AddAssetEntry(AddressableAssetGroup group, string assetPath, string address)
    {
        string guid = AssetDatabase.AssetPathToGUID(assetPath);

        AddressableAssetEntry entry = group.entries.FirstOrDefault(e => e.guid == guid);
        if (entry == null)
        {
            entry = Settings.CreateOrMoveEntry(guid, group, false, false);
        }

        entry.address = address;
        entry.SetLabel(group.Name, true, false, false);
        return entry;
    }

    ///<summary>
    /// 获取指定目录的资源
    /// </summary>
    /// <param name="filter">过滤器：
    /// 若以t:开头，表示用unity的方式过滤;
    /// 若以f:开头，表示用windows的SearchPattern方式过滤;
    /// 若以r:开头，表示用正则表达式的方式过滤。</param>
    public static string[] GetAssets(string folder, string filter)
    {
        if (string.IsNullOrEmpty(folder))
            throw new ArgumentException("folder");
        if (string.IsNullOrEmpty(filter))
            throw new ArgumentException("filter");

        folder = folder.TrimEnd('/').TrimEnd('\\');

        if (filter.StartsWith("t:"))
        {
            string[] guids = AssetDatabase.FindAssets(filter, new string[] { folder });
            string[] paths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            return paths;
        }
        else if (filter.StartsWith("f:"))
        {
            string folderFullPath = Path.GetFullPath(folder);
            string searchPattern = filter.Substring(2);
            string[] files = Directory.GetFiles(folderFullPath, searchPattern, SearchOption.AllDirectories);
            string[] paths = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string assetName = files[i];
                paths[i] = assetName.Substring(assetName.IndexOf("Assets"));
            }
                
            return paths;
        }
        else if (filter.StartsWith("r:"))
        {
            string folderFullPath = Path.GetFullPath(folder);
            string pattern = filter.Substring(2);
            string[] files = Directory.GetFiles(folderFullPath, "*.*", SearchOption.AllDirectories);
            List<string> list = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                string name = Path.GetFileName(files[i]);
                if (name.EndsWith(".meta")) 
                    continue;

                if (Regex.IsMatch(name, pattern))
                {
                    string assetName = files[i];
                    assetName = assetName.Substring(assetName.IndexOf("Assets"));
                    list.Add(assetName.Replace("\\", "/"));
                }
            }
            return list.ToArray();
        }
        else
        {
            throw new InvalidOperationException("Unexpected filter: " + filter);
        }
    }

    static AddressableAssetSettings Settings
    {
        get { return AddressableAssetSettingsDefaultObject.Settings; }
    }
}
