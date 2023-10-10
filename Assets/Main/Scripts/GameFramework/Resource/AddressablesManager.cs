using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System;

namespace GameFramework.Addressable
{
    public class AddressablesManager : MonoSingleton<AddressablesManager>
    {
        const int MAX_ASSETBUNDLE_CREATE_NUM = 5;

        //运行时不被释放的资源，目前就只有loading是不被销毁的
        List<string> DontDestroyList = new List<string>() { "Loading" };

        List<AddressablesAsyncLoader> processingAddressablesAsyncLoader = new List<AddressablesAsyncLoader>();

        List<AssetsAsyncLoader> processingAssetsAsyncLoader = new List<AssetsAsyncLoader>();

        List<SceneAsyncLoader> processingSceneAsyncLoader = new List<SceneAsyncLoader>();

        public void OnEnable() { SpriteAtlasManager.atlasRequested += RequestAtlas; }

        public void OnDisable() { SpriteAtlasManager.atlasRequested -= this.RequestAtlas; }

        private async void RequestAtlas(string tag, Action<SpriteAtlas> callback)
        {
            SpriteAtlas sa = await Addressables.LoadAssetAsync<SpriteAtlas>("Texture/Atlas/" + tag + ".spriteatlas").Task;
            callback(sa);
        }

        public bool IsProsessRunning
        {
            get
            {
                return processingAddressablesAsyncLoader.Count != 0 || processingAssetsAsyncLoader.Count != 0;
            }
        }

        public IEnumerator Initialize()
        {
            yield break;
        }

        #region ============== clear asset and cache
        //cache asset
        List<UnityEngine.Object> assetsCaching = new List<UnityEngine.Object>();
        List<UnityEngine.Object> assetsCachingDontDestroy = new List<UnityEngine.Object>();

        public IEnumerator Cleanup()
        {
            // 等待所有请求完成
            // 要是不等待Unity很多版本都有各种Bug
            yield return new WaitUntil(() =>
            {
                return !IsProsessRunning;
            });
            ClearAssetsCache(true);

            yield break;
        }

        public void ClearAssetsCache(bool isFull = false)
        {
            if (isFull)
            {
                foreach (UnityEngine.Object asset in assetsCachingDontDestroy)
                {
                    Addressables.Release(asset);
                }
                assetsCachingDontDestroy.Clear();
            }


            foreach (UnityEngine.Object asset in assetsCaching)
            {
                Addressables.Release(asset);
            }

            assetsCaching.Clear();
        }

        public void ReleaseAsset(UnityEngine.Object go)
        {
            if (assetsCachingDontDestroy.Contains(go))
            {
                return;
            }
            assetsCaching.Remove(go);
            Addressables.Release(go);
        }
        #endregion

        #region =============== LoadAssetsAsync 批量
        public AssetsAsyncLoader LoadAssetsAsync(string[] addressPaths, Type assetType)
        {
            var loader = AssetsAsyncLoader.Get();
            processingAssetsAsyncLoader.Add(loader);

            loader.Init(addressPaths, assetType);

            return loader;
        }

        void OnProcessAssetsAsyncLoader()
        {
            for (int i = processingAssetsAsyncLoader.Count - 1; i >= 0; i--)
            {
                var loader = processingAssetsAsyncLoader[i];
                loader.Update();
                if (loader.isDone)
                {
                    if (loader.assets != null)
                    {
                        foreach (var asset in loader.assets)
                        {
                            if (DontDestroyList.Contains(asset.Value.name))
                            {
                                if (!assetsCachingDontDestroy.Contains(asset.Value))
                                    assetsCachingDontDestroy.Add(asset.Value);
                            }
                            else
                            {
                                assetsCaching.Add(asset.Value);
                            }
                        }

                    }
                    processingAssetsAsyncLoader.RemoveAt(i);
                }
            }
        }
        #endregion

        #region =============== LoadAssetAsync
        public BaseAssetAsyncLoader LoadAssetAsync(string addressPath, Type assetType)
        {
            var loader = AddressablesAsyncLoader.Get();
            processingAddressablesAsyncLoader.Add(loader);

            loader.Init(addressPath, assetType);

            return loader;
        }

        void OnProcessAddressablesAsyncLoader()
        {
            for (int i = processingAddressablesAsyncLoader.Count - 1; i >= 0; i--)
            {
                var loader = processingAddressablesAsyncLoader[i];
                loader.Update();
                if (loader.isDone)
                {
                    if (loader.asset != null)
                    {
                        if (DontDestroyList.Contains(loader.asset.name))
                        {
                            if (!assetsCachingDontDestroy.Contains(loader.asset))
                                assetsCachingDontDestroy.Add(loader.asset);
                        }
                        else
                        {
                            assetsCaching.Add(loader.asset);
                        }
                    }
                    processingAddressablesAsyncLoader.RemoveAt(i);
                }
            }
        }
        #endregion

        #region =============== LoadAssetSync
        public T LoadAssetSync<T>(string addressPath)
        {
            var loader = Addressables.LoadAssetAsync<T>(addressPath);
            var go = loader.WaitForCompletion();
            UnityEngine.Object ugo = go as UnityEngine.Object;
            if (ugo != null)
            {
                if (DontDestroyList.Contains(ugo.name))
                {
                    if (!assetsCachingDontDestroy.Contains(ugo))
                        assetsCachingDontDestroy.Add(ugo);
                }
                else
                {
                    assetsCaching.Add(ugo);
                }
            }
            return go;
        }
        #endregion

        #region ============== scene loader

        public BaseAssetAsyncLoader LoadSceneAsync(string addressPath, int lsm = 0)
        {
            var loader = SceneAsyncLoader.Get();
            processingSceneAsyncLoader.Add(loader);

            loader.Init(addressPath, (LoadSceneMode)lsm);

            return loader;
        }

        void OnProcessSceneAsyncLoader()
        {
            for (int i = processingSceneAsyncLoader.Count - 1; i >= 0; i--)
            {
                var loader = processingSceneAsyncLoader[i];
                loader.Update();
                if (loader.isDone)
                {
                    if (loader.asset != null)
                    {
                        if (DontDestroyList.Contains(loader.asset.name))
                        {
                            if (!assetsCachingDontDestroy.Contains(loader.asset))
                                assetsCachingDontDestroy.Add(loader.asset);
                        }
                        else
                        {
                            assetsCaching.Add(loader.asset);
                        }
                    }

                    processingSceneAsyncLoader.RemoveAt(i);
                }
            }
        }
        #endregion

        void Update()
        {
            OnProcessAddressablesAsyncLoader();
            OnProcessAssetsAsyncLoader();
            OnProcessSceneAsyncLoader();
        }
    }
}
