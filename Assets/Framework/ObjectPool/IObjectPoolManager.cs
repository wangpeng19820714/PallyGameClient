using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// ����ع�������
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        bool HasObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        bool HasObjectPool(Type objectType);

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        bool HasObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        bool HasObjectPool(Type objectType, string name);

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        bool HasObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        IObjectPool<T> GetObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        ObjectPoolBase GetObjectPool(Type objectType);

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        ObjectPoolBase GetObjectPool(Type objectType, string name);

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition);

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <param name="results">Ҫ��ȡ�Ķ���ء�</param>
        void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results);

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <returns>���ж���ء�</returns>
        ObjectPoolBase[] GetAllObjectPools();

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="results">���ж���ء�</param>
        void GetAllObjectPools(List<ObjectPoolBase> results);

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="sort">�Ƿ���ݶ���ص����ȼ�����</param>
        /// <returns>���ж���ء�</returns>
        ObjectPoolBase[] GetAllObjectPools(bool sort);

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="sort">�Ƿ���ݶ���ص����ȼ�����</param>
        /// <param name="results">���ж���ء�</param>
        void GetAllObjectPools(bool sort, List<ObjectPoolBase> results);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority);

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase;

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority);

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        bool DestroyObjectPool<T>() where T : ObjectBase;

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        bool DestroyObjectPool(Type objectType);

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">Ҫ���ٵĶ�������ơ�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        bool DestroyObjectPool<T>(string name) where T : ObjectBase;

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">Ҫ���ٵĶ�������ơ�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        bool DestroyObjectPool(Type objectType, string name);

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="objectPool">Ҫ���ٵĶ���ء�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase;

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectPool">Ҫ���ٵĶ���ء�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        bool DestroyObjectPool(ObjectPoolBase objectPool);

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        void Release();

        /// <summary>
        /// �ͷŶ�����е�����δʹ�ö���
        /// </summary>
        void ReleaseAllUnused();
    }
}
