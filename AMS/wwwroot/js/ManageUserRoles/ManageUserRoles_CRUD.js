var Details = function (id) {
    var url = "/ManageUserRoles/Details?id=" + id;
    $('#titleExtraBigModal').html("Role Details");
    loadExtraBigModal(url);
};


var AddEdit = function (id) {
    var url = "/ManageUserRoles/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Role");
    }
    else {
        $('#titleExtraBigModal').html("Add Role");
    }
    loadExtraBigModal(url);
};

var Save = function () {
    if (!$("#frmUserRoles").valid()) {
        return;
    }

    var _frmUserRoles = $("#frmUserRoles").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/AddEdit",
        data: _frmUserRoles,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblManageUserRoles').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
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
                type: "POST",
                url: "/ManageUserRoles/Delete?id=" + id,
                success: function (result) {
                    var message = "Role has been deleted successfully. Role ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblManageUserRoles').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};