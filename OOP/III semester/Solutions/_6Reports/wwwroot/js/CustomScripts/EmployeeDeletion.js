$(function (){
    $("a.delete").click(function (){
        if (!confirm("Confirm employee deletion")) return false;
    });
});