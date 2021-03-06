﻿namespace KBehavior {
    public enum Stage {
        Waiting = 0,
        Running,
        Stop,
        Success,
        Failure,
        Error,
        Quit,
    }
    
    public enum ViewDirection {
        Horizontal,
        Vertical,
    }
    
    public enum NodeType {
        /// <summary>
        /// 组合节点
        /// </summary>
        Composite,
        /// <summary>
        /// 修饰节点
        /// </summary>
        Decorator,
        /// <summary>
        /// 条件节点
        /// </summary>
        Condition,
        /// <summary>
        /// 动作节点
        /// </summary>
        Action,
    }
}