<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseView.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Content.Total.BaseView" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
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
        var ListID = "<%=ListID %>";
        var GroupID = "<%=GroupID %>";
        var Status = "<%=Status %>";

        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen ="<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_NoBaseListData = "<%=GetResource("NoBaseListData") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_DocView = "<%=GetResource("DocView") %>";
        
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";

        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Coop/Js/Content/Total/BaseView.js");
    </script>
</head>

<body>
    <form id="form1" runat="server">
                    
    <div class="layer_popup">
        <div class="top-left">
        </div>
        <div class="top-right">
        </div>
        <div class="inside">
            <div class="popup_header">
                <!-- 페이지타이틀 //-->
                <h5><%=GetResource("BaseDoc")%></h5>
            </div>
            <div id="layout_content">
            <div id="toPrint">
            <div class="layout_subbox">
                <h2><%=GetResource("RequireDetail")%></h2>                
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                    <li class="sec"></li>
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
                <table cellspacing="0" class="table_typeB">
                    <colgroup>
                        <col width="80px" />
                        <col />
                        <col width="80px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><%=GetResource("Group")%></th>
                            <td><%=GetResource("First")%> :
                                <asp:Literal ID="ltlItem" runat="server"></asp:Literal>
                                <%=GetResource("Second")%> :
                                <asp:Literal ID="ltlItemSub" runat="server"></asp:Literal>
                            </td>
                            <th><%=GetResource("RequestDate")%></th>
                            <td>
                                <asp:Literal ID="ltlRequestDate" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("CompleteDate")%></th>
                            <td>
                                <asp:Literal ID="ltlCompleteRequestDate" runat="server"></asp:Literal>
                            </td>
                            <th><%=GetResource("OpenYN")%></th>
                            <td>
                                <asp:Literal ID="ltlIsClose" runat="server"></asp:Literal>
                            </td>
                        </tr>              
                    </tbody>
                </table>

                <div class="layout_subbox">
                    <h2><%=GetResource("IngDetail")%></h2>
                    <ul class="btn_right">                                                
                    </ul>
                </div>
                <table cellspacing="0" class="table_typeB">
                    <colgroup>
                        <col width="80px" />
                        <col />
                        <col width="80px" />
                        <col />
                    </colgroup>
                    <tbody>                        
                        <tr>
                            <th><%=GetResource("StartDate")%></th>
                            <td>
                                <asp:Literal ID="ltlStartDate" runat="server"></asp:Literal>
                            </td>
                            <th><%=GetResource("EndDate")%></th>
                            <td>
                                <asp:Literal ID="ltlEndDate" runat="server"></asp:Literal>
                            </td>
                        </tr>    
                        <tr>
                            <th><%=GetResource("Difficulty")%></th>
                            <td>                                
                                <asp:Literal ID="ltlDifficulty" runat="server"></asp:Literal>
                            </td>
                            <th><%=GetResource("State")%></th>
                            <td>
                                <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                            </td>
                        </tr>                     
                    </tbody>
                </table>
                <!-- 수신 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("Both")%></h2>
                    <ul class="btn_right">
                        
                    </ul>
                </div>
                <!-- 수신 리스트 -->
                <div id="phRecRef" style="clear: both;">
                    <table cellspacing="0" class="table_typeB">
                        <colgroup>
                            <col width="80px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th><%=GetResource("Receiver")%></th>
                                <td><asp:Literal ID="ltlReceive" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Referencer")%></th>
                                <td><asp:Literal ID="ltlReference" runat="server"></asp:Literal></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%"></asp:Panel>
            </div>
            </div>
        </div>
        <div class="bottom-left">
        </div>
        <div class="bottom-right">
        </div>
    </div>    
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
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>
