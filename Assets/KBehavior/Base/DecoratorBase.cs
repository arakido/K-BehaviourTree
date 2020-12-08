namespace KBehavior.Base {
    /// <summary>
    /// 装饰节点
    /// 将Child Node返回的结果进行额外处理再返给Parent Node
    /// </summary>
    public class DecoratorBase:NodeBase {
        protected NodeBase childBase;
    }
}