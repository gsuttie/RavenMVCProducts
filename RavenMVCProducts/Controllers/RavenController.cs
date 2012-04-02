using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RavenMVCProducts.Controllers
{
    using Raven.Client;
    using Raven.Client.Document;

    public class RavenController : Controller
    {
        public IDocumentSession _session { get; private set; }

        public const int DefaultPage = 1;
        public const int PageSize = 10;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;
            _session = MvcApplication.Store.OpenSession();
            base.OnActionExecuting(filterContext);

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (_session)
            {
                if (filterContext.Exception != null)
                    return;

                if (_session != null)
                    _session.SaveChanges();
            }
        }

        protected int CurrentPage
        {
            get
            {
                var s = Request.QueryString["page"];
                int result;
                if (int.TryParse(s, out result))
                    return Math.Max(DefaultPage, result);
                return DefaultPage;
            }
        }
    }
}
