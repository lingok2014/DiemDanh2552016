/// <reference path="myScript.js" />

$(function () {
    $.ajaxSetup({ cache: false });//khong lu cache
    $("a[data-modal]").on("click", function (e) {// tao su kien click cho cac thuoc tinh a[data-modal]
        $('#myModalContent').load(this.href, function () {//load dia chi trong href
            $('#myModal').modal({
                keyboard: true
            }, 'show');//hien thi modal tai #myModal
            bindForm(this);//submit du lieu
        });
        return false; //khong load lai form
    });
});
function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.Success) {
                    $('#myModal').modal('hide');
                    $('#progress').hide();
                    location.reload(result);
                }
                else {
                    $('#progress').hide();
                    if (result.Message != null) {
                        alert(result.Message);
                    }
                }
            }
        });
        return false;
    });

}

