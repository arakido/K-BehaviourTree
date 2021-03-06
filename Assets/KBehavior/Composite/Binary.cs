﻿using KBehavior.Base;
using KBehavior.Design;

namespace KBehavior.Composite {
    [Name("二元节点")]
    [Description("通过True/False进入不同的节点")]
    [Icon("Condition")]
    public class Binary :CompositeBase {
        public override int maxChildCount { get; } = 2;
    }
}