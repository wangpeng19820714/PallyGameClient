using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;


public class HybridCLRLoader : MonoSingleton<HybridCLRLoader>
{
    public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
{
    "mscorlib.dll",
    "System.dll",
    "System.Core.dll",
};
    public IEnumerator Initialize()
    {
        yield break;
    }

    void Start()
    {
        this.StartGame();
    }

    void StartGame()
    {
        LoadMetadataForAOTAssemblies();

#if !UNITY_EDITOR
    TextAsset dllBytes = ResourceManager.Instance.LoadTextAssetSync("Assets/Bundle/Hotfix/Game.HotFix.dll");
    var gameAss = System.Reflection.Assembly.Load(dllBytes.bytes);
#else
        var gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Game.HotFix");
#endif
        var hotUpdatePrefab = ResourceManager.Instance.LoadPrefabSync("HotUpdatePrefab");
        GameObject go = hotUpdatePrefab;
    }

    /// <summary>
    /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
    /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        // ���Լ�������aot assembly�Ķ�Ӧ��dll����Ҫ��dll������unity build���������ɵĲü����dllһ�£�������ֱ��ʹ��ԭʼdll��
        // ������BuildProcessors������˴�����룬��Щ�ü����dll�ڴ��ʱ�Զ������Ƶ� {��ĿĿ¼}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} Ŀ¼��

        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyNames)
        {
            byte[] dllBytes = ResourceManager.Instance.LoadTextAssetSync($"Assets/Bundle/Hotfix/{aotDllName}").bytes;
            // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Log.Debug($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
