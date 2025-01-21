$(document).ready(function () {
    document.title = 'User Info From Browser';

    $("#tblUserInfoFromBrowser").DataTable({
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
            "url": "/UserInfoFromBrowser/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                }
            },
            { "data": "BrowserUniqueID", "name": "BrowserUniqueID" },
            { "data": "Lat", "name": "Lat" },
            { "data": "Long", "name": "Long" },
            { "data": "TimeZone", "name": "TimeZone" },
            { "data": "BrowserMajor", "name": "BrowserMajor" },
            { "data": "BrowserName", "name": "BrowserName" },

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
        ],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

