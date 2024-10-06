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

//�ݒ�t�@�C��(appsettings.json)����ݒ�l���擾
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// �ˑ��֌W�̐ݒ�
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TutoRealDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString(nameof(TutoRealDbContext)));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<TutoRealDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache();// �Z�b�V�����̏�Ԃ����������ɕۑ�

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
}).AddCookie("Cookies", options =>
{
    options.LoginPath = "/Login/Index"; // ���O�C���y�[�W�̃p�X���w��
    options.AccessDeniedPath = "/Home/Error"; // �A�N�Z�X���ۃy�[�W�̃p�X���w��
    // ���̑��̃I�v�V����...
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // �Z�b�V������30���Ń^�C���A�E�g����悤�ɐݒ�
    options.Cookie.HttpOnly = true; // �N���C�A���g�T�C�h�̃X�N���v�g����A�N�Z�X�s�ɂ���
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS���g�p���Ă���ꍇ�ɂ̂ݑ��M�����悤�ɂ���
    options.Cookie.IsEssential = true; // GDPR�ɂ����ĕK�{�Ƃ���
});

// IDbConnection �� DI �ɓo�^����
builder.Services.AddScoped<IDbConnection>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString(CommonConst.DBCONTEXT);
    return new SqlConnection(connectionString);
});

// Add services to the container.
builder.Services.AddRazorPages();

// �ˑ��֌W�̒ǉ�
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

// �Z�b�V�����~�h���E�F�A��ǉ�����
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=login}/{action=login}/{id?}");
//pattern: "{controller=StandBy}/{action=Index}/{id?}");
//pattern: "{controller=Book}/{action=Index}/{id?}");

// �G���h�|�C���g�̐ݒ�...
app.MapRazorPages();

app.Run();