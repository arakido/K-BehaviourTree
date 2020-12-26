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
        
        public Styles styles;
        public Icons icons;


        [System.Serializable]
        public class Styles {
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
        public class Icons
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

        public static GUIStyle gridBg => Instance.styles.gridBg;
        public static GUIStyle gridBorder => Instance.styles.gridBorder;
        public static GUIStyle node => Instance.styles.nodeBg;
        public static GUIStyle selectNode => Instance.styles.selectBg;
        public static GUIStyle nameStyle => Instance.styles.name;
        public static GUIStyle nodeOutPortBg => Instance.styles.nodeOutPortBg;
        public static GUIStyle nodeOutPortEmpty => Instance.styles.nodeOutPortEmpty;
        public static GUIStyle nodeOutPortConnected => Instance.styles.nodeOutPortConnected;

        ///----------------------------------------------------------------------------------------------

        public static Texture2D canvasIcon => Instance.icons.canvasIcon;
        public static Texture2D bezierTexture => Instance.icons.bezierTexture;

        ///Return an arrow based on direction vector
        /*public static Texture2D GetDirectionArrow(Vector2 dir) {
            if ( dir.normalized == Vector2.left ) { return arrowLeft; }
            if ( dir.normalized == Vector2.right ) { return arrowRight; }
            if ( dir.normalized == Vector2.up ) { return arrowTop; }
            if ( dir.normalized == Vector2.down ) { return arrowBottom; }
            return circle;
        }*/

        public static void Draw(Rect rect, GUIStyle style) {
            GUI.Box(rect,"");
        }
        
        public static Color GetStatusColor(Stage status) {
            switch ( status ) {
                case ( Stage.Failure ): return new Color(1.0f, 0.3f, 0.3f);
                case ( Stage.Success ): return new Color(0.4f, 0.7f, 0.2f);
                case ( Stage.Running ): return Color.yellow;
                case ( Stage.Stop ): return new Color(0.7f, 0.7f, 1f, 0.8f);
                case ( Stage.Error ): return Color.red;
            }
            return Color.white;
        }
    }
}