using System;

namespace GameFramework.Fsm
{
    public abstract class FsmBase
    {
        private string m_Name;

        /// <summary>
        /// ��ʼ������״̬���������ʵ����
        /// </summary>
        public FsmBase()
        {
            m_Name = string.Empty;
        }

        /// <summary>
        /// ��ȡ����״̬�����ơ�
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            protected set
            {
                m_Name = value ?? string.Empty;
            }
        }

        /// <summary>
        /// ��ȡ����״̬���������ơ�
        /// </summary>
        public string FullName
        {
            get
            {
                return new TypeNamePair(OwnerType, m_Name).ToString();
            }
        }

        /// <summary>
        /// ��ȡ����״̬�����������͡�
        /// </summary>
        public abstract Type OwnerType
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬����״̬��������
        /// </summary>
        public abstract int FsmStateCount
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬���Ƿ��������С�
        /// </summary>
        public abstract bool IsRunning
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬���Ƿ����١�
        /// </summary>
        public abstract bool IsDestroyed
        {
            get;
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬���ơ�
        /// </summary>
        public abstract string CurrentStateName
        {
            get;
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬����ʱ�䡣
        /// </summary>
        public abstract float CurrentStateTime
        {
            get;
        }

        /// <summary>
        /// ����״̬����ѯ��
        /// </summary>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ǰ������ʱ�䣬����Ϊ��λ��</param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// �رղ���������״̬����
        /// </summary>
        internal abstract void Shutdown();
    }
}
