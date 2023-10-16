using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Fsm
{
    public abstract class FsmState<T> where T : class
    {
        public FsmState() {}

        protected internal virtual void OnInit(IFsm<T> fsm)
        {
        }

        protected internal virtual void OnEnter(IFsm<T> fsm)
        {
        }

        protected internal virtual void OnUpdate(IFsm<T> fsm, float elapseSeconds, float realElapseSeconds)
        {
        }

        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {
        }

        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {
        }

        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            (((Fsm<T>)fsm) ?? throw new ArgumentException("FSM is invalid.")).ChangeState<TState>();
        }

        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> obj = ((Fsm<T>)fsm) ?? throw new ArgumentException("FSM is invalid.");
            if ((object)stateType == null)
            {
                throw new ArgumentException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new ArgumentException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            obj.ChangeState(stateType);
        }
    }

}

