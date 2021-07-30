using System;
using System.Collections.Generic;
using System.Linq;

namespace Volmax.ControlPanel.App.Utils
{
    public static class EnumEx
    {
        public static IEnumerable<Attribute> GetAttributes<TEnum>(TEnum t) where TEnum : Enum
        {
            var members = typeof(TEnum).GetMember(t.ToString());
            var memberInfo = members.FirstOrDefault(m => m.DeclaringType == typeof(TEnum));
            if (memberInfo == null) return new Attribute[] { };
            return memberInfo.GetCustomAttributes(typeof(Attribute), false).OfType<Attribute>();
        }
    }
}