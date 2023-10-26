using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    /// <summary>
    /// ����״̬���ӿڡ�
    /// </summary>
    /// <typeparam name="T">����״̬�����������͡�</typeparam>
    public interface IFsm<T> where T : class
    {
        /// <summary>
        /// ��ȡ����״̬�����ơ�
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬���������ơ�
        /// </summary>
        string FullName
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬�������ߡ�
        /// </summary>
        T Owner
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬����״̬��������
        /// </summary>
        int FsmStateCount
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬���Ƿ��������С�
        /// </summary>
        bool IsRunning
        {
            get;
        }

        /// <summary>
        /// ��ȡ����״̬���Ƿ����١�
        /// </summary>
        bool IsDestroyed
        {
            get;
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬��
        /// </summary>
        FsmState<T> CurrentState
        {
            get;
        }

        /// <summary>
        /// ��ȡ��ǰ����״̬��״̬����ʱ�䡣
        /// </summary>
        float CurrentStateTime
        {
            get;
        }

        /// <summary>
        /// ��ʼ����״̬����
        /// </summary>
        /// <typeparam name="TState">Ҫ��ʼ������״̬��״̬���͡�</typeparam>
        void Start<TState>() where TState : FsmState<T>;

        /// <summary>
        /// ��ʼ����״̬����
        /// </summary>
        /// <param name="stateType">Ҫ��ʼ������״̬��״̬���͡�</param>
        void Start(Type stateType);

        /// <summary>
        /// �Ƿ��������״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ��������״̬��״̬���͡�</typeparam>
        /// <returns>�Ƿ��������״̬��״̬��</returns>
        bool HasState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// �Ƿ��������״̬��״̬��
        /// </summary>
        /// <param name="stateType">Ҫ��������״̬��״̬���͡�</param>
        /// <returns>�Ƿ��������״̬��״̬��</returns>
        bool HasState(Type stateType);

        /// <summary>
        /// ��ȡ����״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ��ȡ������״̬��״̬���͡�</typeparam>
        /// <returns>Ҫ��ȡ������״̬��״̬��</returns>
        TState GetState<TState>() where TState : FsmState<T>;

        /// <summary>
        /// ��ȡ����״̬��״̬��
        /// </summary>
        /// <param name="stateType">Ҫ��ȡ������״̬��״̬���͡�</param>
        /// <returns>Ҫ��ȡ������״̬��״̬��</returns>
        FsmState<T> GetState(Type stateType);

        /// <summary>
        /// ��ȡ����״̬��������״̬��
        /// </summary>
        /// <returns>����״̬��������״̬��</returns>
        FsmState<T>[] GetAllStates();

        /// <summary>
        /// ��ȡ����״̬��������״̬��
        /// </summary>
        /// <param name="results">����״̬��������״̬��</param>
        void GetAllStates(List<FsmState<T>> results);

        /// <summary>
        /// �Ƿ��������״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>����״̬�������Ƿ���ڡ�</returns>
        bool HasData(string name);

        /// <summary>
        /// ��ȡ����״̬�����ݡ�
        /// </summary>
        /// <typeparam name="TData">Ҫ��ȡ������״̬�����ݵ����͡�</typeparam>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>Ҫ��ȡ������״̬�����ݡ�</returns>
        TData GetData<TData>(string name) where TData : Variable;

        /// <summary>
        /// ��ȡ����״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>Ҫ��ȡ������״̬�����ݡ�</returns>
        Variable GetData(string name);

        /// <summary>
        /// ��������״̬�����ݡ�
        /// </summary>
        /// <typeparam name="TData">Ҫ���õ�����״̬�����ݵ����͡�</typeparam>
        /// <param name="name">����״̬���������ơ�</param>
        /// <param name="data">Ҫ���õ�����״̬�����ݡ�</param>
        void SetData<TData>(string name, TData data) where TData : Variable;

        /// <summary>
        /// ��������״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <param name="data">Ҫ���õ�����״̬�����ݡ�</param>
        void SetData(string name, Variable data);

        /// <summary>
        /// �Ƴ�����״̬�����ݡ�
        /// </summary>
        /// <param name="name">����״̬���������ơ�</param>
        /// <returns>�Ƿ��Ƴ�����״̬�����ݳɹ���</returns>
        bool RemoveData(string name);
    }
}
