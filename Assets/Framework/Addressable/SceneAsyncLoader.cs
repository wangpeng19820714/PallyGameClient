using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using GameFramework.Resource;
using GameFramework;

namespace GameFramework.Addressable
{
    public class SceneAsyncLoader : BaseAssetAsyncLoader
    {
        static Queue<SceneAsyncLoader> pool = new Queue<SceneAsyncLoader>();
        static int sequence = 0;
        protected bool isOver = false;
        public SceneInstance scene;

        AsyncOperationHandle<SceneInstance> handle;

        public static SceneAsyncLoader Get()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else
            {
                return new SceneAsyncLoader(++sequence);
            }
        }

        public static void Recycle(SceneAsyncLoader creater)
        {
            pool.Enqueue(creater);
        }

        public SceneAsyncLoader(int seqence)
        {
            Sequence = seqence;
        }

        public void Init(string addressPath, LoadSceneMode lsm = LoadSceneMode.Single)
        {
            this.asset = null;
            isOver = false;
            AddressPath = addressPath;

            handle = Addressables.LoadSceneAsync(addressPath + ".unity", lsm);

        }

        public int Sequence
        {
            get;
            protected set;
        }
        public string AddressPath
        {
            get;
            protected set;
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
                return 0.0f;
            }
        }

        public override void Update()
        {
            if (isDone) return;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                this.scene = handle.Result;
                isOver = true;
            }
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Log.Error($"Load asset:{AddressPath} error: {handle.Status}");
                isOver = true;
            }
        }

        public override void Dispose()
        {
            Recycle(this);
        }
    }
}
