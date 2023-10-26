using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    /// <summary>
    /// ����״̬����������
    /// </summary>
    public interface IFsmManager
    {
        /// <summary>
        /// ��ȡ����״̬��������
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>�Ƿ��������״̬����</returns>
        bool HasFsm<T>() where T : class;

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        bool HasFsm(Type ownerType);

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        bool HasFsm<T>(string name) where T : class;

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        bool HasFsm(Type ownerType, string name);

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        IFsm<T> GetFsm<T>() where T : class;

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        FsmBase GetFsm(Type ownerType);

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        IFsm<T> GetFsm<T>(string name) where T : class;

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        FsmBase GetFsm(Type ownerType, string name);

        /// <summary>
        /// ��ȡ��������״̬����
        /// </summary>
        /// <returns>��������״̬����</returns>
        FsmBase[] GetAllFsms();

        /// <summary>
        /// ��ȡ��������״̬����
        /// </summary>
        /// <param name="results">��������״̬����</param>
        void GetAllFsms(List<FsmBase> results);

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        bool DestroyFsm<T>() where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        bool DestroyFsm(Type ownerType);

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">Ҫ���ٵ�����״̬�����ơ�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        bool DestroyFsm<T>(string name) where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">Ҫ���ٵ�����״̬�����ơ�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        bool DestroyFsm(Type ownerType, string name);

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="fsm">Ҫ���ٵ�����״̬����</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        bool DestroyFsm<T>(IFsm<T> fsm) where T : class;

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="fsm">Ҫ���ٵ�����״̬����</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        bool DestroyFsm(FsmBase fsm);
    }
}
