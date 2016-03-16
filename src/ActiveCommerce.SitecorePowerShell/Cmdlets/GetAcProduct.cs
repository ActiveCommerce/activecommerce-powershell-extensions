using System;
using System.Linq;
using System.Management.Automation;
using Sitecore.Ecommerce.DomainModel.Products;
using Microsoft.Practices.Unity;

namespace ActiveCommerce.SitecorePowerShell.Cmdlets
{
    [Cmdlet("Get", "AcProduct")]
    public class GetAcProduct : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Code { get; set; }

        [Parameter]
        public string TypeName { get; set; }

        public GetAcProduct()
        {
            TypeName = typeof(Sitecore.Ecommerce.DomainModel.Products.ProductBaseData).FullName;
        }

        protected override void ProcessRecord()
        {
            Type type = null;
            if (!LanguagePrimitives.TryConvertTo<Type>(TypeName, out type))
            {
                throw new ArgumentException("TypeName", string.Format("{0} is not a valid type", TypeName));
            }
            var productRepository = Sitecore.Ecommerce.Context.Entity.Resolve<IProductRepository>();
            var getMethod = productRepository.GetType().GetMethod("Get", new[] { typeof(string) });
            var genericMethod = getMethod.MakeGenericMethod(type);
            var product = genericMethod.Invoke(productRepository, new object[] { Code });
            WriteObject(product);
        }
    }
}