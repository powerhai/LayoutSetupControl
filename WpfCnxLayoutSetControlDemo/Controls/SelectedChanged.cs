using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCnxLayoutSetControlDemo.Controls
{
    /// <summary>
    /// Select changed properties
    /// </summary>
    public class SelectedChanged
    {

        private static readonly DependencyProperty SelectedChangedCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "SelectedChangedCommandBehavior",
            typeof(ComboBoxSelectChangedCommandBehavior),
            typeof(SelectedChanged),
            null);
        /// <summary>
        /// Command property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(SelectedChanged),
            new PropertyMetadata(OnSetCommandCallback));
        /// <summary>
        /// Command parameter property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(SelectedChanged),
            new PropertyMetadata(OnSetCommandParameterCallback));

        /// <summary>
        /// Sets command to combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <param name="command"></param>
        public static void SetCommand(ComboBox cBox, ICommand command)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            cBox.SetValue(CommandProperty, command);
        }
        /// <summary>
        /// Gets command to combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <returns></returns>
        public static ICommand GetCommand(ComboBox cBox)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            return cBox.GetValue(CommandProperty) as ICommand;
        }
        /// <summary>
        /// Sets command parameter of combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <param name="parameter"></param>
        public static void SetCommandParameter(ComboBox cBox, object parameter)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            cBox.SetValue(CommandParameterProperty, parameter);
        }
        /// <summary>
        /// Gets command parameter combobox
        /// </summary>
        /// <param name="cBox"></param>
        /// <returns></returns>
        public static object GetCommandParameter(ComboBox cBox)
        {
            if (cBox == null) throw new System.ArgumentNullException("cBox");
            return cBox.GetValue(CommandParameterProperty);
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var CBox = dependencyObject as ComboBox;
            if (CBox != null)
            {
                ComboBoxSelectChangedCommandBehavior behavior = GetOrCreateBehavior(CBox);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var CBox = dependencyObject as ComboBox;
            if (CBox != null)
            {
                var behavior = GetOrCreateBehavior(CBox);
                behavior.CommandParameter = e.NewValue;
            }
        }

        private static ComboBoxSelectChangedCommandBehavior GetOrCreateBehavior(ComboBox cBox)
        {
            var behavior = cBox.GetValue(SelectedChangedCommandBehaviorProperty) as ComboBoxSelectChangedCommandBehavior;
            if (behavior == null)
            {
                behavior = new ComboBoxSelectChangedCommandBehavior(cBox);
                cBox.SetValue(SelectedChangedCommandBehaviorProperty, behavior);
            }

            return behavior;
        }


    }
}