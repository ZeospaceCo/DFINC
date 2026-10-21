$(function () {
    $(document).ready(function () {
        ParamArray = {
            'dcparm': Dcparm,
            'menuCatID': MenuCatID,
            'catID': CatID
        };

        CallEncrypt(CallGetParam());
        var url = "./Category/Tree.aspx?p=" + GetEncryptValue;
        //var url = "./Category/DynamicTree.aspx?p=" + GetEncryptValue;
        $("#frmLeft")[0].src = url;





    });
});