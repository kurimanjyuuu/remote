using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 認証サービスの設定
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login/Index"; // ログインページのパス
                options.AccessDeniedPath = "/Home/Error"; // アクセス拒否ページのパス
            });

        // セッション管理の設定
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // セッションのタイムアウト設定
            options.Cookie.HttpOnly = true; // クライアントからのスクリプトアクセスを禁止
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPSでの送信を強制
            options.Cookie.IsEssential = true; // GDPR対応
        });

        // コントローラーとJSONオプションの設定
        services.AddControllersWithViews().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null; // プロパティ名のポリシーを無効
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseSession(); // セッションミドルウェアを追加
        app.UseAuthentication(); // 認証ミドルウェアを追加
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}"); // デフォルトルートの設定
        });
    }
}
