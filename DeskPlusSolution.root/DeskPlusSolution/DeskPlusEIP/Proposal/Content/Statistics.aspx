<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Statistics" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="ifm_layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1>제안제도</h1>
            </div>

            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                         <table class="table_typeC">
                         <caption>제안현황</caption>
                         <colgroup>
                            <col />
                            <col width="400px" />
                         </colgroup>
                         <tbody>
                            <tr>
                               <th class="left">
                                    <asp:DropDownList ID="ddlMaxYear" runat="server" CssClass="searchtext form-control">
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                    </asp:DropDownList> 년 
                                    <asp:DropDownList ID="ddlMaxMonth" runat="server" CssClass="searchtext form-control">
                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    </asp:DropDownList> 월 최대제안자 : <span id="userInfo" class="pro_blue"></span>
                               </th>
                               <th><h2>우수제안소개</h2></th>
                            </tr>
                            <tr>
                                <th class="left">
                                    <asp:DropDownList ID="ddlDeptYear" runat="server" CssClass="searchtext form-control">
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                    </asp:DropDownList> 년
                                    <asp:DropDownList ID="ddlDeptMonth" runat="server" CssClass="searchtext form-control">
                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    </asp:DropDownList> 월 사업부별 제안현황
                                </th>
                                <td rowspan="2"><span id="monthlyReport"></span></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="phDeptList"></div>
                                </td>                    
                            </tr>                            
                         </tbody>
                         </table>

                         <table class="table_typeC">
               	  	        <tr>
                                <th class="left" style="vertical-align:middle;">
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchtext form-control">
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                    </asp:DropDownList> 년 월별 제안현황
                                </th>
                            </tr>
                            <tr>
                                <td>
                                     <div id="phMonthList"></div>
                                </td>
                            </tr>
               	        </table>    
                    </div>
                </div>                
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>   
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
        var StartYears = "<%=StartYears %>";
        var StartMonths = "<%=StartMonths %>";
        var MaxYear = "";
        var MaxMonth = "";
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Statistics.js"></script>
    <script type="text/javascript">
        //JsCall("../Js/Content/Statistics.js");
    </script>
</body>
</html>
