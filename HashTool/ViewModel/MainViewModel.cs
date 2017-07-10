using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HashTool.Helpers;
using HashTool.Model;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using HashTool.Messaging;

namespace HashTool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region private properties
        /// <summary>
        /// Gets the AssemblyInfo property.
        /// </summary>
        private IAssemblyInfo AssemblyInfo { get; }
        #endregion private properties

        private RelayCommand _pickFileCommand;
        private RelayCommand _goCommand;
        private RelayCommand _aboutCommand;

        private string _pickedPath = string.Empty;

        private string _md5Hash = string.Empty;
        private string _sha1Hash = string.Empty;
        private string _sha256Hash = string.Empty;
        private string _sha384Hash = string.Empty;
        private string _sha512Hash = string.Empty;

        private int _computingCount;

        private bool _isMd5Checked = true;
        private bool _isSha1Checked = true;
        private bool _isSha256Checked = true;
        private bool _isSha384Checked = true;
        private bool _isSha512Checked = true;

        private bool _isComputingMd5;
        private bool _isComputingSha1;
        private bool _isComputingSha256;
        private bool _isComputingSha384;
        private bool _isComputingSha512;

        public string PickedPath { get { return _pickedPath; } set { Set(ref _pickedPath, value); GoCommand.RaiseCanExecuteChanged(); ClearHashes(); } }

        public bool IsNotComputing { get { return _computingCount == 0; } set { if (value) ++_computingCount; else --_computingCount; RaisePropertyChanged(); } }
        public string Md5Hash { get { return _md5Hash; } set { Set(ref _md5Hash, value); } }
        public string Sha1Hash { get { return _sha1Hash; } set { Set(ref _sha1Hash, value); } }
        public string Sha256Hash { get { return _sha256Hash; } set { Set(ref _sha256Hash, value); } }
        public string Sha384Hash { get { return _sha384Hash; } set { Set(ref _sha384Hash, value); } }
        public string Sha512Hash { get { return _sha512Hash; } set { Set(ref _sha512Hash, value); } }

        public bool IsMd5Checked { get { return _isMd5Checked; } set { Set(ref _isMd5Checked, value); GoCommand.RaiseCanExecuteChanged(); } }
        public bool IsSha1Checked { get { return _isSha1Checked; } set { Set(ref _isSha1Checked, value); GoCommand.RaiseCanExecuteChanged(); } }
        public bool IsSha256Checked { get { return _isSha256Checked; } set { Set(ref _isSha256Checked, value); GoCommand.RaiseCanExecuteChanged(); } }
        public bool IsSha384Checked { get { return _isSha384Checked; } set { Set(ref _isSha384Checked, value); GoCommand.RaiseCanExecuteChanged(); } }
        public bool IsSha512Checked { get { return _isSha512Checked; } set { Set(ref _isSha512Checked, value); GoCommand.RaiseCanExecuteChanged(); } }

        public bool IsComputingMd5 { get { return _isComputingMd5; } set { IsNotComputing = !value; Set(ref _isComputingMd5, value); } }
        public bool IsComputingSha1 { get { return _isComputingSha1; } set { IsNotComputing = !value; Set(ref _isComputingSha1, value); } }
        public bool IsComputingSha256 { get { return _isComputingSha256; } set { IsNotComputing = !value; Set(ref _isComputingSha256, value); } }
        public bool IsComputingSha384 { get { return _isComputingSha384; } set { IsNotComputing = !value; Set(ref _isComputingSha384, value); } }
        public bool IsComputingSha512 { get { return _isComputingSha512; } set { IsNotComputing = !value; Set(ref _isComputingSha512, value); } }

        /// <summary>
        /// Gets the AppTitle property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string AppTitle => AssemblyInfo.Description; 

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAssemblyInfo assemblyInfo)
        {
            AssemblyInfo = assemblyInfo;
        }
        public RelayCommand PickFileCommand
        {
            get
            {
                return _pickFileCommand ?? (_pickFileCommand = new RelayCommand(() =>
                {
                    Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
                    {
                        DefaultExt = "*.*",
                        Filter = "All Files (*.*)|*.*"
                    };
                    bool? dlgResult = ofd.ShowDialog();
                    if (dlgResult == true)
                    {
                        PickedPath = ofd.FileName;
                    }
                }));
            }
        }

        public RelayCommand GoCommand => _goCommand ?? (_goCommand = new RelayCommand(CalculateChecksums, CanCalculateChecksums));
        public RelayCommand AboutCommand => _aboutCommand ?? (_aboutCommand = new RelayCommand(ShowAboutBox));

        private void ShowAboutBox()
        {
            Messenger.Default.Send(new NotificationMessage(Notifications.ShowAbout));
        }

        private void ClearHashes()
        {
            Md5Hash = Sha1Hash = Sha256Hash = Sha384Hash = Sha512Hash = string.Empty;
        }

        private bool CanCalculateChecksums()
        {
            return !string.IsNullOrWhiteSpace(PickedPath) 
                && File.Exists(PickedPath)
                && (IsMd5Checked || IsSha1Checked || IsSha256Checked || IsSha384Checked || IsSha512Checked);
        }
        private void CalculateChecksums()
        {
            ClearHashes();
            if (CanCalculateChecksums())
            {
                Task.Run(() => Sha512Hash = ComputeSha512Hash(PickedPath));
                Task.Run(() => Sha384Hash = ComputeSha384Hash(PickedPath));
                Task.Run(() => Sha256Hash = ComputeSha256Hash(PickedPath));
                Task.Run(() => Sha1Hash = ComputeSha1Hash(PickedPath));
                Task.Run(() => Md5Hash = ComputeMd5Hash(PickedPath));
            }
        }
        private static string ComputeHashString(HashAlgorithm hasher, string fileName)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                return hasher.ComputeHash(fileStream).ToHexString();
            }
        }

        private string ComputeMd5Hash(string fileName)
        {
            string result = string.Empty;
            if (IsMd5Checked)
            {
                IsComputingMd5 = true;
                using (HashAlgorithm hasher = MD5.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                IsComputingMd5 = false;
            }
            return result;
        }

        private string ComputeSha1Hash(string fileName)
        {
            string result = string.Empty;
            if (IsSha1Checked)
            {
                IsComputingSha1 = true;
                using (HashAlgorithm hasher = SHA1.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                IsComputingSha1 = false;
            }
            return result;
        }
        private string ComputeSha256Hash(string fileName)
        {
            string result = string.Empty;
            if (IsSha256Checked)
            {
                IsComputingSha256 = true;
                using (HashAlgorithm hasher = SHA256.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                IsComputingSha256 = false;
            }
            return result;
        }
        private string ComputeSha384Hash(string fileName)
        {
            string result = string.Empty;
            if (IsSha384Checked)
            {
                IsComputingSha384 = true;
                using (HashAlgorithm hasher = SHA384.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                IsComputingSha384 = false;
            }
            return result;
        }
        private string ComputeSha512Hash(string fileName)
        {
            string result = string.Empty;
            if (IsSha512Checked)
            {
                IsComputingSha512 = true;
                using (HashAlgorithm hasher = SHA512.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                IsComputingSha512 = false;
            }
            return result;
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}