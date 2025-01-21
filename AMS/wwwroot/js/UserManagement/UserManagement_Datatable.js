$(document).ready(function () {
    document.title = 'User Account';

    $("#tblUserAccount").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],


        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/UserManagement/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            {
                data: "UserProfileId", "name": "UserProfileId", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=ViewUserDetails('" + row.UserProfileId + "');>" + row.UserProfileId + "</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='d-block' onclick=ViewImage('" + row.ProfilePicture + "','User_Image');><div class='image'><img src='" + row.ProfilePicture + "' class='img-circle elevation-2' alt='Asset Image'></div></a>";
                }
            },
            {
                data: "UserProfileId", "name": "UserProfileId", render: function (data, type, row) {
                    return "<a href='#' onclick=ViewUserDetails('" + row.UserProfileId + "');>" + row.FirstName + "</a>";
                }
            },
            { "data": "LastName", "name": "LastName", "autoWidth": true },
            { "data": "PhoneNumber", "name": "PhoneNumber", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: "UserProfileId", "name": "UserProfileId", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-plus' onclick=AllocateAsset('" + row.UserProfileId + "');>Allocate</a>";
                },
                Width: "50px",
            },
            {
                data: null, render: function (data, type, row) {
                    return "<select id='" + row.UserProfileId + "' onchange=funAction('" + row.UserProfileId + "'); class='btn-sm' style='width: 70px;'>" +
                        "<option value='0'></option>" +
                        "<option value='1'>Allocate Asset</option>" +
                        "<option value='2'>Edit</option>" +
                        "<option value='3'>Reset Password</option>" +
                        "<option value='4'>Manage Page Access</option>" +
                        "<option value='5'>Delete</option>" +
                        "</select>";
                },
                autoWidth: false,
                Width: "50px",
            },
        ],

        'columnDefs': [
            {
                'targets': [1, 7, 8],
                'orderable': false,
            },
            {
                "width": "10px",
                "targets": 7
            },
        ],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});
