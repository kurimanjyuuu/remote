using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 他のサービスの設定...

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login/Index"; // ログインページのパスを指定
                options.AccessDeniedPath = "/Home/Error"; // アクセス拒否ページのパスを指定
                // その他のオプション...
            });

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // セッションが30分でタイムアウトするように設定
            options.Cookie.HttpOnly = true; // クライアントサイドのスクリプトからアクセス不可にする
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPSを使用している場合にのみ送信されるようにする
            options.Cookie.IsEssential = true; // GDPRにおいて必須とする
        });

        services.AddControllersWithViews();
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

        app.UseSession(); // セッションミドルウェアを追加する

        app.UseAuthentication(); // 認証ミドルウェアを追加する
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}"); // ログインページへのマッピング
        });

    }
}