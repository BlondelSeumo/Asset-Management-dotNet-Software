var Details = function (id) {
    var url = "/AssetSubCategorie/Details?id=" + id;
    $('#titleBigModal').html("Asset Sub Categorie Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/AssetSubCategorie/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Asset Sub Categorie Details");
    }
    else {
        $('#titleBigModal').html("Add Asset Sub Categorie Details");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmAssetSubCategorie").valid()) {
        return;
    }

    var _frmAssetSubCategorie = $("#frmAssetSubCategorie").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/AssetSubCategorie/AddEdit",
        data: _frmAssetSubCategorie,
        success: function (result) {
            Swal.fire({
                title: result.AlertMessage,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblAssetSubCategorie').DataTable().ajax.reload();
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
                url: "/AssetSubCategorie/Delete?id=" + id,
                success: function (result) {
                    var message = "Asset Sub Categorie Details has been deleted successfully. Asset Sub Categorie Details ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblAssetSubCategorie').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
