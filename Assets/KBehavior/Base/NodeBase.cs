namespace KBehavior.Base {
    public class NodeBase {
        public string nodeName { get; }
        public string GIID { get; }
        public Stage Stage { get; protected set; }
        public NodeBase Parent { get; }

        public NodeBase() {
            nodeName = this.GetType().Name;
            Stage = Stage.Waiting;
        }
        
        public Stage Update() {
            Tick();
            return Stage;
        }

        public virtual void Tick() {
        }

        protected void SwitchStage(Stage stage) {
            switch ( stage ) {
                case Stage.Waiting:
                    SwitchWaiting(stage);
                    break;
                case Stage.Running:
                    SwitchRunning(stage);
                    break;
                case Stage.Stop:
                    SwitchStop(stage);
                    break;
                case Stage.Success:
                    SwitchSuccess(stage);
                    break;
                case Stage.Failed:
                    SwitchFailed(stage);
                    break;
                case Stage.Quit:
                    SwitchQuit(stage);
                    break;
            }
        }

        protected virtual void SwitchWaiting(Stage stage) {
            Stage = stage;
        }
        
        protected virtual void SwitchRunning(Stage stage){
            Stage = stage;
        }
        
        protected virtual void SwitchStop(Stage stage){
            Stage = stage;
        }
        
        protected virtual void SwitchSuccess(Stage stage){
            Stage = stage;
        }
        
        protected virtual void SwitchFailed(Stage stage){
            Stage = stage;
        }
        
        protected virtual void SwitchQuit(Stage stage){
            Stage = stage;
        }
    }
}