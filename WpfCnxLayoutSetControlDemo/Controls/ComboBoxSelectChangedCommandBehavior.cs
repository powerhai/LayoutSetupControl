using System.Windows;
using System.Windows.Controls;

namespace WpfCnxLayoutSetControlDemo.Controls
{
    /// <summary>
    /// Combobox select changed command behavior
    /// </summary>
    public class ComboBoxSelectChangedCommandBehavior : CommandBehaviorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public ComboBoxSelectChangedCommandBehavior(ComboBox obj)
        {
            
            obj.SelectionChanged += obj_SelectionChanged;
        }


        void obj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            if (cb.IsLoaded && cb.Visibility == Visibility.Visible && cb.IsDropDownOpen)
                ExecuteCommand(ref e);
        }
    }
}