DashBoard = {}

/**********************************************************************************************/

$(function () {

    $(document).ready(function () {        
        
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            DashBoard.Add();
        });
    });
});


/**************************************************************************************************/
DashBoard.Add = function () {
    ParamArray = {
        'searchTarget': SearchTarget,
        'searchItem': SearchItem,
        'searchDateValue': SearchDateValue
    };

    CallEncrypt(CallGetParam());

    location = "Add.aspx?p=" + GetEncryptValue;
}
