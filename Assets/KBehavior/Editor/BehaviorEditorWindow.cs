using System;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class BehaviorEditorWindow : EditorWindow {
        private const int TOP_HEIGHT = 20;
        private const int SIDE_WIDTH = 5;

        private static NodeEditorManager nodeEditorManager;
        
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
        
        
        private float ViewZoom{ get; set; } = 1;
        private float LineZoom { get; set; } = 1;

        [MenuItem("Tools/KBehavior/Editor")]
        private static void ShowWindow() {
            var window = GetWindow<BehaviorEditorWindow>(false,"Behavior Editor");
            window.minSize = new Vector2(800,600);
            window.Show();
        }

        private void OnEnable() {
            CanvasRect = position;
            wantsMouseMove = true;
            nodeEditorManager = ScriptableObject.CreateInstance<NodeEditorManager>();
        }
        
        private float AspectRation { get; set; }

        private Rect MiniMapRect { get; set; }

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
            CanvasRect = position;
            DrawGrid.Instance.DrawBackground(CanvasRect);
            DrawGrid.Instance.DrawGridLine(CanvasRect,LineZoom);
            HandleEvents.CheckRepaint();
            DrawNodeView();
            HandleEventStage();
        }
        
        private void HandleEventStage() {
            if ( HandleEvents.refresh ) return;
            switch ( HandleEvents.type ) {
                case EventType.MouseDown:
                    if ( HandleEvents.rightMouse ) {
                        AddNode(HandleEvents.position);
                        //e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    //e.Use();
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
                    AddNode(HandleEvents.position);
                    //e.Use();
                    break;
            }
        }

        private Vector2 moustPoint = default;
        private void AddNode(Vector2 point) {
            moustPoint = point;
            EditorTools.InitNodeMenu(OnCreateNodeRequest);
        }

        private void DrawNodeView() {
            BeginWindows();
            nodeEditorManager.DrawNode();
            EndWindows();
            nodeEditorManager.DrawConnects();
        }
        
        
        private void OnCreateNodeRequest(object userdata) {
            /*var newNode = Node.Create(this, nodeType, pos);

            allNodes.Add(newNode);

            if ( primeNode == null ) {
                primeNode = newNode;
            }

            //UpdateNodeIDs(false);
            UndoUtility.SetDirty(this);*/
            if ( userdata is NodeAttribute node ) {
                nodeEditorManager.AddNode(node, moustPoint);
            }
        }
    }
}