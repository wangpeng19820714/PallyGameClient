using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Game.Addressable;
using GameFramework;


namespace Game.Resource
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        //加载单个资源
        class LoadAssetTask
        {
            public Type assetType;
            public string assetName;
            public LoadFinishFunc func;
            public object param;
        }

        public delegate void LoadFinishFunc(object obj, object param);

        private Queue<LoadAssetTask> m_LoadTaskList = new Queue<LoadAssetTask>();

        //加载多个资源
        class LoadAssetsTask
        {
            public Type assetType;
            public string[] assetNames;
            public LoadFinishFunc func;
            public object param;
        }

        private Queue<LoadAssetsTask> m_LoadAssetsTaskList = new Queue<LoadAssetsTask>();

        public void LoadPrefabCallback(string assetName, LoadFinishFunc func, object param = null)
        {
            LoadAssetTask task = new LoadAssetTask();
            task.assetType = typeof(GameObject);
            task.assetName = assetName;
            task.func = func;
            task.param = param;
            m_LoadTaskList.Enqueue(task);
        }

        public GameObject LoadPrefabSync(string assetName)
        {
            GameObject go = LoadAssetSync(assetName);
            return go;
        }

        public void LoadTextAsset(string assetName, LoadFinishFunc func, object param = null)
        {
            LoadAssetTask task = new LoadAssetTask();
            task.assetType = typeof(TextAsset);
            task.assetName = "Tables/" + assetName + ".bytes";
            task.func = func;
            task.param = param;
            m_LoadTaskList.Enqueue(task);
        }

        public TextAsset LoadTextAssetSync(string path, string assetName)
        {
            TextAsset ta = LoadAssetSyncNotInst<TextAsset>(path + assetName + ".bytes");
            return ta;
        }

        public GameObject LoadAssetSync(string assetName)
        {
            GameObject g = null;
#if UNITY_EDITOR
            g = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/Bundle/{0}", assetName));
#else
            g = AddressablesManager.Instance.LoadAssetSync<GameObject>(assetName);
#endif
            if (g == null)
            {
                return null;
            }
            GameObject go = GameObject.Instantiate(g);
            return go;
        }

        public T LoadAssetSyncNotInst<T>(string assetName) where T : UnityEngine.Object
        {
            T go;
#if UNITY_EDITOR
            go = AssetDatabase.LoadAssetAtPath<T>(string.Format("Assets/Bundle/{0}", assetName));
#else
            go = AddressablesManager.Instance.LoadAssetSync<T>(assetName);
#endif
            return go;
        }
    }
}

