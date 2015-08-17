using System.Collections.Specialized;
using System.Linq;
using System.Management.Automation;
using ActiveCommerce.Extensions;

namespace ActiveCommerce.SitecorePowerShell.Cmdlets
{
    [Cmdlet("Get", "AcSites")]
    public class GetAcSites : Cmdlet
    {
        [Parameter]
        public SwitchParameter Dictionary { get; set; }

        protected override void ProcessRecord()
        {
            var sites = Sitecore.Sites.SiteManager.GetSites().EcommerceOnly().Select(x => x.Name);
            if (SwitchParameter.Present)
            {
                var dict = new OrderedDictionary();
                foreach (var site in sites)
                {
                    dict.Add(site, site);
                }
                WriteObject(dict);
            }
            else
            {
                WriteObject(sites);
            }
        }
    }
}