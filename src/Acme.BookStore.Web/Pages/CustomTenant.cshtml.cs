using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore.Web.Pages
{
    public class CustomTenantModel : BookStorePageModel
    {
        [BindProperty]
        public CustomTenantCreateDto TenantCreateDto { get; set; }

        [BindProperty]
        public CustomTenantUpdateDto TenantUpdateDto { get; set; }

        public List<CustomTenantDto> Tenants { get; set; }

        private readonly ITenantAppService _customTenantAppService;

        private readonly ITenantRepository _tenantRepository;

        public CustomTenantModel(
            ITenantAppService customTenantAppService,
            ITenantRepository tenantRepository
        )
        {
            _customTenantAppService = customTenantAppService;
            _tenantRepository = tenantRepository;
            TenantCreateDto = new CustomTenantCreateDto();
            TenantUpdateDto = new CustomTenantUpdateDto();
            Tenants = new List<CustomTenantDto>();
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var tenantList = await _tenantRepository.GetListAsync(maxResultCount: 50);
            Tenants = tenantList
                .Select(static tenant => new CustomTenantDto { Id = tenant.Id, Name = tenant.Name })
                .ToList();
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
            return RedirectToPage();
        }

        public virtual async Task<IActionResult> OnPostUpdateAsync()
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
            return RedirectToPage();
        }

        public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _customTenantAppService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
