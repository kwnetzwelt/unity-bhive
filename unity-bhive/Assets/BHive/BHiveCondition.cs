using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHive
{
    public abstract class BHiveCondition : BHiveNode
    {
        public int positiveChildId
        {
            get;
            set;
        }
        public int negativeChildId
        {
            get;
            set;
        }

        public override void Initialize(BHiveNodeConfig config)
        {
            base.Initialize(config);
            positiveChildId = config.positiveChild;
            negativeChildId = config.negativeChild;
        }

        /// <summary>
        /// If the condition is met, this node will be executed
        /// </summary>
        /// <returns></returns>
        protected abstract bool Condition();

        public override BHiveNode Tick()
        {
            if (Condition())
            {
                return PositiveChildNode;
            }
            else
            {
                return NegativeChildNode;
            }
        }

        public BHiveNode PositiveChildNode
        {
            get
            {
                if (!Controller.AllNodes.ContainsKey(positiveChildId))
                    return null;
                return Controller.AllNodes[positiveChildId];
            }
        }

        public BHiveNode NegativeChildNode
        {
            get
            {
                if (!Controller.AllNodes.ContainsKey(negativeChildId))
                    return null;
                return Controller.AllNodes[negativeChildId];
            }

        }


    }
}
