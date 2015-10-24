using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCnxLayoutSetControlDemo.Controls
{
    /// <summary>
    /// Command behavior base class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandBehaviorBase 
    {
        private ICommand mCommand;
        /// <summary>
        /// Gets or sets command of component
        /// </summary>
        public ICommand Command
        {
            get { return mCommand; }
            set
            {
                mCommand = value;
            }
        }

        private object mCommandParameter;
        /// <summary>
        /// Gets or sets command parameter of component
        /// </summary>
        public object CommandParameter
        {
            get { return mCommandParameter; }
            set
            {
                mCommandParameter = value;
            }
        }
        /// <summary>
        /// Excute command of component
        /// </summary>
        /// <param name="e"></param>
        protected virtual void ExecuteCommand(ref SelectionChangedEventArgs e)
        {
            if (Command != null)
            {
                Command.Execute(new ComboBoxChangeInfo() { BindingObj = CommandParameter, e = e });

            }
        }
    }
}