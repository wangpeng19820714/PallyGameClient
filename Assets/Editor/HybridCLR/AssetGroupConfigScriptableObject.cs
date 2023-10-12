using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AssetGroupData
{
    public string GroupName;

    public string FolderName;

    public string Label;

    public string Filter;

    public string BuildPath;

    public string LoadPath;
}

[CreateAssetMenu(menuName = "ScriptableObjects/AssetGroupSettingScriptableObject")]
public class AssetGroupConfigScriptableObject : ScriptableObject
{
    public List<AssetGroupData> AssetGroupConfig;
}
