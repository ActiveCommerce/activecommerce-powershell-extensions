using System;
using System.Management.Automation;
using ActiveCommerce.ShopContext;

namespace ActiveCommerce.SitecorePowerShell.Cmdlets
{
    [Cmdlet("Switch", "AcShopContext")]
    public class SwitchAcShopContext : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string Site { get; set; }

        [Parameter]
        public string Database { get; set; }

        [Parameter(Mandatory = true)]
        public ScriptBlock ScriptBlock { get; set; }

        public SwitchAcShopContext()
        {
            Database = "master";
        }

        protected override void ProcessRecord()
        {
            var site = Sitecore.Sites.SiteContextFactory.GetSiteContext(Site);
            if (site == null)
            {
                throw new ArgumentException(string.Format("Couldn't find site {0}", Site));
            }
            var database = Sitecore.Data.Database.GetDatabase(Database);
            if (database == null)
            {
                throw new ArgumentException(string.Format("Couldn't find database {0}", Database));
            }
            using (new ShopContextSwitcher(site, database))
            {
                ScriptBlock.Invoke();
            }
        }
    }
}