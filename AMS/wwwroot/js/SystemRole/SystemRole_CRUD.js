var AddNewRole = function () {
    var url = "/SystemRole/AddNewRole";
    $('#titleBigModal').html("Add New Role");
    loadBigModal(url);
};

var SaveAddNewRole = function () {
    if (!$("#frmAddNewRole").valid()) {
        return;
    }

    var _frmAddNewRole = $("#frmAddNewRole").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SystemRole/SaveAddNewRole",
        data: _frmAddNewRole,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    $('#tblSystemRole').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#RoleName').focus();
                        $("#btnSave").val("Save");
                        $('#btnSave').removeAttr('disabled');
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var DeleteRole = function (button) {
    var row = $(button).closest("TR");
    var _RoleName = $("TD", row).eq(1).html();

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
                url: "/SystemRole/DeleteRole?RoleName=" + _RoleName.trim(),
                success: function (result) {
                    var message = "Role has been deleted successfully.";
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblSystemRole').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
