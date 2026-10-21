<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkList.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.ETCLegacy.WorkList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>

    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        //var Callback = "<%=Callback %>";   
        //var TagID = "<%=TagID %>";     
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = 7; //보여줄게시물수
        var ImagePath = "<%=BaseImgPath%>";


        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";

        var R_DocNumber = "<%=GetResource("DocNumber") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_FormName = "<%=GetResource("FormName2") %>";
        var R_FileDel   = "<%=GetResource("Delete") %>";
        var R_Sort = "<%=GetResource("Sort")%>";
        var R_ReportUser= "<%=GetResource("ReportUser") %>";
                        
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/ETC/Js/ETCLegacy/WorkList.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1>특근내역</h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                    <table class="table_typeB">
			        <caption></caption>
			        <colgroup>
                        <col width="150px;" />
                        <col />
                        <col width="70px" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><asp:literal id="lblSelectedReceiptor" runat="server"></asp:literal></th>
                            <td colspan="2">
                            <cc3:ClientSmartTextBox ID="tbWL" ReadOnly="true" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        
                    </tbody>
                    </table>
                    <br /><br />
                <div class="layout_scroll"  style="min-height:430px;height:430px;"> 
                    <div class="table_typeAtop_left"><p></p></div>
                    <div id="phWorkList" style="clear: both;"></div>
                    <div class="table_typeAbottom_left"><p></p></div> 
                    
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>
                    </div>
                    <!-- 페이징 끝 //-->   
                 
                </div>     
                  
            </div>                
        </div>
        <asp:HiddenField id="hdWorkID" runat="server" />
        <asp:HiddenField id="hdSubject" runat="server" />
        <asp:HiddenField id="hdNameBase" runat="server" />
        <asp:HiddenField id="hdWriteTime" runat="server" />
    </div>

</form>
</body>
</html>