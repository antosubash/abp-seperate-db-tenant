using System;
using System.Threading.Tasks;
using Acme.BookStore.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore.Web.Pages.CustomTenant
{
    public class EditModel : BookStorePageModel
    {
        [BindProperty]
        public CustomTenantUpdateDto TenantUpdateDto { get; set; }

        private readonly ITenantAppService _customTenantAppService;

        public EditModel(ITenantAppService customTenantAppService)
        {
            _customTenantAppService = customTenantAppService;
            TenantUpdateDto = new CustomTenantUpdateDto();
        }

        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            var tenantDto = await _customTenantAppService.GetAsync(id);
            
            TenantUpdateDto = new CustomTenantUpdateDto
            {
                Id = tenantDto.Id,
                Name = tenantDto.Name,
            };

            var connectionString = await _customTenantAppService.GetDefaultConnectionStringAsync(id);
            TenantUpdateDto.ConnectionString = connectionString;
            
            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var tenantDto = new TenantUpdateDto { Name = TenantUpdateDto.Name };
            await _customTenantAppService.UpdateAsync(TenantUpdateDto.Id, tenantDto);

            var connectionString = TenantUpdateDto.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                await _customTenantAppService.UpdateDefaultConnectionStringAsync(
                    TenantUpdateDto.Id,
                    connectionString
                );
            }
            
            return RedirectToPage("./Index");
        }
    }
}