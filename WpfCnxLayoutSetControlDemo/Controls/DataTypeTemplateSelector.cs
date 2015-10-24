using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfCnxLayoutSetControlDemo.Models;

namespace WpfCnxLayoutSetControlDemo.Controls
{
    public class CnxItemContainerStyleSelector : StyleSelector
    {
        public Style Style1
        {
            get;
            set;
        }
        public Style Style2
        {
            get;
            set;
        }
        public override Style SelectStyle (object item, DependencyObject container)
        {
            if (item is LcInlet)
                return Style1;
            else
                return Style2;
        }
    }
    public class DataTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LaneTemplate { get; set; }
        public DataTemplate NonLaneTemplate { get; set; }

        public override DataTemplate SelectTemplate (object item, DependencyObject container)
        {
            if(item is LcLaneNonAssign)
                return LaneTemplate;
            else
                return NonLaneTemplate;
        }

    }
}
