<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Content.Forward.View" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Coop.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchStatus = "<%=SearchStatus %>"; //상태값
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var ListCount = "<%=ListCount %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬        
        var Status = "<%=Status %>";
        var ItemID = "<%=ItemID %>"; //1차분류
        var ItemSubID = "<%=ItemSubID %>"; //2차분류

        var ForwardListID = "<%=ForwardListID %>";

        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen ="<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_NoBaseListData = "<%=GetResource("NoBaseListData") %>";
        var R_NoContent= "<%=GetResource("NoContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";

        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";

        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        JsCall("/DeskPlusEIP/Coop/Js/Content/Forward/View.js");
        JsCall("/DeskPlusEIP/Coop/Js/Common/Common.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <!-- START HEADER -->
            <div id="layout_header">
                <h1>
                    <%=GetResource("Detail")%></h1>
                <ul class="btn_left">
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                </ul>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <!-- END HEADER -->
            <div id="layout_content">
                <div id="toPrint">
                    <!-- Status Info Div  -->
                    <div class="i_boxn">
                        <%=WorkProcess%>
                    </div>
                    <!-- Status Info Div  -->
                    <div class="cw_Tbox">
                        <!-- START - REQUEST TEMPATE 1 -->
                        <table cellspacing="0" class="tab_typeCR">
                            <colgroup>
                                <col width="120px" />
                                <col />
                                <col width="120px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                        <%=GetResource("Requester")%>
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltlRequester" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("Receiver")%>
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltlReceive" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("Referencer")%>
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltlReference" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("Title")%>
                                    </th>
                                    <td colspan="3">
                                        <span class="brd_tb">
                                            <asp:Literal ID="ltlSubject" runat="server"></asp:Literal></span>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("DocNumber")%></th>
                                    <td colspan="3"><asp:Literal ID="ltlCoopNumber" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("Group")%>
                                    </th>
                                    <td>
                                        <%=GetResource("First")%>
                                        :
                                        <asp:Literal ID="ltlItem" runat="server"></asp:Literal>
                                        <%=GetResource("Second")%>
                                        :
                                        <asp:Literal ID="ltlItemSub" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        <%=GetResource("OpenYN")%>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlIsClose" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("RequestDate")%>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlRequestDate" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        <%=GetResource("CompleteDate")%>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlCompleteRequestDate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlHeaderStartDate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlStartDate" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlHeaderEndDate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlEndDate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("Difficulty")%>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlDifficulty" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        <%=GetResource("State")%>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" valign='top'>
                                        <div style="overflow: auto;">
                                            <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr <%=IsAttach %>>
                                    <th>
                                        <%=GetResource("AttachName")%>
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltlAttach" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr <%=IsBaseDoc %>>
                                    <th>
                                        <%=GetResource("BaseDoc")%>
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltlBasedoc" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr <%=IsBaseCoop %>>
                                    <th>
                                        업무협업 근거문서
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltlBaseCoop" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <!-- END - HEADER TOTAL 1 -->
                    </div>
                    <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%">
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:TextBox ID="tbSelItemSubID" Width="0" runat="server"></asp:TextBox>
        <input name="tbAddDocID" type="text" id="tbAddDocID" value="" size="160" runat="server" />
        <!-- For. 개발용도: 근거문서 입력용도 //-->
        <!-- ==================================================================================== -->
        <!-- ==================================================================================== -->
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
</body>
</html>