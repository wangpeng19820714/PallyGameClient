using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// ����ع�������
    /// </summary>
    public partial class ObjectPoolManager : MonoSingleton<ObjectPoolManager>, IObjectPoolManager
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const int DefaultPriority = 0;

        private readonly Dictionary<TypeNamePair, ObjectPoolBase> m_ObjectPools;
        private readonly List<ObjectPoolBase> m_CachedAllObjectPools;
        private readonly Comparison<ObjectPoolBase> m_ObjectPoolComparer;

        /// <summary>
        /// ��ʼ������ع���������ʵ����
        /// </summary>
        public ObjectPoolManager()
        {
            m_ObjectPools = new Dictionary<TypeNamePair, ObjectPoolBase>();
            m_CachedAllObjectPools = new List<ObjectPoolBase>();
            m_ObjectPoolComparer = ObjectPoolComparer;
        }

        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        public int Count
        {
            get
            {
                return m_ObjectPools.Count;
            }
        }

        /// <summary>
        /// ����ع�������ѯ��
        /// </summary>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
         void ManagerUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                objectPool.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// �رղ��������ع�������
        /// </summary>
        public override void Dispose()
        {
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                objectPool.Value.Shutdown();
            }

            m_ObjectPools.Clear();
            m_CachedAllObjectPools.Clear();
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return InternalHasObjectPool(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalHasObjectPool(new TypeNamePair(objectType));
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalHasObjectPool(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalHasObjectPool(new TypeNamePair(objectType, name));
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new ArgumentException("Condition is invalid.");
            }

            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                if (condition(objectPool.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return (IObjectPool<T>)InternalGetObjectPool(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalGetObjectPool(new TypeNamePair(objectType));
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return (IObjectPool<T>)InternalGetObjectPool(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalGetObjectPool(new TypeNamePair(objectType, name));
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new ArgumentException("Condition is invalid.");
            }

            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                if (condition(objectPool.Value))
                {
                    return objectPool.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            if (condition == null)
            {
                throw new ArgumentException("Condition is invalid.");
            }

            List<ObjectPoolBase> results = new List<ObjectPoolBase>();
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                if (condition(objectPool.Value))
                {
                    results.Add(objectPool.Value);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <param name="results">Ҫ��ȡ�Ķ���ء�</param>
        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            if (condition == null)
            {
                throw new ArgumentException("Condition is invalid.");
            }

            if (results == null)
            {
                throw new ArgumentException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                if (condition(objectPool.Value))
                {
                    results.Add(objectPool.Value);
                }
            }
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <returns>���ж���ء�</returns>
        public ObjectPoolBase[] GetAllObjectPools()
        {
            return GetAllObjectPools(false);
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="results">���ж���ء�</param>
        public void GetAllObjectPools(List<ObjectPoolBase> results)
        {
            GetAllObjectPools(false, results);
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="sort">�Ƿ���ݶ���ص����ȼ�����</param>
        /// <returns>���ж���ء�</returns>
        public ObjectPoolBase[] GetAllObjectPools(bool sort)
        {
            if (sort)
            {
                List<ObjectPoolBase> results = new List<ObjectPoolBase>();
                foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
                {
                    results.Add(objectPool.Value);
                }

                results.Sort(m_ObjectPoolComparer);
                return results.ToArray();
            }
            else
            {
                int index = 0;
                ObjectPoolBase[] results = new ObjectPoolBase[m_ObjectPools.Count];
                foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
                {
                    results[index++] = objectPool.Value;
                }

                return results;
            }
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="sort">�Ƿ���ݶ���ص����ȼ�����</param>
        /// <param name="results">���ж���ء�</param>
        public void GetAllObjectPools(bool sort, List<ObjectPoolBase> results)
        {
            if (results == null)
            {
                throw new ArgumentException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<TypeNamePair, ObjectPoolBase> objectPool in m_ObjectPools)
            {
                results.Add(objectPool.Value);
            }

            if (sort)
            {
                results.Sort(m_ObjectPoolComparer);
            }
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name)
        {
            return InternalCreateObjectPool(objectType, name, false, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity)
        {
            return InternalCreateObjectPool(objectType, name, false, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, false, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, false, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, false, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, false, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name)
        {
            return InternalCreateObjectPool(objectType, name, true, DefaultExpireTime, DefaultCapacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity)
        {
            return InternalCreateObjectPool(objectType, name, true, DefaultExpireTime, capacity, DefaultExpireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, DefaultCapacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, capacity, expireTime, DefaultPriority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, DefaultExpireTime, capacity, DefaultExpireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, DefaultCapacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(string.Empty, true, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, string.Empty, true, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, expireTime, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name, true, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            return InternalCreateObjectPool(objectType, name, true, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return InternalDestroyObjectPool(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectType));
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">Ҫ���ٵĶ�������ơ�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalDestroyObjectPool(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">Ҫ���ٵĶ�������ơ�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool(Type objectType, string name)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectType, name));
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="objectPool">Ҫ���ٵĶ���ء�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            if (objectPool == null)
            {
                throw new ArgumentException("Object pool is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(typeof(T), objectPool.Name));
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectPool">Ҫ���ٵĶ���ء�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool(ObjectPoolBase objectPool)
        {
            if (objectPool == null)
            {
                throw new ArgumentException("Object pool is invalid.");
            }

            return InternalDestroyObjectPool(new TypeNamePair(objectPool.ObjectType, objectPool.Name));
        }

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        public void Release()
        {
            GetAllObjectPools(true, m_CachedAllObjectPools);
            foreach (ObjectPoolBase objectPool in m_CachedAllObjectPools)
            {
                objectPool.Release();
            }
        }

        /// <summary>
        /// �ͷŶ�����е�����δʹ�ö���
        /// </summary>
        public void ReleaseAllUnused()
        {
            GetAllObjectPools(true, m_CachedAllObjectPools);
            foreach (ObjectPoolBase objectPool in m_CachedAllObjectPools)
            {
                objectPool.ReleaseAllUnused();
            }
        }

        private bool InternalHasObjectPool(TypeNamePair typeNamePair)
        {

            return m_ObjectPools.ContainsKey(typeNamePair);
        }

        private ObjectPoolBase InternalGetObjectPool(TypeNamePair typeNamePair)
        {
            ObjectPoolBase objectPool = null;
            if (m_ObjectPools.TryGetValue(typeNamePair, out objectPool))
            {
                return objectPool;
            }

            return null;
        }

        private IObjectPool<T> InternalCreateObjectPool<T>(string name, bool allowMultiSpawn, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasObjectPool<T>(name))
            {
                throw new ArgumentException(Utility.Text.Format("Already exist object pool '{0}'.", typeNamePair));
            }

            ObjectPool<T> objectPool = new ObjectPool<T>(name, allowMultiSpawn, autoReleaseInterval, capacity, expireTime, priority);
            m_ObjectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        private ObjectPoolBase InternalCreateObjectPool(Type objectType, string name, bool allowMultiSpawn, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            if (objectType == null)
            {
                throw new ArgumentException("Object type is invalid.");
            }

            if (!typeof(ObjectBase).IsAssignableFrom(objectType))
            {
                throw new ArgumentException(Utility.Text.Format("Object type '{0}' is invalid.", objectType.FullName));
            }

            TypeNamePair typeNamePair = new TypeNamePair(objectType, name);
            if (HasObjectPool(objectType, name))
            {
                throw new ArgumentException(Utility.Text.Format("Already exist object pool '{0}'.", typeNamePair));
            }

            Type objectPoolType = typeof(ObjectPool<>).MakeGenericType(objectType);
            ObjectPoolBase objectPool = (ObjectPoolBase)Activator.CreateInstance(objectPoolType, name, allowMultiSpawn, autoReleaseInterval, capacity, expireTime, priority);
            m_ObjectPools.Add(typeNamePair, objectPool);
            return objectPool;
        }

        private bool InternalDestroyObjectPool(TypeNamePair typeNamePair)
        {
            ObjectPoolBase objectPool = null;
            if (m_ObjectPools.TryGetValue(typeNamePair, out objectPool))
            {
                objectPool.Shutdown();
                return m_ObjectPools.Remove(typeNamePair);
            }

            return false;
        }

        private static int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }

        public IEnumerator Initialize()
        {
            yield break;
        }
    }
}
