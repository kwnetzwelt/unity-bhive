using System;
namespace BHive
{
	public enum BHiveState
	{
        /// <summary>
        /// Returned by the node if it is not currently executed.  
        /// </summary>
        Idle,
        /// <summary>
        /// Returned by the node if its execution is still pending (running) and an outcome is not yet determined. 
        /// </summary>
		Running,
        /// <summary>
        /// Returned by the Node if its executed is done. That means it is no longer in charge and the tree is re-examined. 
        /// OnReset will be called on the node soon. 
        /// </summary>
        Done
	}
}

