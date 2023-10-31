using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Resource;

namespace GameFramework.Data
{
    public class DataManager : MonoSingleton<DataManager>
    {
        private Dictionary<string, ScriptableObject> m_DataTables;

        /// <summary>
        /// 初始化数据表管理器。
        /// </summary>
        protected override void Init()
        {
            if (m_DataTables == null)
            {
                m_DataTables = new Dictionary<string, ScriptableObject>();
            }
        }

        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_DataTables.Count;
            }
        }

        public bool HasDataTable(string name)
        {
            bool result = m_DataTables.ContainsKey(name);
            return result;
        }

        public T GetDataTable<T>(string name, bool needCache = true) where T : ScriptableObject
        {
            T data = null;
            if (HasDataTable(name))
            {
                data = m_DataTables[name] as T;
            }
            else
            {
                data = ResourceManager.Instance.LoadDataAssetSync<T>(name);
                if (data == null)
                {
                    Log.Error("Data table [{0}] does not exist", name);
                    return null;
                }
                
                if (needCache)
                    m_DataTables.Add(name, data);
            }
            return data;
        }

        public ScriptableObject[] GetAllDataTables()
        {
            int index = 0;
            ScriptableObject[] results = new ScriptableObject[m_DataTables.Count];
            foreach(KeyValuePair<string, ScriptableObject> dataTable in m_DataTables)
            {
                results[index++] = dataTable.Value;
            }
            return results;
        }

        public bool DestroyDataTable(string name)
        {
            if (HasDataTable(name))
            {
                m_DataTables.Remove(name);
                return true;
            }

            return false;
        }
    }
}

