var Details = function (id) {
    var url = "/SubDepartment/Details?id=" + id;
    $('#titleBigModal').html("Sub Department Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/SubDepartment/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Sub Department");
    }
    else {
        $('#titleBigModal').html("Add Sub Department");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmSubDepartment").valid()) {
        return;
    }

    var _frmSubDepartment = $("#frmSubDepartment").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SubDepartment/AddEdit",
        data: _frmSubDepartment,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblSubDepartment').DataTable().ajax.reload();
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
                type: "DELETE",
                url: "/SubDepartment/Delete?id=" + id,
                success: function (result) {
                    var message = "Sub Department has been deleted successfully. Sub Department ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblSubDepartment').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
