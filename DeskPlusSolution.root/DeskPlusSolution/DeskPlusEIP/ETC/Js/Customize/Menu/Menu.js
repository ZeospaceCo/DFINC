var Menu = {};

$(function () {

    //비용 정산 입력
    $('#LegacyAdd').click(function (e) { e.preventDefault(); Menu.GoList('LegacyAdd'); });
    //비용 정산 리스트
    $('#LegacyList').click(function (e) { e.preventDefault(); Menu.GoList('LegacyList'); });
    //Financial
    //관리자
    $('#LegacyAdmin').click(function (e) { e.preventDefault(); Menu.GoList('LegacyAdmin'); });

    // 카드리스트 
    $('#CardListAdmin').click(function (e) { e.preventDefault(); Menu.GoList('CardListAdmin'); });
    //KRPIA 비용정산
    $('#KRPIA').click(function (e) { e.preventDefault(); Menu.GoList('KRPIA'); });


    // 권한에 따른 메뉴 분기
    if (AdminRole == "1") {
        $('#CardListAdmin').hide();
        $('#LegacyAdmin').show();
        $('#KRPIA').show();
    }
    else {
        $('#CardListAdmin').hide();
        $('#LegacyAdmin').hide();
        $('#KRPIA').hide();       
    }

    Menu.GoList("");
});

Menu.GoList = function (_menuID) {

    var url = "";

    switch (_menuID.toUpperCase()) {
        case "LEGACYADD":
            url = "../User/AddMaster.aspx";
            break;
        case "LEGACYADDCARD":
            url = "../User/AddCardMaster.aspx";
            break;
        case "LEGACYLIST":
            url = "../User/ListUser.aspx";
            break;
        case "LEGACYADMIN":
            url = "../Admin/ListAdmin.aspx";
            break;
        case "CARDLISTADMIN":
            url = "../Admin/CardListAdminEx.aspx";
            break;
        case "KRPIA":
            url = "/DeskPlusEIP/Etc/KRPIA/ERP/List.aspx";
            break;
        default:
            url = "../User/AddMaster.aspx";
            break;
    }
    parent.frmContent.location.href = url;
}