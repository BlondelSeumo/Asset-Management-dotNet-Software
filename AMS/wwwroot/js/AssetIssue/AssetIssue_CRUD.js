var Details = function (id) {
    var url = "/AssetIssue/Details?id=" + id;
    $('#titleBigModal').html("Asset Issue Details");
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/AssetIssue/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Asset Issue");
    }
    else {
        $('#titleExtraBigModal').html("Add Asset Issue");
    }
    loadExtraBigModal(url);
};

var SaveAssetIssue = function () {
    if (!$("#frmAssetIssue").valid()) {
        return;
    }

    var _frmAssetIssue = $("#frmAssetIssue").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/AssetIssue/AddEdit",
        data: _frmAssetIssue,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblAssetIssue').DataTable().ajax.reload();
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
                url: "/AssetIssue/Delete?id=" + id,
                success: function (result) {
                    var message = "Asset Issue has been deleted successfully. Asset Issue ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblAssetIssue').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
