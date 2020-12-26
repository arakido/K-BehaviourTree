using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using KBehavior.Base;

namespace KBehavior.Design {
    public static class EditorTools {
        private static readonly GenericMenu behaviorNodeMenu = new GenericMenu();
        //private static readonly Dictionary<Type, Type> behaviorNodes
        
        public static event System.Action NodeClickAction;

        public static void OnNodeClick() {
            NodeClickAction?.Invoke();
            NodeClickAction = null;
        }
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

        public static void InitNodeMenu(GenericMenu.MenuFunction2 action) {
            if ( behaviorNodeMenu.GetItemCount() <= 0 ) {
                var infos = GetNodeMenuOfType<BehaviorAttribute>();
                for ( int i = 0; i < infos.Count; i++ ) {
                    var info = infos[i];
                    if ( string.IsNullOrEmpty(info.name) ) continue;
                    string path = info.name;
                    if ( !string.IsNullOrEmpty(info.path) ) {
                        path = info.path + "/" + path;
                    }
                    behaviorNodeMenu.AddItem(new GUIContent(path),false, action, info );
                }
            }
            
            behaviorNodeMenu.ShowAsContext();
        }

        private static Dictionary<Type, List<NodeAttribute>> cacheNodeInfos = new Dictionary<Type, List<NodeAttribute>>();
        
        public static List<NodeAttribute> GetNodeMenuOfType<T>() where T : BehaviorAttribute {
            Type t = typeof(T);
            if ( cacheNodeInfos.ContainsKey(t) ) return cacheNodeInfos[t];
            List<NodeAttribute> nodeInfoList = new List<NodeAttribute>();
            Type[] types = GetNodeTypes(typeof(NodeBase));
            foreach ( Type type in types ) {
                NodeAttribute attribute = new NodeAttribute(type);
                nodeInfoList.Add(attribute);
            }
            cacheNodeInfos.Add(t,nodeInfoList);
            return nodeInfoList;
        }

        public static Type[] GetNodeTypes(Type nodeType) {
            List<Type> result = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for ( int i = 0; i < assemblies.Length; i++ ) {
                Assembly assembly = assemblies[i];
                if(assembly.IsDynamic) continue;
                Type[] types = assembly.GetExportedTypes();
                for ( int j = 0; j < types.Length; j++ ) {
                    Type type = types[j];
                    if ( nodeType.IsAssignableFrom(type) && !type.IsAbstract ) {
                        result.Add(type);
                    }
                }
            }
            return result.ToArray();
        }

        private static float labelWidth;

        public static void SetLabelWidth(float width, bool cover = false) {
            labelWidth = EditorGUIUtility.labelWidth;
            if (cover) labelWidth = width;
            EditorGUIUtility.labelWidth = width;
        }

        public static void RestoreLabelWidth() {
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public static Texture2D LoadTexture(string name) {
            return Resources.Load<Texture2D>(name);
        }
        
        public static GUIStyle CreateHeaderStyle()
        {
            var style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.imagePosition = ImagePosition.ImageLeft;
            return style;
        }

        public static void DrawBox(Rect rect, GUIStyle style) {
            GUI.Box(rect,string.Empty,style);
        }

        
    }
}