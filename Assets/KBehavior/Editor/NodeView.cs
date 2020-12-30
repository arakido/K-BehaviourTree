using System;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class NodeView {
        public event Action HandleEventCallBack;
        public int ID;

        public Rect viewRect;
        private Rect headRect;
        private Rect contentRect;

        private GUIContent nameContent;
        private GUIStyle nameStyle;
        private Texture2D backGround;

        public NodeView(int id, NodeAttribute nodeAttribute) {
            ID = id;
            nameContent = new GUIContent(nodeAttribute.name, nodeAttribute.Icon);
            backGround = ViewConfig.Instance.nodeBackground;
            ResizeView();
        }

        private void ResizeView() {
            viewRect = default;
            //viewRect.height = ViewConfig.Instance.nodeHeadHigh;
            viewRect.size = StyleSheet.Styles.name.CalcSize(nameContent) + StyleSheet.Styles.name.contentOffset * 2;

        }
        public void ResizeContent(){}

        public void SetPosition(Vector2 pos) {
            viewRect.position = pos;
        }
        
        public void SetContent(){}

        public void DrawView(bool select) {
            viewRect = GUILayout.Window(ID, viewRect, DrawNode, string.Empty, StyleSheet.Styles.nodeBg,
                                        GUILayout.MinWidth(ViewConfig.Instance.nodeMinSize.x),
                                        GUILayout.MinHeight(ViewConfig.Instance.nodeMinSize.y));
            EditorGUIUtility.AddCursorRect(viewRect, MouseCursor.Link);
            if ( select ) {
                GUI.Box(viewRect, string.Empty, StyleSheet.Styles.selectBg);
            }
        }

        private void DrawNode(int id) {
            Event e = Event.current;
            DrawHeadView();
            DrawContentView();
            HandleEvents();
        }

        private void HandleEvents() {
            HandleEventCallBack?.Invoke();
        }

        private void DrawHeadView() {
            GUILayout.Box(nameContent,StyleSheet.Styles.name);
        }
        
        private void DrawContentView(){}
        
        public void DrawOutPortBg() {
            var outPortRect = new Rect(viewRect.xMin, viewRect.yMax - 4, viewRect.width, 12);
            EditorTools.DrawBox(outPortRect, StyleSheet.Styles.nodeOutPortBg);
        }

        public Rect DrawOutPort(float offset, bool connected) {
            var portRect = new Rect(default, ViewConfig.Instance.nodeOutPotSize);
            portRect.center = new Vector2( viewRect.width * offset  + viewRect.xMin, viewRect.yMax + 6);
            if(!connected)EditorGUIUtility.AddCursorRect(portRect, MouseCursor.ArrowPlus);
            EditorTools.DrawBox(portRect, connected ? StyleSheet.Styles.nodeOutPortConnected : StyleSheet.Styles.nodeOutPortEmpty);
            return portRect;
        }

        public bool ContainPoint(Vector2 point) {
            return viewRect.Contains(point);
        }
    }
}