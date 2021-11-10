using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Steeltoe.Discovery.Client;

namespace dw.UserService
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddDiscoveryClient(Configuration);
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                     builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //{
            //    //o.Authority = Configuration["Jwt:Authority"];
            //    //o.Audience = Configuration["Jwt:Audience"];
            //    o.Authority = "http://localhost:8080/auth/realms/Contactos";
            //    o.Audience = "ContactoClient";
            //    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidAudiences = new string[] { "master-realm", "account", "ContactoClient" }
            //    };
            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            c.NoResult();

            //            c.Response.StatusCode = 500;
            //            c.Response.ContentType = "text/plain";
            //            if (Environment.IsDevelopment())
            //            {
            //                return c.Response.WriteAsync(c.Exception.ToString());
            //            }
            //            return c.Response.WriteAsync("An error occured processing your authentication.");
            //        }
            //    };
            //    o.RequireHttpsMetadata = false;
            //    o.SaveToken = true;
            //    o.ForwardSignOut = "/SingOut";
            //    o.Validate();
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrators", policy => policy.RequireClaim("user_roles", "Administrators"));
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddOpenIdConnect(options =>
           {
               Configuration.GetSection("OpenIdConnect").Bind(options);
               options.SignInScheme = "Cookies";
               options.RequireHttpsMetadata = false; // dev only

               options.Authority = Configuration["Oidc:Authority"];
               options.ClientId = Configuration["Oidc:ClientId"];
               options.ClientSecret = Configuration["Oidc:ClientSecret"];

               options.ResponseType = OpenIdConnectResponseType.Code; //Configuration["Oidc:ResponseType"];
               options.UsePkce = true;
               options.Scope.Add("TEST");
               //options.GetClaimsFromUserInfoEndpoint = true;
               //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

               options.SaveTokens = true;
               //options.Scope.Add("openid");
               //options.Scope.Add("profile");
               //options.Scope.Add("email");
               //options.Scope.Add("claims");

               options.SaveTokens = true;
               //options.Events = new OpenIdConnectEvents
               //{
               //    OnTokenResponseReceived = async ctx =>
               //    {
               //        var a = ctx.Principal;
               //    },
               //    OnAuthorizationCodeReceived = async ctx =>
               //    {
               //        var a = ctx.Principal;
               //    }
               //};

               //options.TokenValidationParameters = new TokenValidationParameters
               //{
               //    NameClaimType = "name",
               //    RoleClaimType = "groups",
               //    ValidateIssuer = true
               //};
               //options.GetClaimsFromUserInfoEndpoint = true;
           });

            // access token: http://localhost:8080/auth/realms/master/protocol/openid-connect/auth?response_type=token&client_id=naos-sample&redirect_uri=https://localhost:5001/signin-oidc
            


            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
