var Details = function (id) {
    var url = "/Supplier/Details?id=" + id;
    $('#titleBigModal').html("Supplier Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/Supplier/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Supplier");
    }
    else {
        $('#titleBigModal').html("Add Supplier");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmSupplier").valid()) {
        return;
    }

    var _frmSupplier = $("#frmSupplier").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/Supplier/AddEdit",
        data: _frmSupplier,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblSupplier').DataTable().ajax.reload();
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
                url: "/Supplier/Delete?id=" + id,
                success: function (result) {
                    var message = "Supplier has been deleted successfully. Supplier ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblSupplier').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
