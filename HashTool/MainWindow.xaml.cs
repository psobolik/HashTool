using System.Windows;
using HashTool.ViewModel;

namespace HashTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.fileNameTextBox.Drop += FileNameTextBox_Drop;
            this.fileNameTextBox.PreviewDragOver += FileNameTextBox_PreviewDragOver;
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void FileNameTextBox_PreviewDragOver(object sender, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                args.Effects = DragDropEffects.Copy;
                args.Handled = true;
            }
        }

        private void FileNameTextBox_Drop(object sender, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var files = args.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null && files.Length == 1)
                {
                    this.fileNameTextBox.Text = files[0];
                    args.Handled = true;
                }
            }
        }

    }
}