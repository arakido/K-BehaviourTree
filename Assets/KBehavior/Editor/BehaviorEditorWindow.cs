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
        
        
        private float _viewZoom = 1;
        private float ViewZoom {
            get => _viewZoom;
            set {
                int d = value > 0 ? -1 : 1;
                float v = _viewZoom + d * EditorConfig.Instance.zoomDelta;
                _viewZoom = Mathf.Clamp(v, EditorConfig.Instance.zoomRange.x, EditorConfig.Instance.zoomRange.y);
                SetLineZoom();
                Repaint();
            }
        }

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

        private void SetLineZoom() {
            if ( ViewZoom >= 1 ) {
                LineZoom = ViewZoom % (3 + 1);
                if ( LineZoom > 3 ) LineZoom -= 1; 
                else if ( LineZoom < 1 ) LineZoom += 1;
            }
            else {
                float ram = ((1 - ViewZoom) * 10) % 3;
                if ( ram < 1 ) ram += 1;
                LineZoom = 1 - ram / 10;
            }
            Debug.Log(LineZoom);
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
                    ViewZoom = Event.current.delta.y;
                    break;
                case EventType.ValidateCommand: break;
                case EventType.ExecuteCommand: break;
            }
        }

        private void SetViewZoom() {
            
        }
    }
}