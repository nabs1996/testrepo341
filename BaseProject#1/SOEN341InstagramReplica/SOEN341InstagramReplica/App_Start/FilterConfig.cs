using System.Web;
using System.Web.Mvc;

namespace SOEN341InstagramReplica
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
