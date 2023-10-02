using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace OPERATION_MNS.Data.EF.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// convert yyyyMMdd -> yyyy-MM-dd
        /// </summary>
        /// <param name="objsource"></param>
        /// <returns></returns>
        public static string ToYYYY_MM_DD(this string objsource)
        {
            if (objsource == null)
            {
                return "";
            }

            return objsource.Substring(0,4) +"-" + objsource.Substring(4,2) + "-" + objsource.Substring(6,2);
        }
        public static double ViewChipWf(this float objSource,string unit)
        {
            if(unit == "CHIP")
            {
                return Math.Round(objSource / 1000, 0);  
            }

            if(Math.Abs(Math.Round(objSource, 0) - Math.Round(objSource,1)) == 0.5)
            {
                return Math.Ceiling(objSource);
            }

            return Math.Round(objSource);
        }

        public static string ChipWFFormat(this double objSource, string n)
        {
            if (objSource == 0)
            {
                return "";
            }
            return objSource.ToString("#,#0");
        }

        public static string ChipWFFormatColor(this float objSource)
        {
            if (objSource < 0)
            {
                return "red";
            }
            return "black";
        }

        public static object CloneObject(this object objSource)
        {
            //Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        //object objPropertyValue = property.GetValue(objSource, null);
                        //if (objPropertyValue == null)
                        //{
                        //    property.SetValue(objTarget, null, null);
                        //}
                        //else
                        //{
                        //    property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                        //}
                    }
                }
            }
            return objTarget;
        }

        public static void CopyPropertiesFrom(this object self, object parent, List<string> arrIgnorr)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType && !arrIgnorr.Contains(fromProperty.Name))
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }

        public static void CopyPropertiesFromWhithoutType(this object self, object parent, List<string> arrIgnorr)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && !arrIgnorr.Contains(fromProperty.Name))
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }

        public static bool IsUpdateData(this object self, object parent, List<string> arrIgnorr)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && !fromProperty.GetValue(parent).NullString().Equals(toProperty.GetValue(self).NullString()) && !arrIgnorr.Contains(fromProperty.Name))
                    {
                        Debug.WriteLine(parent);
                        Debug.WriteLine("selt:" + toProperty.Name + ":" + fromProperty.GetValue(parent).NullString() + "--" + toProperty.GetValue(self).NullString());
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
