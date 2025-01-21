var Details = function (id) {
    var url = "/AssetCategorie/Details?id=" + id;
    $('#titleBigModal').html("Asset Categorie Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/AssetCategorie/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Asset Categorie");
    }
    else {
        $('#titleBigModal').html("Add Asset Categorie");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmAssetCategorie").valid()) {
        return;
    }

    var _frmAssetCategorie = $("#frmAssetCategorie").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/AssetCategorie/AddEdit",
        data: _frmAssetCategorie,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblAssetCategorie').DataTable().ajax.reload();
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
                url: "/AssetCategorie/Delete?id=" + id,
                success: function (result) {
                    var message = "Asset Categorie has been deleted successfully. Asset Categorie ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblAssetCategorie').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
