<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Blog.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.Blog"  EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {


        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '<li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);
    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnBoardBlogPageSize_" + portletDom).val("<%=PageSize %>");
        var boardBlog_PageSize = $("#hdnBoardBlogPageSize_" + portletDom).val();

        // 리스트 전체 카운트
        $("#hdnBoardBlogTotalCount_" + portletDom).val("<%=BoardTotalCount %>");

        // 카테고리 아이디
        $("#hdnBoardBlogCategoryID_" + portletDom).val("<%=BoardCategoryID %>");
        
        if ("<%=IsAuth%>" != "True") {
            $("#cbtnPDel_" + portletDom).remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow");   
        }

        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if($("#hdnTabSlide_" + portletDom).val() == "1")
            {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");     
                $("#hdnTabSlide_" + portletDom).val("0"); 
            }
            else{
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");    
                $("#hdnTabSlide_" + portletDom).val("1");
            }            
        });

        
        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            // 리스트 행 개수 바인딩
            $("#boardBlogListCount_" + portletDom).val($("#hdnBoardBlogPageSize_" + portletDom).val());
            $("#ptListConfig_" + portletDom).slideToggle(600);      
        });

        // 포틀릿 삭제
        $("#cbtnPDel_" + portletDom).click(function (e) {
            e.preventDefault();

            Main.DelTabPortlet(portletDom);
            if (PortletAuth == "True") {
                $("#portlet_" + portletDom).fadeOut("slow");
            }
        });

/*
        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {
                // 첫 페이지로 세팅
                $("#hdnBoardBlogCount_" + portletDom).val("1");
                LoadBoardBlogList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1); 
        });
*/

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
                        
            var paramValue = $("#hdnBoardBlogParamValue_" + portletDom).val();

            if(paramValue != ""){
                //menuCatID=1^catID=57^dcparm=BOARD1
                var paramValueStr = paramValue.split("^");
                var menuCatID = "";
                var catID = "";
                var dcparm = "";

                for(var i=0;i<paramValueStr.length;i++)
                {
                    switch(paramValueStr[i].split("$")[0])
                    {
                        case "menuCatID" :
                            menuCatID = paramValueStr[i].split("$")[1];
                            break;
                        case "catID" :
                            catID = paramValueStr[i].split("$")[1];
                            break;
                        case "dcparm" :
                            dcparm = paramValueStr[i].split("$")[1];
                            break;
                    }
                }
                
                ParamArray = {
                    'menuCatID': menuCatID,
                    'catID': catID,
                    'dcparm': dcparm
                };

                CallEncrypt(CallGetParam());            
                location = "/DeskPlusEIP/Board/Default.aspx?p=" + GetEncryptValue;
            }
        });  


        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            boardBlog_PageSize = $("#boardBlogListCount_" + portletDom).val();
            $("#hdnBoardBlogPageSize_" + portletDom).val(boardBlog_PageSize);            

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", boardBlog_PageSize, "");

            // 첫 페이지로 세팅
            $("#hdnBoardBlogCount_" + portletDom).val("1");
            LoadBoardBlogList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        //$("#ptListConfig_" + portletDom).hide();

        $("#hdnBoardBlogCount_" + portletDom).val("1");

        //setTimeout(function () {
            LoadBoardBlogList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);
    });

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadBoardBlogList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/Blog.asmx/GetBoardList";
        
        ParamArray = {
            'pageSize': $("#hdnBoardBlogPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnBoardBlogCount_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnBoardBlogTotalCount_" + portletDom).val(), // 전체 페이지 카운트
            'categoryID': $("#hdnBoardBlogCategoryID_" + portletDom).val()  // 카테고리 아이디
        };

        CallEncrypt(CallGetParam());

        var paramData = "{p:'" + GetEncryptValue + "'}";

        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTBlog.htm?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);

                if($(".NoListData") != undefined) {                    
                    $(".NoListData").text(R_NoContent);
                 }
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
    function BoardBlog_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var boardBlog_PageSize = $("#hdnBoardBlogPageSize_" + portletDom).val();        // 리스트 행 개수
        var boardBlog_Count = $("#hdnBoardBlogCount_" + portletDom).val();              // 페이지Nuumber
        var boardBlog_TotalCount = $("#hdnBoardBlogTotalCount_" + portletDom).val();    // 전체 페이지 카운트        
        
        // MAX페이지 가져오기
        var boardBlog_MaxCount = 1;

        if (boardBlog_TotalCount > 0) {

            // 몫
            var mok = parseInt((boardBlog_TotalCount / boardBlog_PageSize));
            // 나머지
            var balance = boardBlog_TotalCount % boardBlog_PageSize;

            if (balance != 0) {
                boardBlog_MaxCount = mok + 1;
            }
            else {
                boardBlog_MaxCount = mok;
            }
        }

        if (arg == "next") {
            boardBlog_Count = parseInt(boardBlog_Count) + 1;
        }
        else if (arg == "before") {
            boardBlog_Count = parseInt(boardBlog_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (boardBlog_Count < 1) {
            boardBlog_Count = 1;
            $("#hdnBoardBlogCount_" + portletDom).val(boardBlog_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (boardBlog_Count > boardBlog_MaxCount) {
            boardBlog_Count = boardBlog_MaxCount;
            $("#hdnBoardBlogCount_" + portletDom).val(boardBlog_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnBoardBlogCount_" + portletDom).val(boardBlog_Count);

        LoadBoardBlogList_<%=TabPortletID%>_<%=PortletID%>();
    }
    
</script>

<!-- 포틀릿(리스트:뉴스형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">    
    
        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
            </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>

        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="boardBlogListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </div>
                </div>
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

       <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">           
             <div class="portlet-contents">
                <div class="nav-tabs">                                   
                </div>

                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a class="page-link" tabindex="-1" onclick="BoardBlog_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a class="page-link" onclick="BoardBlog_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>

            </div>
        </div> 

        <div style="display: none">
            <input type="hidden" id="hdnBoardBlogCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardBlogCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnBoardBlogCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardBlogCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnBoardBlogTotalCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardBlogTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnBoardBlogPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardBlogPageSize_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnBoardBlogParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardBlogParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValue %>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>

</div>
<!-- 포틀릿(리스트:뉴스형) End //-->