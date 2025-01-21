var ResetPassword = function (ApplicationUserId) {
    $('#titleMediumModal').html("<h4>Reset Password</h4>");
    var url = "/UserProfile/ResetPassword?ApplicationUserId=" + ApplicationUserId;
    loadMediumModal(url);
};

var SaveResetPassword = function () {
    if (!$("#frmResetPassword").valid()) {
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/UserProfile/SaveResetPassword",
        data: UserProfile_PreparedFormObj(),
        processData: false,
        contentType: false,
        success: function (result) {
            var _error = result.substring(0, 5);
            if (_error == 'error') {
                var _result = result.slice(5);
                Swal.fire({
                    title: _result,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#OldPassword').focus();
                        $("#btnSave").val("Save");
                        $('#btnSave').removeAttr('disabled');
                    }, 400);
                });
            }
            else {
                Swal.fire({
                    title: result,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var EditUserProfile = function (id) {
    var url = "/UserManagement/AddEditUserAccount?id=" + id;
    $('#titleExtraBigModal').html("Add User");
    loadExtraBigModal(url);

    setTimeout(function () {
        $('#FirstName').focus();
    }, 200);
};

var UserProfile_PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('ApplicationUserId', $("#ApplicationUserId").val())
    _FormData.append('OldPassword', $("#OldPassword").val())
    _FormData.append('NewPassword', $("#NewPassword").val())
    _FormData.append('ConfirmPassword', $("#ConfirmPassword").val())
    return _FormData;
}