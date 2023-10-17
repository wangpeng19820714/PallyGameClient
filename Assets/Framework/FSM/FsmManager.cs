using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    public class FsmManager : MonoSingleton<FsmManager>, IFsmManager
    {

        private readonly Dictionary<TypeNamePair, FsmBase> m_Fsms;
        private readonly List<FsmBase> m_TempFsms;

        /// <summary>
        /// ��ʼ������״̬������������ʵ����
        /// </summary>
        public FsmManager()
        {
            m_Fsms = new Dictionary<TypeNamePair, FsmBase>();
            m_TempFsms = new List<FsmBase>();
        }

        /// <summary>
        /// ��ȡ����״̬��������
        /// </summary>
        public int Count
        {
            get
            {
                return m_Fsms.Count;
            }
        }

        public float ElapseSeconds { get; set; }

        public float RealElapseSeconds { get; set; }

        /// <summary>
        /// ����״̬����������ѯ��
        /// </summary>
        void Update()
        {
            m_TempFsms.Clear();
            if (m_Fsms.Count <= 0)
            {
                return;
            }

            foreach (KeyValuePair<TypeNamePair, FsmBase> fsm in m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            foreach (FsmBase fsm in m_TempFsms)
            {
                if (fsm.IsDestroyed)
                {
                    continue;
                }

                fsm.Update(ElapseSeconds, RealElapseSeconds);
            }
        }

        /// <summary>
        /// �رղ���������״̬����������
        /// </summary>
        public override void Dispose()
        {
            foreach (KeyValuePair<TypeNamePair, FsmBase> fsm in m_Fsms)
            {
                fsm.Value.Shutdown();
            }

            m_Fsms.Clear();
            m_TempFsms.Clear();
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new ArgumentException("Owner type is invalid.");
            }

            return InternalHasFsm(new TypeNamePair(ownerType));
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return InternalHasFsm(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new ArgumentException("Owner type is invalid.");
            }

            return InternalHasFsm(new TypeNamePair(ownerType, name));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return (IFsm<T>)InternalGetFsm(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public FsmBase GetFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new ArgumentException("Owner type is invalid.");
            }

            return InternalGetFsm(new TypeNamePair(ownerType));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return (IFsm<T>)InternalGetFsm(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public FsmBase GetFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new ArgumentException("Owner type is invalid.");
            }

            return InternalGetFsm(new TypeNamePair(ownerType, name));
        }

        /// <summary>
        /// ��ȡ��������״̬����
        /// </summary>
        /// <returns>��������״̬����</returns>
        public FsmBase[] GetAllFsms()
        {
            int index = 0;
            FsmBase[] results = new FsmBase[m_Fsms.Count];
            foreach (KeyValuePair<TypeNamePair, FsmBase> fsm in m_Fsms)
            {
                results[index++] = fsm.Value;
            }

            return results;
        }

        /// <summary>
        /// ��ȡ��������״̬����
        /// </summary>
        /// <param name="results">��������״̬����</param>
        public void GetAllFsms(List<FsmBase> results)
        {
            if (results == null)
            {
                throw new ArgumentException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<TypeNamePair, FsmBase> fsm in m_Fsms)
            {
                results.Add(fsm.Value);
            }
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasFsm<T>(name))
            {
                throw new ArgumentException(Utility.Text.Format("Already exist FSM '{0}'.", typeNamePair));
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(typeNamePair, fsm);
            return fsm;
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        public IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class
        {
            TypeNamePair typeNamePair = new TypeNamePair(typeof(T), name);
            if (HasFsm<T>(name))
            {
                throw new ArgumentException(Utility.Text.Format("Already exist FSM '{0}'.", typeNamePair));
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(typeNamePair, fsm);
            return fsm;
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm<T>() where T : class
        {
            return InternalDestroyFsm(new TypeNamePair(typeof(T)));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new ArgumentException("Owner type is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(ownerType));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">Ҫ���ٵ�����״̬�����ơ�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            return InternalDestroyFsm(new TypeNamePair(typeof(T), name));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">Ҫ���ٵ�����״̬�����ơ�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new ArgumentException("Owner type is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(ownerType, name));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="fsm">Ҫ���ٵ�����״̬����</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            if (fsm == null)
            {
                throw new ArgumentException("FSM is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(typeof(T), fsm.Name));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="fsm">Ҫ���ٵ�����״̬����</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm(FsmBase fsm)
        {
            if (fsm == null)
            {
                throw new ArgumentException("FSM is invalid.");
            }

            return InternalDestroyFsm(new TypeNamePair(fsm.OwnerType, fsm.Name));
        }

        private bool InternalHasFsm(TypeNamePair typeNamePair)
        {
            return m_Fsms.ContainsKey(typeNamePair);
        }

        private FsmBase InternalGetFsm(TypeNamePair typeNamePair)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(typeNamePair, out fsm))
            {
                return fsm;
            }

            return null;
        }

        private bool InternalDestroyFsm(TypeNamePair typeNamePair)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(typeNamePair, out fsm))
            {
                fsm.Shutdown();
                return m_Fsms.Remove(typeNamePair);
            }

            return false;
        }

        public IEnumerator Initialize()
        {
            yield break;
        }
    }
}

