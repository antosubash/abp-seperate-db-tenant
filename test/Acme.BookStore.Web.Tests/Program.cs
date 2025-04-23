using Microsoft.AspNetCore.Builder;
using Acme.BookStore;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Acme.BookStore.Web.csproj"); 
await builder.RunAbpModuleAsync<BookStoreWebTestModule>(applicationName: "Acme.BookStore.Web");

public partial class Program
{
}
