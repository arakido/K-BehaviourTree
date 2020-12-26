using System.Collections.Generic;
using KBehavior.Base;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class OutPort {
        public int index;
        public Vector2 position;
        public NodeEditor node;
    }
    
    public class NodeEditor  {
        public string Name { get; set; }
        public int ID { get; set; }
        public Vector2 Position { get; set; }
        public string Description { get; set; }
        
        private NodeBase nodeInfo;
        private NodeAttribute nodeAttribute;
        
        private List<NodeEditor> parentNodes= new List<NodeEditor>();
        private List<OutPort> childNodes = new List<OutPort>();

        private NodeView nodeView;
        private bool isSelect = false;

        public static NodeEditor Create(int id) {
            return new NodeEditor {ID = id};
        }
        
        public void Init(NodeAttribute node, Vector2 pos = default) {
            nodeAttribute = node;
            nodeInfo = (NodeBase) System.Activator.CreateInstance(node.type);
            nodeView = new NodeView(ID, nodeAttribute);
            nodeView.HandleEventCallBack += HandleEventCallBack;
            nodeView.SetPosition(pos);
            CreateOutPorts();
        }

        private void CreateOutPorts() {
            if ( nodeInfo.maxChildCount != 0 ) childNodes.Add(new OutPort());
        }

        public static Vector2 Round(Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }

        public static Rect Round(Rect r)
        {
            return new Rect(Round(r.position), Round(r.size));
        }
        
        public void DrawUI() {
            NodeHandleEvents();
            nodeView.DrawView(isSelect);
            DrawOutPorts();
        }
        
        private void NodeHandleEvents() {
            CheckSelect();
        }

        public void CheckSelect() {
            if ( HandleEvents.leftMouseDown || HandleEvents.rightMouseDown ) {
                bool select = nodeView.ContainPoint(HandleEvents.position);
                SetSelectType(select);
            }
            if (isSelect && HandleEvents.leftMouseUp  || HandleEvents.ContextClick ) {
                GenericMenu menu = GetNodeMenu_Single();
                if ( menu != null ) {
                    EditorTools.NodeClickAction += () => { menu.ShowAsContext(); };
                    HandleEvents.Use();
                }
            }
            
        }

        //Handles events, Mouse downs, ups etc.
        private void HandleEventCallBack() {

            //Node click
            if ( HandleEvents.leftMouseDown || HandleEvents.rightMouseDown ) {
                NodeEditorManager.SetSelectNode(this, !HandleEvents.currentEvent.control);
            }

            //..
            if ( HandleEvents.mouseDrag ) {
                //nodeView.SetPosition(nodeView.position + e.delta);
            }

            //Mouse up
            if ( HandleEvents.mouseUp ) {
                
            }
            if ( /*e.type == EventType.MouseDrag && */HandleEvents.leftMouse ) {
                GUI.DragWindow();
            }
        }

        public void SetSelectType(bool select) {
            if ( isSelect.Equals(select) ) return;
            isSelect = select;
            HandleEvents.RepaintUI();
        }
        
        public static GenericMenu GetNodeMenu_Single() {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Duplicate (CTRL+D)"), false, () => {  });
            menu.AddItem(new GUIContent("Copy Node"), false, () => {  });
            menu.AddItem(new GUIContent("Delete (DEL)"), false, () => { });

            return menu;
        }

        public void AddParentNode(NodeEditor parent) {
            parentNodes.Add(parent);
        }
        
        public void AddChildNode(NodeEditor child, int index) {
            if( childNodes.Count == nodeInfo.maxChildCount ) childNodes[index].node = child;
            else {
                if ( childNodes.Count == 1 ) childNodes.Add(new OutPort());
                childNodes.Add(new OutPort {node = child});
            }
        }

        public void DrawConnectLine() {
            DrawNewLine();
            for ( int i = 0; i < childNodes.Count; i++ ) {
                /*var sourcePos = new Vector2(( ( nodeView.size.x / ( childNodes.Count + 1 ) ) * ( i + 1 ) ) + rect.xMin, rect.yMax + 6);
                var targetPos = new Vector2(connection.targetNode.rect.center.x, connection.targetNode.rect.y);

                var sourcePortRect = new Rect(0, 0, 12, 12);
                sourcePortRect.center = sourcePos;

                var targetPortRect = new Rect(0, 0, 15, 15);
                targetPortRect.center = targetPos;

                var boundRect = RectUtils.GetBoundRect(sourcePortRect, targetPortRect);
                if ( fullDrawPass || drawCanvas.Overlaps(boundRect) ) {

                    //GUI.Box(sourcePortRect, string.Empty, StyleSheet.nodePortConnected);

                    if ( collapsed || connection.targetNode.isHidden ) {
                        continue;
                    }

                    connection.DrawConnectionGUI(sourcePos, targetPos);

                }*/
            }
            
        }

        private void DrawOutPorts() {
            if(nodeInfo.maxChildCount == 0) return;
            nodeView.DrawOutPortBg();
            if ( selectOutPort != null && HandleEvents.leftMouseUp ) {
                selectOutPort = null;
            }

            for ( int i = 0; i < childNodes.Count; i++ ) {
                DrawOutPort(i);
            }

        }

        private OutPort selectOutPort;

        public void DrawOutPort(int index) {
            float offset = (index + 1) / ( childNodes.Count + 1f );
            Rect rect = nodeView.DrawOutPort(offset, childNodes[index].node != null);
            childNodes[index].position = rect.center;
            if ( rect.Contains(HandleEvents.position) ) {
                if ( HandleEvents.leftMouseDown ) {
                    selectOutPort = childNodes[index];
                }
            }
        }

        private void DrawNewLine() {
            if(selectOutPort == null) return;
            var tangA = default(Vector2);
            var tangB = default(Vector2);
            ParadoxNotion.CurveUtils.ResolveTangents(selectOutPort.position, HandleEvents.position, 1,
                                                     ParadoxNotion.PlanarDirection.Vertical, out tangA, out tangB);
            
            Handles.DrawBezier(selectOutPort.position, HandleEvents.position, selectOutPort.position + tangA,
                               HandleEvents.position + tangB, StyleSheet.GetStatusColor(Stage.Stop).WithAlpha(0.8f),
                               StyleSheet.bezierTexture, 3);
            HandleEvents.RepaintUI();
        }
    }
}