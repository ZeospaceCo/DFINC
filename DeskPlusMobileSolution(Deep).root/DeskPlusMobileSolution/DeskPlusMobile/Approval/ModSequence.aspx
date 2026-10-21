<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModSequence.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.ModSequence" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div style="min-height:348px;">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent" >
                <li class="poptitle text-primary">
                    <span>
                        <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("ConfirmAppInfo")%>
                    </span>
                    <div class="float-right date text-dark">
                        <button type="button" class="btn btn-xs btn-primary" id="btConfirm" style="float:none;"><%=GetResource("DocAdd")%></button>
                        <button type="button" class="btn btn-xs btn-primary" id="btClose" style="float:none;"><%=GetResource("Close")%></button>
                    </div>
                </li> 
                </ul>               
                <ul class="mobliecontent">
                    <li class="titleN">
                        <div style="text-align:right">
                            <i class="fas fa-angle-up" size=10 style="cursor:pointer;" onclick="ModSequence.Up()"></i>&nbsp;&nbsp;
                            <i class="fas fa-angle-down" size=10 style="cursor:pointer" onclick="ModSequence.Down()"></i>&nbsp;&nbsp;&nbsp;                            
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent" style="height:250px;overflow-y:scroll" >               
                    <li class="contentblock">
                        <div class="title" id="ulZD0">
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title" id="ulList">
                        </div>
                    </li>
                    <li class="contentblock" id="liZQ">
                        <div class="title" id="ulZQ">
                        </div>
                    </li>
                </ul>                            
             </div>
          </div>
    </div>
</form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
            
        var R_Msg_SeqErrZA = "<%=GetResource("Msg_SeqErrZA") %>";
        var R_Msg_SeqErrZH = "<%=GetResource("Msg_SeqErrZH") %>";
        var R_ETC3 = "<%=GetResource("ETC3") %>";
        var R_ZQ = "<%=GetResource("TagDic_ZP") %>";
        
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/ModSequence.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/ModSequence.js");
    </script>
</body>
</html>
