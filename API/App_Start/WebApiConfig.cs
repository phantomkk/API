using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ); 
            config.Routes.MapHttpRoute(
               name: "GetProductByCode",
               routeTemplate: "api/products/code/{code}",
               defaults: new { controller = "products" }
           );
            config.Routes.MapHttpRoute(
               name: "SearchProduct",
               routeTemplate: "api/products/name/{name}",
               defaults: new { controller = "products" }
           );
            config.Routes.MapHttpRoute(
              name: "GetProductComments",
              routeTemplate: "api/products/{id}/comments",
              defaults: new { controller = "products" }
          ); config.Routes.MapHttpRoute(
               name: "GetMoreProducts",
               routeTemplate: "api/products/{index}/{size}",
               defaults: new { controller = "products" }
           );
            config.Routes.MapHttpRoute(
              name: "GetCategoryProducts",
              routeTemplate: "api/categories/{id}/products",
              defaults: new { controller = "categories" }
          );


            config.Routes.MapHttpRoute(
              name: "Login",
              routeTemplate: "api/users/login/",
              defaults: new { controller = "users" }
          );

            config.Routes.MapHttpRoute(
              name: "Register",
              routeTemplate: "api/users/register/",
              defaults: new { controller = "users" }
          );
            config.Routes.MapHttpRoute(
              name: "UploadAvatar",
              routeTemplate: "api/upload/image/user/",
              defaults: new { controller = "Upload" }
          ); 
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

        }
    }
}
