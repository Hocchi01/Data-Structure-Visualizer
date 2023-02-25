﻿using DataStructureVisualizer.ViewModels.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels
{
    internal class ToolboxViewModel
    {
        public RandomGenerateToolViewModel RandomGenerateTool { get; set; }
        public AddToolViewModel AddTool { get; set; }

        public ToolboxViewModel() 
        { 
            RandomGenerateTool = new RandomGenerateToolViewModel();
            AddTool = new AddToolViewModel();
        }
    }
}