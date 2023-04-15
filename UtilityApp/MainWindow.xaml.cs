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

        private Classes.CommandManager _commandManager;

        public Classes.CommandManager CommandManager {
            get { return _commandManager; }
            set { _commandManager = value; }
        }

        public MainWindow() {
            InitializeComponent();

            _windowBackgroundColor = new(Color.FromArgb(128, 255, 217, 217));
            this.Top = 0;
            this.Left = SystemParameters.PrimaryScreenWidth - WindowWidth;

            this.Width = WindowWidth;
            this.Height = SystemParameters.PrimaryScreenHeight - TaskbarUtilities.GetTaskbarHeight();

            // 
            _commandManager = new();

            // Focus on CMD
            rtbCmd.Document.Blocks.Clear();
            AddLineToRichTextBox(rtbCmd, "> ");
            rtbCmd.CaretPosition = rtbCmd.Document.ContentEnd;
            rtbCmd.Focus();

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

        // TODO: remove this
        private string _tempText;

        public string TempText {
            get { return _tempText; }
            set { _tempText = value; OnPropertyChanged(); }
        }

        private async void CommandLinePreviewKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {

                case Key.Enter:
                    TextPointer caretLineEnd = rtbCmd.CaretPosition.GetLineStartPosition(1);
                    caretLineEnd ??= rtbCmd.Document.ContentEnd;
                    CommandResponse response = await CommandManager.RunCommand(
                        new TextRange(rtbCmd.CaretPosition.GetLineStartPosition(0), caretLineEnd).Text.Trim()[2..]);

                    if (response.Response != "") {
                        AddLineToRichTextBox(rtbCmd, response.Response);
                    }
                    e.Handled = true;

                    // TODO: make this work with multiple lines first
                    //AddLineToRichTextBox(rtbCmd, "> ");
                    //rtbCmd.CaretPosition = rtbCmd.Document.ContentEnd;
                    break;

                case Key.Up:
                    e.Handled = true;
                    break;

                case Key.Down:
                    e.Handled = true;
                    break;

                case Key.Left:
                case Key.Back:
                    if (GetCaretIndex((RichTextBox)sender) <= 2) {
                        e.Handled = true;
                    }
                    break;

                case Key.Right:
                    
                    break;

                default:
                    break;
            }
        }

        private void CommandLinePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (!rtbCmd.IsFocused) {
                rtbCmd.Focus();
            }

            e.Handled = true;
        }

        private static int GetCaretIndex(RichTextBox richTextBox) {
            // TODO: make this a per line thing instead of per all lines
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.CaretPosition).Text.Length;
        }

        private static void AddLineToRichTextBox(RichTextBox richTextBox, string text) {
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // TODO: remove this
        private void rtbCmd_TextChanged(object sender, TextChangedEventArgs e) {
            TempText = GetCaretIndex((RichTextBox)sender).ToString();
        }
    }
}
