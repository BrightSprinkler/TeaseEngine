using System;
using System.Windows.Controls;
using TeaseEngine.Modules;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for SelectModuleControl.xaml
    /// </summary>
    public partial class SelectModuleControl : UserControl
    {
        private BaseModule Module { get; set; }

        public bool IsStartingModule => (bool)IsStartingModuleCheckBox.IsChecked;
        public string ModuleName => Module.Name;
        public event EventHandler OnCheckBoxClick;

        public SelectModuleControl(BaseModule module)
        {
            InitializeComponent();

            Module = module;
            NameLabel.Content = Module.Name;
            DescriptionLabel.Content = Module.Description;
            IsStartingModuleCheckBox.IsEnabled = Module.IsMain;
        }

        private void IsStartingModuleCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnCheckBoxClick?.Invoke(this, e);
        }
    }
}
