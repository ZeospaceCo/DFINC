<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>  
    <script type="text/javascript">
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
        var Add_SelectOptionTop = '<%= GetResource("Add_SelectOptionTop") %>';
        var Add_InputContent = '<%= GetResource("Add_InputContent") %>';
        var Add_InputDate = '<%= GetResource("Add_InputDate") %>';
        var R_ApplyDelete = '<%= GetResource("ApplyDelete") %>';
        var R_FileName = '<%=GetResource("FileName") %>';
        var R_FileType = '<%=GetResource("FileType") %>';
        var R_FileLen = '<%=GetResource("FileLen") %>';
        var R_FileDel = '<%=GetResource("Del") %>';
        var R_TextEmpty = '<%=GetResource("TextEmpty") %>';
        var ScheduleID = "<%=ScheduleID%>";
        var CompanyID = "<%=CompanyID%>";
        var Searchkey = "<%=Searchkey%>";
        var SearchValue = "<%=SearchValue%>";
        var CurrentPage = "<%=CurrentPage%>";
        var Starttime = "<%=Starttime%>";
        var Endtime = "<%=Endtime%>";
        var Order = "<%=Order%>";
        var PageUrl = "<%=PageUrl%>";
        var RepeatValue = "";
        var BaseImgPath = '<%=BaseImgPath%>';
        var Lang = '<%=this.PreLang%>';
        var TextEmpty = '<%=GetResource("TextEmpty")%>';
        var UserID = '<%=UserID%>';
        var NoMine = "<%=GetResource("NoMine")%>";
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var PreLang = "<%=PreLang%>";

        function AddMent(ment) {
            $("#txtrepeat").val(ment);
            $("#hdRepeat").val(RepeatValue);

            //alert($("#hdRepeatChk").val());

            if ($("#hdRepeatChk").val() != "") {
                $("#hdRepeatChk").val('N'); //기존에 반복일정있을때
            }
        }

        function AddShare(user, uid) {
            
            $("#divShare").html('');

            uid = TextReverseToXss2(uid);
            //$("#divtempshare").hide();
            $("#hdShareList").val(uid);

            //var DelImg = "<img src='" + BaseImgPath + '/Lang/' + Lang + "/ico_folderset_del.gif' alt='del' />";

            var arrayid = uid.split(',');

            if (arrayid[0].length > 0)
            {
                for (var i = 0; i < arrayid.length - 1; i++) {
                    var arrayName = arrayid[i].split('$');

                    if (arrayName[0] == "M") {
                        if (arrayName[1] == UserID) {
                            alert(NoMine);
                        }
                        else {
                            if (arrayName[0].length > 0) {
                                $("#divShare").append(
		                              "<li>"
			                        + "<span>" + arrayName[2]  + "</span>"
                                    + "<a onclick='Mod.ShareUserClick(this)' value='" + arrayid[i] + "' style='display:inline-block;' title='" + R_FileDel + "'><i class='fas fa-times-circle'></i></a>"
                                    + "</li>"
	                            );  

                            }
                        }
                    }
                    else {
                        if (arrayName[0].length > 0) {
                            $("#divShare").append(
		                          "<li>"
			                    + "<span>" + arrayName[2]  + "</span>"
                                + "<a onclick='Mod.ShareUserClick(this)' value='" + arrayid[i] + "' style='display:inline-block;' title='" + R_FileDel + "'><i class='fas fa-times-circle'></i></a>"
                                + "</li>"
	                        );  
                        }
                    }
                }
            }
            else{
                 $("#divShare").html('<li></li>');
            }

            /*
            var strHtml = "<table cellspacing='0' class='table_typeT'>";
            strHtml += "<caption></caption>";
            strHtml += "<colgroup><col  /><col width='90px' /></colgroup>";
            strHtml += "<tr><th id='r_name'>" + '<%=GetResource("Name") %>' + "</th><th id='r_title'>" + '<%=GetResource("Del") %>' + "</th></tr>";

            if (arrayid[0].length == 0) {
                strHtml += "<tr><td class='center'>" + TextEmpty + "</td><td>&nbsp;</td></tr>";

            }
            for (var i = 0; i < arrayid.length - 1; i++) {
                var arrayName = arrayid[i].split('$');

                if (arrayName[0] == "M") {
                    if (arrayName[1] == UserID) {
                        alert(NoMine);
                        strHtml += "<tr><td class='center'>" + TextEmpty + "</td><td>&nbsp;</td></tr>";
                    }
                    else {
                        if (arrayName[0].length > 0) {
                            strHtml = strHtml + "<tr><td class='center'>" + arrayName[2] + "</td>";
                            strHtml = strHtml + "<td class='center'><a onclick='javascript:Mod.ShareUserClick(this)' value='" + arrayid[i] + "' >" + DelImg + "</a></td>";
                            strHtml = strHtml + "</tr>";
                        }
                    }
                }
                else {
                    if (arrayName[0].length > 0) {
                        strHtml = strHtml + "<tr><td class='center'>" + arrayName[2] + "</td>";
                        strHtml = strHtml + "<td class='center'><a onclick='javascript:Mod.ShareUserClick(this)' value='" + arrayid[i] + "' >" + DelImg + "</a></td>";
                        strHtml = strHtml + "</tr>";
                    }
                }
            }
            strHtml += "</table>";
            $("#divShare").html(strHtml);
            */
        }

        function RepeatMod(arg) {
            $("#hdRepeatSel").val(arg);
            Mod.Confirm("MOD");
        }

        var s_Distance = "<%=s_Distance %>";
        var s_inquryPerson = "<%=s_inquryPerson %>";
        var s_DatePeriod = "<%=s_DatePeriod %>";
        var s_SearchKind = "<%=s_SearchKind %>";
        var s_SerachValue = "<%=s_SerachValue %>";
        var s_StartDate = "<%=s_StartDate %>";
        var s_EndDate = "<%=s_EndDate %>";
        var s_UserId = "<%=s_UserID %>";
        var s_PageSize = "<%=s_PageSize %>";
        var s_CurrentPage = "<%=s_CurrentPage %>";
        var ConfirmEditorTab = '<%=GetResource("ConfirmEditorTab") %>';
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var TimeAlert = '<%=GetResource("TimeAlert") %>';
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var RNAME = '<%=GetResource("Name") %>';
        var RDEL = '<%=GetResource("Del") %>';
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
        var AlarmAlert = '<%=GetResource("AlarmAlert") %>';
        var EmailAlert = '<%=GetResource("EmailAlert") %>';
        var CheckMail = '<%=GetResource("CheckMail") %>';
        var ACCOUNT_SUCCESS = '<%=GetResource("ACCOUNT_SUCCESS") %>';
        var MsgError = '<%=GetResource("MsgError") %>';

    </script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/Mod.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/Mod.js");             
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write view"><!-- 쓰기 및 보기화면일때 -->
    <div class="layout_full"><!-- layout_full --> 
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("Schedule")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div id="layout_content"><!-- layout_info --> 
            <div class="row">
               <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >

                    <!-- 테이블 시작 -->
		            <table cellspacing="0" class="table_typeB"> 
                    <caption><%=GetResource("ModSchedule")%></caption>
			        <colgroup>
                        <col width="80px" />
                        <col />
			        </colgroup>
                    <tr> 
                        <th><%=GetResource("Title")%></th> 
                        <td>
                        <input type="text" id="txttitle" runat="server" style="width:80%" maxlength="100"  />
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>
                        </td> 
                    </tr> 
                    <tr id="tdDate" style="vertical-align:top;"> 
                        <th><%=GetResource("Date")%></th> 
                        <td >
                            <div id="startEndDate">
                            <input type="text" id="startDate" size="12"  runat="server" readonly="readonly"/> 
                            <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>~
                            <input type="text" id="endDate" size="12"  runat="server" readonly="readonly"/> 
                            <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            </div>
                        </td>
                    </tr>  
                    <tr> 
                        <th><%=GetResource("RepeatSetting")%></th>
                        <td>
                            <span class="aLeft"><input  type="text" class="text required" value="" id="txtrepeat" runat="server" readonly="readonly" style="width:300px !important;">&nbsp;</span>
                            <ul class="btn_smallL">
                            <li><cc3:ClientSmartButton ID="cbtnRepeat" runat="server"  ></cc3:ClientSmartButton></li>
                            <li><cc3:ClientSmartButton ID="cbtnRepCancel" runat="server"  ></cc3:ClientSmartButton></li>
                            </ul>
                        </td>
                    </tr>  
                    <tr> 
                        <th><%=GetResource("PimAlarm")%></th> 
                        <td> 
                            <asp:DropDownList ID="ddlnotikind" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <input name="idEmail" type="text" class="text required" value="" id="idEmail" runat="server" style="width:250px !important;" maxlength="200" />
                            <asp:DropDownList ID="ddltime" runat="server" CssClass="searchselect form-control"><asp:ListItem Text="Time" Value=""></asp:ListItem></asp:DropDownList>
                            <span id="divment"><%=GetResource("BeforeNoti")%></span>
                            <cc3:ClientSmartButton ID="CbtnSetting" runat="server" ></cc3:ClientSmartButton>
                            <span id="msgMail"><%=GetResource("SplitMail")%></span>                                           
                        </td> 
                    </tr>  
                    <tr> 
                        <th><%=GetResource("Category")%></th>
                        <td><asp:DropDownList id="ddlStatus"  runat="server" CssClass="searchselect form-control" ></asp:DropDownList></td>  
                    </tr> 
                    </table> 
                    <!-- 테이블 끝 -->       

                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab">
                            <li><a id="ActvieX"  class="tab_on"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divEditor">
                            <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                        </div>
                    </div>       
               </div><!-- end col-9 content-area-body-left -->
               <div class="col-12 col-lg-3 col-xl-4 content-area-body-right" >
                    <ul> 
                        <!-- 공유대상 리스트 -->    
                        <li class="title">
                            <%=GetResource("ShareSetting")%>
                            <div class="float-right">                            
                            <a href="#" id="CbtnGetUser" title="<%=GetResource("ShareObjSearch")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><ul id="divShare" class="table_typeT ul_typeT" runat="server"><li></li></ul></li>
                    </ul> 
                    <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AttachName")%>
                        </li>
                        <li><div id="placeholder_fileListmod" style="clear: both;"></div></li>
                    </ul>     
                    <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AddTO")%> <%=GetResource("AttachName")%>
                            <div class="float-right">                            
                            <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><div id="placeholder_fileList" style="clear: both;"></div></li>
                    </ul>    
                </div>
            </div>
        </div>
    </div><!-- layout_full --> 
    </div>

    <div style="display:none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="hdRepeatSel" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdRepeatChk" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdRepeat" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdShareList" Style="display: none"   runat="server" Width="0px"></asp:TextBox>

        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>        

        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>
        	<!--  파일 첨부시 필요한 필드 //-->
		<asp:TextBox id="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox id="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
		<asp:TextBox id="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>        
         <iframe id="ifrmDelFile" style="DISPLAY: none" name="ifrmDelFile" src="about:blank" frameBorder="0"></iframe>
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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank" ></iframe>
    </form>    
</body>
</html>