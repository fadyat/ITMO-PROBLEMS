var originalTextBoxValue = null;
$(".form-control").dblclick(function (){

    originalTextBoxValue = $(this).val();
    if($(this).parent().parent().parent().parent().attr("name") !== "Id")
        $(this).attr("readonly", false);
});
$(".form-control").keyup(function (e) {
    if (e.keyCode === 13) {
        $(this).blur();
    }
});
$(".form-control").blur(function () {
    var $this = $(this);
    var ajaxdiv = $this.parent().parent().parent().find(".ajaxdivtd");
    var newName = $this.val();
    var id = $this.parent().parent().parent().parent().parent().attr("id")/*.substring(3);*/
    var url = "/Task/ChangeData";
    var inputName = $this.parent().parent().parent().parent().attr("name")
    if (newName.length < 3) {
        alert("Name must be at least 3 characters long.");
        $this.attr("readonly", true);
        $this.val(originalTextBoxValue);
        originalTextBoxValue = null;
        return false;
    }

    $.post(url, { newName: newName, id: id, inputName : inputName }, function (data) {
        var response = data.trim();
        if (originalTextBoxValue == null || newName === originalTextBoxValue){

        }
        else if (response === "invalidData"){
            $this.val(originalTextBoxValue);
            ajaxdiv.html("<div class='alert alert-danger'>Invalid data</div>").show();
        }
        else {
            ajaxdiv.html("<div class='alert alert-success'>Saved!</div>").show();
        }

        originalTextBoxValue = null;
        if (inputName === "Status"){
            $this.attr("value", newName);
            var newDataStatus;
            if(newName == "Open"){
                newDataStatus = 1;
            }
            else if(newName == "Active"){
                newDataStatus = 2
            }
            else if(newDataStatus == "Resolved"){
                newDataStatus = 3;
            }
            $this.parent().parent().parent().parent().parent().attr("data-status", newDataStatus);
            ChangeColor();
        }
        setTimeout(function () {
            ajaxdiv.fadeOut("fast", function () {
                ajaxdiv.html("");
            });
        }, 3000);
    }).done(function () {
        $this.attr("readonly", true);
    });
});
