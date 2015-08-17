using System;
using System.Management.Automation;
using Microsoft.Practices.Unity;

namespace ActiveCommerce.SitecorePowerShell.Cmdlets
{
    [Cmdlet("Resolve", "AcObject")]
    public class ResolveAcObjectCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string TypeName { get; set; }

        [Parameter(Position = 1)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            Type type = null;
            if (!LanguagePrimitives.TryConvertTo<Type>(TypeName, out type))
            {
                throw new ArgumentException("TypeName", string.Format("{0} is not a valid type", TypeName));
            }
            WriteObject(string.IsNullOrEmpty(Name)
                ? Sitecore.Ecommerce.Context.Entity.Resolve(type)
                : Sitecore.Ecommerce.Context.Entity.Resolve(type, Name));
        }
    }
}