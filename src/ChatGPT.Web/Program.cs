var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------------------------------------
// Add services to the container.
// ----------------------------------------------------------------------------------------------------
// ----- Get Application Settings into objects
var jsonSettingsFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "applicationSettings.json");
builder.Configuration
  .AddJsonFile(jsonSettingsFile)
  .AddEnvironmentVariables()
  .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), true);
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
var settings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

// set the application title from the app settings
ChatGPT.Data.Constants.Initialize(settings);

//// TODO: change the aapplication logger to use a custom logger and/or Serilog...
//// https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/logging?view=aspnetcore-6.0
builder.Logging.SetMinimumLevel(LogLevel.Warning);
MyLogger.InitializeLogger(settings);

builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSingleton<AppSettings>(settings);

builder.Services.AddSingleton<ISimpleImageService>(new SimpleImageService(settings));
builder.Services.AddSingleton<ISimpleChatService>(new SimpleChatService(settings));
builder.Services.AddSingleton<IOKChatService>(new OKChatService(settings));
builder.Services.AddSingleton<IOKImageService>(new OKImageService(settings));

// ----- Configure Authentication ---------------------------------------------------------------------
var authSettings = builder.Configuration.GetSection("AzureAD");
var enableAuth = !string.IsNullOrEmpty(authSettings["TenantId"]);
if (enableAuth)
{
    // ----- Configure Authentication ---------------------------------------------------------------------
    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
      .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAD"));
    builder.Services.AddControllersWithViews()
      .AddMicrosoftIdentityUI();
    // ----- Configure Authorization ----------------------------------------------------------------------
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = options.DefaultPolicy;
    });
    // Add isAdmin claim and admin role membership attribute
    builder.Services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();
}

// ----- Configure Context Accessor -------------------------------------------------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();

//// ---- Add third party component startups ------------------------------------------------------------
builder.Services
    .AddBlazorise(options =>
    {
        // options.ChangeTextOnKeyPress = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();
builder.Services.AddSweetAlert2(options =>
{
    options.Theme = SweetAlertTheme.Default;
});
builder.Services.AddBlazoredLocalStorage();

// ----------------------------------------------------------------------------------------------------
// Configure application
// ----------------------------------------------------------------------------------------------------
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
// you can use this option to allow larger amounts of data to go through your signalrHub and pages and components
//     builder.Services.AddServerSideBlazor().AddHubOptions(
//       options => { options.MaximumReceiveMessageSize = 500000; });

// ----- Configure APIs -------------------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

//// If using Swagger, configure the API versioning properties of the project. 
//builder.Services.AddApiVersioningConfigured();

//// Add a Swagger generator and Automatic Request and Response annotations:
//if (settings.EnableSwagger)
//{
//    builder.Services.AddSwaggerGen(options =>
//    {
//        options.DocumentFilter<CustomSwaggerFilter>();
//        options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
//        // See https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments
//        var documentationPath = Path.Combine(AppContext.BaseDirectory, "DadABase.Web.xml");
//        options.IncludeXmlComments(documentationPath);
//        options.SwaggerDoc("v1", new OpenApiInfo
//        {
//            Version = "v1",
//            Title = "The Dad-A-Base",
//            Description = "An ASP.NET Core Web API for storing Dad Mys",
//            TermsOfService = new Uri("http://luppes.com/privacy"),
//            License = new OpenApiLicense
//            {
//                Name = "License",
//                Url = new Uri("http://luppes.com/license")
//            }
//        });
//    });
//    // apply a filter to remove things like the auto-generated MicrosoftIdentity APIs
//    builder.Services.AddSwaggerGen(gen =>
//    {
//    });
//}

// ----------------------------------------------------------------------------------------------------
// Start up application
// ----------------------------------------------------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//if (settings.EnableSwagger)
//{
//    // Enable middleware to serve the generated OpenAPI definition as JSON files.
//    app.UseSwagger();
//    // Navigate to: https://localhost:<port>/swagger/v1/swagger.json
//    app.UseSwaggerUI();
//}

//// NOTE: when doing Docker and Azure Container Services, the authentication redirect is failing and is redirecting to
//// 'HTTP', not 'HTTPS', which causes the auth to fail.
//// See https://stackoverflow.com/questions/53353601/redirect-after-authentication-is-to-http-when-it-should-be-https
//// See https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-2.1
//app.Use((context, next) =>
//{
//    context.Request.Scheme = "https";
//    return next();
//});
//app.UseForwardedHeaders();

// required if you want to use html, css, or image files...
app.UseStaticFiles();
app.UseHttpsRedirection();

// routing matches HTTP request and dispatches them to proper endpoings
app.UseRouting();

if (enableAuth)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
