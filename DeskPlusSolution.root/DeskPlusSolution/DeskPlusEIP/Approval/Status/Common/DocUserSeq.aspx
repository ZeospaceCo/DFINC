<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocUserSeq.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.Common.DocUserSeq" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layer_popup">
        <div class="inside">
            
            <div class="popup_header">
            <h1><%=GetResource("AppInfo")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
            </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:450px;height:450px;">
                <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="80px" />
                    <col />
                    <col width="110px" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th class="center"><%=GetResource("DocName")%></th>
                        <td><%=DocName %></td>
                        <th class="center"><%=GetResource("SendDeptUser")%></th>
                        <td><%=DeptName%>/<%=TitleName %>/<%=WriteName %></td>                        
                    </tr>
                    <tr>
                        <th class="center"><%=GetResource("DateOfWrite")%></th>
                        <td colspan="3"><%=WriteDate %></td>
                    </tr>
                    <tr>
                        <th class="center"><%=GetResource("Subject")%></th>
                        <td colspan="3"><%=DocSubject %></td>
                    </tr>
                </tbody>
                </table>

                <!-- 결재자정보 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("ApprovalUserInfo")%></h2>
                </div>
                
                <div id="placeholder_Info" style="clear: both;"></div>
                

                 <!-- 참조자정보 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("CarbonInfo")%></h2>
                </div>
                
                <div id="placeholder_CA" style="clear: both;"></div>
                
            </div>

        </div>
    </div>
    
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ChildDocID = "<%=ChildDocID %>";
        var GroupID = "<%=GroupID %>";
        var DocumentID = "<%=DocumentID %>";

        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDownload = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DocInfo = "<%=GetResource("DocInfo") %>";
        var R_DocView = "<%=GetResource("DocView") %>";
        var R_Send = "<%=GetResource("Send") %>";
        var R_Receive = "<%=GetResource("Receive") %>";

        var R_Gubun = "<%=GetResource("ZDZP") %>";
        var R_Tag = "<%=GetResource("ApprovalTag") %>";
        var R_UserInfo = "<%=GetResource("ApprovalUserInfo") %>";
        var R_Date = "<%=GetResource("ApproveDate") %>";
        var R_DeptName = "<%=GetResource("TagDic_INFO5") %>";
        var R_TitleName = "<%=GetResource("TagDic_INFO4") %>";
        var R_Name = "<%=GetResource("TagDic_INFO3") %>";

        var R_ZD0 = "<%=GetResource("ZD0") %>";
        var R_ZD = "<%=GetResource("ApproveS") %>";
        var R_ZH = "<%=GetResource("R_ZH") %>";
        var R_ZA = "<%=GetResource("ZA") %>";
        var R_ZR = "<%=GetResource("ZR") %>";
        var R_ApprovalTypeP = "<%=GetResource("ApprovalTypeP") %>";
        var R_ApprovalTypeL = "<%=GetResource("ApprovalTypeL") %>";
        var R_CarbonInfo = "<%=GetResource("CarbonInfo") %>";
        var R_ViewDate = "<%=GetResource("ViewDate") %>";

        var ImagePath = "<%=BaseImgPath%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Status/Common/DocUserSeq.js"></script>       
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Status/Common/DocUserSeq.js");
    </script>
</body>
</html>
