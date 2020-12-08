using System;
using UnityEngine;

namespace KBehavior.Design {
#region Attribute
    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : Attribute {
        public NameAttribute(string name){}
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute {
        public DescriptionAttribute(string description){}
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MenuPathAttribute : Attribute {
        public MenuPathAttribute(params string[] path){}
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class IconAttribute : Attribute {
        public IconAttribute(string name){}
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ColorAttribute : Attribute {
        public ColorAttribute(string hexColor){}
        public ColorAttribute(Color color){}
    }
    

#endregion
    
    
    
    public class DesignAttributes {
        
        
        
    }
}