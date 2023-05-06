using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OPERATION_MNS.Utilities.Common
{
    public static class DeepEqual
    {
        public static bool DeepEquals(this object obj, object another, List<string> ignore)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            //So sánh class của 2 object, nếu khác nhau thì trả fail
            if (obj.GetType() != another.GetType()) return false;

            if (!obj.GetType().IsClass) return obj.Equals(another);

            var result = true;
            //Lấy toàn bộ các properties của obj
            //sau đó so sánh giá trị của từng property
            foreach (var property in obj.GetType().GetProperties().Where(p => !p.GetIndexParameters().Any()))
            {
                if (!ignore.Contains(property.Name))
                {
                    var objValue = property.GetValue(obj);
                    var anotherValue = property.GetValue(another);

                    if ((objValue == null && anotherValue != null) ||
                        (objValue != null && anotherValue == null) ||
                        (objValue != null && !objValue.DeepEquals(anotherValue, ignore))
                       )
                    {
                        result = false;
                    }

                }
            }

            return result;
        }

        public static bool JSONEquals(this object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }
    }

    public static class DataTableToJson
    {
        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
