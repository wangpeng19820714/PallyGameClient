using System;

namespace GameFramework.ObjectPool
{
    public partial class ObjectPoolManager : MonoSingleton<ObjectPoolManager>, IObjectPoolManager
    {
        /// <summary>
        /// �ڲ�����
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        private sealed class Object<T> : IReference where T : ObjectBase
        {
            private T m_Object;
            private int m_SpawnCount;

            /// <summary>
            /// ��ʼ���ڲ��������ʵ����
            /// </summary>
            public Object()
            {
                m_Object = null;
                m_SpawnCount = 0;
            }

            /// <summary>
            /// ��ȡ�������ơ�
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Object.Name;
                }
            }

            /// <summary>
            /// ��ȡ�����Ƿ񱻼�����
            /// </summary>
            public bool Locked
            {
                get
                {
                    return m_Object.Locked;
                }
                internal set
                {
                    m_Object.Locked = value;
                }
            }

            /// <summary>
            /// ��ȡ��������ȼ���
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_Object.Priority;
                }
                internal set
                {
                    m_Object.Priority = value;
                }
            }

            /// <summary>
            /// ��ȡ�Զ����ͷż���ǡ�
            /// </summary>
            public bool CustomCanReleaseFlag
            {
                get
                {
                    return m_Object.CustomCanReleaseFlag;
                }
            }

            /// <summary>
            /// ��ȡ�����ϴ�ʹ��ʱ�䡣
            /// </summary>
            public DateTime LastUseTime
            {
                get
                {
                    return m_Object.LastUseTime;
                }
            }

            /// <summary>
            /// ��ȡ�����Ƿ�����ʹ�á�
            /// </summary>
            public bool IsInUse
            {
                get
                {
                    return m_SpawnCount > 0;
                }
            }

            /// <summary>
            /// ��ȡ����Ļ�ȡ������
            /// </summary>
            public int SpawnCount
            {
                get
                {
                    return m_SpawnCount;
                }
            }

            /// <summary>
            /// �����ڲ�����
            /// </summary>
            /// <param name="obj">����</param>
            /// <param name="spawned">�����Ƿ��ѱ���ȡ��</param>
            /// <returns>�������ڲ�����</returns>
            public static Object<T> Create(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new ArgumentException("Object is invalid.");
                }

                Object<T> internalObject = ReferencePool.Acquire<Object<T>>();
                internalObject.m_Object = obj;
                internalObject.m_SpawnCount = spawned ? 1 : 0;
                if (spawned)
                {
                    obj.OnSpawn();
                }

                return internalObject;
            }

            /// <summary>
            /// �����ڲ�����
            /// </summary>
            public void Clear()
            {
                m_Object = null;
                m_SpawnCount = 0;
            }

            /// <summary>
            /// �鿴����
            /// </summary>
            /// <returns>����</returns>
            public T Peek()
            {
                return m_Object;
            }

            /// <summary>
            /// ��ȡ����
            /// </summary>
            /// <returns>����</returns>
            public T Spawn()
            {
                m_SpawnCount++;
                m_Object.LastUseTime = DateTime.UtcNow;
                m_Object.OnSpawn();
                return m_Object;
            }

            /// <summary>
            /// ���ն���
            /// </summary>
            public void Unspawn()
            {
                m_Object.OnUnspawn();
                m_Object.LastUseTime = DateTime.UtcNow;
                m_SpawnCount--;
                if (m_SpawnCount < 0)
                {
                    throw new ArgumentException(Utility.Text.Format("Object '{0}' spawn count is less than 0.", Name));
                }
            }

            /// <summary>
            /// �ͷŶ���
            /// </summary>
            /// <param name="isShutdown">�Ƿ��ǹرն����ʱ������</param>
            public void Release(bool isShutdown)
            {
                m_Object.Release(isShutdown);
                ReferencePool.Release(m_Object);
            }
        }
    }
}
