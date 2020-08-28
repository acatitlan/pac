#region Copyright (c) 2013-2013 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2000-2013 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion Copyright (c) 2000-2012 cjlc

using System;

namespace ExtensionMethods
{
    using System.Reflection;

    public static class ExtensionsClass
    {
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : value.ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string str)
        {
            StringValue = str;
        }

        public string StringValue { get; set; }
    }
}
