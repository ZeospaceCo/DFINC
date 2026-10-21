<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Move.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Dept.Move" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("MoveFolder")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="80px" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("FolderName")%></th>
                        <td><input type="text" id="folderName" readonly="readonly" size="width:95%"/></td>
                    </tr>
                </tbody>
                </table>
                <div class="layout_scroll" style="min-height:285px;height:285px;">
                <zeoAppFolderTree:UCAppFolderTreeScript ID="UCAppFolderTreeScript1" SetCallbackComplete="onCallbackComplete" SetNodeSelect="onNodeSelect" 
                    SetCabinetList = "Dept_Common" UseCheckBox="false" UsePubFolder="true" runat="server" />
                </div>
            </div>
        </div>
    </div>

    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var SelFolderID = "";
        var DocID = "<%= DocID %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_MoveCategory = "<%=GetResource("SelMovefolder")%>";          //"이동할 폴더를 선택해 주세요.";
        var R_Processing ="<%=GetResource("MSG_SUCCESS")%>";           //"처리되었습니다.";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Dept/Move.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Dept/Move.js");
    </script>
</body>
</html>
