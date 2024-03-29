﻿using DataStructureVisualizer.Views.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DataStructureVisualizer.Common
{
    internal static class Comm
    {
        #region 颜色板
        public static Color ActiveColor = new Color() { ScA = 1.0F, R = 139, G = 195, B = 74 };
        public static Color PrimaryColor = new Color() { ScA = 1.0F, R = 103, G = 58, B = 183 };
        public static Color SecondaryColor = new Color() { ScA = 1.0F, R = 174, G = 234, B = 0 };
        public static Color WhiteColor = new Color() { ScA = 1.0F, R = 255, G = 255, B = 255 };
        public static readonly Color NewColor = new Color() { ScA = 0.7F, R = 255, G = 152, B = 0 };
        public static readonly Color TransparentColor = new Color() { ScA = 0.0F };

        public static SolidColorBrush ActiveColorBrush = new SolidColorBrush(ActiveColor);
        public static SolidColorBrush PrimaryColorBrush = new SolidColorBrush(PrimaryColor);
        public static SolidColorBrush WhiteColorBrush = new SolidColorBrush(WhiteColor);
        public static readonly SolidColorBrush NewColorBrush = new SolidColorBrush(NewColor);
        public static readonly SolidColorBrush TransparentColorBrush = new SolidColorBrush(TransparentColor);
        #endregion

        /// <summary>
        /// 得到集合在一定范围的规范值集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="gMin"></param>
        /// <param name="gMax"></param>
        /// <returns></returns>
        public static float[] GetGradient(int[] values, float gMin = 0.0F, float gMax = 1.0F)
        {
            float[] gradient = Enumerable.Repeat(gMin, values.Length).ToArray();
            if (values == null || values.Length == 0)
            {
                return gradient;
            }

            int vMax = values.Max();
            int vMin = values.Min();
            // values 值均相同时，返回中间值
            if (vMax == vMin)
            {
                return Enumerable.Repeat((gMin + gMax) / 2, values.Length).ToArray();
            }

            for (int i = 0; i < values.Length; i++)
            {
                gradient[i] += (gMax - gMin) * ((float)(values[i] - vMin) / (vMax - vMin));
            }

            return gradient;
        }
        public static float[] GetGradient(ObservableCollection<int> values, float gMin = 0.0F, float gMax = 1.0F)
        {
            return GetGradient(ObservableCollectionToArray(values), gMin, gMax);
        }

        public static float GetSingleGradientVal(int value, IEnumerable<int> values, float gMin = 0.0F, float gMax = 1.0F)
        {
            int vMax = values.Max();
            int vMin = values.Min();
            if (vMax == vMin)
            {
                return (gMin + gMax) / 2;
            }
            return gMin + (gMax - gMin) * ((float)(value - vMin) / (vMax - vMin));
        }

        /// <summary>
        /// 根据集合元素值计算相应颜色（透明度）梯度
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<Color> GetColorGradientByValues(int[] values)
        {
            List<Color> colors = new List<Color>();
            if (values == null || values.Length == 0)
            {
                return colors;
            }

            float minOpacity = 0.5F;

            float[] opacities = GetGradient(values, minOpacity);

            for (int i = 0; i < values.Length; i++)
            {
                Color color = PrimaryColor;
                color.ScA = opacities[i];
                colors.Add(color);
            }

            return colors;
        }
        public static List<Color> GetColorGradientByValues(ObservableCollection<int> values)
        {
            return GetColorGradientByValues(ObservableCollectionToArray(values));
        }
        public static List<Color> GetColorGradientByValues(List<int> values)
        {
            return GetColorGradientByValues(ListToArray(values));
        }


        public static int[] ObservableCollectionToArray(ObservableCollection<int> list)
        {
            int[] arr = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = list[i];
            }

            return arr;
        }

        public static List<int> ObservableCollectionToList(ObservableCollection<int> oc)
        {
            var list = new List<int>();
            for (int i = 0; i < oc.Count; i++)
            {
                list.Add(oc[i]);
            }

            return list;
        }

        public static int[] ListToArray(List<int> list)
        {
            var arr = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = list[i];
            }
            return arr;
        }

        /// <summary>
        /// 获取 ItemsControl 中的子项 Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ic"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetItemFromItemsControlByIndex<T>(ItemsControl ic, int index, string name = "item") where T : UIElement
        {
            var item = ic.ItemContainerGenerator.ContainerFromIndex(index) as ContentPresenter;
            return (item.ContentTemplate.FindName(name, item)) as T;
        }

        public static DataItemUserControlBase GetItemFromItemsControlByIndex(ItemsControl ic, int index, string name = "item")
        {
            var item = ic.ItemContainerGenerator.ContainerFromIndex(index) as ContentPresenter;
            return item.ContentTemplate.FindName(name, item) as DataItemUserControlBase;
        }

        public static T DeepCopy<T>(T obj) where T : class
        {
            if (obj == null) return null;

            Type type = obj.GetType();

            // 如果是值类型或字符串类型，直接返回
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }

            // 如果是数组类型，创建一个新的数组并递归拷贝每个元素
            if (type.IsArray)
            {
                var elementType = Type.GetType(
                    type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                var copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCopy(array.GetValue(i)), i);
                }
                return (T)(object)copied;
            }

            // 如果是集合类型，创建一个新的集合并递归拷贝每个元素
            if (obj is IEnumerable)
            {
                var elementType = type.GetGenericArguments()[0];
                var collection = Activator.CreateInstance(type);
                foreach (var item in obj as IEnumerable)
                {
                    var copied = DeepCopy(item);
                    type.GetMethod("Add").Invoke(collection, new[] { copied });
                }
                return (T)collection;
            }

            // 如果是自定义类型，递归拷贝每个属性
            var newObj = Activator.CreateInstance(type);
            foreach (var field in type.GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var fieldValue = field.GetValue(obj);
                var copied = DeepCopy(fieldValue);
                field.SetValue(newObj, copied);
            }
            foreach (var property in type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!property.CanWrite) continue;
                var propertyValue = property.GetValue(obj);
                var copied = DeepCopy(propertyValue);
                property.SetValue(newObj, copied);
            }
            return (T)newObj;
        }

        //public static T DeepCopy2<T>(T obj)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        var formatter = new BinaryFormatter();
        //        formatter.Serialize(stream, obj);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return (T)formatter.Deserialize(stream);
        //    }
        //}

        //public static object DeepCopy(object obj)
        //{
        //    Type type = obj.GetType();
        //    object newObj = Activator.CreateInstance(type);
        //    PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //    foreach (PropertyInfo propertyInfo in propertyInfos)
        //    {
        //        if (propertyInfo.CanWrite)
        //        {
        //            if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
        //            {
        //                propertyInfo.SetValue(newObj, propertyInfo.GetValue(obj));
        //            }
        //            else
        //            {
        //                object childObj = propertyInfo.GetValue(obj);
        //                if (childObj == null)
        //                {
        //                    propertyInfo.SetValue(newObj, null);
        //                }
        //                else
        //                {
        //                    propertyInfo.SetValue(newObj, DeepCopy(childObj));
        //                }
        //            }
        //        }
        //    }
        //    return newObj;
        //}

        // public static Guid guid = new Guid();

        public static uint uniqueCode = 0;

        public static string GetUniqueString() 
        {
            return (uniqueCode++).ToString();
        }
    }
}
