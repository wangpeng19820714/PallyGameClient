using System;
using System.Runtime.InteropServices;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// ������Ϣ��
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ObjectInfo
    {
        private readonly string m_Name;
        private readonly bool m_Locked;
        private readonly bool m_CustomCanReleaseFlag;
        private readonly int m_Priority;
        private readonly DateTime m_LastUseTime;
        private readonly int m_SpawnCount;

        /// <summary>
        /// ��ʼ��������Ϣ����ʵ����
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <param name="locked">�����Ƿ񱻼�����</param>
        /// <param name="customCanReleaseFlag">�����Զ����ͷż���ǡ�</param>
        /// <param name="priority">��������ȼ���</param>
        /// <param name="lastUseTime">�����ϴ�ʹ��ʱ�䡣</param>
        /// <param name="spawnCount">����Ļ�ȡ������</param>
        public ObjectInfo(string name, bool locked, bool customCanReleaseFlag, int priority, DateTime lastUseTime, int spawnCount)
        {
            m_Name = name;
            m_Locked = locked;
            m_CustomCanReleaseFlag = customCanReleaseFlag;
            m_Priority = priority;
            m_LastUseTime = lastUseTime;
            m_SpawnCount = spawnCount;
        }

        /// <summary>
        /// ��ȡ�������ơ�
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// ��ȡ�����Ƿ񱻼�����
        /// </summary>
        public bool Locked
        {
            get
            {
                return m_Locked;
            }
        }

        /// <summary>
        /// ��ȡ�����Զ����ͷż���ǡ�
        /// </summary>
        public bool CustomCanReleaseFlag
        {
            get
            {
                return m_CustomCanReleaseFlag;
            }
        }

        /// <summary>
        /// ��ȡ��������ȼ���
        /// </summary>
        public int Priority
        {
            get
            {
                return m_Priority;
            }
        }

        /// <summary>
        /// ��ȡ�����ϴ�ʹ��ʱ�䡣
        /// </summary>
        public DateTime LastUseTime
        {
            get
            {
                return m_LastUseTime;
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
    }
}
