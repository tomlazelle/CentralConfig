using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace CentralConfig.Common
{
    public static class ObjectExtensions
    {
        public static T ToPOCO<T>(ExpandoObject source, T destination)
        {
            var dictionary = (IDictionary<string, object>) source;
            foreach (var propertyInfo in typeof (T).GetProperties())
            {
                var lower = propertyInfo.Name.ToLower();
                var index = dictionary.Keys.SingleOrDefault(k => k.ToLower() == lower);
                if (index != null)
                    propertyInfo.SetValue((object) destination, dictionary[index], (object[]) null);
            }
            return destination;
        }

        public static object ToDynamic(object value)
        {
            var dictionary = (IDictionary<string, object>) new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(value.GetType()))
                dictionary.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(value));
            return dictionary as ExpandoObject;
        }
    }
}