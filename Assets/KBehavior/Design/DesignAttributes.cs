using System;
using UnityEngine;

namespace KBehavior.Design {
    
#region Attribute

    public class BehaviorAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : BehaviorAttribute {
        public readonly string name;

        public NameAttribute(string name) {
            this.name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : BehaviorAttribute {
        public readonly string description;

        public DescriptionAttribute(string description) {
            this.description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MenuPathAttribute : BehaviorAttribute {
        public readonly string path;

        public MenuPathAttribute(params string[] path) {
            this.path = String.Join("/",path);
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class IconAttribute : BehaviorAttribute {
        public readonly string icon;

        public IconAttribute(string name) {
            icon = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ColorAttribute : BehaviorAttribute {
        public ColorAttribute(string hexColor){}
        public ColorAttribute(Color color){}
    }
    

#endregion
    
    public static class AttributeTools{
        public static T GetAttribute<T>(this Type type) where T: BehaviorAttribute{
            object[] attributes =  type.GetCustomAttributes(true);
            for ( int i = 0; i < attributes.Length; i++ ) {
                var att = (Attribute) attributes[i];
                var attType = att.GetType();
                Type t = typeof(T);
                if ( t.IsAssignableFrom(attType) ) {
                    if ( type.IsDefined(attType, false) ) return (T) attributes[i];
                }
            }

            return null;
        }
    }

    public struct NodeAttribute {
        public Type type;
        public string name;
        public string path;
        private string iconName;
        public string description;
        private Texture _icon;

        public Texture Icon {
            get {
                if ( _icon == null && !string.IsNullOrEmpty(iconName) ) {
                    _icon = EditorTools.LoadTexture(iconName);
                }
                return _icon;
            }
        }

        public NodeAttribute(Type t) {
            type = t;
            var nameAttribute = type.GetAttribute<NameAttribute>();
            name = nameAttribute != null ? nameAttribute.name : string.Empty;
            var pathAttribute = type.GetAttribute<MenuPathAttribute>();
            path = pathAttribute != null ? pathAttribute.path : string.Empty;
            var iconAttribute = type.GetAttribute<IconAttribute>();
            iconName = iconAttribute != null ? iconAttribute.icon : string.Empty;
            var descriptionAttribute = type.GetAttribute<DescriptionAttribute>();
            description = descriptionAttribute != null ? descriptionAttribute.description : string.Empty;
            _icon = null;
        }
    }
    
    public class DesignAttributes {
        
        
        
    }
}