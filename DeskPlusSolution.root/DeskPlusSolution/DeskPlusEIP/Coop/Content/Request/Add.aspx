<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Content.Request.Add" %>

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
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SearchStatus = "<%=SearchStatus %>"; //상태값     
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var ItemID = "<%=ItemID %>";
        var ItemSubID = "<%=ItemSubID %>";
        var FavorUser = "<%=FavorUser %>";
        
        var ReceiveName = "";
        var ReferenceName = "";
        var EleDom = "";
        var TplItemSubID = "<%=TplItemSubID %>";   
        
        //전자결재 근거문서용     
        var BaseDocIDs = "<%=BaseDocIDs %>";        
        var BaseDocCnt = 0; //전제결재 근거문서 새창에서 사용함.

        //업무협업 근거문서용
        var CoopBaseIDs = "";
        var CoopBaseCnt = 0; //전제결재 근거문서 새창에서 사용함.

        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";
        var R_AleadyRegisted = "<%= GetResource("AleadyRegisted") %>";
        var R_Del = "<%= GetResource("Del") %>";
        var R_FileName = "<%= GetResource("FileName") %>";
        var R_FileType = "<%= GetResource("FileType") %>";
        var R_FileLen = "<%= GetResource("FileLen") %>";
        var R_FileDel = "<%= GetResource("Del") %>";
        var R_NoAttachData = "<%= GetResource("NoAttachData") %>";
        var R_NoContent = "<%= GetResource("NoContent") %>";
        var NoContent = "<%= GetResource("NoContent") %>";        
        var R_MovePage = "<%= GetResource("MovePage") %>";
        var R_SelectFirst = "<%= GetResource("SelectFirst") %>";
        var R_SelectSecond = "<%= GetResource("SelectSecond") %>";
        var R_InputCompleteDate = "<%= GetResource("InputCompleteDate") %>";
        var R_InputReceive = "<%= GetResource("InputReceive") %>";
        var R_InputTitle = "<%= GetResource("InputTitle") %>";
        var R_ScreenShot = "<%= GetResource("ScreenShot") %>";        
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";        

        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";

        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";        
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        JsCall("/DeskPlusEIP/Coop/Js/Content/Request/Add.js");
        JsCall("/DeskPlusEIP/Coop/Js/Common/Common.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Request")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <li class="btn_blue"><cc3:ClientSmartButton ID="cbtnAdd" runat="server" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
                </ul>
            </div>
            <div id="phRecRef" style="clear: both;">
                <div class="layout_Tbox">
                    <table cellspacing="0" class="tab_typeCO">
                        <colgroup>
                            <col width="100px" />
                            <col />
                            <col width="100px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th><%=GetResource("Receiver")%></th>
                                <td colspan="3">                                    
                                    <asp:HiddenField ID="hdReceiveID" runat="Server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdReceiveName" runat="Server"></asp:HiddenField>
                                    <div id="InputDiv1" style="float:left;height: 32px; width: 90%; padding-left: 2px; padding-right:2px; padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>

                                    <ul class="btn_right">
                                        <cc3:ClientSmartButton ID="cbtnReceive" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Referencer")%></th>
                                <td colspan="3">
                                    <asp:HiddenField ID="hdReferenceID" runat="Server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdReferenceName" runat="Server"></asp:HiddenField>
                                    <div id="InputDiv2" style="float:left;height: 32px; width: 90%; padding-left: 2px; padding-right:2px;padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>

                                    <ul class="btn_right">
                                        <cc3:ClientSmartButton ID="cbtnReference" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Group")%></th>
                                <td>
                                    <%=GetResource("First")%>
                                    :
                                    <asp:DropDownList ID="ddlItem" runat="server">
                                        <asp:ListItem Text="------" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    <%=GetResource("Second")%>
                                    :
                                    <asp:DropDownList ID="ddlItemSub" runat="server">
                                        <asp:ListItem Text="------" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th><%=GetResource("RequestDate")%></th>
                                <td><%= System.DateTime.Now.ToShortDateString() %></td>
                            </tr>
                            <tr id="tdDate">
                                <th><%=GetResource("CompleteDate")%></th>
                                <td><cc3:ClientSmartTextBox ID="tbxCompleteRequestDate" InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:80px !important;"></cc3:ClientSmartTextBox></td>
                                <td colspan="2">
                                    <asp:CheckBox ID="cbIsClose" runat="server" />
                                    <%=GetResource("Secret")%>
                                </td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Title")%></th>
                                <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="tbxTitle" Width="98%"></cc3:ClientSmartTextBox></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab">
                        <li><a id="ActvieX" class="tab_on"><span>ActiveX</span> </a></li>
                        <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                    </ul>
                    <div class="divEditor">
                        <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                    </div>
                </div>
                <!-- 첨부파일 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("AttachName")%></h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnSwfFileAdd" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                <!-- 첨부파일 리스트 -->
                
                <div id="placeholder_file" style="clear: both;"></div>
                
                <!-- 첨부파일 리스트 -->
                <!-- 근거문서 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("BaseDoc")%></h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnDocAdd" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                <!-- 근거문서 리스트 -->
                
                <table class="table_typeA" id="contentTable">
                    <caption>
                        <%=GetResource("BaseDocList")%></caption>
                    <colgroup>
                        <col width="100px" />
                        <col width="200px" />
                        <col width="120px" />
                        <col />
                        <col width="120px" />
                        <col width="60px" />
                        <col width="60px" />
                        <col width="70px" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th><%=GetResource("Group")%></th>
                            <th><%=GetResource("DocNumber")%></th>
                            <th><%=GetResource("FormName")%></th>
                            <th id="j_title"><%=GetResource("Title")%></th>
                            <th class="center"><%=GetResource("ReportUser")%></th>
                            <th class="center" id="j_view"><%=GetResource("DocView")%></th>
                            <th class="center" id="j_del"><%=GetResource("Del")%></th>
                            <th id="j_orders"><%=GetResource("ChangeOrder")%></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="noContent">
                            <td colspan="8" class="center"><span class='NoBaseListData' /></td>
                        </tr>
                    </tbody>
                </table>
                
                
                <!-- COOP 근거문서 리스트 -->
                <div class="layout_subbox">
                    <h2>업무협업 근거문서</h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnCoopAdd" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                
                
                <table class="table_typeA" id="contentCoopTable">
                    <caption>업무협업 근거문서</caption>
                    <colgroup>                        
                        <col />
                        <col width="60px" />
                        <col width="60px" />
                        <col width="70px" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th class="center">제목</th>
                            <th class="center">문서보기</th>
                            <th class="center">삭제</th>
                            <th class="center">순서변경</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id ="noCoopContent">
                            <td colspan="4" class="center"><span class='NoBaseCoopListData' /></td>
                        </tr>
                    </tbody>
                </table>
                
                <!-- COOP 근거문서 리스트 -->

            </div>
        </div>
    </div>
    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"   runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbSelItemSubID" Width="0" runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>
        <asp:TextBox ID="tbBaseDocIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbBaseCoopIDs" runat="server"></asp:TextBox>
        <input name="tbAddDocID" type="text" id="tbAddDocID" value="" size="160" runat="server" />
        <input name="tbAddCoopID" type="text" id="tbAddCoopID" value="" size="160" runat="server" />
        <!-- For. 개발용도: 근거문서 입력용도 //-->
        <!-- ==================================================================================== -->
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>
