namespace KBehavior.Design {
    public class Singleton<T> where T: new(){
        private static T instance;

        public static T Instance {
            get {
                if ( instance == null ) {
                    instance = System.Activator.CreateInstance<T>();
                }

                return instance;
            }
        }
    }
}