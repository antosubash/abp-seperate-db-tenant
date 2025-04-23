using System;
using System.Threading.Tasks;
using Acme.BookStore.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore.Web.Pages.CustomTenant
{
    public class CreateModel : BookStorePageModel
    {
        [BindProperty]
        public CustomTenantCreateDto TenantCreateDto { get; set; }

        private readonly ITenantAppService _customTenantAppService;

        public CreateModel(ITenantAppService customTenantAppService)
        {
            _customTenantAppService = customTenantAppService;
            TenantCreateDto = new CustomTenantCreateDto();
        }

        public virtual IActionResult OnGet()
        {
            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var tenantDto = new TenantCreateDto
            {
                Name = TenantCreateDto.Name,
                AdminEmailAddress = TenantCreateDto.AdminEmailAddress,
                AdminPassword = TenantCreateDto.AdminPassword,
            };
            
            var createdTenant = await _customTenantAppService.CreateAsync(tenantDto);
            
            var connectionString = TenantCreateDto.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                await _customTenantAppService.UpdateDefaultConnectionStringAsync(
                    createdTenant.Id,
                    connectionString
                );
            }
            
            return RedirectToPage("./Index");
        }
    }
}