using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    internal sealed class Fsm<T> : FsmBase, IFsm<T> where T : class
    {
        private T m_Owner;

        private readonly Dictionary<Type, FsmState<T>> m_States;

        private Dictionary<string, Variable> m_Datas;

        private FsmState<T> m_CurrentState;

        private float m_CurrentStateTime;

        private bool m_IsDestroyed;

        public T Owner => m_Owner;

        public override Type OwnerType => typeof(T);

        public override int FsmStateCount => m_States.Count;

        public override bool IsRunning => m_CurrentState != null;

        public override bool IsDestroyed => m_IsDestroyed;

        public FsmState<T> CurrentState => m_CurrentState;

        public override string CurrentStateName
        {
            get
            {
                if (m_CurrentState == null)
                {
                    return null;
                }

                return m_CurrentState.GetType().FullName;
            }
        }

        public override float CurrentStateTime => m_CurrentStateTime;

        public Fsm()
        {
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            m_Datas = null;
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

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
            foreach (FsmState<T> fsmState in states)
            {
                if (fsmState == null)
                {
                    throw new ArgumentException("FSM states is invalid.");
                }

                Type type = fsmState.GetType();
                if (fsm.m_States.ContainsKey(type))
                {
                    throw new ArgumentException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name).ToString(), type));
                }

                fsm.m_States.Add(type, fsmState);
                fsmState.OnInit(fsm);
            }

            return fsm;
        }

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

                Type type = state.GetType();
                if (fsm.m_States.ContainsKey(type))
                {
                    throw new ArgumentException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name).ToString(), type));
                }

                fsm.m_States.Add(type, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        public void Clear()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, isShutdown: true);
            }

            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                state.Value.OnDestroy(this);
            }

            base.Name = null;
            m_Owner = null;
            m_States.Clear();
            if (m_Datas != null)
            {
                foreach (KeyValuePair<string, Variable> data in m_Datas)
                {
                    if (data.Value != null)
                    {
                        ReferencePool.Release(data.Value);
                    }
                }

                m_Datas.Clear();
            }

            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new ArgumentException("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new ArgumentException(Utility.Text.Format("FSM '{0}' can not start state '{1}' which is not exist.", new TypeNamePair(typeof(T), base.Name).ToString(), typeof(TState).FullName));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new ArgumentException("FSM is running, can not start again.");
            }

            if ((object)stateType == null)
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
                throw new ArgumentException(Utility.Text.Format("FSM '{0}' can not start state '{1}' which is not exist.", new TypeNamePair(typeof(T), base.Name).ToString(), stateType.FullName));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public bool HasState<TState>() where TState : FsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }

        public bool HasState(Type stateType)
        {
            if ((object)stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            return m_States.ContainsKey(stateType);
        }

        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> value = null;
            if (m_States.TryGetValue(typeof(TState), out value))
            {
                return (TState)value;
            }

            return null;
        }

        public FsmState<T> GetState(Type stateType)
        {
            if ((object)stateType == null)
            {
                throw new ArgumentException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new ArgumentException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> value = null;
            if (m_States.TryGetValue(stateType, out value))
            {
                return value;
            }

            return null;
        }

        //
        // 摘要:
        //     获取有限状态机的所有状态。
        //
        // 返回结果:
        //     有限状态机的所有状态。
        public FsmState<T>[] GetAllStates()
        {
            int num = 0;
            FsmState<T>[] array = new FsmState<T>[m_States.Count];
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                array[num++] = state.Value;
            }

            return array;
        }

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

        public TData GetData<TData>(string name) where TData : Variable
        {
            return (TData)GetData(name);
        }

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

            Variable value = null;
            if (m_Datas.TryGetValue(name, out value))
            {
                return value;
            }

            return null;
        }

        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            SetData(name, (Variable)data);
        }

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

            Variable data2 = GetData(name);
            if (data2 != null)
            {
                ReferencePool.Release(data2);
            }

            m_Datas[name] = data;
        }

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

            Variable data = GetData(name);
            if (data != null)
            {
                ReferencePool.Release(data);
            }

            return m_Datas.Remove(name);
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_CurrentState != null)
            {
                m_CurrentStateTime += elapseSeconds;
                m_CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
            }
        }

        internal override void Shutdown()
        {
            ReferencePool.Release(this);
        }

        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        internal void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new ArgumentException("Current state is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new ArgumentException(Utility.Text.Format("FSM '{0}' can not change state to '{1}' which is not exist.", new TypeNamePair(typeof(T), base.Name).ToString(), stateType.FullName));
            }

            m_CurrentState.OnLeave(this, isShutdown: false);
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
    }
}
