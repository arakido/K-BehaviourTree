using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace KBehavior.Design {
    public class ViewConfig : ScriptableBase { 
        [BoxGroup("Grid")]
        public int pieceWidth = 10;
        [BoxGroup("Grid")]
        public int pieceLenght = 10;
        [BoxGroup("Grid")]
        public Color lineColor = new Color(0.2f, 0.2f, 0.2f);
        [BoxGroup("Grid")]
        public Color gridLineColor = new Color(0.3f, 0.3f, 0.3f);
        [BoxGroup("Grid")]
        public Vector2 miniMapSize = new Vector2(170, 100);
        [BoxGroup("Grid")]
        public int snapStep = 12;
        
        [BoxGroup("Grid")]
        public float zoomDelta = 0.1f;
        [BoxGroup("Grid")]
        [Range(1,10)]
        public int maxZoom = 10;
        [BoxGroup("Grid")]
        [Range(1,5)]
        public int maxLineZoom = 3;
        public int GridWidth => pieceWidth * pieceLenght;

        [BoxGroup("Editor")]
        public float panSpeed = 1.2f;
        [BoxGroup("Editor")]
        public Texture2D gridTexture;
        [BoxGroup("Editor")]
        public Texture2D failureSymbol;
        [BoxGroup("Editor")]
        public Texture2D successSymbol;
        
        [BoxGroup("Node Config")]
        public Texture2D nodeBackground;
        [BoxGroup("Node Config")]
        public Vector2 nodeMinSize= new Vector2(100,50);
        [BoxGroup("Node Config")]
        public Texture2D selectBackground;
        [BoxGroup("Node Config")]
        public Vector2 selectOffset = default;
        [BoxGroup("Node Config")]
        public Texture2D nodeGradient;
        [BoxGroup("Node Config")]
        public Vector2 nodeOutPotSize = new Vector2(10,10);

        [BoxGroup("Node Colors")]
        public Color compositeColor;
        [BoxGroup("Node Colors")]
        public Color decoratorColor;
        [BoxGroup("Node Colors")]
        public Color conditionalColor;
        [BoxGroup("Node Colors")]
        public Color serviceColor;
        [BoxGroup("Node Colors")]
        public Color taskColor;

        [Title("Status Colors")]
        public Color defaultNodeBackgroundColor;

        public Color selectedColor;
        public Color runningColor;
        public Color abortColor;
        public Color referenceColor;
        public Color evaluateColor;
        public Color rootSymbolColor = new Color(0.3f, 0.3f, 0.3f, 1f);

        [Title("Runtime Colors")]
        public Color runningStatusColor = new Color(0.1f, 1f, 0.54f, 1f);

        public Color successColor = new Color(0.1f, 1f, 0.54f, 0.25f);
        public Color failureColor = new Color(1f, 0.1f, 0.1f, 0.25f);
        public Color abortedColor = new Color(0.1f, 0.1f, 1f, 0.25f);
        public Color interruptedColor = new Color(0.7f, 0.5f, 0.3f, 0.4f);
        public Color defaultConnectionColor = Color.white;

        [Title("Connection Lines")]
        public float defaultConnectionWidth = 4f;

        public float runningConnectionWidth = 4f;

        [Title("Node Body Layout")]
        [Tooltip("Controls additional node size.")]
        public Vector2 nodeSizePadding = new Vector2(12f, 6f);

        [Tooltip("Controls the thickness of left and right edges.")]
        public float nodeWidthPadding = 12f;

        [Tooltip("Controls how thick the ports are. Changes the nodes overall height too.")]
        public float portHeight = 20f;

        [Tooltip("Control how far the ports extend in the node.")]
        public float portWidthTrim = 50f;

        public float nodeHeadHigh = 32f;
        public float statusIconSize = 16f;
        
        public Vector2 headOffset = new Vector2(10,10);

        private static ViewConfig instance = null;
        public static ViewConfig Instance {
            get {
                if ( instance == null ) {
                    instance = LoadDefaultPreferences();
                }
                return instance;
            }
            set => instance = value;
        }

        private static ViewConfig LoadDefaultPreferences() {
            var prefs = Resources.Load<ViewConfig>("KBehaviorEditorConfig");

            if ( prefs == null ) {
                Debug.LogWarning("Failed to load KBehaviorEditorConfig");
                prefs = CreateInstance<ViewConfig>();
            }

            return prefs;
        }
    }
}