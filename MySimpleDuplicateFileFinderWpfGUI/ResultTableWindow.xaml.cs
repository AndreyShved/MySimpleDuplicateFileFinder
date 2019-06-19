using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

using MySimpleDuplicateFileFinder;

namespace MySimpleDuplicateFileFinderWpfGUI
{
    public partial class ResultTableWindow : Window
    {

        public ResultTableWindow()
        {
            InitializeComponent();
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