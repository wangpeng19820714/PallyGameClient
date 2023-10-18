using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    public partial class ObjectPoolManager : MonoSingleton<ObjectPoolManager>, IObjectPoolManager
    {
        /// <summary>
        /// ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private readonly MultiDictionary<string, Object<T>> m_Objects;
            private readonly Dictionary<object, Object<T>> m_ObjectMap;
            private readonly ReleaseObjectFilterCallback<T> m_DefaultReleaseObjectFilterCallback;
            private readonly List<T> m_CachedCanReleaseObjects;
            private readonly List<T> m_CachedToReleaseObjects;
            private readonly bool m_AllowMultiSpawn;
            private float m_AutoReleaseInterval;
            private int m_Capacity;
            private float m_ExpireTime;
            private int m_Priority;
            private float m_AutoReleaseTime;

            /// <summary>
            /// ��ʼ������ص���ʵ����
            /// </summary>
            /// <param name="name">��������ơ�</param>
            /// <param name="allowMultiSpawn">�Ƿ�������󱻶�λ�ȡ��</param>
            /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
            /// <param name="capacity">����ص�������</param>
            /// <param name="expireTime">����ض������������</param>
            /// <param name="priority">����ص����ȼ���</param>
            public ObjectPool(string name, bool allowMultiSpawn, float autoReleaseInterval, int capacity, float expireTime, int priority)
                : base(name)
            {
                m_Objects = new MultiDictionary<string, Object<T>>();
                m_ObjectMap = new Dictionary<object, Object<T>>();
                m_DefaultReleaseObjectFilterCallback = DefaultReleaseObjectFilterCallback;
                m_CachedCanReleaseObjects = new List<T>();
                m_CachedToReleaseObjects = new List<T>();
                m_AllowMultiSpawn = allowMultiSpawn;
                m_AutoReleaseInterval = autoReleaseInterval;
                Capacity = capacity;
                ExpireTime = expireTime;
                m_Priority = priority;
                m_AutoReleaseTime = 0f;
            }

            /// <summary>
            /// ��ȡ����ض������͡�
            /// </summary>
            public override Type ObjectType
            {
                get
                {
                    return typeof(T);
                }
            }

            /// <summary>
            /// ��ȡ������ж����������
            /// </summary>
            public override int Count
            {
                get
                {
                    return m_ObjectMap.Count;
                }
            }

            /// <summary>
            /// ��ȡ��������ܱ��ͷŵĶ����������
            /// </summary>
            public override int CanReleaseCount
            {
                get
                {
                    GetCanReleaseObjects(m_CachedCanReleaseObjects);
                    return m_CachedCanReleaseObjects.Count;
                }
            }

            /// <summary>
            /// ��ȡ�Ƿ�������󱻶�λ�ȡ��
            /// </summary>
            public override bool AllowMultiSpawn
            {
                get
                {
                    return m_AllowMultiSpawn;
                }
            }

            /// <summary>
            /// ��ȡ�����ö�����Զ��ͷſ��ͷŶ���ļ��������
            /// </summary>
            public override float AutoReleaseInterval
            {
                get
                {
                    return m_AutoReleaseInterval;
                }
                set
                {
                    m_AutoReleaseInterval = value;
                }
            }

            /// <summary>
            /// ��ȡ�����ö���ص�������
            /// </summary>
            public override int Capacity
            {
                get
                {
                    return m_Capacity;
                }
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Capacity is invalid.");
                    }

                    if (m_Capacity == value)
                    {
                        return;
                    }

                    m_Capacity = value;
                    Release();
                }
            }

            /// <summary>
            /// ��ȡ�����ö���ض������������
            /// </summary>
            public override float ExpireTime
            {
                get
                {
                    return m_ExpireTime;
                }

                set
                {
                    if (value < 0f)
                    {
                        throw new ArgumentException("ExpireTime is invalid.");
                    }

                    if (ExpireTime == value)
                    {
                        return;
                    }

                    m_ExpireTime = value;
                    Release();
                }
            }

            /// <summary>
            /// ��ȡ�����ö���ص����ȼ���
            /// </summary>
            public override int Priority
            {
                get
                {
                    return m_Priority;
                }
                set
                {
                    m_Priority = value;
                }
            }

            /// <summary>
            /// ��������
            /// </summary>
            /// <param name="obj">����</param>
            /// <param name="spawned">�����Ƿ��ѱ���ȡ��</param>
            public void Register(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new ArgumentException("Object is invalid.");
                }

                Object<T> internalObject = Object<T>.Create(obj, spawned);
                m_Objects.Add(obj.Name, internalObject);
                m_ObjectMap.Add(obj.Target, internalObject);

                if (Count > m_Capacity)
                {
                    Release();
                }
            }

            /// <summary>
            /// ������
            /// </summary>
            /// <returns>Ҫ���Ķ����Ƿ���ڡ�</returns>
            public bool CanSpawn()
            {
                return CanSpawn(string.Empty);
            }

            /// <summary>
            /// ������
            /// </summary>
            /// <param name="name">�������ơ�</param>
            /// <returns>Ҫ���Ķ����Ƿ���ڡ�</returns>
            public bool CanSpawn(string name)
            {
                if (name == null)
                {
                    throw new ArgumentException("Name is invalid.");
                }

                LinkedListRange<Object<T>> objectRange = default(LinkedListRange<Object<T>>);
                if (m_Objects.TryGetValue(name, out objectRange))
                {
                    foreach (Object<T> internalObject in objectRange)
                    {
                        if (m_AllowMultiSpawn || !internalObject.IsInUse)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// ��ȡ����
            /// </summary>
            /// <returns>Ҫ��ȡ�Ķ���</returns>
            public T Spawn()
            {
                return Spawn(string.Empty);
            }

            /// <summary>
            /// ��ȡ����
            /// </summary>
            /// <param name="name">�������ơ�</param>
            /// <returns>Ҫ��ȡ�Ķ���</returns>
            public T Spawn(string name)
            {
                if (name == null)
                {
                    throw new ArgumentException("Name is invalid.");
                }

                LinkedListRange<Object<T>> objectRange = default(LinkedListRange<Object<T>>);
                if (m_Objects.TryGetValue(name, out objectRange))
                {
                    foreach (Object<T> internalObject in objectRange)
                    {
                        if (m_AllowMultiSpawn || !internalObject.IsInUse)
                        {
                            return internalObject.Spawn();
                        }
                    }
                }

                return null;
            }

            /// <summary>
            /// ���ն���
            /// </summary>
            /// <param name="obj">Ҫ���յĶ���</param>
            public void Unspawn(T obj)
            {
                if (obj == null)
                {
                    throw new ArgumentException("Object is invalid.");
                }

                Unspawn(obj.Target);
            }

            /// <summary>
            /// ���ն���
            /// </summary>
            /// <param name="target">Ҫ���յĶ���</param>
            public void Unspawn(object target)
            {
                if (target == null)
                {
                    throw new ArgumentException("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.Unspawn();
                    if (Count > m_Capacity && internalObject.SpawnCount <= 0)
                    {
                        Release();
                    }
                }
                else
                {
                    throw new ArgumentException(Utility.Text.Format("Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.", new TypeNamePair(typeof(T), Name), target.GetType().FullName, target));
                }
            }

            /// <summary>
            /// ���ö����Ƿ񱻼�����
            /// </summary>
            /// <param name="obj">Ҫ���ñ������Ķ���</param>
            /// <param name="locked">�Ƿ񱻼�����</param>
            public void SetLocked(T obj, bool locked)
            {
                if (obj == null)
                {
                    throw new ArgumentException("Object is invalid.");
                }

                SetLocked(obj.Target, locked);
            }

            /// <summary>
            /// ���ö����Ƿ񱻼�����
            /// </summary>
            /// <param name="target">Ҫ���ñ������Ķ���</param>
            /// <param name="locked">�Ƿ񱻼�����</param>
            public void SetLocked(object target, bool locked)
            {
                if (target == null)
                {
                    throw new ArgumentException("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.Locked = locked;
                }
                else
                {
                    throw new ArgumentException(Utility.Text.Format("Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.", new TypeNamePair(typeof(T), Name), target.GetType().FullName, target));
                }
            }

            /// <summary>
            /// ���ö�������ȼ���
            /// </summary>
            /// <param name="obj">Ҫ�������ȼ��Ķ���</param>
            /// <param name="priority">���ȼ���</param>
            public void SetPriority(T obj, int priority)
            {
                if (obj == null)
                {
                    throw new ArgumentException("Object is invalid.");
                }

                SetPriority(obj.Target, priority);
            }

            /// <summary>
            /// ���ö�������ȼ���
            /// </summary>
            /// <param name="target">Ҫ�������ȼ��Ķ���</param>
            /// <param name="priority">���ȼ���</param>
            public void SetPriority(object target, int priority)
            {
                if (target == null)
                {
                    throw new ArgumentException("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.Priority = priority;
                }
                else
                {
                    throw new ArgumentException(Utility.Text.Format("Can not find target in object pool '{0}', target type is '{1}', target value is '{2}'.", new TypeNamePair(typeof(T), Name), target.GetType().FullName, target));
                }
            }

            /// <summary>
            /// �ͷŶ���
            /// </summary>
            /// <param name="obj">Ҫ�ͷŵĶ���</param>
            /// <returns>�ͷŶ����Ƿ�ɹ���</returns>
            public bool ReleaseObject(T obj)
            {
                if (obj == null)
                {
                    throw new ArgumentException("Object is invalid.");
                }

                return ReleaseObject(obj.Target);
            }

            /// <summary>
            /// �ͷŶ���
            /// </summary>
            /// <param name="target">Ҫ�ͷŵĶ���</param>
            /// <returns>�ͷŶ����Ƿ�ɹ���</returns>
            public bool ReleaseObject(object target)
            {
                if (target == null)
                {
                    throw new ArgumentException("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject == null)
                {
                    return false;
                }

                if (internalObject.IsInUse || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                {
                    return false;
                }

                m_Objects.Remove(internalObject.Name, internalObject);
                m_ObjectMap.Remove(internalObject.Peek().Target);

                internalObject.Release(false);
                ReferencePool.Release(internalObject);
                return true;
            }

            /// <summary>
            /// �ͷŶ�����еĿ��ͷŶ���
            /// </summary>
            public override void Release()
            {
                Release(Count - m_Capacity, m_DefaultReleaseObjectFilterCallback);
            }

            /// <summary>
            /// �ͷŶ�����еĿ��ͷŶ���
            /// </summary>
            /// <param name="toReleaseCount">�����ͷŶ���������</param>
            public override void Release(int toReleaseCount)
            {
                Release(toReleaseCount, m_DefaultReleaseObjectFilterCallback);
            }

            /// <summary>
            /// �ͷŶ�����еĿ��ͷŶ���
            /// </summary>
            /// <param name="releaseObjectFilterCallback">�ͷŶ���ɸѡ������</param>
            public void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                Release(Count - m_Capacity, releaseObjectFilterCallback);
            }

            /// <summary>
            /// �ͷŶ�����еĿ��ͷŶ���
            /// </summary>
            /// <param name="toReleaseCount">�����ͷŶ���������</param>
            /// <param name="releaseObjectFilterCallback">�ͷŶ���ɸѡ������</param>
            public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                if (releaseObjectFilterCallback == null)
                {
                    throw new ArgumentException("Release object filter callback is invalid.");
                }

                if (toReleaseCount < 0)
                {
                    toReleaseCount = 0;
                }

                DateTime expireTime = DateTime.MinValue;
                if (m_ExpireTime < float.MaxValue)
                {
                    expireTime = DateTime.UtcNow.AddSeconds(-m_ExpireTime);
                }

                m_AutoReleaseTime = 0f;
                GetCanReleaseObjects(m_CachedCanReleaseObjects);
                List<T> toReleaseObjects = releaseObjectFilterCallback(m_CachedCanReleaseObjects, toReleaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count <= 0)
                {
                    return;
                }

                foreach (T toReleaseObject in toReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            /// <summary>
            /// �ͷŶ�����е�����δʹ�ö���
            /// </summary>
            public override void ReleaseAllUnused()
            {
                m_AutoReleaseTime = 0f;
                GetCanReleaseObjects(m_CachedCanReleaseObjects);
                foreach (T toReleaseObject in m_CachedCanReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            /// <summary>
            /// ��ȡ���ж�����Ϣ��
            /// </summary>
            /// <returns>���ж�����Ϣ��</returns>
            public override ObjectInfo[] GetAllObjectInfos()
            {
                List<ObjectInfo> results = new List<ObjectInfo>();
                foreach (KeyValuePair<string, LinkedListRange<Object<T>>> objectRanges in m_Objects)
                {
                    foreach (Object<T> internalObject in objectRanges.Value)
                    {
                        results.Add(new ObjectInfo(internalObject.Name, internalObject.Locked, internalObject.CustomCanReleaseFlag, internalObject.Priority, internalObject.LastUseTime, internalObject.SpawnCount));
                    }
                }

                return results.ToArray();
            }

            internal override void Update(float elapseSeconds, float realElapseSeconds)
            {
                m_AutoReleaseTime += realElapseSeconds;
                if (m_AutoReleaseTime < m_AutoReleaseInterval)
                {
                    return;
                }

                Release();
            }

            internal override void Shutdown()
            {
                foreach (KeyValuePair<object, Object<T>> objectInMap in m_ObjectMap)
                {
                    objectInMap.Value.Release(true);
                    ReferencePool.Release(objectInMap.Value);
                }

                m_Objects.Clear();
                m_ObjectMap.Clear();
                m_CachedCanReleaseObjects.Clear();
                m_CachedToReleaseObjects.Clear();
            }

            private Object<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new ArgumentException("Target is invalid.");
                }

                Object<T> internalObject = null;
                if (m_ObjectMap.TryGetValue(target, out internalObject))
                {
                    return internalObject;
                }

                return null;
            }

            private void GetCanReleaseObjects(List<T> results)
            {
                if (results == null)
                {
                    throw new ArgumentException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<object, Object<T>> objectInMap in m_ObjectMap)
                {
                    Object<T> internalObject = objectInMap.Value;
                    if (internalObject.IsInUse || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                    {
                        continue;
                    }

                    results.Add(internalObject.Peek());
                }
            }

            private List<T> DefaultReleaseObjectFilterCallback(List<T> candidateObjects, int toReleaseCount, DateTime expireTime)
            {
                m_CachedToReleaseObjects.Clear();

                if (expireTime > DateTime.MinValue)
                {
                    for (int i = candidateObjects.Count - 1; i >= 0; i--)
                    {
                        if (candidateObjects[i].LastUseTime <= expireTime)
                        {
                            m_CachedToReleaseObjects.Add(candidateObjects[i]);
                            candidateObjects.RemoveAt(i);
                            continue;
                        }
                    }

                    toReleaseCount -= m_CachedToReleaseObjects.Count;
                }

                for (int i = 0; toReleaseCount > 0 && i < candidateObjects.Count; i++)
                {
                    for (int j = i + 1; j < candidateObjects.Count; j++)
                    {
                        if (candidateObjects[i].Priority > candidateObjects[j].Priority
                            || candidateObjects[i].Priority == candidateObjects[j].Priority && candidateObjects[i].LastUseTime > candidateObjects[j].LastUseTime)
                        {
                            T temp = candidateObjects[i];
                            candidateObjects[i] = candidateObjects[j];
                            candidateObjects[j] = temp;
                        }
                    }

                    m_CachedToReleaseObjects.Add(candidateObjects[i]);
                    toReleaseCount--;
                }

                return m_CachedToReleaseObjects;
            }
        }
    }
}
