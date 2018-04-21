using System.Web;
using System.Web.Mvc;

namespace Guaflix_1104017_1169317
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
