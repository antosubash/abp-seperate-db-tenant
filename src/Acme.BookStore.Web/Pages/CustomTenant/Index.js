$(function () {
    // Using custom service instead of the built-in tenant service
    const service = volo.abp.tenantManagement.tenant;
    
    const dataTable = $('#TenantsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(service.getList),
            columnDefs: [
                {
                    title: 'Actions',
                    rowAction: {
                        items: [
                            {
                                text: 'Edit',
                                visible: abp.auth.isGranted('AbpTenantManagement.Tenants.Update'),
                                action: function (data) {
                                    window.location.href = `/CustomTenant/Edit/${data.record.id}`;
                                }
                            },
                            {
                                text: 'Delete',
                                visible: abp.auth.isGranted('AbpTenantManagement.Tenants.Delete'),
                                action: function (data) {
                                    window.location.href = `/CustomTenant/Delete/${data.record.id}`;
                                }
                            }
                        ]
                    }
                },
                {
                    title: 'Tenant Name',
                    data: "name"
                }
            ]
        })
    );

    // Create Tenant button
    $('#NewTenantButton').click(function (e) {
        e.preventDefault();
        window.location.href = '/CustomTenant/Create';
    });
});