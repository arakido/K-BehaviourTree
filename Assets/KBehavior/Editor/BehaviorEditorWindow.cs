using System;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class BehaviorEditorWindow : EditorWindow {
        private const int TOP_HEIGHT = 20;
        private const int SIDE_WIDTH = 5;
        
        private Rect _canvasRect;
        public Rect CanvasRect {
            get => _canvasRect;
            set {
                _canvasRect = Rect.MinMaxRect(SIDE_WIDTH, TOP_HEIGHT, value.width - SIDE_WIDTH, value.height - SIDE_WIDTH);
                AspectRation = _canvasRect.width / _canvasRect.height;
                MiniMapRect = Rect.MinMaxRect(_canvasRect.xMax - EditorConfig.Instance.miniMapSize.y * AspectRation,
                                              _canvasRect.yMax - EditorConfig.Instance.miniMapSize.y,
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
            window.wantsMouseMove = true;
            window.Show();
        }

        private void OnEnable() {
            CanvasRect = position;
        }
        
        private float AspectRation { get; set; }

        private Rect MiniMapRect { get; set; }

        private void SetZoom(float value) {
            int d = value > 0 ? -1 : 1;
            float delta = d * (ViewZoom < 1
                ? EditorConfig.Instance.zoomDelta / EditorConfig.Instance.maxZoom
                : EditorConfig.Instance.zoomDelta);
            ViewZoom = Mathf.Clamp(ViewZoom + delta, 0.1f, EditorConfig.Instance.maxZoom);

            if (ViewZoom >= 1) LineZoom = Mathf.Floor(ViewZoom / EditorConfig.Instance.maxLineZoom) + ViewZoom % EditorConfig.Instance.maxLineZoom;
            else {
                float scale = ViewZoom * 10;
                LineZoom = Mathf.Floor(scale / EditorConfig.Instance.maxLineZoom) + scale % EditorConfig.Instance.maxLineZoom;
            }
            
            Repaint();
        }

        private void OnGUI() {
            CanvasRect = position;
            DrawLineGrid();
            HandleEvents();
        }
        
        private void DrawTextureGrid(Vector2 pan = default) {
            Rect canvas = CanvasRect;
            Texture texture = EditorTools.CreateGridTexture(10,10);
                    
            var size = canvas.size;
            var center = size / 2f;

            float xOffset = -(center.x * ViewZoom + pan.x) / texture.width;
            float yOffset = ((center.y - size.y) * ViewZoom + pan.y) / texture.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            float tileAmountX = Mathf.Round(size.x * ViewZoom) / texture.width;
            float tileAmountY = Mathf.Round(size.y * ViewZoom) / texture.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            GUI.DrawTextureWithTexCoords(canvas, texture, new Rect(tileOffset, tileAmount));
        }

        private void DrawLineGrid(Vector2 offset = default) {
            if ( Event.current.type != EventType.Repaint ) { return; }
            float step = EditorConfig.Instance.pieceWidth * LineZoom ;
            float gap = EditorConfig.Instance.GridWidth * LineZoom ;

            GL.PushMatrix();
            GL.Begin(1);
            for ( float x = CanvasRect.xMin; x < CanvasRect.xMax; x += step ) {
                DrawLine(new Vector2(x, CanvasRect.yMin), new Vector2(x, CanvasRect.yMax),
                         x % gap < step ? EditorConfig.Instance.gridLineColor : EditorConfig.Instance.gridBgColor);
            }
            
            GL.End();
            GL.PopMatrix();
            
            GL.PushMatrix();
            GL.Begin(1);
            for ( float y = CanvasRect.yMin; y < CanvasRect.yMax; y += step ) {
                DrawLine(new Vector2(CanvasRect.xMin, y), new Vector2(CanvasRect.xMax, y),
                         y % gap < step ? EditorConfig.Instance.gridLineColor : EditorConfig.Instance.gridBgColor);
            }
            GL.End();
            GL.PopMatrix();
            
            GL.PushMatrix();
            GL.Begin(1);
            for ( float x = CanvasRect.xMin; x < CanvasRect.xMax; x += step ) {
                if ( x % gap < step )
                    DrawLine(new Vector2(x, CanvasRect.yMin), new Vector2(x, CanvasRect.yMax),
                             EditorConfig.Instance.gridLineColor);
            }
            GL.End();
            GL.PopMatrix();
        }

        private void DrawLine(Vector2 p1, Vector2 p2, Color c) {
            GL.Color(c);
            GL.Vertex(p1);
            GL.Vertex(p2);
        }

        private void HandleEvents() {
            if ( Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout ) return;
            switch ( Event.current.type ) {
                case EventType.MouseDown: break;
                case EventType.MouseUp:
                    Event.current.Use();
                    break;
                case EventType.MouseMove:
                    Event.current.Use();
                    break;
                case EventType.MouseDrag: break;
                case EventType.KeyDown: break;
                case EventType.KeyUp: break;
                case EventType.ScrollWheel:
                    SetZoom(Event.current.delta.y);
                    break;
                case EventType.ValidateCommand: break;
                case EventType.ExecuteCommand: break;
            }
        }

        private void SetViewZoom() {
            
        }
    }
}