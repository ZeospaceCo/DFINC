<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CirDocUser.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.CirDocUser" %>
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
            <div class="layout_full"></div>
            <div id="popup_header">
            <h1><%=GetResource("DocInfo")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
            </ul>
            </div>

            <div class="layout_scroll" style="min-height:380px;height:380px;">
            
                <div id="layout_content">
                <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="80px" />
                    <col />
                    <col width="80px" />
                    <col />
                    <col width="100px" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("DocName")%></th>
                        <td><%=DocName %></td>
                        <th><%=GetResource("WriteName")%></th>
                        <td><%=WriteName %> / <%=DeptName%> / <%=TitleName %></td>
                        <th><%=GetResource("DateOfWrite")%></th>
                        <td><%=WriteDate %></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Subject")%></th>
                        <td colspan="5"><%=DocSubject %></td>
                    </tr>
                </tbody>
                </table>

                <!-- 첨부파일 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("AttachName")%></h2>
                    <ul class="btn_right">
                        <li id="zipline1"><cc3:ClientSmartButton ID="cbtnZIP" runat="server" /></li>
                        <li id="zipline2" class="sec"></li>
                        <cc3:ClientSmartButton ID="cbtnShowImage" runat="server" BeginEndTag="li" />
                    </ul>
                </div>
                
                <div id="placeholder_file" style="clear: both;"></div>
                


                <!-- 근거문서 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("BaseDoc")%></h2>
                </div>
                
                <div id="placeholder_baseDoc" style="clear: both;"></div>
                

            </div>

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
        var CirDocumentID = "<%=CirDocumentID %>";

        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDownload = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DocInfo = "<%=GetResource("DocInfo") %>";
        var R_DocView = "<%=GetResource("DocView") %>";

        var R_MSG_SelectDownLoad = "<%=GetResource("MSG_SelectDownLoad")%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Carboncopy/CirDocUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Carboncopy/CirDocUser.js");
    </script>
</body>
</html>
