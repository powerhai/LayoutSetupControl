using System.Windows.Controls;

namespace WpfCnxLayoutSetControlDemo.Controls
{
    /// <summary>
    /// Combo box change infomation
    /// </summary>
    public class ComboBoxChangeInfo
    {
        /// <summary>
        /// Gets or sets combo box object
        /// </summary>
        public object BindingObj { get; set; }
        /// <summary>
        /// Gets or sets selection changed event args
        /// </summary>
        public SelectionChangedEventArgs e { get; set; }
    }
}
