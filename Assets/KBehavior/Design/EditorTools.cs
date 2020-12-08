using UnityEngine;

namespace KBehavior.Design {
    public static class EditorTools {
        public static Color WithAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }
        
        private static GUIStyle _gridStyle;
        public static GUIStyle GridStyle {
            get {
                if ( _gridStyle == null ) {
                    Texture2D texture2D = EditorTools.CreateGridTexture(10, 10);
                        
                    _gridStyle = new GUIStyle();
                    _gridStyle.normal.background = texture2D;
                    _gridStyle.active.background = texture2D;
                    _gridStyle.hover.background = texture2D;
                    _gridStyle.focused.background = texture2D;
                }
                return _gridStyle;
            }
        }
        
        public static Texture2D CreateGridTexture(int pixel, int count) {
            Color bc = new Color(0.2f, 0.2f, 0.2f);
            Color lc = new Color(0.23f, 0.23f, 0.23f);
            Color sc = new Color(0.16f, 0.16f, 0.16f);
            int len = count * pixel;
            Texture2D texture2D = new Texture2D(len,len,TextureFormat.RGBA32,false);
            for ( int x = 0; x < len; x++ ) {
                for ( int y = 0; y < len; y++ ) {
                    if(x % len == 0 || y % len == 0) texture2D.SetPixel(x, y, sc);
                    else if(x % count == 0 || y % count == 0) texture2D.SetPixel(x, y, lc);
                    else texture2D.SetPixel(x, y, bc);
                }
            }
            texture2D.hideFlags = HideFlags.HideAndDontSave;
            texture2D.Apply();
            return texture2D;
        }
        
        public static Texture2D GenerateGridTexture(int width, Color bgc, int split, Color lc) {
            Texture2D tex = new Texture2D(width, width);
            Color[] cols = new Color[width * width];
            for (int y = 0; y < width; y++) {
                for (int x = 0; x < width; x++) {
                    Color col = bgc;
                    if (y % split == 0 || x % split == 0) col = Color.Lerp(lc, bgc, 0.65f);
                    if (y == width - 1 || x == width - 1) col = Color.Lerp(lc, bgc, 0.35f);
                    cols[(y * width) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.Apply();
            return tex;
        }
    }
}