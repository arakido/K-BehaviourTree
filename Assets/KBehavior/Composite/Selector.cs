using KBehavior.Base;
using KBehavior.Design;

namespace KBehavior.Composite {
    [Name("选择节点")]
    [Description(@"从左到右顺序执行，直到遇到字节的返回Success")]
    [MenuPath("Decorators")]
    public class Selector :CompositeBase{
        protected override void SwitchFailed(Stage stage) {
            Stage = SwitchNext() ? Stage.Running : stage;
        }
        
        protected override void SwitchQuit(Stage stage){
            Stage = SwitchNext() ? Stage.Running : Stage.Success;
        }
    }
}