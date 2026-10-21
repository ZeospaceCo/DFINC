
$(document).ready(function () {
    
    var portletDom = iframe_tabPortletID + "_" + iframe_portletID;

    if (iframe_IsAuth != "True") {
        $("#basicMenu_" + portletDom + " .del").remove();
    }

    //최소화 여부
    if (iframe_IsMini == "1") {
        $("#ptContent_" + portletDom).fadeOut("slow");
    }    

    $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {
        var pID = el[0].id.replace('ptConfig_', '');
        IFrame_ContextMenuWork(pID, action, el, pos);
    });


    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();
        Main.Popup();
    });
    

});


/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function IFrame_ContextMenuWork (id, action, el, pos) {
    switch (action) {
        case "del":
            {
                Main.DelTabPortlet(id);
                if (PortletAuth == "True") {
                    $("#portlet_" + id).fadeOut("slow");
                }
                break;
            }
        case "min":
            {
                Main.ModMinMaxPortlet(iframe_tabPortletID, iframe_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }

        case "max":
            {
                Main.ModMinMaxPortlet(iframe_tabPortletID, iframe_portletID, "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }

        case "info":
            {

                break;
            }
    }
}

