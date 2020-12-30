using System.Collections.Generic;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class GraphDrawEditor:Singleton<GraphDrawEditor> {
        private const int TOP_HEIGHT = 20;
        private const int SIDE_WIDTH = 5;
        
        private Rect _canvasRect;
        public Rect CanvasRect {
            get => _canvasRect;
            set {
                _canvasRect = Rect.MinMaxRect(SIDE_WIDTH, TOP_HEIGHT, value.width - SIDE_WIDTH, value.height - SIDE_WIDTH);
                AspectRation = _canvasRect.width / _canvasRect.height;
                MiniMapRect = Rect.MinMaxRect(_canvasRect.xMax - ViewConfig.Instance.miniMapSize.y * AspectRation,
                                              _canvasRect.yMax - ViewConfig.Instance.miniMapSize.y,
                                              _canvasRect.xMax - 2,
                                              _canvasRect.yMax - 2);
            }
        }
        
        private float AspectRation { get; set; }

        private Rect MiniMapRect { get; set; }
        
        public static List<NodeEditor> selectNodes = new List<NodeEditor>();
        public OutPort selectOutPort;
        
        private List<NodeEditor> nodeList = new List<NodeEditor>();
        
        private Vector2 moustPoint = default;
        
        public void ShowEnumPanel(Vector2 point) {
            moustPoint = point;
            EditorTools.InitNodeMenu(OnCreateNodeRequest);
        }
        
        private void OnCreateNodeRequest(NodeAttribute node) {
            AddNode(node, moustPoint);
        }

        public void AddNode(NodeAttribute node, Vector2 point) {
            NodeEditor nodeEditor = NodeEditor.Create(nodeList.Count + 1);
            nodeEditor.Init(node, point);
            if ( selectOutPort != null ) {
                selectOutPort.parent.AddChildNode(nodeEditor,selectOutPort.index);
                selectOutPort = null;
            }
            nodeList.Add(nodeEditor);
        }
        
        public void AddNode(NodeAttribute node, Vector2 point, NodeEditor parent, int index) {
            NodeEditor nodeEditor = NodeEditor.Create(nodeList.Count + 1);
            nodeEditor.Init(node, point);
            parent.AddChildNode(nodeEditor,index);
            nodeList.Add(nodeEditor);
        }

        public void DrawNode() {
            for ( int i = 0; i < nodeList.Count; i++ ) {
                nodeList[i].DrawUI();
            }
        }

        public void DrawConnects() {
            for ( int i = 0; i < nodeList.Count; i++ ) {
                var node = nodeList[i];
                CheckConnectNode(node);
            }
            DrawNewLine();
        }
        
        private void CheckConnectNode(NodeEditor node) {
            if ( selectOutPort != null && HandleEvents.leftMouseUp ) {
                if(node.ContainPoint(HandleEvents.position)) {
                    selectOutPort.parent.AddChildNode(node,selectOutPort.index);
                    selectOutPort = null;
                    HandleEvents.Use();
                }
            }
        }
        
        private void DrawNewLine() {
            if(selectOutPort == null) return;
            EditorTools.ResolveTangents(selectOutPort.position, HandleEvents.position, ViewDirection.Vertical, 1,
                                        out Vector2 startTang, out Vector2 endTang);
            startTang += selectOutPort.position;
            endTang += HandleEvents.position;

            Handles.DrawBezier(selectOutPort.position, HandleEvents.position, startTang,
                               endTang, StyleSheet.Colors.stop, StyleSheet.Icons.bezierTexture, 3);
            HandleEvents.RepaintUI();
            
            if ( HandleEvents.leftMouseUp ) {
                Vector2 position = HandleEvents.position;
                NodeEditor parent = selectOutPort.parent;
                int index = selectOutPort.index;
                selectOutPort = null;
                EditorTools.InitNodeMenu((node) => {
                                             AddNode(node, position, parent, index);
                                         });
                HandleEvents.Use();
            }
        }

        public static void MoveSelectNode(Vector2 delta) {
            for ( int i = 0; i < selectNodes.Count; i++ ) {
                selectNodes[i].position += delta;
            }
        }

        public static void AddSelectNode(NodeEditor node, bool only = true) {
            if ( only ) {
                while ( selectNodes.Count > 0 ) {
                    selectNodes[0].SetSelectType(false);
                    selectNodes.RemoveAt(0);
                }
            }
            selectNodes.Add(node);
            node.SetSelectType(true);
        }
        
        public static void RemoveSelectNode(NodeEditor node) {
            if ( selectNodes.Contains(node) ) {
                node.SetSelectType(false);
                selectNodes.Remove(node);
            }
        }

        public static void ClearSelectNode() {
            while ( selectNodes.Count > 0 ) {
                selectNodes[0].SetSelectType(false);
                selectNodes.RemoveAt(0);
            }
        }
    }
}