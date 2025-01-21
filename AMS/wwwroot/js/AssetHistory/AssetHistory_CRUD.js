var Details = function (id) {
    var url = "/AssetHistory/Details?id=" + id;
    $('#titleBigModal').html("Asset History Details");
    loadBigModal(url);
};

var AssetDetails = function (id) {
    var url = "/Asset/Details?id=" + id;
    $('#titleExtraBigModal').html("Asset Details");
    loadExtraBigModal(url);
};

var AddEdit = function (id) {
    var url = "/AssetHistory/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Asset History");
    }
    else {
        $('#titleMediumModal').html("Add Asset History");
    }
    loadMediumModal(url);
};

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/AssetHistory/Delete?id=" + id,
                success: function (result) {
                    var message = "Asset History has been deleted successfully. AssetHistory ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        text: 'Deleted!',
                        onAfterClose: () => {
                            location.reload();
                        }
                    });
                }
            });
        }
    });
};
