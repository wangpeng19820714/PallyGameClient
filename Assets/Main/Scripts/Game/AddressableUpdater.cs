using GameFramework.Addressable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace GameLogic
{
    public class AddressableUpdater : MonoBehaviour
    {
        Text statusText;
        //Slider slider;

        float totalTime = 0f;
        bool needUpdateRes = false;

        void Awake()
        {
            statusText = transform.gameObject.AddComponent<Text>();
        }

        void Start()
        {
            totalTime = 0f;
            statusText.text = "正在检测资源更新...";
        }


        public void StartCheckUpdate()
        {
            StartCoroutine(checkUpdate());
        }

        IEnumerator checkUpdate()
        {
            var start = DateTime.Now;


            var initHandle = Addressables.InitializeAsync();
            yield return initHandle;

            var a = Addressables.RuntimePath;
            var checkHandle = Addressables.CheckForCatalogUpdates(false);
            yield return checkHandle;
            Debug.Log(string.Format("CheckIfNeededUpdate use {0}ms", (DateTime.Now - start).Milliseconds));
            Debug.Log($"catalog count: {checkHandle.Result.Count} === check status: {checkHandle.Status}");
            if (checkHandle.Status == AsyncOperationStatus.Succeeded)
            {
                List<string> catalogs = checkHandle.Result;
                if (catalogs != null && catalogs.Count > 0)
                {

                    needUpdateRes = true;

                    statusText.text = "正在更新资源...";

                    start = DateTime.Now;
                    AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                    yield return updateHandle;

                    var locators = updateHandle.Result;
                    Debug.Log($"locator count: {locators.Count}");

                    foreach (var item in locators)
                    {
                        List<object> keys = new List<object>();
                        keys.AddRange(item.Keys);

                        var sizeHandle = Addressables.GetDownloadSizeAsync(item.Keys);
                        yield return sizeHandle;

                        long size = sizeHandle.Result;
                        Debug.Log($"download size:{size}");

                        if (size > 0)
                        {
                            var downloadHandle = Addressables.DownloadDependenciesAsync(item.Keys, Addressables.MergeMode.Union);
                            while (!downloadHandle.IsDone)
                            {
                                float percentage = downloadHandle.PercentComplete;
                                Debug.Log($"download pregress: {percentage}");

                                yield return null;
                            }
                            Addressables.Release(downloadHandle);
                        }
                    }

                    Debug.Log(string.Format("UpdateFinish use {0}ms", (DateTime.Now - start).Milliseconds));
                    yield return UpdateFinish();

                    Addressables.Release(updateHandle);
                }

                Addressables.Release(checkHandle);
            }


            needUpdateRes = false;

            yield return StartGame();

        }

        IEnumerator StartGame()
        {
            statusText.text = "正在准备资源...";

            //更新外后才开始启动全局游戏逻辑
            //GameManager.instance.Init();
            //GameManager.instance.EnterWorld();

            Destroy(gameObject, 0.5f);
            yield break;
        }

        IEnumerator UpdateFinish()
        {
            //slider.normalizedValue = 1f;
            statusText.text = "正在准备资源...";


            // 重启资源管理器
            yield return AddressablesManager.Instance.Cleanup();
            yield return AddressablesManager.Instance.Initialize();

            //reload tables
            /*
            AddressablesManager.Instance.ReleaseTables();
            BaseAssetAsyncLoader tabloader = AddressablesManager.Instance.LoadAssetAsync(AddressableConfig.TableAssetsPathMapFileName, typeof(TextAsset));
            yield return tabloader;

            TextAsset tablemaptext = tabloader.asset as TextAsset;
            string[] tables = tablemaptext.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            AddressablesManager.Instance.ReleaseAsset(tabloader.asset);
            tabloader.Dispose();
            TableAsyncLoader tableLoader = AddressablesManager.Instance.LoadTableAsync(tables);
            yield return tableLoader;
            */

            yield break;
        }

        private void Update()
        {
            if (needUpdateRes)
            {
                totalTime += Time.deltaTime;

                var progress = totalTime % 10;
            }

        }
    }
}
