using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TutoRealBF;
using TutoRealBL;
using TutoRealCommon;
using TutoRealDA;
using TutoRealDA.Master;
using TutoRealDA.StandBy;
using TutoRealIF;

var builder = WebApplication.CreateBuilder(args);

//設定ファイル(appsettings.json)から設定値を取得
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// 依存関係の設定
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TutoRealDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString(nameof(TutoRealDbContext)));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<TutoRealDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache();// セッションの状態をメモリ内に保存

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
}).AddCookie("Cookies", options =>
{
    options.LoginPath = "/Login/Index"; // ログインページのパスを指定
    options.AccessDeniedPath = "/Home/Error"; // アクセス拒否ページのパスを指定
    // その他のオプション...
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // セッションが30分でタイムアウトするように設定
    options.Cookie.HttpOnly = true; // クライアントサイドのスクリプトからアクセス不可にする
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPSを使用している場合にのみ送信されるようにする
    options.Cookie.IsEssential = true; // GDPRにおいて必須とする
});

// IDbConnection を DI に登録する
builder.Services.AddScoped<IDbConnection>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString(CommonConst.DBCONTEXT);
    return new SqlConnection(connectionString);
});

// Add services to the container.
builder.Services.AddRazorPages();

// 依存関係の追加
builder.Services.AddScoped<ITutoRealBaseBF, TutoRealBaseBF>();
builder.Services.AddScoped<ITutoRealBaseIF, TutoRealBaseBL>();
builder.Services.AddScoped<TutoRealBaseDA, StandByDA>();
builder.Services.AddScoped<TutoRealBaseDA, MasterDA>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// セッションミドルウェアを追加する
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=login}/{action=login}/{id?}");
//pattern: "{controller=StandBy}/{action=Index}/{id?}");
//pattern: "{controller=Book}/{action=Index}/{id?}");

// エンドポイントの設定...
app.MapRazorPages();

app.Run();