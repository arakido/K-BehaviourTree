using System.Collections.Generic;
using KBehavior.Base;
using KBehavior.Design;
using ParadoxNotion;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class OutPort {
        public int index;
        public Vector2 position;
        public NodeEditor parent;
        public NodeEditor child;

        public OutPort(NodeEditor node) {
            parent = node;
        }
    }
    
    public class NodeEditor  {
        public string Name { get; set; }
        public int ID { get; set; }

        public Vector2 position {
            get => nodeView.viewRect.position;
            set => nodeView.viewRect.position = value;
        }

        public string Description { get; set; }
        
        private NodeBase nodeInfo;
        private NodeAttribute nodeAttribute;
        
        private List<NodeEditor> parentNodes= new List<NodeEditor>();
        private List<OutPort> childNodes = new List<OutPort>();
        private OutPort leftOutPort;
        private OutPort rightOutPort;

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
            if ( nodeInfo.maxChildCount != 0 ) {
                leftOutPort = new OutPort(this);
                rightOutPort = new OutPort(this);
            }
        }

        private OutPort CreateOutPort() {
            OutPort outPort = new OutPort(this);
            childNodes.Add(outPort);
            return outPort;
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
            nodeView.DrawView(isSelect);
            DrawOutPorts();
        }
        
        private void HandleEventCallBack() {
            if ( isSelect ) {
                if(HandleEvents.doubleClick ) {
                    GraphDrawEditor.AddSelectNode(this, !HandleEvents.control);
                }
                else if ( HandleEvents.leftMouseDown && HandleEvents.control){
                    GraphDrawEditor.RemoveSelectNode(this);
                }
            }
            else if ( HandleEvents.leftMouseDown ) {
                GraphDrawEditor.AddSelectNode(this, !HandleEvents.control);
                //HandleEvents.Use();
            }
            else if(HandleEvents.rightMouseDown){
                GraphDrawEditor.AddSelectNode(this, !HandleEvents.control);
                HandleEvents.Use();
            }
            else if ( HandleEvents.rightMouseUp && isSelect ) {
                GenericMenu menu = GetNodeMenu();
                menu?.ShowAsContext();
                HandleEvents.Use();
            }
            
            if ( HandleEvents.leftMouse) {
                if ( HandleEvents.mouseDrag ) {
                    GraphDrawEditor.MoveSelectNode(HandleEvents.currentEvent.delta);
                }
                GUI.DragWindow();
            }
            else if ( HandleEvents.leftMouseUp ) {
                SortNode();
            }
        }
        
        private GenericMenu GetNodeMenu() {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Duplicate (CTRL+D)"), false, () => {  });
            menu.AddItem(new GUIContent("Copy Node"), false, () => {  });
            menu.AddItem(new GUIContent("Delete (DEL)"), false, () => { });

            return menu;
        }

        private void SortNode() {
            for ( int i = 0; i < parentNodes.Count; i++ ) {
                if ( GraphDrawEditor.selectNodes.Contains(this) ) {
                    parentNodes[i].SortChildNode();
                }
            }
        }

        public void SortChildNode() {
            childNodes.Sort((a, b) => a.child.position.x.CompareTo(b.child.position.x));
        }

        

        public void SetSelectType(bool select) {
            if ( isSelect.Equals(select) ) return;
            isSelect = select;
            HandleEvents.RepaintUI();
        }

        public bool AddParentNode(NodeEditor parent) {
            if ( parentNodes.Contains(parent) ) return false;
            if(!nodeInfo.multipleParent && parentNodes.Count >= 1) return false;
            parentNodes.Add(parent);
            return true;
        }
        
        public void AddChildNode(NodeEditor child, int index) {
            if ( !child.AddParentNode(this) ) return;
            if ( childNodes.Count == nodeInfo.maxChildCount ) {
                if ( index >= childNodes.Count ) {
                    index = childNodes.Count - 1;
                }
                else index = 0;
                childNodes[index].child = child;
            }
            else {
                OutPort outPort = CreateOutPort();
                outPort.child = child;
                outPort.index = index;
            }
            child.parentNodes.Add(this);
        }

        public bool ContainPoint(Vector2 point) {
            return nodeView.ContainPoint(point);
        }

        private void DrawOutPorts() {
            if ( nodeInfo.maxChildCount == 0 ) return;
            nodeView.DrawOutPortBg();

            int count = childNodes.Count;

            if ( count == nodeInfo.maxChildCount ) {
                for ( int i = 0; i < childNodes.Count; i++ ) {
                    DrawOutPort(childNodes[i], count, i);
                }
            }
            else if ( count == 0 ) {
                DrawOutPort(leftOutPort, 1, 0);
            }
            else {
                count += 2;
                DrawOutPort(leftOutPort, count, 0);
                for ( int i = 0; i < childNodes.Count; i++ ) {
                    DrawOutPort(childNodes[i], count, i + 1);
                }

                DrawOutPort(rightOutPort, count, count - 1);
            }
        }

        private void DrawOutPort(OutPort outPort, int count, int index) {
            float offset = (index + 1) / ( count + 1f );
            bool connect = outPort.child != null;
            Rect rect = nodeView.DrawOutPort(offset, connect);
            outPort.position = rect.center;
            if ( rect.Contains(HandleEvents.position) ) {
                if ( HandleEvents.leftMouseDown ) {
                    GraphDrawEditor.Instance.selectOutPort = outPort;
                }
            }
            DrawConnectLine(outPort.position, outPort.child);
        }
        
        
        private void DrawConnectLine(Vector2 startPoint, NodeEditor node) {
            if (node == null) return;
            var targetPos = new Vector2(node.nodeView.viewRect.center.x, node.nodeView.viewRect.center.y);

            var sourcePortRect = new Rect(0, 0, 12, 12);
            sourcePortRect.center = startPoint;

            var targetPortRect = new Rect(0, 0, 15, 15);
            targetPortRect.center = targetPos;

            var boundRect = RectUtils.GetBoundRect(sourcePortRect, targetPortRect);
            if ( GraphDrawEditor.Instance.CanvasRect.Overlaps(boundRect) ) {

                GUI.Box(sourcePortRect, string.Empty, StyleSheet.Styles.nodeOutPortConnected);
                DrawConnectionGUI(sourcePortRect, targetPortRect, node);

            }
        }

        public void DrawConnectionGUI(Rect startRect, Rect endRect, NodeEditor node) {
            EditorTools.ResolveTangents(startRect.center, endRect.center, ViewDirection.Vertical, 1, out Vector2 fromTangent, out Vector2 toTangent);
            /*if ( sourceNode == targetNode ) {
                fromTangent = fromTangent.normalized * 120;
                toTangent = toTangent.normalized * 120;
            }

            centerRect.center = ParadoxNotion.CurveUtils.GetPosAlongCurve(fromPos, toPos, fromTangent, toTangent, 0.55f);*/

            DrawConnection(startRect.center, endRect.center);

        }
        
        void DrawConnection(Vector2 fromPos, Vector2 toPos) {
            var shadow = new Vector2(3.5f, 3.5f);
            EditorTools.ResolveTangents(fromPos, toPos, ViewDirection.Vertical, 1,
                                        out Vector2 startTang, out Vector2 endTang);
            startTang += fromPos;
            endTang += toPos;
            Handles.DrawBezier(fromPos, toPos + shadow, startTang + shadow, endTang + shadow, ViewConfig.Instance.connectShadow, StyleSheet.Icons.bezierTexture, 10f);
            Handles.DrawBezier(fromPos, toPos, startTang, endTang, StyleSheet.Colors.stop, StyleSheet.Icons.bezierTexture, 4);

            /*GUI.color = color.WithAlpha(1);
            GUI.DrawTexture(endRect, StyleSheet.GetDirectionArrow(toTangent.normalized));
            GUI.color = Color.white;*/
        }
    }
}