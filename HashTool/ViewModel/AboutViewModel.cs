using GalaSoft.MvvmLight;
using HashTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string AppTitle { get { return AssemblyInfo.Description; } }

        /// <summary>
        /// Gets the Copyright property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string Copyright { get { return this.AssemblyInfo.Copyright; } }

        /// <summary>
        /// Gets the Version property from the <see cref="AssemblyInfo" />.
        /// </summary>
        public string Version { get { return string.Format(System.Globalization.CultureInfo.CurrentCulture, "v {0}", AssemblyInfo.Version); } }

        #endregion public properties

        #region constructor
        /// <summary>
        /// Initializes a new instance of the AboutViewModel class.
        /// </summary>
        public AboutViewModel(IAssemblyInfo assemblyInfo)
        {
            this.AssemblyInfo = assemblyInfo;
        }
        #endregion constructor
    }
}
