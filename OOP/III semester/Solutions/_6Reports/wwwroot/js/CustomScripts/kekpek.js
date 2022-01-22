var
    statusFilter, employeeFilter

function updateFilters() {
    $('.task-list-row').hide().filter(function() {
        var
            self = $(this),
            result = true; // not guilty until proven guilty
        
        /*alert(self.data('status'));*/
        if (employeeFilter && (employeeFilter != 'All')) {
            result = result && employeeFilter == self.data('employee');
        }
        if (statusFilter && (statusFilter != 0)) {
            result = result && statusFilter == self.data('status');
        }
        /*alert(statusFilter == self.data('status'));
        alert("!!! " + employeeFilter == self.data('employee'))*/
        //alert(statusFilter + " " + employeeFilter + " " + self.data('status') + " " + self.data("employee") + " "/* + statusFilter === self.data('status')*/);
        return result;
    }).show();
}

// Assigned User Dropdown Filter
$('#StatusFilter').on('change', function() {
    statusFilter = this.value;
    updateFilters();
});

// Task Status Dropdown Filter
$('#EmployeeFilter').on('change', function() {
    employeeFilter = this.value;
    updateFilters();
});


/*
future use for a text input filter
$('#search').on('click', function() {
    $('.box').hide().filter(function() {
        return $(this).data('order-number') == $('#search-criteria').val().trim();
    }).show();
});*/