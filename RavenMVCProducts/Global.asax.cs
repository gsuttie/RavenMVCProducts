using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using MvcMiniProfiler;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using RavenMVCProducts.Indexes;
using RavenMVCProducts.Models;
using RavenMVCProducts.ViewModels;

namespace RavenMVCProducts
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        public static IDocumentStore Store { get; set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{*id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Store = new DocumentStore{ConnectionStringName = "RavenDB", DefaultDatabase = "Products"};
            Store.Initialize();

            Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(Store);
            MvcMiniProfiler.RavenDb.Profiler.AttachTo((DocumentStore)Store);


            //Move this to a config file later
            Mapper.CreateMap<Product, ProductViewModel>()
               .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
               .ForMember(x => x.CategoryId, o => o.MapFrom(m => m.CategoryId))
               .ForMember(x => x.SupplierId, o => o.MapFrom(m => m.SupplierId))
               .ForMember(x => x.Name, o => o.MapFrom(m => m.Name))
               .ForMember(x => x.Code, o => o.MapFrom(m => m.Code))
               .ForMember(x => x.StandardCost, o => o.MapFrom(m => m.StandardCost))
               .ForMember(x => x.ListPrice, o => o.MapFrom(m => m.ListPrice))
               .ForMember(x => x.UnitsOnStock, o => o.MapFrom(m => m.UnitsOnStock))
               .ForMember(x => x.UnitsOnOrder, o => o.MapFrom(m => m.UnitsOnOrder))
               .ForMember(x => x.Discontinued, o => o.MapFrom(m => m.Discontinued));

            TryCreatingIndexesOrRedirectToErrorPage();
        }


        private static void TryCreatingIndexesOrRedirectToErrorPage()
        {
            try
            {
                IndexCreation.CreateIndexes(typeof(Indexes.Indexes.ProductsByAll).Assembly, Store);
            }
            catch (WebException e)
            {
                var socketException = e.InnerException as SocketException;
                if (socketException == null)
                    throw;

                switch (socketException.SocketErrorCode)
                {
                    case SocketError.AddressNotAvailable:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                    case SocketError.TimedOut:
                    case SocketError.ConnectionRefused:
                    case SocketError.HostDown:
                    case SocketError.HostUnreachable:
                    case SocketError.HostNotFound:
                        HttpContext.Current.Response.Redirect("~/RavenNotReachable.htm");
                        break;
                    default:
                        throw;
                }
            }
        }
    }
}