using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public static UserControl GetItemFromItemsControlByIndex(ItemsControl ic, int index, string name = "item")
        {
            var item = ic.ItemContainerGenerator.ContainerFromIndex(index) as ContentPresenter;
            return item.ContentTemplate.FindName(name, item) as UserControl;
        }

        public static object DeepCopy(object obj)
        {
            Type type = obj.GetType();
            object newObj = Activator.CreateInstance(type);
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.CanWrite)
                {
                    if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(newObj, propertyInfo.GetValue(obj));
                    }
                    else
                    {
                        object childObj = propertyInfo.GetValue(obj);
                        if (childObj == null)
                        {
                            propertyInfo.SetValue(newObj, null);
                        }
                        else
                        {
                            propertyInfo.SetValue(newObj, DeepCopy(childObj));
                        }
                    }
                }
            }
            return newObj;
        }
    }
}
