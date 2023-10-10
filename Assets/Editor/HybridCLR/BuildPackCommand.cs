using HybridCLR.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        Debug.Log("BuildFinish path = " + m_Settings.RemoteCatalogBuildPath.GetValue(m_Settings));
    }

    [MenuItem("HybridCLR/Build Pack/Print Path", priority = 3)]
    public static void PrintBuildPath()
    {
        Debug.Log("BuildPath = " + Addressables.BuildPath);
        Debug.Log("PlayerBuildDataPath = " + Addressables.PlayerBuildDataPath);
        Debug.Log("RemoteCatalogBuildPath = " + AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings));
    }

    public static void ClearAllAddressBuild()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetSettings.CleanPlayerContent(settings.ActivePlayerDataBuilder);
        var serverDataPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
        Debug.Log("clear serverdata " + serverDataPath);
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
            Debug.Log("No updated resources");
            return;
        }
        StringBuilder sbuider = new StringBuilder();
        sbuider.AppendLine("Need Update Assets:");
        foreach (var entry in entrys)
        {
            sbuider.AppendLine(entry.address);
        }
        Debug.Log(sbuider.ToString());
        var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
        ContentUpdateScript.CreateContentUpdateGroup(m_Settings, entrys, groupName);
    }
    public static string GetServerDataPath()
    {
        var path = Application.dataPath.Replace("Assets", "ServerData");
        path = FormatFilePath(path);
        return path;
    }
    public static string FormatFilePath(string filePath)
    {
        var path = filePath.Replace('\\', '/');
        path = path.Replace("//", "/");
        return path;
    }

    public static void SetAddresablesAssets()
    {

    }

     // 创建分组
     static AddressableAssetGroup CreateGroup<T>(string groupName)
     {
         AddressableAssetGroup group = Settings.FindGroup(groupName);
         if (group == null)
             group = Settings.CreateGroup(groupName, false, false, false, null, typeof(T));
         Settings.AddLabel(groupName, false);
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
}
