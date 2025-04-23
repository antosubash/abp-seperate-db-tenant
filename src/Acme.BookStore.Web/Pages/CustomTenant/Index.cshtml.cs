using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore.Web.Pages.CustomTenant
{
    public class IndexModel : BookStorePageModel
    {
        public List<CustomTenantDto> Tenants { get; set; }

        private readonly ITenantAppService _customTenantAppService;
        private readonly ITenantRepository _tenantRepository;

        public IndexModel(
            ITenantAppService customTenantAppService,
            ITenantRepository tenantRepository
        )
        {
            _customTenantAppService = customTenantAppService;
            _tenantRepository = tenantRepository;
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
    }
}