using System.Collections.Generic;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class NodeEditorManager : ScriptableObject {
        public static Queue<NodeEditor> selectNodes = new Queue<NodeEditor>();

        private List<NodeEditor> nodeList = new List<NodeEditor>();

        public void AddNode(NodeAttribute node, Vector2 point = default) {
            NodeEditor nodeEditor = NodeEditor.Create(nodeList.Count + 1);
            nodeEditor.Init(node, point);
            nodeList.Add(nodeEditor);
        }

        public void DrawNode() {
            for ( int i = 0; i < nodeList.Count; i++ ) {
                nodeList[i].DrawUI();
            }
        }

        public void DrawConnects() {
            for ( int i = 0; i < nodeList.Count; i++ ) {
                nodeList[i].DrawConnectLine();
            }
        }

        public static void SetSelectNode(NodeEditor node, bool only = true) {
            if ( only ) {
                while ( selectNodes.Count > 0 ) {
                    selectNodes.Dequeue().SetSelectType(false);
                }
            }
            selectNodes.Enqueue(node);
            node.SetSelectType(true);
        }
    }
}