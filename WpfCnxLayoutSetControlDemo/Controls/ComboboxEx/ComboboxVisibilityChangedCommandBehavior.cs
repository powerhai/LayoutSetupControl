using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCnxLayoutSetControlDemo.Controls.ComboboxEx
{
    public class ComboboxVisibilityChangedCommandBehavior 
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
        protected virtual void ExecuteCommand( )
        {
            if (Command != null)
            {
                Command.Execute(CommandParameter ); 
            }
        }
        public ComboboxVisibilityChangedCommandBehavior(ComboBox obj)
        {
            
            obj.IsVisibleChanged += ObjOnIsVisibleChanged;
            
        }
        
        private void ObjOnIsVisibleChanged (object sender, DependencyPropertyChangedEventArgs e)
        {
           
            var cb = (ComboBox)sender; 
            if (cb.IsLoaded && cb.Visibility == Visibility.Visible )
                ExecuteCommand( );
        }

        
    }
}