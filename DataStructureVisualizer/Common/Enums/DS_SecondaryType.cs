using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Enums
{
    enum DS_SecondaryType
    {
        Undefined,
        // 线性表
        Array,
        LinkedList,
        Queue,
        Stack,

        // 树
        BinaryTree,
        BinarySearchTree,

        // 图
        DirectedGraph,
        UndirectedGraph,

        // 哈希表
        Hashtable,
    }
}
