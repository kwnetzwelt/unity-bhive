using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace BHive.Editor
{
	public class BHiveWindow : UnityEditor.EditorWindow
	{
        public Vector2 actionMousePosition;

        Vector2 dragOffset;

		const int kWindowHeight = 80;
		const int kWindowWidth = 160;

		[SerializeField]
		BHiveRuntimeController currentController;



		
        Texture2D inTexture;
        Texture2D outTexture;
        Color green = new Color32(13, 148, 19, 255);
		Color red = new Color32(148,13,13,255);
        BHiveNodeConfig _focusedNode;
        BHiveNodeConfig focusedNode
        {
            get
            {
                return _focusedNode;

            }
            set
            {

                if (value != _focusedNode)
                {
                    _focusedNode = value;
                    UpdateFocusedNode();
                }
            }
        }

        private bool isSeekingNegativeChild;
        private bool isSeekingChild;
        private Vector2[] scrollViews = new Vector2[1];

		[MenuItem("Window/BHive")]
		static void ShowEditor() {
			EditorWindow.GetWindow<BHiveWindow>("BHive");

		}
		
		void OnEnable()
		{
            if (currentController != null)
                FocusDefaultNode();

            //inTexture = Resources.Load<Texture2D>("bhive/in");
            //outTexture = Resources.Load<Texture2D>("bhive/out");
		}
		void OnDisable()
		{
            
		}

        void Initialize()
        {
            FocusDefaultNode();
        }
        void UpdateFocusedNode()
        {
            if (focusedNode == null)
                return;
            var attribute = System.Attribute.GetCustomAttribute(focusedNode.InferType(), typeof(BHiveParametersAttribute)) as BHiveParametersAttribute;
            // make sure all parameters are in the configuration
            List<BHiveDataContainer.DataEntry> newConfig = new List<BHiveDataContainer.DataEntry>();
            if (attribute != null)
            { 
                List<string> validKeys = new List<string>(attribute.Configuration);
                foreach(var kv in focusedNode.Configuration)
                {
                    if(validKeys.Contains(kv.Key) )
                    {
                        newConfig.Add(kv);
                    }
                }

                foreach(var key in validKeys)
                {
                    if(newConfig.Find(x => x.Key == key) == null)
                    {
                        newConfig.Add(new BHiveDataContainer.DataEntry() { Key = key, Value = "" });
                    }
                }
            }

            focusedNode.Configuration = newConfig;
        }

        void FocusDefaultNode()
        {
            if (currentController == null)
                return;
            BHiveNodeConfig defaultNode = currentController.configuration.Nodes.Find( x => x.isDefault);
            if(defaultNode == null && currentController.configuration.Nodes.Count > 0)
                defaultNode = currentController.configuration.Nodes[0];

            if (defaultNode == null)
                return;

            dragOffset = -defaultNode.Position + new Vector2( position.size.x/2, 25);
        }

		void OnGUI() {

			if (currentController == null) {
				var go = Selection.activeGameObject;
				if(go != null)
				{
					currentController = go.GetComponent<BHiveRuntimeController>();
                    Initialize();
				}else
				{
					return;
				}
			}

            DrawBackground();

            if (currentController == null)
                return;

			DrawCurves();

            BeginWindows();
                

			DrawWindows();


			EndWindows();
            DrawMenu();

			DrawContext();
            DragView();

            DrawInspector();

            if(Event.current.keyCode == KeyCode.Escape)
            {
                isSeekingChild = false;
            }
            if(Event.current.keyCode == KeyCode.F)
            {
                FocusDefaultNode();
                Repaint();
            }
            if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
            {
                focusedNode = null;
                Repaint();
            }
            if (Event.current.type == EventType.mouseUp)
            {
                Save();
                Repaint();
            }
            if (isSeekingChild || Application.isPlaying)
                Repaint();
		}

		Vector2 GetNodePosition(Rect r)
		{
			Vector2 v = new Vector2(r.x + kWindowWidth / 2,r.y) ;
            v = v - dragOffset;
            return v;
		}

		Rect GetWindowRect(Vector2 position)
		{
			Rect r = new Rect(position.x - kWindowWidth / 2 , position.y, kWindowWidth, kWindowHeight);
            r.position = r.position + dragOffset;
            return r;
		}
        Rect GetInRect(Vector2 position)
        {
            Rect r = new Rect(position.x - 8 , position.y - 16 , 16, 16);
            r.position = r.position + dragOffset;
            return r;
        }
        Rect GetOutRect(Vector2 position)
        {
            Rect r = new Rect(position.x - 8, position.y + kWindowHeight, 16, 16);
            r.position = r.position + dragOffset;
            return r;
        }

        void DrawControllerConfiguration()
        {

            GUILayout.Label("Controller");
            if (Application.isPlaying)
            {
                foreach (var kv in currentController.configController.Configuration)
                {
                    GUILayout.Label(kv.Key);
                    GUILayout.TextField(kv.Value);
                }
            }
        }
        void DrawNodeConfiguration()
        {
            GUILayout.Label(focusedNode.InferType().Name + " " + focusedNode.Id, "LargeLabel");

            GUILayout.Label("Type: " + (focusedNode.isCondition ? "Condition" : "Action"));

            GUILayout.Label("Description: ", "MiniLabel");
            focusedNode.CustomDescription = GUILayout.TextArea(focusedNode.CustomDescription);
            GUILayout.Space(15);

            foreach (var kv in focusedNode.Configuration)
            {
                GUILayout.Label(kv.Key + ":","MiniLabel");
                kv.Value = GUILayout.TextField(kv.Value);
                GUILayout.Space(8);
            }
        }
        void DrawInspector()
        {


            GUILayout.BeginArea(new Rect(0, 18, 250, position.size.y - 18), "", "TE NodeBackground");

            scrollViews[0] = GUILayout.BeginScrollView(scrollViews[0], GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            GUILayout.BeginVertical();

            if (focusedNode == null)
            {
                DrawControllerConfiguration();
            }
            else
            {
                DrawNodeConfiguration();
            }
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            if (GUI.changed)
                EditorUtility.SetDirty(currentController);
        }

		void DrawWindows()
		{ 
            
            
			for(int i = 0;i< currentController.configuration.Nodes.Count;i++)
			{

				var n = currentController.configuration.Nodes[i];

                string name = n.InferType().Name;
                if (n.IsBroken)
                    name = "Broken";
                var r = GUI.Window(i, GetWindowRect(n.Position), DrawNodeWindow, name, GetNodeStyle(n,i));
                
				var p = GetNodePosition(r);
                if(p != n.Position)
                {
                    //Debug.Log(p);
                    n.Position = p;
                }
			}
		}

        private string GetNodeStyle(BHiveNodeConfig n, int index)
        {
            string focused = "";
            if(focusedNode != null)
                focused = n.Id == focusedNode.Id ? " on" : "";

            int color = 2;
            if (n.isCondition)
                color = 5;

            if(Application.isPlaying)
            {
                if(currentController.configController.nodeStack.Contains(
                    currentController.configController.AllNodes[n.Id]))
                    color = 3;
            }
            if (n.IsBroken)
                color = 6;
            return "flow node " + color + focused;
            
        }

        void OpenContextMenu(BHiveNodeConfig nodeConfig)
        {

            GenericMenu gm = new GenericMenu();

            if(nodeConfig.isCondition)
            {
                gm.AddItem(new GUIContent("Add Positive"), false, AddPositiveChildNode, nodeConfig);
                gm.AddItem(new GUIContent("Add Negative"), false, AddNegativeChildNode, nodeConfig);
                gm.AddItem(new GUIContent("Set Default"), false, SetDefaultNode, nodeConfig);
            }

            gm.AddItem(new GUIContent("Clear Parent"), false, ClearParent, nodeConfig);



            gm.AddSeparator("");
            gm.AddItem(new GUIContent("Delete Node"), false, DeleteNode, nodeConfig);
            gm.ShowAsContext();
        }

		void DrawNodeWindow(int id) {
			var nodeConfig = currentController.configuration.Nodes[id];
            if (Event.current.isMouse && Event.current.button == 1)
            {
                focusedNode = nodeConfig; 
                OpenContextMenu(nodeConfig);
                
                Event.current.Use();
            } 
            else if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
            {
                if (isSeekingChild)
                {
                    AddChildNode(nodeConfig, isSeekingNegativeChild);
                }
                focusedNode = nodeConfig;
            }

            
            GUI.DragWindow();
            
            
            GUILayout.BeginArea(new Rect(0, 20, kWindowWidth, kWindowHeight-20));
            if (nodeConfig.IsBroken)
                GUILayout.Label(nodeConfig.SerializedTypeName);
            GUILayout.Label(nodeConfig.CustomDescription, "WordWrappedMiniLabel");
            GUILayout.EndArea();
            if (nodeConfig.isDefault)
                GUI.Box(new Rect(kWindowWidth - 16, 5, 12, 12), "", "U2D.dragDot");

            if(nodeConfig.isCondition)
            {
                GUI.color = green;
                GUI.Box(new Rect(0, kWindowHeight-12, 30, 12), "", "flow node hex 0");
                GUI.color = red;
                GUI.Box(new Rect(kWindowWidth-30, kWindowHeight - 12, 30, 12), "", "flow node hex 0");
                GUI.color = Color.white;
            }

		}
        /*
        private void PromoteNode(object userData)
        {
            var nodeConfig = userData as BHiveNodeConfig;
            foreach (var n in currentController.configuration.Nodes)
            {
                if (n.Children.Contains(nodeConfig.Id))
                {
                    n.SortChild(nodeConfig, -1);
                    break;
                }
            }
        }

        private void DegradeNode(object userData)
        {
            var nodeConfig = userData as BHiveNodeConfig;
            foreach (var n in currentController.configuration.Nodes)
            {
                if(n.Children.Contains(nodeConfig.Id))
                {
                    n.SortChild(nodeConfig, +1);
                    break;
                }
            }
        }
        */

        private void DeleteNode(object userData)
        {
            var nodeConfig = userData as BHiveNodeConfig;
            foreach(var n in currentController.configuration.Nodes)
            {
                if (n.isCondition)
                {
                    if (n.negativeChild == nodeConfig.Id)
                        n.negativeChild = 0;
                    if (n.positiveChild == nodeConfig.Id)
                        n.positiveChild = 0;
                }

            }
            currentController.configuration.Nodes.Remove(nodeConfig);
            focusedNode = null;
        }

		void DrawCurves()
		{
			foreach (var n in currentController.configuration.Nodes) {
                for(int i = 0;i<n.Children.Length;i++)
                {
                    if (n.Children[i] == 0)
                        continue;

                    var node = currentController.configuration.GetNodeById(n.Children[i]);

                    Color color = Color.yellow;
                    if(n.isCondition)
                    {
                        if (node.Id == n.positiveChild)
                            color = green;
                        else
                            color = red;
                    }
                    if(isSeekingChild && n == focusedNode)
                        DrawNodeCurveToChild(n.Position, node.Position, Color.white, i, 3);
                    else
                        DrawNodeCurveToChild(n.Position, node.Position, color, i, 2);
                }
			}
            if(isSeekingChild)
            {
                DrawNodeCurveToChild(focusedNode.Position, Event.current.mousePosition - dragOffset, Color.magenta, 1, 2);
            }
		}

        void DrawNodeCurveToChild(Vector3 start, Vector3 end, Color color, int idx, int count)
        {
            if (count > 1)
                count--;
            start += Vector3.up * (kWindowHeight);
            start += new Vector3(dragOffset.x, dragOffset.y, 0);
            float width = (kWindowWidth - kWindowWidth * 0.18f);
            start += Vector3.right * (idx / (float)(count)) * width - Vector3.right * width / 2.0f;
            end += new Vector3(dragOffset.x,dragOffset.y,0);
			Vector3 startTan = start + Vector3.up * 50;
			Vector3 endTan = end + Vector3.down * 50;
			Color shadowCol = new Color(0, 0, 0, .6f);
			//for (int i = 0; i < 3; i++) // Draw a shadow
			Handles.DrawBezier(start, end, startTan, endTan, shadowCol, null,  8);

			Handles.DrawBezier(start, end, startTan, endTan, color,null, 6);
		}

        void DrawBackground()
        {
            GUI.Box(new Rect(0, 0, position.size.x, position.size.y), "", "AnimationCurveEditorBackground");
        }

		void DrawMenu()
		{
			GUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
			GUILayout.Label(currentController.name);

			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();
		}
        public void AddChildNode(BHiveNodeConfig node, bool isNegative)
        {
            
            
            if(focusedNode.isCondition)
            {
                
                if (isNegative)
                {
                    focusedNode.negativeChild = node.Id;
                }else
                {
                    focusedNode.positiveChild = node.Id;
                }
            }

            isSeekingChild = false;
        }

        public void AddPositiveChildNode(object node)
        {
            isSeekingNegativeChild = false;
            isSeekingChild = true;
        }

        public void AddNegativeChildNode(object node)
        {
            isSeekingNegativeChild = true;
            isSeekingChild = true;
        }
        

        public void ClearParent(object node)
        {
            var nodeConfig = node as BHiveNodeConfig;
            foreach(var n in currentController.configuration.Nodes)
            {
                if (n.positiveChild == nodeConfig.Id)
                    n.positiveChild = 0;
                if (n.negativeChild == nodeConfig.Id)
                    n.negativeChild = 0;
            }
        }


        public void SetDefaultNode(object node)
        {
            var nodeConfig = node as BHiveNodeConfig;
            foreach(var n in currentController.configuration.Nodes)
            {
                n.isDefault = n == nodeConfig;
            }
            Save();
        }

		public void AddNodeType (object userData)
		{
			var myData = userData as BHiveNode.BHiveNodeType;

            var conf = new BHiveNodeConfig();
            conf.Id = currentController.configuration.GetNextNodeId();
            conf.isDefault = currentController.configuration.Nodes.Count == 0;
            conf.Position = actionMousePosition - dragOffset;
            conf.SetType( myData.type );
            conf.isCondition = (typeof(BHiveCondition).IsAssignableFrom(myData.type));
            conf.CustomDescription = conf.GetDescription();
            currentController.configuration.Nodes.Add(conf);
            Save();
            
		}

        void Save()
        {
            EditorUtility.SetDirty(currentController.configuration);
            AssetDatabase.SaveAssets();
        }

		void DrawContext()
		{
			Event e = Event.current;
			if (e.type == EventType.ContextClick) {

                actionMousePosition = e.mousePosition;

				GenericMenu gm = new GenericMenu();

				foreach(var t in BHiveNode.GetAllTypes(typeof(BHiveAction)))
				{
					gm.AddItem(new GUIContent("Action/" + t.title), false, AddNodeType, t);
				}

                foreach (var t in BHiveNode.GetAllTypes(typeof(BHiveCondition)))
                {
                    gm.AddItem(new GUIContent("Condition/" + t.title), false, AddNodeType, t);
                }

				gm.ShowAsContext();
				e.Use();
			}
		}

        void DragView()
        {
            Event e = Event.current;
            if(e.type == EventType.mouseDrag && e.button == 2)
            {
                dragOffset += e.delta;
                e.Use();
                Repaint();
                
            }
        }

        void Cleanup()
        {
            int i = 0;
            foreach(var n in currentController.configuration.Nodes)
            {

                n.Position = Vector2.zero + i * new Vector2(kWindowWidth,kWindowHeight);
                i++;
            }
            dragOffset = Vector2.zero;
        }

    }
}

