using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Installer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HybridCLR.Editor
{
    public class BuildPlayerCommand
    {
        public static void CopyAssets(string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            foreach (var srcFile in Directory.GetFiles(Application.streamingAssetsPath))
            {
                string dstFile = $"{outputDir}/{Path.GetFileName(srcFile)}";
                File.Copy(srcFile, dstFile, true);
            }
        }

        [MenuItem("HybridCLR/Build/Win64")]
        public static void Build_Win64()
        {
            BuildTarget target = BuildTarget.StandaloneWindows64;
            BuildTarget activeTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeTarget != BuildTarget.StandaloneWindows64 && activeTarget != BuildTarget.StandaloneWindows)
            {
                Debug.LogError("�����е�Winƽ̨�ٴ��");
                return;
            }
            // Get filename.
            string outputPath = $"{SettingsUtil.ProjectDir}/Release-Win64";

            var buildOptions = BuildOptions.CompressWithLz4;

            string location = $"{outputPath}/HybridCLRTrial.exe";

            PrebuildCommand.GenerateAll();
            Debug.Log("====> Build App");
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/Scenes/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = target,
                targetGroup = BuildTargetGroup.Standalone,
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("���ʧ��");
                return;
            }

            Debug.Log("====> �����ȸ�����Դ�ʹ���");
            BuildAssetsCommand.BuildAndCopyAOTHotUpdateDlls();
            //BashUtil.CopyDir(Application.streamingAssetsPath, $"{outputPath}/HybridCLRTrial_Data/StreamingAssets", true);
#if UNITY_EDITOR
            Application.OpenURL($"file:///{location}");
#endif
        }

        [MenuItem("HybridCLR/Build/Android")]
        public static void Build_Android()
        {
            BuildTarget target = BuildTarget.Android;
            BuildTarget activeTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeTarget != BuildTarget.Android && activeTarget != BuildTarget.Android)
            {
                Debug.LogError("�����е�Andriodƽ̨�ٴ��");
                return;
            }
            // Get filename.
            string outputPath = $"{SettingsUtil.ProjectDir}/Release-Android";

            var buildOptions = BuildOptions.CompressWithLz4;

            string location = $"{outputPath}/HybridCLRTrial.apk";

            PrebuildCommand.GenerateAll();
            Debug.Log("====> Build App");
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/Scenes/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = target,
                targetGroup = BuildTargetGroup.Android,
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("���ʧ��");
                return;
            }

            Debug.Log("====> �����ȸ�����Դ�ʹ���");
            BuildAssetsCommand.BuildAndCopyAOTHotUpdateDlls();
            //BashUtil.CopyDir(Application.streamingAssetsPath, $"{outputPath}/HybridCLRTrial_Data/StreamingAssets", true);
#if UNITY_EDITOR
            Application.OpenURL($"file:///{location}");
#endif
        }
    }
}