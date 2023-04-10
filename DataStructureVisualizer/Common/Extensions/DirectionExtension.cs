using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Extensions
{
    public static class DirectionExtension
    {
        public static bool IsHorizontal(this Direction direction)
        {
            return direction == Direction.Left || direction == Direction.Right;
        }

        /// <summary>
        /// 判断是否为坐标轴正方向
        /// </summary>
        /// <param name="direction"></param>
        /// <remarks>y 的正半轴朝下；x 的正半轴朝右</remarks>
        /// <returns></returns>
        public static bool IsPositive(this Direction direction)
        {
            return direction == Direction.Right || direction == Direction.Down;
        }
    }
}
