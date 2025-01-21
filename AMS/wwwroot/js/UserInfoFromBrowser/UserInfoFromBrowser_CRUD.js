var Details = function (id) {
    var url = "/UserInfoFromBrowser/Details?id=" + id;
    $('#titleExtraBigModal').html("User Info From Browser Details");
    loadExtraBigModal(url);
};

