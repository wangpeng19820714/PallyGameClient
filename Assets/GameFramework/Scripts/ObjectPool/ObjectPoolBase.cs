using System;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// ����ػ��ࡣ
    /// </summary>
    public abstract class ObjectPoolBase
    {
        private readonly string m_Name;

        /// <summary>
        /// ��ʼ������ػ������ʵ����
        /// </summary>
        public ObjectPoolBase()
            : this(null)
        {
        }

        /// <summary>
        /// ��ʼ������ػ������ʵ����
        /// </summary>
        /// <param name="name">��������ơ�</param>
        public ObjectPoolBase(string name)
        {
            m_Name = name ?? string.Empty;
        }

        /// <summary>
        /// ��ȡ��������ơ�
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// ��ȡ������������ơ�
        /// </summary>
        public string FullName
        {
            get
            {
                return new TypeNamePair(ObjectType, m_Name).ToString();
            }
        }

        /// <summary>
        /// ��ȡ����ض������͡�
        /// </summary>
        public abstract Type ObjectType
        {
            get;
        }

        /// <summary>
        /// ��ȡ������ж����������
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// ��ȡ��������ܱ��ͷŵĶ����������
        /// </summary>
        public abstract int CanReleaseCount
        {
            get;
        }

        /// <summary>
        /// ��ȡ�Ƿ�������󱻶�λ�ȡ��
        /// </summary>
        public abstract bool AllowMultiSpawn
        {
            get;
        }

        /// <summary>
        /// ��ȡ�����ö�����Զ��ͷſ��ͷŶ���ļ��������
        /// </summary>
        public abstract float AutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// ��ȡ�����ö���ص�������
        /// </summary>
        public abstract int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// ��ȡ�����ö���ض������������
        /// </summary>
        public abstract float ExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// ��ȡ�����ö���ص����ȼ���
        /// </summary>
        public abstract int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        /// <param name="toReleaseCount">�����ͷŶ���������</param>
        public abstract void Release(int toReleaseCount);

        /// <summary>
        /// �ͷŶ�����е�����δʹ�ö���
        /// </summary>
        public abstract void ReleaseAllUnused();

        /// <summary>
        /// ��ȡ���ж�����Ϣ��
        /// </summary>
        /// <returns>���ж�����Ϣ��</returns>
        public abstract ObjectInfo[] GetAllObjectInfos();

        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        internal abstract void Shutdown();
    }
}
