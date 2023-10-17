using System;
using System.Runtime.InteropServices;

namespace GameFramework
{    /// <summary>
     /// ���ó���Ϣ��
     /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ReferencePoolInfo
    {
        private readonly Type m_Type;
        private readonly int m_UnusedReferenceCount;
        private readonly int m_UsingReferenceCount;
        private readonly int m_AcquireReferenceCount;
        private readonly int m_ReleaseReferenceCount;
        private readonly int m_AddReferenceCount;
        private readonly int m_RemoveReferenceCount;

        /// <summary>
        /// ��ʼ�����ó���Ϣ����ʵ����
        /// </summary>
        /// <param name="type">���ó����͡�</param>
        /// <param name="unusedReferenceCount">δʹ������������</param>
        /// <param name="usingReferenceCount">����ʹ������������</param>
        /// <param name="acquireReferenceCount">��ȡ����������</param>
        /// <param name="releaseReferenceCount">�黹����������</param>
        /// <param name="addReferenceCount">��������������</param>
        /// <param name="removeReferenceCount">�Ƴ�����������</param>
        public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            m_Type = type;
            m_UnusedReferenceCount = unusedReferenceCount;
            m_UsingReferenceCount = usingReferenceCount;
            m_AcquireReferenceCount = acquireReferenceCount;
            m_ReleaseReferenceCount = releaseReferenceCount;
            m_AddReferenceCount = addReferenceCount;
            m_RemoveReferenceCount = removeReferenceCount;
        }

        /// <summary>
        /// ��ȡ���ó����͡�
        /// </summary>
        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// ��ȡδʹ������������
        /// </summary>
        public int UnusedReferenceCount
        {
            get
            {
                return m_UnusedReferenceCount;
            }
        }

        /// <summary>
        /// ��ȡ����ʹ������������
        /// </summary>
        public int UsingReferenceCount
        {
            get
            {
                return m_UsingReferenceCount;
            }
        }

        /// <summary>
        /// ��ȡ��ȡ����������
        /// </summary>
        public int AcquireReferenceCount
        {
            get
            {
                return m_AcquireReferenceCount;
            }
        }

        /// <summary>
        /// ��ȡ�黹����������
        /// </summary>
        public int ReleaseReferenceCount
        {
            get
            {
                return m_ReleaseReferenceCount;
            }
        }

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public int AddReferenceCount
        {
            get
            {
                return m_AddReferenceCount;
            }
        }

        /// <summary>
        /// ��ȡ�Ƴ�����������
        /// </summary>
        public int RemoveReferenceCount
        {
            get
            {
                return m_RemoveReferenceCount;
            }
        }
    }

}
