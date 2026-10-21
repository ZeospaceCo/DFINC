<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListTot.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.ListTot" EnableViewState="false" %>
<script type="text/javascript">        
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        //conMenu += '<li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //총카운트 가져오기
        var boardTotalCount = "<%= BoardTotalCount %>";
        if(Number(boardTotalCount) > 50) { boardTotalCount = "50"; }
        $("#hdnBoardListTotalCount_" + portletDom).val(boardTotalCount)

        // 리스트행 개수 -- 6 초기화
        $("#hdnBoardListPageSize_" + portletDom).val("6");
        var boardList_PageSize = $("#hdnBoardListPageSize_" + portletDom).val();        

        // 카테고리 아이디 -- 전체카테고리임(미사용)
        $("#hdnBoardListCategoryID_" + portletDom).val("0");        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }
        
        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {        
            
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);
            
            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnBoardListPageSize_" + portletDom).val());
        });

        //상세바로가기
        $(".ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();
            //전체 리스트 가져오는거라.. 갈곳이 없음..
        });        

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            //미사용
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        $("#hdnBoardListCount_" + portletDom).val("0");

        //setTimeout(function () {            
                LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
         //   }, 1);
        
        /*
        if ($.browser.msie) {
		    LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
	    }
	    else
	    {
            setTimeout(function () {            
                LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
            }, 1);
	    }
        */

    });

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadBoardList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        

        //게시판 리스트
        urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardTotList";

        ParamArray = {
            'pageSize': $("#hdnBoardListPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnBoardListCount_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnBoardListTotalCount_" + portletDom).val() // 전체 페이지 카운트
        };

        CallEncrypt(CallGetParam());
        paramData = "{p:'" + GetEncryptValue + "'}";

        $.ajax({
            type: "POST",
            url: urlPage,
            async: true,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTList.htm?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);

                if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}
                if($(".TitleIcon") != undefined) {$(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif');}

            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
    }

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function BoardList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var boardList_PageSize = $("#hdnBoardListPageSize_" + portletDom).val();        // 리스트 행 개수
        var boardList_Count = $("#hdnBoardListCount_" + portletDom).val();              // 페이지Nuumber
        var boardList_TotalCount = $("#hdnBoardListTotalCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var boardList_MaxCount = 1;
        if (boardList_TotalCount > 0) {

            // 몫
            var mok = parseInt((boardList_TotalCount / boardList_PageSize));
            // 나머지
            var balance = boardList_TotalCount % boardList_PageSize;

            if (balance != 0) {
                boardList_MaxCount = mok + 1;                
            }
            else {
                boardList_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            boardList_Count = parseInt(boardList_Count) + 1;
        }
        else if (arg == "before") {
            boardList_Count = parseInt(boardList_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (boardList_Count < 1) {
            boardList_Count = 1;
            $("#hdnBoardListCount_" + portletDom).val(boardList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (boardList_Count > boardList_MaxCount) {            
            boardList_Count = boardList_MaxCount;
            $("#hdnBoardListCount_" + portletDom).val(boardList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnBoardListCount_" + portletDom).val(boardList_Count);

        LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
    }

    /**************************************************
        작성자   : 
        작성일자 : 2011-05-10
        작성목적 : 새창으로 댓글리스트 보기
    ***************************************************/
    function TabBoardShort(p_categoryID, p_listID) {
    //alert(p_categoryID);
    //alert(p_listID);
        ParamArray = {
            'categoryID': p_categoryID,
            'listID': p_listID
        };

        CallEncrypt(CallGetParam());

        //var url = "/DeskPlusEIP/Board/Content/ShortReplyList.aspx?p=" + GetEncryptValue;
        var url = "/DeskPlusEIP/Board/Content/ViewShortReplyList.aspx?p=" + GetEncryptValue;
        //PopupWin(800, 690, url, "", "", true);     
        WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList", "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
    } 

    
</script>

<!-- 포틀릿(리스트:일반형) Start //-->

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">  
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" > 
        <div class="portlet-header">
        <li class="title headtitle"><a class="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
        <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset">
		<!--<a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a>//-->		
	</li>
        </div>
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" style="display:none;">
            <li class="poboxlistset">
            <span class="listsetbox">
                <select id="boardListCount_<%=TabPortletID%>_<%=PortletID%>">
                    <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                    <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                    <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                    <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                </select>
            </span>
            <span class="listsetbtn"><a id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><%=GetResource("Save")%></a></span>
            <!--span class="listsetbtn"><a id="cbtnClose_<%=TabPortletID%>_<%=PortletID%>"><%=GetResource("Close")%></a></span-->
            </li>
            </div>

            <div id="plList_<%=TabPortletID%>_<%=PortletID%>" style="min-height:125px;">
                <div style="padding-top:50px;text-align:center;">
                    
                </div>
            </div>

            <li>
            <ul class="ptl_more">
                <a onclick="BoardList_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><img src="<%=BaseImgPath%>/Main/ico_b.gif" alt="<%=GetResource("Prev")%>" title="<%=GetResource("Prev")%>" /></a>
                <a onclick="BoardList_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><img src="<%=BaseImgPath%>/Main/ico_a.gif" alt="<%=GetResource("Next")%>" title="<%=GetResource("Next")%>" /></a>
            </ul>
            </li>
            <div style="display: none">
                <input type="hidden" id="hdnBoardListCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListCount_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnBoardListCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnBoardListTotalCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnBoardListPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListPageSize_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnBoardListParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardListParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValue %>" />
            </div>
        </div>
    </ul>
</div>
<!-- 포틀릿(리스트:일반형) End //-->