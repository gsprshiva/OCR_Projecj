using avenir.invoicescanning.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IScannerApi, AwsScannerApi>(p => new AwsScannerApi(new HttpClient()
{
    BaseAddress = new Uri("https://ifurpuubmpboc4wkav5hxys3vm0suyiu.lambda-url.ap-south-1.on.aws/")
}));

builder.Services.AddTransient<AmazonS3Service>(p =>
    new AmazonS3Service(
        builder.Configuration["AWSS3Setting:AccessKey"],
        builder.Configuration["AWSS3Setting:SecretKey"],
        builder.Configuration["AWSS3Setting:BucketName"],
        builder.Configuration["AWSS3Setting:DefaultFolder"],
        builder.Configuration["AWSS3Setting:FileUrlFormat"]
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
