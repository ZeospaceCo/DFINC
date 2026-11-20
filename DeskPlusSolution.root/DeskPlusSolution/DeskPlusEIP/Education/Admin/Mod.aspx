<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>수정</h1>
                <!-- 버튼 //-->
                <ul class="btn_right">              
                    <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
                </ul>
            </div>
            <div id="layout_content">
               <div class="row"> 
                  <div class="col-12 col-lg-9 col-xl-8 content-area-body-left">
                    <div class="layout_Tbox">
                    <table cellspacing="0" class="table_typeB">
                        <colgroup>
                            <col width="80px" />
                            <col />
                            <col width="80px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>분류</th>
                                <td colspan="3"><asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList>  </td>
                            </tr>
                            <tr>
                                <th>대상자</th>
                                <td colspan="3">   
                                    <asp:HiddenField ID="hdReceiveID" runat="Server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdReceiveLabels" runat="server" />
                                    <div id="InputTO" style="width: 75%; padding-left: 2px; padding-right:2px;display: inline-block;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"><asp:Literal ID="ltlReceiveTargets" runat="server" /></div>                                    
                                    <span class="btn_right"><cc3:ClientSmartButton ID="cbtnReceive" runat="server"></cc3:ClientSmartButton></span>
                                </td>                               
                            </tr>                                                       
                            <tr>
                                <th>기간</th>
                                <td><cc3:ClientSmartTextBox ID="tbxStartDate" ReadOnly InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                ~ <cc3:ClientSmartTextBox ID="tbxEndDate" ReadOnly InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                </td>          
                                <th>첨부파일필수</th>      
                                <td><asp:DropDownList ID="ddlFileYN" runat="server" CssClass="searchselect form-control">
                                         <asp:ListItem Text="Y" Value="Y" />
                                         <asp:ListItem Text="N" Value="N" />
                                      </asp:DropDownList>
                                </td>                       
                            </tr>
                            <tr>
                                <th>팝업설정</th>
                                <td>
                                   <input id="cbPopUp" type="checkbox" name="cbPopUp" runat="server" />팝업사용
                                </td>
                                <th>알람</th>
                                <td><asp:DropDownList ID="ddlAlarmDay" runat="server" CssClass="searchselect form-control"></asp:DropDownList>&nbsp;(일전 미완료자 재알람 발송)</td>                                
                            </tr>
                            <tr>
                                <th>과정명</th>
                                <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="tbxTitle" Width="98%" MaxLength="100" Required="true"></cc3:ClientSmartTextBox></td>
                            </tr>
                        </tbody>
                    </table>
                    </div>
                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab" style="display:none">
                            <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX" class="tab_on"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divNonEditor">
                            <div id="divXFE" style="height: 5px;">  
                                <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>                      
                                <script type="text/javascript">
                                    if (<%=HasEducation ? "true" : "false" %>) {
                                    xfe = new XFE({
                                        basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                        width: '100%',
                                        height: '550px',
                                        ignoreMinHeight: true,
                                        onLoad: function (editor) { Mod.InitializeEditor(editor); }
                                    });
                                    }
                                </script>
                            </div>
                        </div>
                    </div>
                  </div><!-- end col content-area-body-right -->
                  <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  >
                    <ul>
                        <!-- 공지첨부 -->
                        <li class="title">
                             공지첨부
                        </li>
                        <!-- 기존 공지첨부은 삭제 확인 시 실제 파일까지 즉시 삭제합니다. -->
                        <li><asp:Literal ID="ltlExistingFiles" runat="server" /></li>
                    </ul>
                    <ul>
                        <!-- 새 공지첨부 -->
                        <li class="title">
                             추가할 공지첨부
                             <div class="float-right">
                                 <a href="#" id="cbtnSwfFileAdd" title="파일추가"><i class="fas fa-plus-circle text-secondary"></i></a>
                             </div>
                        </li>
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                    </ul>

                  </div><!-- end col  -->
    
               </div><!-- end row  -->
            </div><!-- end layout_content  -->
		</div><!-- end layout_full  -->
	</div><!-- end layout_body  -->
    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"   runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  style="display: none" rows="1" cols="20"></textarea>
        <asp:LinkButton ID="lbtnMod" runat="server" OnClick="lbtnMod_Click"></asp:LinkButton>
    </div>
    </form>
    <asp:Literal ID="ltlError" runat="server" />
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
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
        var HasEducation = <%=HasEducation ? "true" : "false" %>;
        var ListID = <%=ListID%>;
        var EducationToday = <%=EducationToday %>;
        var OriginalStartDateValue = <%=OriginalStartDateValue.ToString(System.Globalization.CultureInfo.InvariantCulture) %>;
        var OriginalEndDateValue = <%=OriginalEndDateValue.ToString(System.Globalization.CultureInfo.InvariantCulture) %>;
        var OriginalAlarmDayValue = <%=OriginalAlarmDayValue.ToString(System.Globalization.CultureInfo.InvariantCulture) %>;
        var ReturnViewUrl = "<%=HttpUtility.JavaScriptStringEncode(ReturnViewUrl ?? "") %>";

        var ReceiveName = "";
        var BtnCheck = "N";     
      
        var SwfUploadGUID = "<%=HttpUtility.JavaScriptStringEncode(SwfUploadGUID) %>";        
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=HttpUtility.JavaScriptStringEncode(BaseImgPath) %>";
        var NonActiveX = "<%=HttpUtility.JavaScriptStringEncode(NonActiveX)%>";        
        var PreLang = "<%=HttpUtility.JavaScriptStringEncode(PreLang)%>";
        var PreLangReal = "<%=HttpUtility.JavaScriptStringEncode(PreLangReal) %>"; //ja 
        var ClientQueryString = "<%=HttpUtility.JavaScriptStringEncode(Page.ClientQueryString) %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=(PreLangReal == "en-gb") ? "en-GB" : PreLang%>.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/Mod.js?v=10"></script>
</body>
</html>


