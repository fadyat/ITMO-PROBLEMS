$(function () {

    $("#EmployeeFilter").on("change", function () {
        var url = $(this).val();

        if (url) {
            window.location = "/Task/List?filterEmployee=" + url;
        }
        return false;
    });
});