<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ToSchList.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.ToSchList" %>

<script type="text/javascript">
    var ToSchList_Count = 1;
    var ToSchList_TotalCount = "<%=ToSchListTotalCount %>";
    var ToSchList_PageSize = "<%=PageSize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnToSchListPageSize_" + portletDom).val("<%=PageSize %>");
        var toSchListPageSize = $("#hdnToSchListPageSize_" + portletDom).val();

        // 리스트 전체 카운트
        $("#hdnToSchListTotalCount_" + portletDom).val("<%=ToSchListTotalCount %>");

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
            $("#toSchListCount_" + portletDom).val($("#hdnToSchListPageSize_" + portletDom).val());
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

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnToSchListCount_" + portletDom).val("1");
                LoadToSchList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);            
        });

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
             
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";
            //parent.frmTop.Menu.GoParam("WebApproval");
        });

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            toSchListPageSize = $("#toSchListCount_" + portletDom).val();
            $("#hdnToSchListPageSize_" + portletDom).val(toSchListPageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", toSchListPageSize);

            // 첫 페이지로 세팅
            $("#hdnToSchListCount_" + portletDom).val("1");
            LoadToSchList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        $("#hdnToSchListCount_" + portletDom).val("1");

        //setTimeout(function () {
            LoadToSchList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);

    });

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadToSchList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        
        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/ToSchList.asmx/GetToSchList";

        ParamArray = {
            'pageSize': $("#hdnToSchListPageSize_" + portletDom).val(),      // 리스트 행 개수
            'pageSizeNo': $("#hdnToSchListCount_" + portletDom).val(),       // 페이지Nuumber
            'totalCount': $("#hdnToSchListTotalCount_" + portletDom).val()   // 전체 페이지 카운트
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
                $('#phToSchList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/JTToSchList.htm?seq=" + Guids(), null, { filter_data: false });
                $('#phToSchList_' + portletDom).processTemplate(data.d);

                if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}
                if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

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
    function ToSchList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var toSchList_PageSize = $("#hdnToSchListPageSize_" + portletDom).val();        // 리스트 행 개수
        var toSchList_Count = $("#hdnToSchListCount_" + portletDom).val();              // 페이지Nuumber
        var toSchList_TotalCount = $("#hdnToSchListTotalCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var toSchList_MaxCount = 1;
        if (toSchList_TotalCount > 0) {

            // 몫
            var mok = parseInt((toSchList_TotalCount / toSchList_PageSize));
            // 나머지
            var balance = toSchList_TotalCount % toSchList_PageSize;

            if (balance != 0) {
                toSchList_MaxCount = mok + 1;                
            }
            else {
                toSchList_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            toSchList_Count = parseInt(toSchList_Count) + 1;
        }
        else if (arg == "before") {
            toSchList_Count = parseInt(toSchList_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (toSchList_Count < 1) {
            toSchList_Count = 1;
            $("#hdnToSchListCount_" + portletDom).val(toSchList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (toSchList_Count > toSchList_MaxCount) {            
            toSchList_Count = toSchList_MaxCount;
            $("#hdnToSchListCount_" + portletDom).val(toSchList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnToSchListCount_" + portletDom).val(toSchList_Count);

        LoadToSchList_<%=TabPortletID%>_<%=PortletID%>();
    }

    function ToSch_PopupView(arg, dis) {

        var url = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
        ParamArray = {
            'ScheduleID': arg,
            'DisplayID': dis
        };
        CallEncrypt(CallGetParam());
        WindowObjectReference = PopupWinOpen(url, "p=" + GetEncryptValue, "pimpop", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");    
    }

</script>

<!-- 포틀릿(리스트:일반형) Start //-->
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
                    <select id="toSchListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
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
            <div class="content-tabs" id="phToSchList_<%=TabPortletID%>_<%=PortletID%>">    
            </div>
            <!-- 이전/이후 //-->
            <nav aria-label="Page navigation example">
                <ul class="pagination pagination-sm justify-content ptl_more">
                <li class="page-item"><a class="page-link" tabindex="-1" onclick="ToSchList_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                <li class="page-item"><a class="page-link" onclick="ToSchList_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                </ul>
            </nav>

        </div>
    </div>   
         
    <div style="display: none">
        <input type="hidden" id="hdnToSchListTotalCount_<%=TabPortletID%>_<%=PortletID%>"  name="hdnToSchListTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnToSchListPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnToSchListPageSize_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnToSchListCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnToSchListCount_<%=TabPortletID%>_<%=PortletID%>" />
        <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
    </div> 
</div>
<!-- 포틀릿(리스트:일반형) End //-->