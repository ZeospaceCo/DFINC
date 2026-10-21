<%@ Page Language="c#" CodeBehind="CalendarDialog.aspx.cs" AutoEventWireup="false" Inherits="Zeospace.DeskPlus.Common.Web.Calendar.CalendarDialog" %>
<%@ Register TagPrefix="uc1" TagName="Calendar" Src="Calendar.ascx" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
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

    <script language="javascript">
        $(function () {
            $(document).ready(function () {      
            
                $('#cbtnConfirm').click(function (e) {
                    CalendarSelect();                   
                });           
                
                $('#cbtnCancel').click(function (e) {
                    WinClose();                   
                });               

            });
        });

		function CalendarSelect()
		{
		    var dateSeparator = "<%=Request["dateSeparator"]%>"; // 나중에 적용
		
			var of = opener.document.forms[0];
			var f = document.forms.Calendar;			
			
			var hdYear1 = f.hdYear1.value;
			var hdMonth1 = f.hdMonth1.value;
			var hdDay1 = f.hdDay1.value;

            var hdYear2 = f.hdYear2.value;
			var hdMonth2 = f.hdMonth2.value;
			var hdDay2 = f.hdDay2.value;

            var startDate	= new Date(hdYear1, hdMonth1 - 1, hdDay1); //시작일
			var endDate		= new Date(hdYear2, hdMonth2 - 1, hdDay2); //종료일

            var companyID = 0;
            var holiYN = "N";   //휴일 플래그
            var holyCnt = 0;    //총 휴일 수            
            var yoil = "";      //요일
            
            //날짜 기간 구하기
            var interval = endDate - startDate;
            var day = 1000*60*60*24;	
            var oDuration = parseInt(interval/day) + 1;
//               
//            //날짜 기간 동안 휴일 수 체크         
//            for (var i = 0; i < oDuration; i++)
//            {   
//                //요일 구하기
//                yoil = getDayOfWeek(startDate);
//                companyID = <%=Request["companyID"]%>
//                //공휴일인지 체크
//                holiYN = AjaxGetToDayByHolyS(companyID, startDate.getFullYear(), (startDate.getMonth() + 1), startDate.getDate());

//                //주말이 아닌 공휴일일때 총 휴일 수 카운팅
//                if (holiYN == "Y" && yoil != "토" && yoil != "일")
//                {
//                    holyCnt += 1;
//                }            

//                //주말일때 총 휴일 수 카운팅
//                if (yoil == "토" || yoil == "일")
//                {
//                    holyCnt += 1;
//                }

//                //1일 더하기
//                startDate.setDate(startDate.getDate() + 1); 
//            }
            
            //시작일 원래대로
			startDate = new Date(hdYear1, hdMonth1 -1, hdDay1);
            
            /*
            var startHalf = 0;
            var endHalf = 0;

            startHalf = document.getElementById("ddlStartHalf").value;
            endHalf = document.getElementById("ddlEndHalf").value;
            */
            
            var selectDate1 = hdYear1 + "-" + hdMonth1 + "-" + hdDay1;
            var selectDate2 = hdYear2 + "-" + hdMonth2 + "-" + hdDay2;
            /*
            if (selectDate1 == selectDate2)
            {
                if (startHalf != "0" && endHalf != "0") {
                    alert("휴가기간은 0일이 될 수 없습니다.");
                    return;
                } 
            }

            if (selectDate1 != selectDate2)
            {
                if (startHalf == "1" || endHalf == "2") {
                    alert("휴가기간은 연속된 날짜만 등록 가능합니다.");
                    return;
                } 
            }
			*/
			if ("<%=Request["func"]%>" != "")
			{
			    if ("<%=Request["dateLength"]%>" == "8")
			    {
				    if (hdMonth1.length == 1)
				    {
				        hdMonth1 = "0" + hdMonth1; 
				    }
				    if (hdDay1.length == 1)
				    {
				        hdDay1 = "0" + hdDay1; 
				    }
				    if (hdMonth2.length == 1)
				    {
				        hdMonth2 = "0" + hdMonth2; 
				    }
				    if (hdDay2.length == 1)
				    {
				        hdDay2 = "0" + hdDay2; 
				    }
				}
				
                //주말, 공휴일을 뺀 실제 일수도 같이 넘긴다.
                top.opener.<%=Request["func"]%>(hdYear1, hdMonth1, hdDay1, hdYear2, hdMonth2, hdDay2, oDuration);
			}

			window.close();
		}

        //요일 구하기
        function getDayOfWeek(now) 
        {	
		    var day = now.getDay(); //일요일=0,월요일=1,...,토요일=6
		    var week = new Array('일','월','화','수','목','금','토');
		    return week[day];
	    }

		function WinClose()
		{            
			window.returnValue='close';
			self.close();			
		}
		/*
		function window.onload()
        {
            
			try 
            {
				aW = 160;
				aH = 188;
				w = screen.availWidth;
				h = screen.availHeight;

				window.moveTo ((w-aW)/2, (h-aH)/2);
			}catch(e)
            {
				document.location.reload();
			}
           
		}
        */
    </script>
    
