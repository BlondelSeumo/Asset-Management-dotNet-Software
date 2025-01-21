var funAction = function (UserProfileId) {
    var _Action = $("#" + UserProfileId).val();

    if (_Action == 1)
        AllocateAsset(UserProfileId);
    if (_Action == 2)
        AddEditUserAccount(UserProfileId);
    else if (_Action == 3)
        ResetPasswordAdmin(UserProfileId);
    else if (_Action == 4)
        UpdateUserRole(UserProfileId);
    else if (_Action == 5)
        DeleteUserAccount(UserProfileId);
    $("#" + UserProfileId).prop('selectedIndex', 0);
};

var ViewUserDetails = function (Id) {
    var url = "/UserManagement/ViewUserDetails?Id=" + Id;
    $('#titleBigModal').html("User Details ");
    loadBigModal(url);
};

var ResetPasswordAdmin = function (id) {
    $('#titleMediumModal').html("<h4>Reset Password</h4>");
    var url = "/UserManagement/ResetPasswordAdmin?id=" + id;
    loadMediumModal(url);
};

var AddEditUserAccount = function (id) {
    var url = "/UserManagement/AddEditUserAccount?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit User");
    }
    else {
        $('#titleExtraBigModal').html("Add User");
    }
    loadExtraBigModal(url);

    setTimeout(function () {
        $('#FirstName').focus();
    }, 200);
};

var SaveUser = function () {
    if (!$("#ApplicationUserForm").valid()) {
        return;
    }

    if (!FieldValidation('#FirstName')) {
        FieldValidationAlert('#FirstName', 'First Name is Required.', "warning");
        return;
    }
    if (!FieldValidation('#LastName')) {
        FieldValidationAlert('#LastName', 'Last Name is Required.', "warning");
        return;
    }

    if (!$("#ApplicationUserForm").valid()) {
        FieldValidationAlert('#ConfirmPassword', 'Please fill up all input properly.', "warning");
        return;
    }

    var _UserProfileId = $("#UserProfileId").val();
    if (_UserProfileId > 0) {
        $("#btnSave").prop('value', 'Updating User');
    }
    else {
        $("#btnSave").prop('value', 'Creating User');
    }
    $('#btnSave').prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "/UserManagement/AddEditUserAccount",
        data: PreparedFormObj(),
        processData: false,
        contentType: false,
        success: function (result) {
            $('#btnSave').prop('disabled', false);
            $("#btnSave").prop('value', 'Save');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnAddEditUserAccountClose").click();
                    if (result.CurrentURL == "/") {
                        setTimeout(function () {
                            $("#tblRecentRegisteredUser").load("/ #tblRecentRegisteredUser");
                        }, 1000);
                    }
                    else if (result.CurrentURL == "/UserProfile/Index") {
                        $("#divUserProfile").load("/UserProfile/Index #divUserProfile");
                    }
                    else {
                        $('#tblUserAccount').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#Email').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteUserAccount = function (id) {
    Swal.fire({
        title: 'Do you want to delete this user?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/UserManagement/DeleteUserAccount?id=" + id,
                success: function (result) {
                    var message = "User has been deleted successfully. User Id: " + result.Email;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblUserAccount').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

var UpdateUserRole = function (id) {
    $('#titleExtraBigModal').html("<h4>Manage Page Access</h4>");
    var url = "/ManageUserRoles/UpdateUserRole?id=" + id;
    loadExtraBigModal(url);
};

var SaveUpdateUserRole = function () {
    $("#btnUpdateRole").val("Please Wait");
    $('#btnUpdateRole').attr('disabled', 'disabled');

    var _frmManageRole = $("#frmManageRole").serialize();
    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/SaveUpdateUserRole",
        data: _frmManageRole,
        success: function (result) {
            $("#btnUpdateRole").val("Save");
            $('#btnUpdateRole').removeAttr('disabled');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblUserAccount').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var AllocateAsset = function (id) {
    var url = "/Asset/AllocateAsset?id=" + id;
    $('#titleExtraBigModal').html("Allocate Asset");
    loadExtraBigModal(url);
};

var PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('Id', $("#Id").val())
    _FormData.append('UserProfileId', $("#UserProfileId").val())
    _FormData.append('ApplicationUserId', $("#ApplicationUserId").val())
    _FormData.append('ProfilePictureDetails', $('#ProfilePictureDetails')[0].files[0])

    _FormData.append('FirstName', $("#FirstName").val())
    _FormData.append('LastName', $("#LastName").val())
    _FormData.append('EmployeeTypeId', $("#EmployeeTypeId").val())
    _FormData.append('PhoneNumber', $("#PhoneNumber").val())
    _FormData.append('Email', $("#Email").val())
    _FormData.append('PasswordHash', $("#PasswordHash").val())
    _FormData.append('ConfirmPassword', $("#ConfirmPassword").val())
    _FormData.append('Address', $("#Address").val())
    _FormData.append('Country', $("#Country").val())
    _FormData.append('RoleId', $("#RoleId").val())
    _FormData.append('IsApprover', $("#IsApprover").val())

    _FormData.append('EmployeeId', $("#EmployeeId").val())
    _FormData.append('DateOfBirth', $("#DateOfBirth").val())
    _FormData.append('Designation', $("#Designation").val())
    _FormData.append('Department', $("#Department").val())
    _FormData.append('SubDepartment', $("#SubDepartment").val())
    _FormData.append('JoiningDate', $("#JoiningDate").val())
    _FormData.append('LeavingDate', $("#LeavingDate").val())

    _FormData.append('CurrentURL', $("#CurrentURL").val())
    return _FormData;
}