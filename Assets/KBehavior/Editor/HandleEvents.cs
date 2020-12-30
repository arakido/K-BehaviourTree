using UnityEngine;

namespace KBehavior.Editor {
    public static class HandleEvents {
        public static Event currentEvent => Event.current;
        public static EventType type => currentEvent.type;

        public static Vector2 position => currentEvent.mousePosition;
        public static bool leftMouse => currentEvent.button == 0;
        public static bool rightMouse => currentEvent.button == 1;
        public static bool middleMouse => currentEvent.button == 2;
        public static bool mouseDown => currentEvent.type == EventType.MouseDown;
        public static bool leftMouseDown => mouseDown && leftMouse;
        public static bool rightMouseDown => mouseDown && rightMouse;
        public static bool middleMouseDown => mouseDown && middleMouse;
        
        public static bool mouseUp => currentEvent.type == EventType.MouseUp;
        public static bool leftMouseUp => mouseUp && leftMouse;
        public static bool rightMouseUp => mouseUp && rightMouse;
        public static bool middleMouseUp => mouseUp && middleMouse;

        public static bool ContextClick => currentEvent.type == EventType.ContextClick;

        public static bool mouseMove => currentEvent.type == EventType.MouseMove;
        public static bool mouseDrag => currentEvent.type == EventType.MouseDrag;
        public static bool leftMouseDrag => leftMouse && mouseDrag;

        public static bool repaint => currentEvent.type == EventType.Repaint;
        public static bool layout => currentEvent.type == EventType.Layout;
        public static bool refresh => repaint || layout;

        public static bool doubleClick => currentEvent.clickCount == 2;

        public static bool control => currentEvent.control;
        public static Vector2 delta => currentEvent.delta;

        public static void Use() {
            currentEvent.Use();
        }

        public static void CheckRepaint() {
            if(mouseMove || mouseDrag){
                RepaintUI();
            }
        }
        
        public static void RepaintUI() {
            UnityEditor.HandleUtility.Repaint();
        }
    }
}