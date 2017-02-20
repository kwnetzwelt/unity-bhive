using System;
using System.Collections.Generic;

namespace BHive
{
    public abstract class BHiveAction : BHiveNode
    {
        public event System.Action<BHiveAction> BecomeActive;
        public event System.Action<BHiveAction> WillBeReset;

        IEnumerator<BHiveState> mCurrentState;
        public BHiveState CurrentState
        {
            get
            {
                return mCurrentState == null ? BHiveState.Idle : mCurrentState.Current;
            }
        }


        protected int TicksRunning = 0;


        public void Start()
        {
            if (BecomeActive != null)
                BecomeActive(this);

            OnBecomeActive();
            mCurrentState = Update(); 
            TicksRunning++;
        }

        public override BHiveNode Tick()
        {
            if (TicksRunning == 0)
            {
                Start();
                return this;
            }

            mCurrentState.MoveNext();
            if(mCurrentState.Current == BHiveState.Running)
            {
                TicksRunning++;
                return this;
            }else
            {
                Reset();
                return null;
            }



            
        }

        public void Reset()
        {
            if (WillBeReset != null)
                WillBeReset(this);
            OnReset();
            TicksRunning = 0;
            mCurrentState = null;
            
            
        }


        protected abstract void OnBecomeActive();
        protected abstract void OnReset();

        protected abstract IEnumerator<BHiveState> Update();
    }
}
