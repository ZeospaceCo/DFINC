<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListTop.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Statistics.ListTop" %>
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

<div id="layout_body">

    <div class="layout_full">

        <!-- Header //-->
        <div id="layout_header">
            <h1><%=GetResource("StatTop1")%></h1>
            <!-- Total //-->
            <table class="table_typeB">
            <colgroup>
                <col width="110px" />
                <col />
                <col width="110px" />
                <col />
                <col width="110px" />
                <col />
                <col width="110px" />
                <col />
                <col width="110px" />
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("StatTop2")%></th>
                    <td class="txt_point"><asp:Label ID="lbTot" runat="server" Text="0"></asp:Label> <%=GetResource("StatUnit")%></td>
                    <th><%=GetResource("StatTop3")%></th>
                    <td><asp:Label ID="lbC" runat="server" Text="0"></asp:Label> <%=GetResource("StatUnit")%></td>
                    <th><%=GetResource("StatTop4")%></th>
                    <td><asp:Label ID="lbW" runat="server" Text="0"></asp:Label> <%=GetResource("StatUnit")%></td>
                    <th><%=GetResource("StatTop5")%></th>
                    <td><asp:Label ID="lbR" runat="server" Text="0"></asp:Label> <%=GetResource("StatUnit")%></td>
                    <th><%=GetResource("StatTop6")%></th>
                    <td><asp:Label ID="lbD" runat="server" Text="0"></asp:Label> <%=GetResource("StatUnit")%></td>
                </tr>
            </tbody>
            </table>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                </li>
                <li>
                    <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                </li>
                <li class="searchterm"><input type="text" id="startDate" size="10" class="clsDate searchtext form-control"/></li>
                <li class="searchterm">-</li>
                <li class="searchterm"><input type="text" id="endDate" size="10" class="clsDate searchtext form-control"/></li>
                <li class="searchterm">
                    <cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton>
                </li> 
            </ul>
        </div>
        
        <!-- Contents //-->
        <div id="layout_content">
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                    <!-- Tab //-->
                    <div class="layout_srh clearfix">

                        <div class="float-left">
                            <ul class="tab">
                                <li style="border-bottom: 0px;"><a href="javascript:ListTop.SetTab('Form');" class="tab_on" id="tabForm"><%=GetResource("StatTop7")%></a></li>
                                <li style="border-bottom: 0px;"><a href="javascript:ListTop.SetTab('Dept');" id="tabDept"><%=GetResource("StatTop8")%></a></li>
                                <li style="border-bottom: 0px;" class="last"><a href="javascript:ListTop.SetTab('User');" id="tabUser"><%=GetResource("StatTop9")%></a></li>
                            </ul>
                        </div>

                    </div>

                    <!-- Contents //-->
                    <div class="app_statistics_cont">

                        <div id="placeholder"></div>

                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                            <h3><span id="pageInfo"></span></h3>
	                        <span id="pager"></span>
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <!-- 페이징 끝 //-->

                    </div>
                </div><!-- end col-3 content-area-body-right -->
		    </div><!-- end row  -->
        </div>

    </div>

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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SearchField = "<%=SearchField %>";      //검색키워드
        var CurrentPage = "<%=CurrentPage %>";      //현재페이지 (PageNo)
        var TotalItemCount = "<%=TotalItemCount %>";    //게시물수 (전체)
        var RecordPerPage = "<%=RecordPerPage %>";      //보여줄게시물수 (PageSize)
        var TabMenu = "<%=TabMenu %>";                  //양식별(Form), 부서별(Dept), 개인별(User)
        var SelYears = "<%=SelYears %>"; 
        var sDate = "<%=sDate %>";
        var eDate = "<%=eDate %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        // 다국어
        var R_Previous  = "<%=GetResource("Previous") %>";
        var R_Next      = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Statistics/ListTop.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Statistics/ListTop.js");
    </script>

</body>
</html>
