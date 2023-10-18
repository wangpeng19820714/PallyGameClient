using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// �ͷŶ���ɸѡ������
    /// </summary>
    /// <typeparam name="T">�������͡�</typeparam>
    /// <param name="candidateObjects">Ҫɸѡ�Ķ��󼯺ϡ�</param>
    /// <param name="toReleaseCount">��Ҫ�ͷŵĶ���������</param>
    /// <param name="expireTime">������ڲο�ʱ�䡣</param>
    /// <returns>��ɸѡ��Ҫ�ͷŵĶ��󼯺ϡ�</returns>
    public delegate List<T> ReleaseObjectFilterCallback<T>(List<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;
}
