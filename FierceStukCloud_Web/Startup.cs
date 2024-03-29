using FierceStukCloud_Web.Data;
using FierceStukCloud_Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;



namespace FierceStukCloud_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           // ��������, ����� �� �������������� �������� ��� ��������� ������
                           ValidateIssuer = true,
                           // ������, �������������� ��������
                           ValidIssuer = AuthOptions.ISSUER,

                           // ����� �� �������������� ����������� ������
                           ValidateAudience = true,
                           // ��������� ����������� ������
                           ValidAudience = AuthOptions.AUDIENCE,
                           // ����� �� �������������� ����� �������������
                           ValidateLifetime = true,

                           // ��������� ����� ������������
                           IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                           // ��������� ����� ������������
                           ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddDbContext<FierceStukCloudDbContext>(o
                =>o.UseSqlServer(@"Data Source=88.135.50.215,1433; Initial Catalog=FSC_Data;
                                    User ID=Ivan; Password=789xxx44XX; Connect Timeout=30;
                                    Encrypt=False; TrustServerCertificate=False;
                                    ApplicationIntent=ReadWrite; MultiSubnetFailover=False"));
            //services.AddControllers(); //.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSignalR(o =>
            {
                o.MaximumReceiveMessageSize = 1048576;
            });
            services.AddMvc();
           
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            //var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddConsole();
            //});
            //ILogger logger = loggerFactory.CreateLogger<Startup>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<FierceStukCloudHub>("/hub", options => {
                    options.ApplicationMaxBufferSize = 1024;
                    options.TransportMaxBufferSize = 1024;               
                });
            });
        }
    }
}
