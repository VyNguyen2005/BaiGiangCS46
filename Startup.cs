using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {

    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints((endpoints)=>
        {
            endpoints.MapGet("/", async (context)=>
            {
                
                var menu = HtmlHelper.MenuTop(
                    HtmlHelper.DefaultMenuTopItems()

                    , context.Request
                );
                var html = HtmlHelper.HtmlDocument("XIN CHAO", menu + HtmlHelper.HtmlTrangchu());

                await context.Response.WriteAsync(html);
            });
            endpoints.MapGet("/abc.html", async (context)=>
            {
                await context.Response.WriteAsync("Trang gioi thieu");
            });
            endpoints.MapGet("/RequestInfo", async (context)=>
            {
                var menu = HtmlHelper.MenuTop(
                    HtmlHelper.DefaultMenuTopItems()

                    , context.Request
                );
                
                var info = RequestProcess.RequestInfo(context.Request).HtmlTag("div", "container");

                var html = HtmlHelper.HtmlDocument("Thong tin Request", menu + info );

                await context.Response.WriteAsync(html);

            });
            endpoints.MapGet("/Encoding", async (context)=>
            {
                await context.Response.WriteAsync("Encoding");
            });
            endpoints.MapGet("/Cookies/{*action}", async (context)=>
            {
                var menu = HtmlHelper.MenuTop(
                    HtmlHelper.DefaultMenuTopItems(),
                    context.Request
                );

                var action = context.GetRouteValue("action")??"read";
                string message = " ";
                if(action.ToString() == "write")
                {
                    var option = new CookieOptions()
                    {
                        Path ="/",
                        Expires = DateTime.Now.AddDays(1)
                    };
                    context.Response.Cookies.Append("masanpham", "3756964", option);
                    message = "Cookie duoc ghi";

                }
                else
                {
                    var listcokie = context.Request.Cookies.Select((header) => $"{header.Key}: {header.Value}".HtmlTag("li"));
                    message = string.Join("", listcokie).HtmlTag("ul");
                    
                    
                }

                var huongdan = "<a class=\"btn btn-danger\" href=\"/Cookies/read\"> Doc Cookie </a><a class=\"btn btn-success\" href=\"/Cookies/write\"> Ghi Cookie </a>";
                huongdan = huongdan.HtmlTag("div", "container mt-4");
                message = message.HtmlTag("div", "alert alert-danger");

                var html = HtmlHelper.HtmlDocument("Cookies: " + action, menu + huongdan + message);


                await context.Response.WriteAsync(html);
            });
            endpoints.MapGet("/Json", async (context)=>
            {

                var p = new {
                    TenSP = "Dong Ho ABC",
                    Gia = 500000,
                    NgaySX = new DateTime(2023, 12, 31)
                };

                context.Response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(p);

                await context.Response.WriteAsync(json);
                
            });
            endpoints.MapMethods("/Form", new string[] { "POST", "GET"}, async (context)=>
            {
                var menu = HtmlHelper.MenuTop(
                    HtmlHelper.DefaultMenuTopItems()

                    , context.Request
                );
                
                var formhtml = RequestProcess.ProcessForm(context.Request);

                var html = HtmlHelper.HtmlDocument("Test submit form html", menu +formhtml);

                await context.Response.WriteAsync(html);
                
                
                
            });
            
        });
        app.Map("/admin", (IApplicationBuilder app1)=>
        {
            app1.UseRouting();
            app1.UseEndpoints((endpoints)=>
            {
                endpoints.MapGet("/product", async (context)=>
                {
                    await context.Response.WriteAsync("Trang san pham");
                });
            });
            app1.Run(async (context)=>
            {
                await context.Response.WriteAsync("Trang thong tin");
            });
        });

    }
}