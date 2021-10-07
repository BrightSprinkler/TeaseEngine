using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using TeaseEngine.Controls;
using TeaseEngine.Models;
using TeaseEngine.Modules;
using TeaseEngine.Utils;
using Path = System.IO.Path;

namespace TeaseEngine.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private IEnumerable<BaseModule> Modules { get; set; }
        private SelectStorageFileControl CurrentSelectStorageFileControl { get; set; }

        public string StartingModuleName
        {
            get
            {
                foreach (SelectModuleControl control in ModuleStackPanel.Children)
                {
                    if (control.IsStartingModule)
                        return control.ModuleName;
                }

                return null;
            }
        }

        public SettingsWindow(IEnumerable<BaseModule> modules)
        {
            InitializeComponent();

            Modules = modules;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadModules(!(bool)ShowOnlyMainModulesCheckBox.IsChecked);
            LoadStorage();
        }

        private void LoadStorage()
        {
            StorageFiles.Children.Clear();

            foreach (string file in Directory.GetFiles(new PathManager().StorageDirectory))
            {
                SelectStorageFileControl selectStorageFileControl = new SelectStorageFileControl(file);

                selectStorageFileControl.MouseDown += (sender, e) =>
                 {
                     CurrentSelectStorageFileControl = selectStorageFileControl;
                     StorageText.Text = selectStorageFileControl.CurrentContent;
                 };

                selectStorageFileControl.OnRestore += (sender, e) =>
                {
                    if(CurrentSelectStorageFileControl == selectStorageFileControl)
                    {
                        StorageText.Text = selectStorageFileControl.CurrentContent;
                    }
                };

                StorageFiles.Children.Add(selectStorageFileControl);
            }
        }

        private void LoadModules(bool showAll)
        {
            ModuleStackPanel.Children.Clear();
            Settings settings = new StorageService().Read("Settings", new Settings());

            foreach (BaseModule module in Modules.Where(x => showAll || x.IsMain))
            {
                SelectModuleControl selectModuleControl = new SelectModuleControl(module);
                selectModuleControl.OnCheckBoxClick += (sender, e) =>
                {
                    foreach (SelectModuleControl control in ModuleStackPanel.Children)
                    {
                        if (control == selectModuleControl) continue;

                        control.IsStartingModuleCheckBox.IsChecked = false;
                    }
                };

                if (module.Name == settings.StartingModule)
                    selectModuleControl.IsStartingModuleCheckBox.IsChecked = true;

                ModuleStackPanel.Children.Add(selectModuleControl);
            }

        }

        private void ShowOnlyMainModulesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            LoadModules(!(bool)ShowOnlyMainModulesCheckBox.IsChecked);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StartingModuleName is null) return;

            new StorageService().Write("Settings", new Settings()
            {
                StartingModule = StartingModuleName
            });

            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "dll",
                Multiselect = true,
                ValidateNames = true
            };

            if ((bool)saveFileDialog.ShowDialog())
                AddModules(saveFileDialog.FileNames);
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            AddModules((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        private static void AddModules(IEnumerable<string> files)
        {
            PathManager pathManager = new PathManager();

            foreach (string file in files)
            {
                if (!file.EndsWith(".dll")) continue;
                File.Copy(file, Path.Combine(pathManager.ModuleDirectory, Path.GetFileName(file)), true);
            }

            System.Windows.MessageBox.Show("Please restart the application to see the new modules.");
        }

        private void StorageText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (CurrentSelectStorageFileControl is null) return;
            CurrentSelectStorageFileControl.CurrentContent = StorageText.Text;
        }
    }
}
