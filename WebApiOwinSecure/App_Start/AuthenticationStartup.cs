using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApiOwinSecure.App_Start.AuthenticationStartup))]

namespace WebApiOwinSecure.App_Start
{
    public class AuthenticationStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Uygulamanızı nasıl yapılandıracağınız hakkında daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=316888 adresini ziyaret edin
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var myProvider = new APIAUTHORIZATIONSERVERPROVIDER();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"), // token alinacak url
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), //token gecerlilik suresi
                Provider = myProvider 
            };
            //AppBuilder' a token üretimini gerçekleştirmek için ilgili authorization ayarlari verilir.
            app.UseOAuthAuthorizationServer(options);
            //Yetkilendirme tipi olarak Bearer Auth. kullanacagimizi belirtiyoruz
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
