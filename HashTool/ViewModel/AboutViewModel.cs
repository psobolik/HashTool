using GalaSoft.MvvmLight;
using HashTool.Model;

namespace HashTool.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region private properties
        /// <summary>
        /// Gets the AssemblyInfo property.
        /// </summary>
        private IAssemblyInfo AssemblyInfo { get; }
        #endregion private properties

        #region public properties
        /// <summary>
        /// Gets the AppTitle property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string AppTitle => AssemblyInfo.Description;

        /// <summary>
        /// Gets the Copyright property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string Copyright => AssemblyInfo.Copyright;

        /// <summary>
        /// Gets the Version property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string Version => $"v {AssemblyInfo.Version}";

        #endregion public properties

        #region constructor
        /// <summary>
        /// Initializes a new instance of the AboutViewModel class.
        /// </summary>
        public AboutViewModel(IAssemblyInfo assemblyInfo)
        {
            AssemblyInfo = assemblyInfo;
        }
        #endregion constructor
    }
}
