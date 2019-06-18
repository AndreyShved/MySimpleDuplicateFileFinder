using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ConsoleApp1;

namespace WpfApp1
{
    public partial class MainWindow2 : Window
    {

        public MainWindow2()
        {
            InitializeComponent();
            var scanResult = FileDuplicateFinder.ScanWithHashes(@"C:\Users");
            DisplayFileDuplicates(scanResult);
        }

        public void DisplayFileDuplicates(IDictionary<string, List<string>> fileDuplicates)
        {
            List<FileDuplicatePathAndHash> duplicatesWithHashList = new List<FileDuplicatePathAndHash>();
            foreach (var hash in fileDuplicates.Keys)
            {
                var duplicatesList = fileDuplicates[hash];
                duplicatesWithHashList.AddRange(duplicatesList.Select(fileDuplicatePath => new FileDuplicatePathAndHash { Path = fileDuplicatePath, Hash = hash }).ToList());
            }
            ListCollectionView collectionView = new ListCollectionView(duplicatesWithHashList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Hash"));
            myDataGrid.ItemsSource = collectionView;

        }
        
    }

    public class FileDuplicatePathAndHash
    {
        public string Path { get; set; }
        public string Hash { get; set; }
    }
}