using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HashTool.Model
{
    public class AssemblyInfo : IAssemblyInfo
    {
        private AssemblyName _assemblyName;

        public string Title
        {
            get { return GetAttributeValue<AssemblyTitleAttribute>(a => a.Title); }
        }
        public string Description
        {
            get { return GetAttributeValue<AssemblyDescriptionAttribute>(a => a.Description); }
        }
        public string Configuration
        {
            get { return GetAttributeValue<AssemblyConfigurationAttribute>(a => a.Configuration); }
        }
        public string Product
        {
            get { return GetAttributeValue<AssemblyProductAttribute>(a => a.Product); }
        }
        public string Company
        {
            get { return GetAttributeValue<AssemblyCompanyAttribute>(a => a.Company); }
        }
        public string Copyright
        {
            get { return GetAttributeValue<AssemblyCopyrightAttribute>(a => a.Copyright); }
        }
        public string Trademark
        {
            get { return GetAttributeValue<AssemblyTrademarkAttribute>(a => a.Trademark); }
        }
        public string Culture
        {
            get { return GetAttributeValue<AssemblyCultureAttribute>(a => a.Culture); }
        }
        public string Version
        {
            get
            {
                Version version = AssemblyName.Version;
                return version == null ? null : version.ToString();
            }
        }
        public string DisplayName
        {
            get { return AssemblyName.FullName; }
        }
        public string ProcessorArchecture
        {
            get { return AssemblyName.ProcessorArchitecture.ToString(); }
        }

        private Assembly Assembly { get; set; }
        private AssemblyName AssemblyName { get { return _assemblyName ?? (_assemblyName = Assembly.GetName()); } }

        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
        }

        protected string GetAttributeValue<T>(Func<T, string> callback, string defaultValue = null) where T : System.Attribute
        {
            T attribute = Assembly.GetCustomAttribute<T>();
            return attribute == null ? defaultValue : callback(attribute);
        }
    }
}
