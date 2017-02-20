using System;
using UnityEngine;
namespace BHive
{
	public class BHiveRuntimeController : MonoBehaviour
	{
        public int tickRate = 1;
        int lastTick = 0;
		public BHiveController configController
		{
			get;
			set;
		}
		public BHiveConfiguration configuration;

        void OnEnable()
        {
            configController = new BHiveController();
            
            configController.Start(configuration);
            foreach (var n in configController.AllNodes)
            {
                n.Value.Target = this.gameObject;
            }
        }

        void Update()
        {
            lastTick++;
            if (lastTick >= tickRate)
            {
                configController.Tick();
                lastTick = 0;
            }
        }
	}
}