using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCnxLayoutSetControlDemo.Controls.ComboboxEx
{
    
    public class ComboboxExtend
    {

        private static readonly DependencyProperty VisibilityChangedCommandBehaviorProperty = DependencyProperty.RegisterAttached(
           "VisibilityChangedCommandBehavior",
           typeof(ComboboxVisibilityChangedCommandBehavior),
           typeof(ComboboxExtend),
           null);


        /// <summary>
        /// Command property
        /// </summary>
        public static readonly DependencyProperty VisibilityCommandProperty = DependencyProperty.RegisterAttached(
            "VisibilityCommand",
            typeof(ICommand),
            typeof(ComboboxExtend),
            new PropertyMetadata(OnSetVisibilityCommandCallback));
        /// <summary>
        /// Command parameter property
        /// </summary>
        public static readonly DependencyProperty VisibilityCommandParameterProperty = DependencyProperty.RegisterAttached(
            "VisibilityCommandParameter",
            typeof(object),
            typeof(ComboboxExtend),
            new PropertyMetadata(OnSetVisibilityCommandParameterCallback));

        /// <summary>
        /// Sets command to combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <param name="command"></param>
        public static void SetVisibilityCommand(ComboBox cBox, ICommand command)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            cBox.SetValue(VisibilityCommandProperty, command);
        }
        /// <summary>
        /// Gets command to combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <returns></returns>
        public static ICommand GetVisibilityCommand(ComboBox cBox)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            return cBox.GetValue(VisibilityCommandProperty) as ICommand;
        }
        /// <summary>
        /// Sets command parameter of combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <param name="parameter"></param>
        public static void SetVisibilityCommandParameter(ComboBox cBox, object parameter)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            cBox.SetValue(VisibilityCommandParameterProperty, parameter);
        }
        /// <summary>
        /// Gets command parameter combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <returns></returns>
        public static object GetVisibilityCommandParameter(ComboBox cBox)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            return cBox.GetValue(VisibilityCommandParameterProperty);
        }

        private static void OnSetVisibilityCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var CBox = dependencyObject as ComboBox;
            if (CBox != null)
            {
                ComboboxVisibilityChangedCommandBehavior behavior = GetOrCreateBehavior(CBox);//= GetOrCreateBehavior(CBox);
                behavior.Command = e.NewValue as ICommand;
                
            }
        }

        private static void OnSetVisibilityCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var CBox = dependencyObject as ComboBox;
            if (CBox != null)
            {
                ComboboxVisibilityChangedCommandBehavior behavior =  GetOrCreateBehavior(CBox);//= GetOrCreateBehavior(CBox);
                behavior.CommandParameter = e.NewValue;
            }
        }

        private static ComboboxVisibilityChangedCommandBehavior GetOrCreateBehavior(ComboBox cBox)
        {
            var behavior = cBox.GetValue(VisibilityChangedCommandBehaviorProperty) as ComboboxVisibilityChangedCommandBehavior;
            if (behavior == null)
            {
                behavior = new ComboboxVisibilityChangedCommandBehavior(cBox);
                cBox.SetValue(VisibilityChangedCommandBehaviorProperty, behavior);
            }

            return behavior;
        }

    }
}
