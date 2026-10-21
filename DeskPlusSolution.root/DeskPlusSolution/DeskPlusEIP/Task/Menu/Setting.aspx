<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Menu.Setting" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("MenuSetting") %></h1>
                <span class="app_caption">
                    <%=GetResource("MenuGuide") %>
                </span>
                <ul class="btn_right">
                    <cc3:clientsmartbutton id="cbtnAll" runat="server" beginendtag="li" />
                    <cc3:clientsmartbutton id="cbtnUnAll" runat="server" beginendtag="li" />
                    <cc3:clientsmartbutton id="cbtnOk" runat="server" beginendtag="li" class="btn btn-secondary"/>
                </ul>
            </div>
            <div id="layout_content">
              <div class="row">
                 <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
				    <div style="padding:5px;display:inline-block; float:left;"">
                        <asp:DropDownList ID="ddlSetting" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </div>
                    <div id="phSettingList" style="clear: both;"></div>
                </div><!-- end col content-area-body-right -->
              </div>
			</div><!-- end layout_content  -->
        
	</div><!-- end layout_full  -->
</div><!-- end layout_body  -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var IsType = "<%=IsType %>"
        var UserItemList = "<%=UserItemList %>"
        var R_Category = "<%=GetResource("Category") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_NoListData = "<%=GetResource("NoListData") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Menu/Setting.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Menu/Setting.js");
    </script>
</body>
</html>