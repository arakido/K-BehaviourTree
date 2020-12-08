using KBehavior.Base;
using KBehavior.Design;

namespace KBehavior.Composite {
    [Name("序列节点")]
    [Description(@"从左到右顺序执行，直到遇到字节的返回Failed")]
    [MenuPath("Composite")]
    public class Sequence:CompositeBase {
        protected override void SwitchSuccess(Stage stage) {
            Stage = SwitchNext() ? Stage.Running : stage;
        }
        
        protected override void SwitchQuit(Stage stage){
            Stage = SwitchNext() ? Stage.Running : Stage.Success;
        }
    }
}