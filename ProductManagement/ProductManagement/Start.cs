using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Configration;
using ProductManagement.Data;
using ProductManagement.Services;
using System.Data.SqlClient;
using System.Text;
using System.Text.Encodings.Web;


namespace ProductManagement
{
    public class Startup
    {
        #region Fields.
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration _configuration { get; }
        #endregion

        #region Ctor.
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            });

            var dbOptions = _configuration.GetOptions<DatabaseSettings>(DatabaseSettings.SectionName);
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(new SqlConnection(dbOptions.ConnectionString)), ServiceLifetime.Scoped);

            services.AddScoped<DbContext, MyDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));
            services.AddScoped(typeof(IDbHelperSevices), typeof(DbHelperSevices));


            services.AddResponseCaching();
            var jwtOptions = _configuration.GetOptions<JwtSettings>(JwtSettings.SectionName);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthorization();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IHttpContextAccessor httpContextAccessor)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseResponseCaching();
            app.UseAuthentication();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints .MapControllerRoute(
                            name: "Default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapGet("/", () =>
                {
                    return "Welcome Api Project";
                });
            });
        }

    }
}
