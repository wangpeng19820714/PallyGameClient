using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    /// <summary>
    /// ����״̬����
    /// </summary>
    /// <typeparam name="T">����״̬�����������͡�</typeparam>
    internal sealed class Fsm<T> : FsmBase, IReference, IFsm<T> where T : class
    {
        private T m_Owner;
        private readonly Dictionary<Type, FsmState<T>> m_States;
        private Dictionary<string, Variable> m_Datas;
        private FsmState<T> m_CurrentState;
        private float m_CurrentStateTime;
        private bool m_IsDestroyed;

        /// <summary>
        /// ��ʼ������״̬������ʵ����
        /// </summary>
        public Fsm()
        {
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            m_Datas = null;
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        /// <summary>
        /// ��ȡ����״̬�������ߡ�
        /// </summary>
        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }

        /// <summary>
        /// ��ȡ����״̬�����������͡�
        /// </summary>
        public override Type OwnerType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// ��ȡ����״̬����״̬��������
        /// </summary>
        public override int FsmStateCount
        {
            get
            {
                return m_States.Count;
            }
        }

        /// <summary>
        /// ��ȡ����״̬���Ƿ��������С�
        /// </summary>
        public override bool IsRunning
        {
            get
            {
                return m_CurrentState != null;
            }
        }

        /// <summary>
        /// ��ȡ����״̬���Ƿ����١�
        /// </summary>
        public override bool IsDestroyed
        {
            get
            {
                return m_IsDestroyed;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬��
        /// </summary>
        public FsmState<T> CurrentState
        {
            get
            {
                return m_CurrentState;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬���ơ�
        /// </summary>
        public override string CurrentStateName
        {
            get
            {
                return m_CurrentState != null ? m_CurrentState.GetType().FullName : null;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬����ʱ�䡣
        /// </summary>
        public override float CurrentStateTime
        {
            get
            {
                return m_CurrentStateTime;
            }
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>����������״̬����</returns>
        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                throw new ArgumentException("FSM owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                throw new ArgumentException("FSM states is invalid.");
            }

            Fsm<T> fsm = ReferencePool.Acquire<Fsm<T>>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new ArgumentException("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new ArgumentException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name), stateType.FullName));
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>����������״̬����</returns>
        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new ArgumentException("FSM owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new ArgumentException("FSM states is invalid.");
            }

            Fsm<T> fsm = ReferencePool.Acquire<Fsm<T>>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new ArgumentException("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new ArgumentException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name), stateType.FullName));
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        public void Clear()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, true);
            }

            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                state.Value.OnDestroy(this);
            }

            Name = null;
            m_Owner = null;
            m_States.Clear();

            if (m_Datas != null)
            {
                foreach (KeyValuePair<string, Variable> data in m_Datas)
                {
                    if (data.Value == null)
                    {
                        continue;
                    }

                    ReferencePool.Release(data.Value);
                }

                m_Datas.Clear();
            }

            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        /// <summary>
        /// ��ʼ����״̬����
        /// </summary>
        /// <typeparam name="TState">Ҫ��ʼ������״̬��״̬���͡�</typeparam>
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new ArgumentException("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new ArgumentException(Utility.Text.Format("FSM '{0}' can not start state '{1}' which is not exist.", new TypeNamePair(typeof(T), Name), typeof(TState).FullName));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// ��ʼ����״̬����
        /// </summary>
        /// <param name="stateType">Ҫ��ʼ������״̬��״̬���͡�</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new ArgumentException("FSM is running, can not start again.");
            }

            if (stateType == null)
            {
                throw new ArgumentException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new ArgumentException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new ArgumentException(Utility.Text.Format("FSM '{0}' can not start state '{1}' which is not exist.", new TypeNamePair(typeof(T), Name), stateType.FullName));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// �Ƿ��������״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ��������״̬��״̬���͡�</typeparam>
        /// <returns>�Ƿ��������״̬��״̬��</returns>
        public bool HasState<TState>() where TState : FsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }

        /// <summary>
        /// �Ƿ��������״̬��״̬��
        /// </summary>
        /// <param name="stateType">Ҫ��������״̬��״̬���͡�</param>
        /// <returns>�Ƿ��������״̬��״̬��</returns>
        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new ArgumentException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new ArgumentException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            return m_States.ContainsKey(stateType);
        }

        /// <summary>
        /// ��ȡ����״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ��ȡ������״̬��״̬���͡�</typeparam>
        /// <returns>Ҫ��ȡ������״̬��״̬��</returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }

            return null;
        }

        /// <summary>
        /// ��ȡ����״̬��״̬��
        /// </summary>
        /// <param name="stateType">Ҫ��ȡ������״̬��״̬���͡�</param>
        /// <returns>Ҫ��ȡ������״̬��״̬��</returns>
        public FsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new ArgumentException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new ArgumentException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        /// <summary>
        /// ��ȡ����״̬��������״̬��
        /// </summary>
        /// <returns>����״̬��������״̬��</returns>
        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] results = new FsmState<T>[m_States.Count];
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        /// <summary>
        /// ��ȡ����״̬��������״̬��
        /// </summary>
        /// <param name="results">����״̬��������״̬��</param>
        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new ArgumentException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                results.Add(state.Value);
            }
        }

        /// <summary>
        /// �Ƿ��������״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>����״̬�������Ƿ���ڡ�</returns>
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return false;
            }

            return m_Datas.ContainsKey(name);
        }

        /// <summary>
        /// ��ȡ����״̬�����ݡ�
        /// </summary>
        /// <typeparam name="TData">Ҫ��ȡ������״̬�����ݵ����͡�</typeparam>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>Ҫ��ȡ������״̬�����ݡ�</returns>
        public TData GetData<TData>(string name) where TData : Variable
        {
            return (TData)GetData(name);
        }

        /// <summary>
        /// ��ȡ����״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>Ҫ��ȡ������״̬�����ݡ�</returns>
        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return null;
            }

            Variable data = null;
            if (m_Datas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }

        /// <summary>
        /// ��������״̬�����ݡ�
        /// </summary>
        /// <typeparam name="TData">Ҫ���õ�����״̬�����ݵ����͡�</typeparam>
        /// <param name="name">����״̬���������ơ�</param>
        /// <param name="data">Ҫ���õ�����״̬�����ݡ�</param>
        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            SetData(name, (Variable)data);
        }

        /// <summary>
        /// ��������״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <param name="data">Ҫ���õ�����״̬�����ݡ�</param>
        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                m_Datas = new Dictionary<string, Variable>(StringComparer.Ordinal);
            }

            Variable oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            m_Datas[name] = data;
        }

        /// <summary>
        /// �Ƴ�����״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>�Ƿ��Ƴ�����״̬�����ݳɹ���</returns>
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return false;
            }

            Variable oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            return m_Datas.Remove(name);
        }

        /// <summary>
        /// ����״̬����ѯ��
        /// </summary>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += elapseSeconds;
            m_CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// �رղ���������״̬����
        /// </summary>
        internal override void Shutdown()
        {
            ReferencePool.Release(this);
        }

        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ�л���������״̬��״̬���͡�</typeparam>
        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <param name="stateType">Ҫ�л���������״̬��״̬���͡�</param>
        internal void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new ArgumentException("Current state is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new ArgumentException(Utility.Text.Format("FSM '{0}' can not change state to '{1}' which is not exist.", new TypeNamePair(typeof(T), Name), stateType.FullName));
            }

            m_CurrentState.OnLeave(this, false);
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
    }
}
