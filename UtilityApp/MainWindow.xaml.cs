using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using UtilityApp.Classes;

namespace UtilityApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {

        public const double WindowWidth = 500.0;

        private SolidColorBrush _windowBackgroundColor;

        public SolidColorBrush WindowBackgroundColor {
            get { return _windowBackgroundColor; }
            set { _windowBackgroundColor = value; OnPropertyChanged(); }
        }

        private GlobalHotKeyManager? _globalHotKeyManager;

        public GlobalHotKeyManager? GlobalHotKeyManager {
            get { return _globalHotKeyManager; }
            set { _globalHotKeyManager = value; }
        }

        public MainWindow() {
            InitializeComponent();

            _windowBackgroundColor = new(Color.FromArgb(128, 255, 217, 217));
            this.Top = 0;
            this.Left = SystemParameters.PrimaryScreenWidth - WindowWidth;

            this.Width = WindowWidth;
            this.Height = SystemParameters.PrimaryScreenHeight - TaskbarUtilities.GetTaskbarHeight();

            // Bottom
            this.DataContext = this;
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);

            GlobalHotKeyManager = new GlobalHotKeyManager(this);
            GlobalHotKeyManager.Start();
        }

        protected override void OnClosed(EventArgs e) {

            GlobalHotKeyManager?.Stop();

            base.OnClosed(e);
        }

        private void MinimizeWindowClick(object sender, RoutedEventArgs e) {
            this.Hide();
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e) {
            this.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
