<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Content.Reference.Mod" %>


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
    <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchStatus = "<%=SearchStatus %>"; //상태값     
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어          
        var ItemID = "<%=ItemID %>"; //1차분류
        var ItemSubID = "<%=ItemSubID %>"; //2차분류  

        var ListCount = "<%=ListCount %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬        
        var ListID = "<%=ListID %>";
        var GroupID = "<%=GroupID %>";
        var Status = "<%=Status %>";
        var ListOrder = "<%=ListOrder %>";//리스트order
        
        //전자결재 근거문서용  
        var BaseDocIDs = "<%=BaseDocIDs %>";
        var BaseDocCnt = 0; //전제결재 근거문서 새창에서 사용함.

        //업무협업 근거문서용
        var CoopBaseIDs = "";
        var CoopBaseCnt = 0; //업무협업 근거문서 새창에서 사용함.

        var R_ApplyDelete ="<%=GetResource("ApplyDelete") %>";
        var R_AleadyRegisted = "<%=GetResource("AleadyRegisted") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";
        var R_NoBaseListData = "<%=GetResource("NoBaseListData") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var R_MovePage = "<%=GetResource("MovePage") %>";
        var R_SelectFirst = "<%=GetResource("SelectFirst") %>";
        var R_SelectSecond = "<%=GetResource("SelectSecond") %>";
        var R_InputCompleteDate = "<%=GetResource("InputCompleteDate") %>";
        var R_InputReceive = "<%=GetResource("InputReceive") %>";
        var R_InputTitle = "<%=GetResource("InputTitle") %>";
        var R_Title = "<%=GetResource("Title")%>";
        var R_State = "<%=GetResource("State")%>";
        var R_FirtstGroup = "<%=GetResource("First")%>"+"<%=GetResource("Group")%>";
        var R_SecondGroup = "<%=GetResource("Second")%>"+"<%=GetResource("Group")%>";
        var R_RequestDate = "<%=GetResource("RequestDate")%>";
        var R_StartDate = "<%=GetResource("StartDate")%>";
        var R_EndDate = "<%=GetResource("EndDate")%>"; 
        var R_NoContent= "<%=GetResource("NoContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";       

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
        JsCall("/DeskPlusEIP/Coop/Js/Content/Reference/Mod.js");
        JsCall("/DeskPlusEIP/Coop/Js/Common/Common.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("CoWork")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <li class="btn_blue"><cc3:ClientSmartButton ID="cbtnAdd" runat="server" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
                </ul>
            </div>
            <div id="phRecRef">
                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab">
                        <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                        <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                    </ul>
                    <div style="padding-bottom: 40px; height: 450px !important;">
                        <div id="divXFE" style="height: 5px;">                        
                            <script type="text/javascript">
                                xfe = new XFE({
                                    basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                    width: '100%',
                                    height: '470px'
                                });
                                xfe.render('divXFE');
                            </script>
                        </div>
                    </div>
                </div> 
                <!-- 첨부파일 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("AttachName")%></h2>
                    <ul class="btn_right">                        
                    </ul>
                </div>                
                
                <div id="placeholder_fileList" style="clear: both;"></div>
                

                <div class="layout_subbox">
                    <h2><%=GetResource("AddAttachFile")%></h2>
                    <ul class="btn_right">
                        <li>
                            <cc3:ClientSmartButton ID="cbtnSwfFileAdd" runat="server"></cc3:ClientSmartButton>
                        </li>
                    </ul>
                </div>                
                
                <div id="placeholder_file" style="clear: both;"></div>
                
                <!-- 첨부파일 -->
                <!-- 근거문서 -->
                <div class="layout_subbox">
                    <h2><%=GetResource("BaseDoc")%></h2>
                    <ul class="btn_right">                        
                    </ul>
                </div>                
                
                <div id="placeholder_basedocList" style="clear: both;"></div>
                

                <div class="layout_subbox">
                    <h2><%=GetResource("AddBaseDoc")%></h2>
                    <ul class="btn_right">
                        <li>
                            <cc3:ClientSmartButton ID="cbtnDocAdd" runat="server"></cc3:ClientSmartButton>
                        </li>
                    </ul>
                </div>                
                
                <table class="table_typeA" id="contentTable">
                <caption><%=GetResource("BaseDocList")%></caption>
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
                
                <!-- 추가할 근거문서  끝-->

                <!-- COOP 근거문서 리스트 -->
                <div class="layout_subbox">
                    <h2>업무협업 근거문서</h2>
                    <ul class="btn_right">                        
                    </ul>
                </div>                
                
                <div id="placeholder_baseCoopList" style="clear: both;"></div>
                

                <div class="layout_subbox">
                    <h2>추가할 업무협업 근거문서</h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnCoopAdd" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                
                
                <table class="table_typeA" id="contentCoopTable">
                    <caption>추가할 업무협업 근거문서</caption>
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
        <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"  runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>        
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbSelItemSubID" Width="0" runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>

        <asp:TextBox ID="tbBaseDocIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbBaseCoopIDs" runat="server"></asp:TextBox>
        <input name="tbAddDocID" type="text" id="tbAddDocID" value="" size="160" runat="server" />
        <input name="tbAddCoopID" type="text" id="tbAddCoopID" value="" size="160" runat="server" />

        <!--  파일 첨부시 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbScreenShot" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank" frameborder="0"></iframe>
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
