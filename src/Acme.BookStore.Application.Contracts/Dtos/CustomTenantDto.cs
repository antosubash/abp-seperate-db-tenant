using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Dtos
{
    public class CustomTenantDto : EntityDto<Guid>
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [EmailAddress]
        [StringLength(256)]
        public string AdminEmailAddress { get; set; }

        public string AdminPassword { get; set; }

        public List<KeyValuePair<string, string>> ConnectionStrings { get; set; }
    }

    public class CustomTenantCreateDto
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string AdminEmailAddress { get; set; }

        [Required]
        [StringLength(128)]
        public string AdminPassword { get; set; }

        [StringLength(1024)]
        public string ConnectionString { get; set; }
    }

    public class CustomTenantUpdateDto : EntityDto<Guid>
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [EmailAddress]
        [StringLength(256)]
        public string AdminEmailAddress { get; set; }

        [StringLength(1024)]
        public string ConnectionString { get; set; }
    }

    public class CustomTenantGetListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}