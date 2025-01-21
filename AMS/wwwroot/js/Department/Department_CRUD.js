var Details = function (id) {
    var url = "/Department/Details?id=" + id;
    $('#titleBigModal').html("Department Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/Department/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Department");
    }
    else {
        $('#titleBigModal').html("Add Department");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmDepartment").valid()) {
        return;
    }

    var _frmDepartment = $("#frmDepartment").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/Department/AddEdit",
        data: _frmDepartment,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblDepartment').DataTable().ajax.reload();
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
                url: "/Department/Delete?id=" + id,
                success: function (result) {
                    var message = "Department has been deleted successfully. Department ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblDepartment').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
