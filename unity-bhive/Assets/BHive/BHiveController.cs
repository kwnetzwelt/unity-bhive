using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BHive
{
	public class BHiveController : BHiveDataContainer
	{


		public BHiveController()
		{
		}

        public BHiveNode CurrentNode
        {
            get;
            set;
        }

		public BHiveNode DefaultNode
		{
			get;
			set;
		}


        public Dictionary<string,string> Configuration
        {
            get
            {
                return configuration;
            }
        }

        public Stack<BHiveNode> nodeStack = new Stack<BHiveNode>();


		public void Tick()
		{
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (CurrentNode != null)
            {

                var node = CurrentNode.Tick();
                CurrentNode = node;
                
            }else
            {
                CurrentNode = DefaultNode.Tick();
                nodeStack.Clear();
            }

            if(nodeStack.Count < 1 || nodeStack.Peek() != CurrentNode)
                nodeStack.Push(CurrentNode);
		}

		public Dictionary<int, BHiveNode> AllNodes = new Dictionary<int, BHiveNode>();

        
		public void Start(BHiveConfiguration pConfiguration)
        {
            AllNodes = new Dictionary<int, BHiveNode>();
            LoadConfiguration(pConfiguration.Configuration);
            System.Type ActionType = typeof(BHiveAction);
			// creates runtime instances of nodes and initializes them with their configuration
            foreach (var n in pConfiguration.Nodes)
			{
				var type = n.InferType();
				BHiveNode nodeInstance = (BHiveNode) Activator.CreateInstance(type);
				nodeInstance.Controller = this;
                
                AllNodes [n.Id] = nodeInstance;
                if (n.isDefault)
                    DefaultNode = nodeInstance;

                nodeInstance.Initialize(n);
			}
           
			// Do consistency checks
			foreach (var kv in AllNodes) {
				if(kv.Value.Parent != -1)
					Assert( AllNodes.ContainsKey( kv.Value.Parent ), "Parent node not found. ");
			}
		}


		void Assert(bool thatTrue, string error)
		{
            if(!thatTrue)
			    throw new Exception("Consistency Check failed: " + error);
		}
        
	}
}

