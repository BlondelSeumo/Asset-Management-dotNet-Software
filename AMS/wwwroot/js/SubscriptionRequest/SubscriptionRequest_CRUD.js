var DetailsSubscriptionRequest = function (id) {
    var url = "/SubscriptionRequest/Details?id=" + id;
    $('#titleBigModal').html("Subscription Request Details");
    loadBigModal(url);
};


var AddEditSubscriptionRequest = function (id) {
    var url = "/SubscriptionRequest/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Subscription Request");
    }
    else {
        $('#titleBigModal').html("Add Subscription Request");
    }
    loadBigModal(url);
};

var SaveSubscriptionRequest = function () {
    if (!$("#frmSubscriptionRequest").valid()) {
        return;
    }

    var _frmSubscriptionRequest = $("#frmSubscriptionRequest").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SubscriptionRequest/AddEdit",
        data: _frmSubscriptionRequest,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblSubscriptionRequest').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteSubscriptionRequest = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/SubscriptionRequest/Delete?id=" + id,
                success: function (result) {
                    var message = "Subscription Request has been deleted successfully. Subscription Request ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblSubscriptionRequest').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};


var OpenSubscribe = function () {
    $('#titleBigModal').html("Subscribe for Next Update.");
    var url = "/SubscriptionRequest/OpenSubscribe";
    loadBigModal(url);
};

var SaveSubscribe = function () {
    if (!$("#frmSubscriptionRequest").valid()) {
        return;
    }
    var _Email = $("#Email").val();
    var _TimeZone = new Date();

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SubscriptionRequest/SaveSubscriptionRequest?_Email=" + _Email + "&_TimeZone= " + _TimeZone,
        //data: _frmSubscriptionRequest,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Subscribe");
                $('#btnSave').removeAttr('disabled');
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}