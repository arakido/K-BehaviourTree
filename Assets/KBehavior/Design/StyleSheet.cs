using System.Collections.Generic;
using UnityEngine;

namespace KBehavior.Design {
    [System.Serializable]
    public class StyleSheet : ScriptableBase {
        [ContextMenu("Lock")] private void Lock() => hideFlags = HideFlags.NotEditable;
        [ContextMenu("UnLock")] private void UnLock() => hideFlags = HideFlags.None;

        private static string assetName = "StyleSheetConfig";
        
        private static StyleSheet instance;
        public static StyleSheet Instance => instance ? instance : instance = LoadStyle();

        private static StyleSheet LoadStyle() {
            return Resources.Load<StyleSheet>(assetName);
        }
        
        public StyleConfig styles;
        public IconConfig icons;
        public ColorConfig colors;

        public static StyleConfig Styles => Instance.styles;
        public static IconConfig Icons => Instance.icons;
        public static ColorConfig Colors => Instance.colors;


        [System.Serializable]
        public class StyleConfig {
            public GUIStyle gridBg;
            public GUIStyle gridBorder;
            public GUIStyle nodeBg;
            public GUIStyle selectBg;
            public GUIStyle name;
            public GUIStyle nodeOutPortBg;
            public GUIStyle nodeOutPortEmpty;
            public GUIStyle nodeOutPortConnected;
            public GUIStyle bezierLine;
        }

        [System.Serializable]
        public class IconConfig
        {
            public Texture2D bezierTexture;

            [Header("Colorized")]
            public Texture2D circle;
            public Texture2D arrowLeft;
            public Texture2D arrowRight;
            public Texture2D arrowTop;
            public Texture2D arrowBottom;

            [Header("Fixed")]
            public Texture2D canvasIcon;
        }
        
        [System.Serializable]
        public class ColorConfig {
            public Color failure = new Color(1.0f, 0.3f, 0.3f);
            public Color success = new Color(0.4f, 0.7f, 0.2f);
            public Color running = Color.yellow;
            public Color stop = new Color(0.7f, 0.7f, 1f, 0.8f);
            public Color error = Color.red;
        }

        /*public static Texture2D GetDirectionArrow(Vector2 dir) {
            if ( dir.normalized == Vector2.left ) { return arrowLeft; }
            if ( dir.normalized == Vector2.right ) { return arrowRight; }
            if ( dir.normalized == Vector2.up ) { return arrowTop; }
            if ( dir.normalized == Vector2.down ) { return arrowBottom; }
            return circle;
        }*/
    }
}