using System;
using System.Collections.Generic;
using KBehavior.Base;

namespace KBehavior {
    public static class NodePool {
        private static Dictionary<Type,Stack<NodeBase>> nodePool = new Dictionary<Type, Stack<NodeBase>>();
        public static T PopNode<T>() where T:NodeBase, new() {
            if (nodePool.TryGetValue(typeof(T), out Stack<NodeBase> nodeStack)) {
                return (T) nodeStack.Pop();
            }
            return new T();
        }
        
        public static void PushNode<T>(T item) where T:NodeBase, new() {
            Type type = typeof(T);
            Stack<NodeBase> nodeStack;
            if (nodePool.TryGetValue(type, out nodeStack)) {
                nodeStack.Push(item);
            }
            else {
                nodeStack = new Stack<NodeBase>();
                nodeStack.Push(item);
                nodePool.Add(type, nodeStack);
            }
        }
    }
}