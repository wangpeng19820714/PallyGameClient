using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using GameFramework;
using GameFramework.Addressable;
using GameFramework.Resource;
using GameFramework.Fsm;
using GameFramework.ObjectPool;

public class LaunchLogic : MonoBehaviour
{
    AddressableUpdater updater;

    private void Awake()
    {
    }

    void Start()
    {
        //启动游戏
        StartCoroutine(InitGame());
    }

    private void OnDestroy()
    {
        //Destroy(m_GameStartPic);
    }

    IEnumerator InitGame()
    {
        var start = DateTime.Now;

        Application.runInBackground = true;

        //锁帧60
        Application.targetFrameRate = 60;

#if !UNITY_EDITOR
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

        // 启动资源管理模块
        start = DateTime.Now;
        yield return AddressablesManager.Instance.Initialize();
        //Logger.Log(string.Format("AssetBundleManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds));
        //启动加载器
        yield return HybridCLRLoader.Instance.Initialize();

        //启动状态机管理器
        yield return FsmManager.Instance.Initialize();

        //启动对象池管理器
        yield return ObjectPoolManager.Instance.Initialize();

        //添加UI根节点
        GameObject objUIRoot = GameObject.Find("UIRoot");
        if (objUIRoot != null)
        {
            DontDestroyOnLoad(objUIRoot);
        }

        DontDestroyOnLoad(Camera.main);

        //添加UNITY事件系统节点
        GameObject EventSystemObj = GameObject.Find("EventSystem");
        if (EventSystemObj != null)
        {
            DontDestroyOnLoad(EventSystemObj);
        }

        //添加Reporter根节点
        GameObject objReporter = GameObject.Find("Reporter");
        if (objReporter != null)
        {
            DontDestroyOnLoad(objReporter);
        }

        updater = gameObject.AddComponent<AddressableUpdater>();
#if !UNITY_EDITOR
        // 开始更新
        if (updater != null)
        {
            updater.StartCheckUpdate();
        }
#else
        StartGame();
#endif

        yield break;
    }

    public void StartGame()
    {

        // ResourceManager.Instance.LoadPrefabSync("LoadDataExample");
       GameObject gameLogic =  ResourceManager.Instance.LoadPrefabSync("GameLogic");
        DontDestroyOnLoad(gameLogic);
    }
}

