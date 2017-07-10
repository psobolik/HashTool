using System.Windows;
using HashTool.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using HashTool.Messaging;
using HashTool.View;

namespace HashTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow //: Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            RegisterAboutService();
            FileNameTextBox.Drop += FileNameTextBox_Drop;
            FileNameTextBox.PreviewDragOver += FileNameTextBox_PreviewDragOver;
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void RegisterAboutService()
        {
            Messenger.Default.Register<NotificationMessage>(
                    this,
                    msg =>
                    {
                        if (msg.Notification == Notifications.ShowAbout)
                        {
                            AboutView view = new AboutView { Owner = this, };
                            view.ShowDialog();
                        }
                    });
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
                    FileNameTextBox.Text = files[0];
                    args.Handled = true;
                }
            }
        }

    }
}