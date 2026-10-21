<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VotePopup.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Content.VotePopup" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup view">
        <div class="layout_full">
        <div class="popup_header">
                <h1><%=GetResource("TitF25")%></h1>
                <ul class="btn_right">
                   <li><cc3:ClientSmartButton ID="cbtnVote" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                   <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>
        <div class="dotLine pb-0 mb-0"></div>
        <div id="layout_content">
            <div class="row">
           <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!-- width 100%(col-12) -->

                <div id="prtArea">
                <!-- 설문정보 시작 //-->
                <table class="table_typeB">
			    <caption><%=GetResource("TitInfo")%></caption>
			    <colgroup>
                    <col width="80px" />
                    <col width="40%" />
                    <col width="80px" />
                    <col />
			    </colgroup>
			    <tbody>
				    <tr>
					    <th><%=GetResource("TitF1")%></th>
					    <td colspan="3">
                            <asp:LinkButton ID="ltGroupName" runat="server"></asp:LinkButton>
                            <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                        </td>
				    </tr> 
				    <tr>
					    <th><%=GetResource("TitF2")%></th>
					    <td>
                            <asp:Literal ID="ltSDate" runat="server"></asp:Literal> ~ <asp:Literal ID="ltEDate" runat="server"></asp:Literal>
                            <asp:Label ID="lbSub" runat="server" CssClass="txt_point"></asp:Label>
                        </td>
                        <th><%=GetResource("TitF3")%></th>
                        <td>
                            <span class="aLeft">
                                <asp:Literal ID="ltDeptNm" runat="server"></asp:Literal>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th><%=GetResource("TitF4")%></th>
                        <td>
                            <ul class="aLeft">
                                <li><asp:checkbox runat="server" id="cbIsMain" Checked="true" Enabled="false"></asp:checkbox></li>
                                <li><asp:checkbox runat="server" id="cbIsReply" Checked="true" Enabled="false"></asp:checkbox></li>
                                <li><asp:checkbox runat="server" id="cbIsOpen" Checked="true" Enabled="false"></asp:checkbox></li>
                                <li><asp:checkbox runat="server" id="cbIsVoteUser" Checked="true" Enabled="false"></asp:checkbox></li>
                                <li><asp:checkbox runat="server" id="cbIsSecret" Checked="false" Enabled="false"></asp:checkbox></li>
                            </ul>
                        </td>
                        <th><%=GetResource("TitUnitAddF4")%></th>
                        <td><asp:Literal ID="ltIsActive" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("TitF9")%></th>
                        <td><div class="opn_target"><asp:Literal ID="ltTarget" runat="server"></asp:Literal></div></td>
                        <th><%=GetResource("TitF10")%></th>
                        <td><div class="opn_target"><asp:Literal ID="ltAdmin" runat="server"></asp:Literal></div></td>
                    </tr>
                     <tr>
                    <th><%=GetResource("TitF11") %></th>
                    <td colspan="3">
                        <div style="min-height:300px;width:100%; overflow:auto;">
                            <asp:Literal ID="ltCont" runat="server"></asp:Literal>
                        </div>
                    </td>
                </tr>
			</tbody>
		    </table> 
                <!-- 설문정보 끝 //-->
                <div class="layout_subbox">
                    <%=GetResource("TitF12")%>
                </div>
                <div class="opn_unitlist"><div id="divUnitCont"></div></div>

                <br />

                </div>
               </div><!-- width 100%(col-12) -->

                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!-- width 100%(col-12) -->
                <!-- 파일리스트 시작 //-->
                <ul>
                    <li class="title"><%=GetResource("TitF13")%></li>
                    <li><div id="phFileList" style="clear:both;"></div></li>
                
                <!-- 파일리스트 끝 //-->

                <!-- 짧은답글 시작 //-->

                <li class="title"><%=GetResource("TitF17")%></li>
                <li class="reply"><div id="phReply" style="clear:both;"></div></li>
                
                </ul>
                </div><!-- end col-3 content-area-body-right -->
                <!-- 짧은답글 끝 //-->
                </div>

            </div>
    </div>


    <div style="display:none;">
        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
        <asp:TextBox ID="tbHiddenVals" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>

        <textarea id="txtArea" width="300" height="300"></textarea>
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

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var Popup = "1";                    //팝업창여부
        var MenuID = "<%=MenuID%>";         //메뉴
        var PageSize = "<%=PageSize%>";     //보여질 게시물수
        var PageNo = "<%=PageNo %>";        //현재페이지
        var SKey = "<%=SKey %>";            //검색키워드
        var SVal = "<%=SVal %>";            //검색어
        var SPeriod = "<%=SPeriod%>";       //기간검색
        var SPeriodVal = "<%=SPeriodVal%>"; //기간검색어
        var Order = "<%=Order %>";          //정렬
        var QuesID = "<%=QuesID%>";
        var ListType = "<%=ListType %>";    //리스트 형식 일반리스트(1) / 블로그(2) / 앨범(3)
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PageUrl = "<%=Request.Url.ToString()%>";
        //리소스
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Delete = "<%=GetResource("Msg_Del") %>";
        var R_Msg_ErrOpt = "<%=GetResource("Msg_ErrOpt") %>";
        var R_Msg_ErrReq = "<%=GetResource("Msg_ErrReq") %>";
        var R_TitReq = "<%=GetResource("TitReq") %>";
        var R_TitBig = "<%=GetResource("TitBig") %>";
        var R_JTF13 = "<%=GetResource("JTF13") %>";
        var R_JTF14 = "<%=GetResource("JTF14") %>";
        var R_JTF15 = "<%=GetResource("JTF15") %>";
        var R_JTF16 = "<%=GetResource("JTF16") %>";
        var R_JTF20 = "<%=GetResource("JTF20") %>";
        var R_JTF21 = "<%=GetResource("JTF21") %>";
        var R_JTNoData = "<%=GetResource("JTNoData") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Content/Vote.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Content/Vote.js");
    </script>
</body>
</html>
