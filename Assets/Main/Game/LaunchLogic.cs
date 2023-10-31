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
        //������Ϸ
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

        //��֡60
        Application.targetFrameRate = 60;

#if !UNITY_EDITOR
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

        // ������Դ����ģ��
        start = DateTime.Now;
        yield return AddressablesManager.Instance.Initialize();
        //Logger.Log(string.Format("AssetBundleManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds));
        //����������
        yield return HybridCLRLoader.Instance.Initialize();

        //����״̬��������
        yield return FsmManager.Instance.Initialize();

        //��������ع�����
        yield return ObjectPoolManager.Instance.Initialize();

        //���UI���ڵ�
        GameObject objUIRoot = GameObject.Find("UIRoot");
        if (objUIRoot != null)
        {
            DontDestroyOnLoad(objUIRoot);
        }

        DontDestroyOnLoad(Camera.main);

        //���UNITY�¼�ϵͳ�ڵ�
        GameObject EventSystemObj = GameObject.Find("EventSystem");
        if (EventSystemObj != null)
        {
            DontDestroyOnLoad(EventSystemObj);
        }

        //���Reporter���ڵ�
        GameObject objReporter = GameObject.Find("Reporter");
        if (objReporter != null)
        {
            DontDestroyOnLoad(objReporter);
        }

        updater = gameObject.AddComponent<AddressableUpdater>();
#if !UNITY_EDITOR
        // ��ʼ����
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

