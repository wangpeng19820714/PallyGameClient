using Game;
using GameFramework.Addressable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace GameLogic
{
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
            //启动加载器
            HybridCLRLoader.Instance.Initialize();
        }
    }
}

