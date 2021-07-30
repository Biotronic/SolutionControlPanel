using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Volmax.ControlPanel.App.Properties;

namespace Volmax.ControlPanel.App.Processes
{
    public class MarkAttribute : Attribute
    {
        private readonly string _icon;

        public MarkAttribute(string text, string icon)
        {
            Text = text;
            _icon = icon;
        }

        public string Text { get; }

        public Image Image
        {
            get
            {
                var mi = typeof(Resources).GetMember(_icon, MemberTypes.Property,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                    .OfType<PropertyInfo>()
                    .First();

                return mi.GetValue(null) as Image;
            }
        }
    }
}