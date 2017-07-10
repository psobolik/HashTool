using System;
using System.Reflection;

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
        public string Version => AssemblyName.Version?.ToString();
        public string DisplayName => AssemblyName.FullName;
        public string ProcessorArchecture => AssemblyName.ProcessorArchitecture.ToString();
        private Assembly Assembly { get; }
        private AssemblyName AssemblyName => _assemblyName ?? (_assemblyName = Assembly.GetName());

        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
        }

        private string GetAttributeValue<T>(Func<T, string> callback, string defaultValue = null) where T : Attribute
        {
            T attribute = Assembly.GetCustomAttribute<T>();
            return attribute == null ? defaultValue : callback(attribute);
        }
    }
}
