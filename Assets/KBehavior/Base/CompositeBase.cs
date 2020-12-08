using System;
using System.Collections.Generic;
namespace KBehavior.Base {
    
    public class CompositeBase:NodeBase {
        protected int currentIndex;
        protected List<NodeBase> childNode  = new List<NodeBase>();

        public NodeBase CurrentNode {
            get {
                if ( currentIndex < childNode.Count ) return childNode[currentIndex];
                return null;
            }
        }

        public NodeBase AddNode(NodeBase node) {
            childNode.Add(node);
            return this;
        }

        public NodeBase RemoveNode(NodeBase node) {
            childNode.Remove(node);
            return this;
        }

        public bool ContainsNode(NodeBase node) {
            return childNode.Contains(node);
        }

        public void Shuffle() {
            int count = childNode.Count;
            if (count <= 0) return;
            Random random = new Random();
            for ( int i = count - 1; i > 0; i++ ) {
                int index = random.Next(i + 1);
                NodeBase temp = childNode[i];
                childNode[i] = childNode[index];
                childNode[index] = temp;
            }
        }
        
        public override void Tick() {
            NodeBase node = this.CurrentNode;
            if(node == null)return;
            SwitchStage(node.Stage);
        }

        protected bool SwitchNext() {
            currentIndex++;
            return currentIndex < childNode.Count;
        }
    }
}