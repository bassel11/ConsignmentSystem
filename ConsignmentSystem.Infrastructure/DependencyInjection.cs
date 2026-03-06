using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Domain.Repositories;
using ConsignmentSystem.Infrastructure.Identity;
using ConsignmentSystem.Infrastructure.Pdf;
using ConsignmentSystem.Infrastructure.Persistence;
using ConsignmentSystem.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ConsignmentSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. إعداد الـ DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IConsignmentRepository, ConsignmentRepository>();
            services.AddScoped<IConsignmentItemRepository, ConsignmentItemRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IReceiptPdfGenerator, ReceiptPdfGenerator>();
            services.AddDataProtection();

            // 2. إعداد محرك الهوية مع تطبيق معايير الأمان للأنظمة المالية
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // إعدادات كلمة المرور الصارمة
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // إعدادات الحظر لحماية النظام من هجمات التخمين (Brute Force)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // اشتراط فرادة الإيميل
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<ApplicationRole>() // استخدام الكلاس المخصص
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); // الآن ستعمل بدون أخطاء بفضل -FrameworkReference

            // 💡 تسجيل IIdentityService
            services.AddScoped<IIdentityService, IdentityService>();

            // 💡 إعدادات الـ JWT Bearer Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // اجعله true في بيئة الإنتاج
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // إلغاء وقت السماح الإضافي لتنتهي صلاحية التوكن بدقة
                };
            });



            return services;
        }
    }
}