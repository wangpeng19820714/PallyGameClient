using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Fsm
{
    /// <summary>
    /// ����״̬��״̬���ࡣ
    /// </summary>
    /// <typeparam name="T">����״̬�����������͡�</typeparam>
    public abstract class FsmState<T> where T : class
    {
        /// <summary>
        /// ��ʼ������״̬��״̬�������ʵ����
        /// </summary>
        public FsmState()
        {
        }

        /// <summary>
        /// ����״̬��״̬��ʼ��ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        protected internal virtual void OnInit(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// ����״̬��״̬����ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        protected internal virtual void OnEnter(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// ����״̬��״̬��ѯʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
        protected internal virtual void OnUpdate(IFsm<T> fsm, float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// ����״̬��״̬�뿪ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        /// <param name="isShutdown">�Ƿ��ǹر�����״̬��ʱ������</param>
        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {
        }

        /// <summary>
        /// ����״̬��״̬����ʱ���á�
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ�л���������״̬��״̬���͡�</typeparam>
        /// <param name="fsm">����״̬�����á�</param>
        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new ArgumentException("FSM is invalid.");
            }

            fsmImplement.ChangeState<TState>();
        }

        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        /// <param name="stateType">Ҫ�л���������״̬��״̬���͡�</param>
        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new ArgumentException("FSM is invalid.");
            }

            if (stateType == null)
            {
                throw new ArgumentException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new ArgumentException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            fsmImplement.ChangeState(stateType);
        }
    }

}

