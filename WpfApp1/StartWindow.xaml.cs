using System.Windows;

namespace MySimpleDuplicateFileFinderWpfGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ScanButton.IsEnabled = false;
            var scanResultWindow = new ResultTableWindow();
            await scanResultWindow.ScanForDuplicates(this.txtEditor.Text);
            scanResultWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" This action is not implemented yet! ");
        }
    }
}
