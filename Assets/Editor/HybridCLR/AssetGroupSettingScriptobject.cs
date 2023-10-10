using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AssetGroupData
{
    [SerializeField]
    public string GroupName;

    [SerializeField]
    public string FolderName;

    [SerializeField]
    public string Label;

    [SerializeField]
    public string BuildPath;

    [SerializeField]
    public string LoadPath;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AssetGroupSettingScriptableObject", order = 1)]
public class AssetGroupSettingScriptableObject : ScriptableObject
{
    public List<AssetGroupData> AssetGroupSetting;
}
