using System;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class BehaviorEditorWindow : EditorWindow {
        
        private float ViewZoom{ get; set; } = 1;
        private float LineZoom { get; set; } = 1;

        [MenuItem("Tools/KBehavior/Editor")]
        private static void ShowWindow() {
            var window = GetWindow<BehaviorEditorWindow>(false,"Behavior Editor");
            window.Show();
        }

        private void OnEnable() {
            GraphDrawEditor.Instance.CanvasRect = position;
            wantsMouseMove = true;
            minSize = new Vector2(800,600);
        }
        
        

        private void SetZoom(float value) {
            int d = value > 0 ? -1 : 1;
            float delta = d * (ViewZoom < 1
                ? ViewConfig.Instance.zoomDelta / ViewConfig.Instance.maxZoom
                : ViewConfig.Instance.zoomDelta);
            ViewZoom = Mathf.Clamp(ViewZoom + delta, 0.1f, ViewConfig.Instance.maxZoom);

            if (ViewZoom >= 1) LineZoom = Mathf.Floor(ViewZoom / ViewConfig.Instance.maxLineZoom) + ViewZoom % ViewConfig.Instance.maxLineZoom;
            else {
                float scale = ViewZoom * 10;
                LineZoom = Mathf.Floor(scale / ViewConfig.Instance.maxLineZoom) + scale % ViewConfig.Instance.maxLineZoom;
            }
            
            Repaint();
        }

        private void OnGUI() {
            GraphDrawEditor.Instance.CanvasRect = position;
            DrawGrid.Instance.DrawBackground();
            DrawGrid.Instance.DrawGridLine(LineZoom);
            HandleEvents.CheckRepaint();
            DrawNodeView();
            HandleEventStage();
        }
        
        private void HandleEventStage() {
            if ( HandleEvents.refresh ) return;
            switch ( HandleEvents.type ) {
                case EventType.MouseDown:
                    GraphDrawEditor.ClearSelectNode();
                    if ( HandleEvents.rightMouse ) {
                        GraphDrawEditor.Instance.ShowEnumPanel(HandleEvents.position);
                    }
                    HandleEvents.Use();
                    break;
                case EventType.MouseUp:
                    break;
                case EventType.MouseMove:
                    break;
                case EventType.MouseDrag: break;
                case EventType.KeyDown: break;
                case EventType.KeyUp: break;
                case EventType.ScrollWheel:
                    SetZoom(HandleEvents.currentEvent.delta.y);
                    break;
                case EventType.ValidateCommand: break;
                case EventType.ExecuteCommand: break;
                case EventType.ContextClick:
                    GraphDrawEditor.Instance.ShowEnumPanel(HandleEvents.position);
                    //e.Use();
                    break;
            }
        }

        

        private void DrawNodeView() {
            BeginWindows();
            GraphDrawEditor.Instance.DrawNode();
            EndWindows();
            GraphDrawEditor.Instance.DrawConnects();
        }
        
    }
}