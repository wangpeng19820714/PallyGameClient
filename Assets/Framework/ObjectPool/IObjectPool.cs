using System;
using System.Security.AccessControl;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// ����ؽӿڡ�
    /// </summary>
    /// <typeparam name="T">�������͡�</typeparam>
    public interface IObjectPool<T> where T : ObjectBase
    {
        /// <summary>
        /// ��ȡ��������ơ�
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// ��ȡ������������ơ�
        /// </summary>
        string FullName
        {
            get;
        }

        /// <summary>
        /// ��ȡ����ض������͡�
        /// </summary>
        Type ObjectType
        {
            get;
        }

        /// <summary>
        /// ��ȡ������ж����������
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// ��ȡ��������ܱ��ͷŵĶ����������
        /// </summary>
        int CanReleaseCount
        {
            get;
        }

        /// <summary>
        /// ��ȡ�Ƿ�������󱻶�λ�ȡ��
        /// </summary>
        bool AllowMultiSpawn
        {
            get;
        }

        /// <summary>
        /// ��ȡ�����ö�����Զ��ͷſ��ͷŶ���ļ��������
        /// </summary>
        float AutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// ��ȡ�����ö���ص�������
        /// </summary>
        int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// ��ȡ�����ö���ض������������
        /// </summary>
        float ExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// ��ȡ�����ö���ص����ȼ���
        /// </summary>
        int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="spawned">�����Ƿ��ѱ���ȡ��</param>
        void Register(T obj, bool spawned);

        /// <summary>
        /// ������
        /// </summary>
        /// <returns>Ҫ���Ķ����Ƿ���ڡ�</returns>
        bool CanSpawn();

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <returns>Ҫ���Ķ����Ƿ���ڡ�</returns>
        bool CanSpawn(string name);

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns>Ҫ��ȡ�Ķ���</returns>
        T Spawn();

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���</returns>
        T Spawn(string name);

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="obj">Ҫ���յĶ���</param>
        void Unspawn(T obj);

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="target">Ҫ���յĶ���</param>
        void Unspawn(object target);

        /// <summary>
        /// ���ö����Ƿ񱻼�����
        /// </summary>
        /// <param name="obj">Ҫ���ñ������Ķ���</param>
        /// <param name="locked">�Ƿ񱻼�����</param>
        void SetLocked(T obj, bool locked);

        /// <summary>
        /// ���ö����Ƿ񱻼�����
        /// </summary>
        /// <param name="target">Ҫ���ñ������Ķ���</param>
        /// <param name="locked">�Ƿ񱻼�����</param>
        void SetLocked(object target, bool locked);

        /// <summary>
        /// ���ö�������ȼ���
        /// </summary>
        /// <param name="obj">Ҫ�������ȼ��Ķ���</param>
        /// <param name="priority">���ȼ���</param>
        void SetPriority(T obj, int priority);

        /// <summary>
        /// ���ö�������ȼ���
        /// </summary>
        /// <param name="target">Ҫ�������ȼ��Ķ���</param>
        /// <param name="priority">���ȼ���</param>
        void SetPriority(object target, int priority);

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="obj">Ҫ�ͷŵĶ���</param>
        /// <returns>�ͷŶ����Ƿ�ɹ���</returns>
        bool ReleaseObject(T obj);

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="target">Ҫ�ͷŵĶ���</param>
        /// <returns>�ͷŶ����Ƿ�ɹ���</returns>
        bool ReleaseObject(object target);

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        void Release();

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        /// <param name="toReleaseCount">�����ͷŶ���������</param>
        void Release(int toReleaseCount);

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        /// <param name="releaseObjectFilterCallback">�ͷŶ���ɸѡ������</param>
        void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        /// <param name="toReleaseCount">�����ͷŶ���������</param>
        /// <param name="releaseObjectFilterCallback">�ͷŶ���ɸѡ������</param>
        void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback);

        /// <summary>
        /// �ͷŶ�����е�����δʹ�ö���
        /// </summary>
        void ReleaseAllUnused();
    }
}
