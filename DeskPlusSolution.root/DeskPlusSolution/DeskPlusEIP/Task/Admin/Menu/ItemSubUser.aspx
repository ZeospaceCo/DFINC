<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemSubUser.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Admin.Menu.ItemSubUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
   <meta charset="UTF-8">
   <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
     <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ToRowMax = <%=ToRowMax %>;
        var CcRowMax = <%=CcRowMax %>
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var SubToArrayList = "<%=SubToArrayList %>";
        var SubCcArrayList = "<%=SubCcArrayList %>";               
        var R_ProcessingError = "<%=GetResource("ErrorProcessing") %>";
        var R_ToDept = "<%=GetResource("ToDept") %>";
        var R_SelectAdd = "<%=GetResource("SelectAdd") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Admin/Menu/ItemSubUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Admin/Menu/ItemSubUser.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
   
<div class="layer_popup">   

    <div class="inside">
         <div id="popup_header">
            <h1><%=GetResource("Target")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnSave" runat="server" class="btn btn-secondary"/></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
         </div>
         <div class="dotLine"></div>
         <div class="popup_layout_style3">
		     <table cellspacing="0" border="0"  style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="270px" style="text-align:center;"> 
                        <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true" />
                         </div>
                    </td>
          	        <td valign="top" align="center" style="padding-top:10px;width:100px;" height="200">
                       <div class="layout_scroll" style="min-height:350px;height:400px;" >   
                          <table cellspacing="0" border="0">  
                            <tr>
                                <td style="padding-top:50px;"><cc3:ClientSmartButton ID="CbtnToAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></td>
                            </tr>
                            <tr>   
                                <td style="padding-top:150px;"><cc3:ClientSmartButton ID="CbtnCcAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></td>
                            </tr>
                          </table>
                       </div>
                    </td>                                         
          	        <td valign="top" >    
                        <div class="layout_scroll" style="min-height:350px;height:418px;" >   
                              <table cellspacing="0" border="0">  
                              <tr>
                                  <td>      	        
                                    <div class="layout_scroll"  style="min-height:150px;height:200px;" >   
                                        <table cellspacing="0" id="contentToTable" class="table_typeB" style="width:100% !important;">  
                                        <caption></caption>
                                        <colgroup>
                                            <col width="*" />                           
                                            <col width="70px"/>
                                        </colgroup>
                                        <thead>
                                        <tr>
                                            <th class="center"><%=GetResource("Receive")%><%=GetResource("Target")%></th>                           
                                            <th class="center"><%=GetResource("Del")%></th>
                                        </tr>  
                                        </thead>    
                                         <%=SubToTable%>          
                                        </table>
                                    </div>
          	                    </td>    
                             </tr>                        
                             <tr>                        
                                <td>
                                    <div class="layout_scroll"  style="min-height:150px;height:200px;" >   
                                        <table cellspacing="0" id="contentCcTable" class="table_typeB" style="width:100% !important;">  
                                        <caption></caption>
                                        <colgroup>
                                            <col width="*" />                           
                                            <col width="70px"/>
                                        </colgroup>
                                        <thead>
                                        <tr>
                                            <th class="center"><%=GetResource("Reference")%><%=GetResource("Target")%></th>                          
                                            <th class="center"><%=GetResource("Del")%></th>
                                        </tr>  
                                        </thead>    
                                            <%=SubCcTable%>          
                                        </table>
                                     </div>
                                  </td>
                               </tr>
		                       </table>    

                        </div>
                    </td>
                </tr>
             </table>
         </div>
    </div>
</div>   

    <div style="display: none">    
    <asp:TextBox ID="tbItemSubToUserValue" runat="server" Width="0"></asp:TextBox>      
    <asp:TextBox ID="tbItemSubCcUserValue" runat="server" Width="0"></asp:TextBox>      
    <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>    
</form>
</body>

</html>