using System.Threading.Tasks;
using System.Windows;
using MySimpleDuplicateFileFinder;

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
            ScanButton.IsEnabled = false;
            var scanResultWindow = new ResultTableWindow();
            var path = txtEditor.Text;
            var scanResult = await Task.Factory.StartNew(async (dirPath) =>
            {
                var pathToScan = dirPath as string;
                return await FileDuplicateFinder.FastScanWithHashesAsync(pathToScan);
            }, path);
            var awaitedScanResult = await scanResult;
            scanResultWindow.DisplayFileDuplicates(awaitedScanResult);
            scanResultWindow.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" This action is not implemented yet! ");
        }
    }
}
