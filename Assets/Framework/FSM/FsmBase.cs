using System;

namespace GameFramework.Fsm
{
    public abstract class FsmBase
    {
        private string m_Name;

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

        public string FullName => new TypeNamePair(OwnerType, m_Name).ToString();

        public abstract Type OwnerType { get; }

        public abstract int FsmStateCount { get; }

        public abstract bool IsRunning { get; }

        public abstract bool IsDestroyed { get; }

        public abstract string CurrentStateName { get; }

        public abstract float CurrentStateTime { get; }

        public FsmBase()
        {
            m_Name = string.Empty;
        }

        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        internal abstract void Shutdown();
    }
}
