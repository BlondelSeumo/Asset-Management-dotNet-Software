var Details = function (id) {
    var url = "/Designation/Details?id=" + id;
    $('#titleMediumModal').html("Designation Details");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Designation/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Designation");
    }
    else {
        $('#titleMediumModal').html("Add Designation");
    }
    loadMediumModal(url);
};

var Save = function () {
    if (!$("#frmDesignation").valid()) {
        return;
    }

    var _frmDesignation = $("#frmDesignation").serialize();
    $.ajax({
        type: "POST",
        url: "/Designation/AddEdit",
        data: _frmDesignation,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblDesignation').DataTable().ajax.reload();
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
                url: "/Designation/Delete?id=" + id,
                success: function (result) {
                    var message = "Designation has been deleted successfully. Designation ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblDesignation').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
