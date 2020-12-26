using System;
using KBehavior.Design;
using UnityEditor;
using UnityEngine;

namespace KBehavior.Editor.Tools {
    public class Tools : EditorWindow {
        [MenuItem("Tools/KBehavior/Tools")]
        private static void ShowWindow() {
            var window = GetWindow<Tools>();
            window.titleContent = new GUIContent("Tools");
            window.Show();
        }

        private void OnGUI() {
            GUILayout.Window(0, new Rect(100,100,300,300), (id) => { Debug.LogError(1111); }, "dfdf");

            EditorTools.SetLabelWidth(70);
            CreateAsset<StyleSheet>();
            EditorTools.RestoreLabelWidth();
        }

        private void DrawCreateGroup<T>() where T: ScriptableBase{
            EditorGUILayout.BeginFoldoutHeaderGroup(true,$"Create {typeof(T).Name}");
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Save Path:",ScriptableBase.savePath);
            if ( GUILayout.Button("...",GUILayout.Width(100)) ) {
                ScriptableBase.savePath = EditorUtility.OpenFolderPanel("Select Save Path", Application.streamingAssetsPath, "");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            string asstName = EditorGUILayout.TextField("Save Name:",typeof(T).Name);
            if ( GUILayout.Button("Create",GUILayout.Width(100)) ) {
                T so = ScriptableObject.CreateInstance<T>();
                string name = $"/{asstName}.asset";
                AssetDatabase.CreateAsset(so, ScriptableBase.savePath + name);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);
        }

        private void CreateAsset<T>() where T : ScriptableBase {
            if ( GUILayout.Button($"Create {typeof(T).Name}") ) {
                string path = EditorUtility.SaveFilePanel("Save Path", Application.streamingAssetsPath, typeof(T).Name,"asset");
                ScriptableBase.savePath = path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));
                T so = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(so, ScriptableBase.savePath);
            }
        }
    }
}