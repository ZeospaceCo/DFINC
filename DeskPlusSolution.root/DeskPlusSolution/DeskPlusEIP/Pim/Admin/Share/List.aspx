<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.Share.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
     <form id="form1" runat="server">
       <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("ShareManage")%></h1>
            <!-- 버튼 //-->

            <ul class="btn_right">

            </ul>
        </div>
        
        
        <div id="layout_info">
        
        

        
        
        <table cellspacing="0" class="table_typeA"> 
        <colgroup>
                    <col width="250px" />
                    <col width="270px"  />
                    <col width="110px" />
                    <col width="/" />
                    
			    </colgroup>
              <thead>
                   <tr>
                        <th><%=GetResource("ObjePerson")%></th>
                        <th><%=GetResource("SelectObjectPerson")%></th>
                        <th></th>
                        <th><%=GetResource("CurrentObjectPerson")%></th>
                    </tr>
             </thead>
         <tr >
         <td valign="top"  style="text-align:center;"> 
           <iframe id="frmFolderContent" name="frmFolderContent" class="pane ui-layout-center" width="100%" height="800px" frameborder="0" scrolling="auto" src="ShareUserPop.aspx" >
           </iframe>
         </td>

         <td   valign="top" > 
          	  <div class="layer_scroll_box" style="height:800px">
                <zeoTree:UCUserDeptTreeScript ID="UCUserDeptTreeScript1" runat="server" />
               </div>
            </td>

         <td valign="top"   >
              <ul class="btn_right">
               <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" ></cc3:ClientSmartButton></li>
             </ul>
         </td>

         <td valign="top">
             <div class="layer_scroll_box_250">
              <table cellspacing="0" id="contentTable"   style="width:100% !important;">  
              <tr>
                       <th class="table_title"><%=GetResource("ObjePerson")%></th>                            
                       <th class="table_100" align="center"><%=GetResource("Del")%></th>
             </tr>
             </table>
             </div>
             <br />
               <ul class="btn_right">
                   <li >
                      <cc3:ClientSmartButton ID="cbtnConfirm" runat="server"  ></cc3:ClientSmartButton> 
                   </li>
               </ul>
           </td>      
    </tr>   
    
    </table>
        
        
    </div>

   </div>
   
   <div style="display: none">
      <input name="hdseluser" type="text" id="hdseluser" />

        <input name="tvSelectedID" type="text" id="tvSelectedID" />
         <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
         <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
         <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>        
    <script type="text/javascript">
        var CatID = "<%=CatID %>";
        var CategoryID = "<%=CategoryID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var UserId = "<%=UserID%>";
        var CompanyID = "<%=CompanyID%>";

        function PopAddUser(val) {
            var arSel = val.split('|');
            if (arSel[0] == "M") {
                $("#hdseluser").val(arSel[1]);
                List.GetOpenUser(arSel[1]);
            }
        }

        var objectSelect = '<%=GetResource("objectSelect") %>';
        var ShareSelect = '<%=GetResource("ShareSelect") %>';
        var SharePesonSuccess = '<%=GetResource("SharePesonSuccess") %>';

    </script> 
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/Share/List.js" ></script>    
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Pim/Js/Admin/Share/List.js");
    </script>
</body>
</html>
