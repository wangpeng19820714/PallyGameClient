using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game.Resource;

namespace Game.Addressable
{
    public class AssetsAsyncLoader : BaseAssetAsyncLoader
    {
        static Queue<AssetsAsyncLoader> pool = new Queue<AssetsAsyncLoader>();
        static int sequence = 0;
        protected bool isOver = false;
        private Dictionary<string, AsyncOperationHandle> handles = new Dictionary<string, AsyncOperationHandle>();
        public Dictionary<string, UnityEngine.Object> assets = new Dictionary<string, UnityEngine.Object>();

        public int Sequence
        {
            get;
            protected set;
        }
        public Type AssetType
        {
            get;
            protected set;
        }

        public string[] AddressPath
        {
            get;
            protected set;
        }

        public static AssetsAsyncLoader Get()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else
            {
                return new AssetsAsyncLoader(++sequence);
            }
        }

        public static void Recycle(AssetsAsyncLoader creater)
        {
            pool.Enqueue(creater);
        }

        public AssetsAsyncLoader(int seqence)
        {
            Sequence = seqence;
        }

        public void Init(string[] assetsPaths, Type assetType)
        {
            assets.Clear();
            isOver = false;
            AssetType = assetType;
            AddressPath = assetsPaths;
            foreach (string assetPath in assetsPaths)
            {
                if (AssetType == typeof(Sprite))
                {
                    handles.Add(assetPath, Addressables.LoadAssetAsync<Sprite>(assetPath));
                }
                else if (AssetType == typeof(TextAsset))
                {
                    handles.Add(assetPath, Addressables.LoadAssetAsync<TextAsset>(assetPath));
                }
                else if (AssetType == typeof(Animation))
                {
                    handles.Add(assetPath, Addressables.LoadAssetAsync<AnimationClip>(assetPath));
                }
                else if (AssetType == typeof(AudioClip))
                {
                    handles.Add(assetPath, Addressables.LoadAssetAsync<AudioClip>(assetPath));
                }
                else if (AssetType == typeof(RenderTexture))
                {
                    handles.Add(assetPath, Addressables.LoadAssetAsync<RenderTexture>(assetPath));
                }
                else
                {
                    handles.Add(assetPath, Addressables.LoadAssetAsync<GameObject>(assetPath));
                }
            }
        }

        public override bool IsDone()
        {
            return isOver;
        }

        public override float Progress()
        {
            if (isDone)
            {
                return 1.0f;
            }
            else
            {
                return assets.Count / (handles.Count + assets.Count);
            }
        }

        public override void Update()
        {
            if (isDone) return;

            if (handles.Count > 0)
            {
                var rmKeys = new List<string>();
                foreach (KeyValuePair<string, AsyncOperationHandle> kv in handles)
                {
                    if (kv.Value.IsValid() && kv.Value.Status == AsyncOperationStatus.Succeeded)
                    {
                        assets.Add(kv.Key, kv.Value.Result as UnityEngine.Object);
                        rmKeys.Add(kv.Key);
                    }
                }
                foreach (var key in rmKeys)
                {
                    handles.Remove(key);
                }
            }
            else
            {
                isOver = true;
            }
        }

        public override void Dispose()
        {
            if (handles.Count > 0)
            {
                return;
            }

            foreach (KeyValuePair<string, UnityEngine.Object> kv in assets)
            {
                Addressables.Release(kv.Value);
            }
            assets.Clear();
        }
    }
}
