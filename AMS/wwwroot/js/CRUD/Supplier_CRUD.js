var Details = function (id) {
    var url = "/Supplier/Details?id=" + id;
    $('#titleMediumModal').html("Supplier Details");
    loadMediumModal(url);
};

var AddEdit = function (id) {
    var url = "/Supplier/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Supplier");
    }
    else {
        $('#titleMediumModal').html("Add Supplier");
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
                url: "/Supplier/Delete?id=" + id,
                success: function (result) {
                    var message = "Supplier has been deleted successfully. Supplier ID: " + result.Id;
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

