<%@ Page Language="c#" CodeBehind="CalendarDialogById.aspx.cs" AutoEventWireup="false" Inherits="Zeospace.DeskPlus.Common.Web.Calendar.CalendarDialogById" %>
<%@ Register TagPrefix="uc1" TagName="Calendar" Src="Calendar.ascx" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    
    <script language="javascript">

        $(function () {
            $(document).ready(function () {

                //확인
                $('#cbtnConfirm').click(function (e) {
                    e.preventDefault();
                    CalendarSelect();
                });

                //취소
                $('#cbtnCancel').click(function (e) {
                    e.preventDefault();
                    //if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
                    window.returnValue='close';
			        self.close();
                });

            });
        });

		function CalendarSelect()
		{
		    var dateSeparator = "<%=Request["dateSeparator"]%>"; // 나중에 적용
		
			var of = opener.document.forms[0];
			var f = document.Calendar;			
			
			//var hdYear = f.hdYear.value;
			//var hdMonth = f.hdMonth.value;
			//var hdDay = f.hdDay.value;

            var hdYear = $("#hdYear").val();
			var hdMonth = $("#hdMonth").val();
			var hdDay = $("#hdDay").val();
			
			if ("<%=Request["func"]%>" != "")
			{
			    if ("<%=Request["dateLength"]%>" == "8")
			    {
				    if (hdMonth.length == 1)
				    {
				        hdMonth = "0" + hdMonth; 
				    }
				    if (hdDay.length == 1)
				    {
				        hdDay = "0" + hdDay; 
				    }
				    top.opener.<%=Request["func"]%>("<%=Request["tagID"]%>", hdYear, hdMonth, hdDay);
				}
				else
				{
                    
				    if (hdMonth.length == 1)
				    {
				        hdMonth = "0" + hdMonth; 
				    }
				    if (hdDay.length == 1)
				    {
				        hdDay = "0" + hdDay; 
				    }
                    
				    top.opener.<%=Request["func"]%>("<%=Request["tagID"]%>",hdYear, hdMonth, hdDay);
				    
				    //top.opener.<%=Request["func"]%>(hdYear, hdMonth, hdDay);
				}
			}

			window.close();
		}
		
		/*
		function window.onload(){
			try {
				aW = 160;
				aH = 188;
				w = screen.availWidth;
				h = screen.availHeight;

				window.moveTo ((w-aW)/2, (h-aH)/2);
			}catch(e){
				document.location.reload();
			}
		}
        */
		</script>
    <style type="text/css">
        
    .CalendarSingle
    {
      font-family:"gulim";
      font-size:9pt;
      width:100%;
      height:100%;
    }

    .TitleStyle td
    {
      font-size:9pt;
      font-weight:bold;
      padding-top:3px;
      padding-bottom:3px;
      color:#5b5b5b;
      background-color:#ebebeb;
    }

    .DayStyle
    {
      padding-top:2px;
      background-color:#FFFFFF;
      text-align:center;
      border-width:0px;
      border-color:#FFFFFF;
      border-style:solid;
      cursor:pointer;

      font-size:9pt;
    }


    .DayStyle A:link
    {
       text-decoration:none;
       color:blue;
    }

    .HeaderSun { COLOR:#e87657; font-family:"gulim","MS UI Gothic"; font-size:9pt; font-weight:bold; text-decoration:none; }
    .HeaderSat { COLOR:#699add; font-family:"gulim","MS UI Gothic"; font-size:9pt; font-weight:bold; text-decoration:none; }


    .DayHeaderStyle
    {
      padding-top:3px;
      font-weight:bold;
      padding-bottom:3px;
      color:#5F5F5F;
      font-size:9pt;
      background-color:#FFFFFF;
    }

    .WeekendDayStyle
    {
      padding-top:2px;
      background-color:#FFFFFF;
    }

    .OtherMonthDayStyle
    {
    /***
      padding-top:2px;
      background-color: white;
      border-width:1px;
      border-color:white;
      border-style:solid;
      cursor:pointer;
      ***/

     _visibility:hidden;
    }
    .OtherMonthDayStyle a{visibility:hidden;}

    .NextPrevStyle
    {

    }

    .TodayDayStyle
    {
      padding-top:2px;
      font-weight:bold;
      text-decoration:underline;
    }

    .SelectorStyle
    {
      text-decoration:none;
    }

    .SelectedDayStyle
    {
      padding-top:2px;
      font-weight:bold;
    }
    
    .TdPopUpBoxLine
    {
	    background-color	:#DCE1E7;
    } 

    .TdAreaBGA
	{
		border-top : solid 1  #D9D5C5;
		border-bottom : solid 1 #D9D5C5; 
		border-left : solid 1  #D9D5C5;
		border-right : solid 1  #D9D5C5;
		background-color:#EDEAE5; 
	}
    </style>
</head>
<body >
    <form id="Calendar" name="Calendar" method="post" runat="server">
    <table id="LayoutTable" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td width="5" rowspan="5"></td>
            <td height="5" colspan="3"></td>
            <td width="5" rowspan="5"></td>
        </tr>
        <tr>
            <td width="1" height="1" class="TdPopUpBoxLine"></td>
            <td></td>
            <td width="1" height="1" class="TdPopUpBoxLine"></td>
        </tr>
        <tr>
            <td width="1"></td>
            <td align="center" valign="top">
                <table width="100%" height="100%" cellspacing="5" border="0">
                    <tr>
                        <td valign="top">
                            <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
                                <!--//############################### Title End ##################################//-->
                                <tr>
                                    <td valign="top">
                                        <!--//############################### Content Start ##################################//-->
                                        <table width="100%" height="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <table width="100%" cellpadding="2" cellspacing="0">
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <uc1:Calendar ID="Calendar1" runat="server"></uc1:Calendar>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table cellpadding="5" cellspacing="0" border="0" align="center" class="table_typeC">
                                                        <tr>
                                                            <th align="left">
                                                                <asp:DropDownList ID="ddlYear" runat="server" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                &nbsp;
                                                                <asp:DropDownList ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                &nbsp;
                                                                <asp:DropDownList ID="ddlDay" runat="server" OnSelectedIndexChanged="ddlDay_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                 &nbsp;
                                                            </th>                                                        
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                 <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" />
                                                                 <cc3:ClientSmartButton ID="cbtnCancel" runat="server" />                                                              
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <!--//############################### Content Start ##################################//-->
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="1"></td>
        </tr>
        <tr>
            <td height="1" class="TdPopUpBoxLine"></td>
            <td></td>
            <td height="1" class="TdPopUpBoxLine"></td>
        </tr>
        <tr>
            <td height="5" colspan="3"></td>
        </tr>
    </table>


                

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input type="hidden" runat="server" id="hdYear">
        <input type="hidden" runat="server" id="hdMonth">
        <input type="hidden" runat="server" id="hdDay">
    </div>
    <!-- ==================================================================================== -->
    </form>
</body>
</html>