</head>
<body>
    <form id="Calendar" name="Calendar" method="post" runat="server">
    <table id="LayoutTable" border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <table width="100%" cellpadding="0" cellspacing="15" border="0" align="center" class="table_typeC">
                                                                    <tr>
                                                                        <th width="100%" align="left">
                                                                            <asp:DropDownList ID="ddlStartYear" runat="server" OnSelectedIndexChanged="ddlStartYear_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                            <asp:DropDownList ID="ddlStartMonth" runat="server" OnSelectedIndexChanged="ddlStartMonth_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                            <asp:DropDownList ID="ddlStartDay" runat="server" OnSelectedIndexChanged="ddlStartDay_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>                                                                            
                                                                        </th>                                                                                                                                       
                                                                    </tr>                                                        
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>                                           
                                        </table>
                                        <!--//############################### Content Start ##################################//-->
                                    </td>
                                    <td valign="top" width="5px"></td>
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
                                                                            <uc1:Calendar ID="Calendar2" runat="server"></uc1:Calendar>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" cellpadding="0" cellspacing="5" border="0" align="center" class="table_typeC">
                                                                    <tr>
                                                                        <th width="100%" align="left">
                                                                            <asp:DropDownList ID="ddlEndYear" runat="server" OnSelectedIndexChanged="ddlEndYear_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                            <asp:DropDownList ID="ddlEndMonth" runat="server" OnSelectedIndexChanged="ddlEndMonth_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                                                            <asp:DropDownList ID="ddlEndDay" runat="server" OnSelectedIndexChanged="ddlEndDay_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>                                                                                                                                                        
                                                                        </th>                                                                 
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                </td>
                                            </tr>                                          
                                        </table>
                                        <!--//############################### Content Start ##################################//-->
                                    </td>
                                </tr>
                                <tr>
								    <td colspan="3">
									    <table cellspacing="0" cellpadding="0" width="100%" border="0">
										    <tr>
											    <td style="PADDING-RIGHT: 3px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 7px" align="right">
												    <table cellspacing="0" cellpadding="0" border="0">
													    <tr>
														    <td align="right" class="TdGap">    
                                                                <ul class="btn_right">                                                                                                                            
                                                                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                                                                </ul>
                                                            </td>
														    <td align="left" class="TdGap">   
                                                                <ul class="btn_right">                                                                                                                             
                                                                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                                                                </ul>
                                                            </td>
													    </tr>
												    </table>
											    </td>
										    </tr>
									    </table>
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
    <INPUT type="hidden" runat="server" id="hdYear1" NAME="hdYear1"> 
    <INPUT type="hidden" runat="server" id="hdMonth1" NAME="hdMonth1">
	<INPUT type="hidden" runat="server" id="hdDay1" NAME="hdDay1"> 
    <INPUT type="hidden" runat="server" id="hdYear2" NAME="hdYear2">
    <INPUT type="hidden" runat="server" id="hdMonth2" NAME="hdMonth2"> 
    <INPUT type="hidden" runat="server" id="hdDay2" NAME="hdDay2">
    <!-- ==================================================================================== -->
    </form>
</body>
</html>
