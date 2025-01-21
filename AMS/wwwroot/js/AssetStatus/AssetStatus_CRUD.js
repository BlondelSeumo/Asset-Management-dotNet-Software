var Details = function (id) {
    var url = "/AssetStatus/Details?id=" + id;
    $('#titleBigModal').html("Asset Status Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/AssetStatus/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Asset Status");
    }
    else {
        $('#titleBigModal').html("Add Asset Status");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmAssetStatus").valid()) {
        return;
    }

    var _frmAssetStatus = $("#frmAssetStatus").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/AssetStatus/AddEdit",
        data: _frmAssetStatus,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblAssetStatus').DataTable().ajax.reload();
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
                url: "/AssetStatus/Delete?id=" + id,
                success: function (result) {
                    var message = "Asset Status has been deleted successfully. Asset Status ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblAssetStatus').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
