using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor {
    public class DrawGrid : Singleton<DrawGrid> {
        public void DrawBackground() {
            Rect canvasRect = GraphDrawEditor.Instance.CanvasRect;
            EditorTools.DrawBox(canvasRect, StyleSheet.Styles.gridBg);
            EditorTools.DrawBox(canvasRect, StyleSheet.Styles.gridBorder);
        }
        
        public void DrawGridLine(float zoom, Vector2 offset = default) {
            if ( Event.current.type != EventType.Repaint )  return; 
            Rect canvasRect = GraphDrawEditor.Instance.CanvasRect;
            float step = ViewConfig.Instance.pieceWidth * zoom ;
            float gap = ViewConfig.Instance.GridWidth * zoom ;

            for ( float x = canvasRect.xMin; x < canvasRect.xMax; x += step ) {
                if(x % gap >= step)
                    DrawLine(new Vector2(x, canvasRect.yMin), new Vector2(x, canvasRect.yMax), ViewConfig.Instance.lineColor);
            }
            
            for ( float y = canvasRect.yMin; y < canvasRect.yMax; y += step ) {
                DrawLine(new Vector2(canvasRect.xMin, y), new Vector2(canvasRect.xMax, y),
                         y % gap < step ? ViewConfig.Instance.gridLineColor : ViewConfig.Instance.lineColor);
            }
            for ( float x = canvasRect.xMin; x < canvasRect.xMax; x += step ) {
                if ( x % gap < step )
                    DrawLine(new Vector2(x, canvasRect.yMin), new Vector2(x, canvasRect.yMax), ViewConfig.Instance.gridLineColor);
            }
            Handles.color= Color.white;
        }

        
        private void DrawLine(Vector2 p1, Vector2 p2, Color c) {
            Handles.color = c;
            Handles.DrawLine(p1, p2);
        }
        
        public void DrawTextureGrid(Rect canvasRect,float zoom,Vector2 pan = default) {
            Texture texture = EditorTools.CreateGridTexture(10,10);
                    
            var size = canvasRect.size;
            var center = size / 2f;

            float xOffset = -(canvasRect.x * zoom + pan.x) / texture.width;
            float yOffset = ((canvasRect.y - size.y) * zoom + pan.y) / texture.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            float tileAmountX = Mathf.Round(size.x * zoom) / texture.width;
            float tileAmountY = Mathf.Round(size.y * zoom) / texture.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            GUI.DrawTextureWithTexCoords(canvasRect, texture, new Rect(tileOffset, tileAmount));
        }

    }
}