using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HashTool.Helpers;
using HashTool.Model;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        private string _welcomeTitle = string.Empty;
        private string _pickedPath = string.Empty;

        private string _md5Hash = string.Empty;
        private string _sha1Hash = string.Empty;
        private string _sha256Hash = string.Empty;
        private string _sha384Hash = string.Empty;
        private string _sha512Hash = string.Empty;

        private bool _isMd5Checked = true;
        private bool _isSha1Checked = true;
        private bool _isSha256Checked = true;
        private bool _isSha384Checked = true;
        private bool _isSha512Checked = true;

        private bool _isComputingMd5 = false;
        private bool _isComputingSha1 = false;
        private bool _isComputingSha256 = false;
        private bool _isComputingSha384 = false;
        private bool _isComputingSha512 = false;

        public string PickedPath { get { return _pickedPath; } set { Set(ref _pickedPath, value); GoCommand.RaiseCanExecuteChanged(); ClearHashes(); } }

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

        public bool IsComputingMd5 { get { return _isComputingMd5; } set { Set(ref _isComputingMd5, value); } }
        public bool IsComputingSha1 { get { return _isComputingSha1; } set { Set(ref _isComputingSha1, value); } }
        public bool IsComputingSha256 { get { return _isComputingSha256; } set { Set(ref _isComputingSha256, value); } }
        public bool IsComputingSha384 { get { return _isComputingSha384; } set { Set(ref _isComputingSha384, value); } }
        public bool IsComputingSha512 { get { return _isComputingSha512; } set { Set(ref _isComputingSha512, value); } }

        /// <summary>
        /// Gets the AppTitle property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string AppTitle { get { return AssemblyInfo.Description; } }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAssemblyInfo assemblyInfo)
        {
            this.AssemblyInfo = assemblyInfo;
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
                        this.PickedPath = ofd.FileName;
                    }
                }));
            }
        }

        public RelayCommand GoCommand
        {
            get
            {
                return _goCommand ?? (_goCommand = new RelayCommand(() =>
                {
                    CalculateChecksums();
                },
                () =>
                {
                    return CanCalculateChecksums();
                }));
            }
        }
        private void ClearHashes()
        {
            this.Md5Hash = this.Sha1Hash = this.Sha256Hash = this.Sha384Hash = this.Sha512Hash = string.Empty;
        }

        private bool CanCalculateChecksums()
        {
            return !string.IsNullOrWhiteSpace(this.PickedPath) && (this.IsMd5Checked || this.IsSha1Checked || this.IsSha256Checked || this.IsSha384Checked || this.IsSha512Checked);
        }
        public void CalculateChecksums()
        {
            ClearHashes();
            if (CanCalculateChecksums())
            {
                Task.Run(() => this.Sha512Hash = ComputeSha512Hash(this.PickedPath));
                Task.Run(() => this.Sha384Hash = ComputeSha384Hash(this.PickedPath));
                Task.Run(() => this.Sha256Hash = ComputeSha256Hash(this.PickedPath));
                Task.Run(() => this.Sha1Hash = ComputeSha1Hash(this.PickedPath));
                Task.Run(() => this.Md5Hash = ComputeMd5Hash(this.PickedPath));
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
            if (this.IsMd5Checked)
            {
                this.IsComputingMd5 = true;
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
            if (this.IsSha1Checked)
            {
                this.IsComputingSha1 = true;
                using (HashAlgorithm hasher = SHA1.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                this.IsComputingSha1 = false;
            }
            return result;
        }
        private string ComputeSha256Hash(string fileName)
        {
            string result = string.Empty;
            if (this.IsSha256Checked)
            {
                this.IsComputingSha256 = true;
                using (HashAlgorithm hasher = SHA256.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                this.IsComputingSha256 = false;
            }
            return result;
        }
        private string ComputeSha384Hash(string fileName)
        {
            string result = string.Empty;
            if (this.IsSha384Checked)
            {
                this.IsComputingSha384 = true;
                using (HashAlgorithm hasher = SHA384.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                this.IsComputingSha384 = false;
            }
            return result;
        }
        private string ComputeSha512Hash(string fileName)
        {
            string result = string.Empty;
            if (this.IsSha512Checked)
            {
                this.IsComputingSha512 = true;
                using (HashAlgorithm hasher = SHA512.Create())
                {
                    result = ComputeHashString(hasher, fileName);
                }
                this.IsComputingSha512 = false;
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