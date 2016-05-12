using System;
using System.Management.Automation;
using ActiveCommerce.Extensions;
using Microsoft.Practices.Unity;

namespace ActiveCommerce.SitecorePowerShell.Cmdlets
{
    [Cmdlet("Get", "AcShopGeneralSettings")]
    public class GetAcShopGeneralSettings : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var shopContext = Sitecore.Ecommerce.Context.Entity.Resolve<Sitecore.Ecommerce.ShopContext>();
            WriteObject(shopContext.GetGeneralSettings());
        }
    }

    [Cmdlet("Get", "AcShopBusinessSettings")]
    public class GetAcShopBusinessSettings : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var shopContext = Sitecore.Ecommerce.Context.Entity.Resolve<Sitecore.Ecommerce.ShopContext>();
            WriteObject(shopContext.GetBusinessSettings());
        }
    }
}