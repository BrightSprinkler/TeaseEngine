using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeaseEngine.Utils;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for SelectStorageFileControl.xaml
    /// </summary>
    public partial class SelectStorageFileControl : UserControl
    {
        private string InternalCurrentContent;

        public string Path { get; private set; }
        public string OriginalContent { get; private set; }
        public string CurrentContent { get => InternalCurrentContent; set { InternalCurrentContent = value; UpdateUI(); } }
        public string ErrorMessage { get; private set; }

        public string StorageName => System.IO.Path.GetFileNameWithoutExtension(Path);
        public bool HasUnsavedChanges => OriginalContent != CurrentContent;
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        private Logger Logger = App.Logging.GetLogger<SelectStorageFileControl>();

        public EventHandler OnRestore { get; set; }

        public SelectStorageFileControl(string path)
        {
            InitializeComponent();

            Path = path;
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(Path, JToken.Parse(CurrentContent).ToString(Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
                ErrorMessage = ex.Message;
                UpdateUI();
                return;
            }

            ErrorMessage = "";
            Load();
        }

        public void Restore()
        {
            Load();
            OnRestore?.Invoke(this, new EventArgs());
        }

        private void Load()
        {
            OriginalContent = File.ReadAllText(Path);
            CurrentContent = OriginalContent;
            UpdateUI();
        }

        public void UpdateUI()
        {
            NameLabel.Content = StorageName;
            ErrorLabel.Content = ErrorMessage;
            RestoreButton.IsEnabled = HasUnsavedChanges;
            SaveButton.IsEnabled = HasUnsavedChanges;
            ToolTip = ErrorMessage;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            Restore();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
    }
}
