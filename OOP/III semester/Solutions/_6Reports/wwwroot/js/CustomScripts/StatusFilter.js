$(function () {

    $("#StatusFilter").on("change", function () {
        var url = $(this).val();

        if (url) {
            window.location = "/Task/List?filterStatusId=" + url;
        }
        return false;
    });
});