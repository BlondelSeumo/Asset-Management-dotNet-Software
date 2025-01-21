var Details = function (id) {
    var url = "/Comment/Details?id=" + id;
    $('#titleMediumModal').html("Comment Details");
    loadMediumModal(url);
};

var AssetDetails = function (id) {
    var url = "/Asset/Details?id=" + id;
    $('#titleBigModal').html("Asset Details");
    loadBigModal(url);
};

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
                url: "/Comment/Delete?id=" + id,
                success: function (result) {                
                    Swal.fire({
                        title: result,
                        icon: 'info',
                        onAfterClose: () => {                            
                            $('#tblComment').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
