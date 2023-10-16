using HybridCLR.Editor.Commands;
using System.IO;
using UnityEditor;
using UnityEngine;
using Game;
using GameFramework;

namespace HybridCLR.Editor
{
    public static class BuildAssetsCommand
    {
        public static string HybridCLRBuildCacheDir => Application.dataPath + "/HybridCLRBuildCache";

        public static string AssetBundleOutputDir => $"{HybridCLRBuildCacheDir}/AssetBundleOutput";

        public static string AssetBundleSourceDataTempDir => $"{HybridCLRBuildCacheDir}/AssetBundleSourceData";


        public static string GetAssetBundleOutputDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleOutputDir}/{target}";
        }

        public static string GetAssetBundleTempDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleSourceDataTempDir}/{target}";
        }

        public static string ToRelativeAssetPath(string s)
        {
            return s.Substring(s.IndexOf("Assets/"));
        }

        [MenuItem("HybridCLR/Build/BuildAssetsAndCopyToBundle")]
        public static void BuildAndCopyAOTHotUpdateDlls()
        {
            CompileDllCommand.CompileDllActiveBuildTarget();
            CopyAOTAssembliesToAddressable();
            CopyHotUpdateAssembliesToAddressable();
            AssetDatabase.Refresh();
        }

        public static void CopyAOTAssembliesToAddressable()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            string assetBundlePathDst = $"{Application.dataPath}/Bundle/Hotfix";
            Directory.CreateDirectory(assetBundlePathDst);

            foreach (var dll in HybridCLRLoader.AOTMetaAssemblyNames)
            {
                string srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
                if (!File.Exists(srcDllPath))
                {
                    Log.Error($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                string dllBytesPath = $"{assetBundlePathDst}/{dll}.bytes";
                File.Copy(srcDllPath, dllBytesPath, true);
               Log.Debug($"[CopyAOTAssembliesToAddressable] copy AOT dll {srcDllPath} -> {dllBytesPath}");
            }
        }

        public static void CopyHotUpdateAssembliesToAddressable()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;

            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            //string hotfixAssembliesDstDir = Application.streamingAssetsPath;
            string assetBundleDst = $"{Application.dataPath}/Bundle/Hotfix";
            Directory.CreateDirectory(assetBundleDst);

            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesIncludePreserved)
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}";
                string dllBytesPath = $"{assetBundleDst}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
                Log.Debug($"[CopyHotUpdateAssembliesToAddressable] copy hotfix dll {dllPath} -> {dllBytesPath}");
            }
        }

        public static void BuildAssetBundleByTarget(BuildTarget target, bool buildAot)
        {
            CompileDllCommand.CompileDll(target);
            if (buildAot)
            {
                CopyAOTAssembliesToAddressable();
            }
            CopyHotUpdateAssembliesToAddressable();
            AssetDatabase.Refresh();
        }

        public static void BuildSceneAssetBundleActiveBuildTarget()
        {
            BuildAssetBundleByTarget(EditorUserBuildSettings.activeBuildTarget, true);
        }

        public static void BuildSceneAssetBundleActiveBuildTargetExcludeAOT()
        {
            BuildAssetBundleByTarget(EditorUserBuildSettings.activeBuildTarget, false);
        }
    }
}

