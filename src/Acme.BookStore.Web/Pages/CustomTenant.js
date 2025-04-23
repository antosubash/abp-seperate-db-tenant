$(function () {
    const l = abp.localization.getResource('AbpTenantManagement');
    // Changed to use our custom service instead of the built-in tenant service
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
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpTenantManagement.Tenants.Update'),
                                action: function (data) {
                                    editTenant(data.record.id);
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('AbpTenantManagement.Tenants.Delete'),
                                confirmMessage: function (data) {
                                    return l('TenantDeletionConfirmationMessage', data.record.name);
                                },
                                action: function (data) {
                                    service.delete(data.record.id)
                                        .then(function () {
                                            dataTable.ajax.reload();
                                            abp.notify.success(l('SuccessfullyDeleted'));
                                        });
                                }
                            }
                        ]
                    }
                },
                {
                    title: l('TenantName'),
                    data: "name"
                }
            ]
        })
    );

    // Create Tenant
    $('#NewTenantButton').click(function (e) {
        e.preventDefault();
        $('#CreateTenantForm')[0].reset();
        $('#CreateTenantModal').modal('show');
    });

    $('#CreateTenantModal .save-button').click(function () {
        if (!$('#CreateTenantForm').valid()) {
            return;
        }
        
        const form = $('#CreateTenantForm').serializeFormToObject();
        
        service.create(form)
            .then(function () {
                $('#CreateTenantModal').modal('hide');
                dataTable.ajax.reload();
                abp.notify.success(l('SuccessfullyCreated'));
            });
    });

    // Edit Tenant
    function editTenant(id) {
        // Fix the Promise chain by ensuring proper Promise handling
        const getPromise = service.get(id);
        
        // Check if getPromise is a proper Promise with then method
        if (getPromise && typeof getPromise.then === 'function') {
            getPromise
                .then(function (result) {
                    $('#TenantUpdateDto_Id').val(result.id);
                    $('#TenantUpdateDto_Name').val(result.name);
                    $('#TenantUpdateDto_AdminEmailAddress').val(result.adminEmailAddress);
                    $('#TenantUpdateDto_ConnectionString').val(result.connectionString);
                    
                    $('#EditTenantModal').modal('show');
                })
                .catch(function(error) {
                    abp.notify.error("Error loading tenant data: " + error.message);
                });
        } else {
            // Alternative approach if service.get doesn't return a proper Promise
            try {
                const result = service.get(id);
                $('#TenantUpdateDto_Id').val(result.id);
                $('#TenantUpdateDto_Name').val(result.name);
                $('#TenantUpdateDto_AdminEmailAddress').val(result.adminEmailAddress);
                $('#TenantUpdateDto_ConnectionString').val(result.connectionString);
                
                $('#EditTenantModal').modal('show');
            } catch (error) {
                abp.notify.error("Error loading tenant data: " + error.message);
            }
        }
    }

    $('#EditTenantModal .save-button').click(function () {
        if (!$('#EditTenantForm').valid()) {
            return;
        }

        const form = $('#EditTenantForm').serializeFormToObject();
        
        service.update(form.id, form)
            .then(function () {
                $('#EditTenantModal').modal('hide');
                dataTable.ajax.reload();
                abp.notify.success(l('SuccessfullyUpdated'));
            });
    });
});