using System.Web;
using System.Web.Mvc;

namespace Postmen.Sender.Api.Framework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
