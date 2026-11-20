TargetList = {}

$(function () {
    $('#cbtnClose').click(function (e) {
        e.preventDefault();
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    });

    if (!HasEducation) return;

    // 데이터를 불러오기 전에 공통 페이저를 초기화합니다.
    $('#pager').pager({
        pager: '#pager',
        pagerMode: 'text',
        totalItemCount: TotalItemCount,
        recordPerPage: RecordPerPage,
        currentPage: CurrentPage,
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            TargetList.LoadData();
        },
        prev10Text: R_Previous,
        next10Text: R_Next,
        selectedPageFontColor: 'orange'
    });

    $('#ddlListCount').change(function () {
        TargetList.PageSize();
    });

    TargetList.LoadData();
});

/***********************************************************************************************/
TargetList.PageSize = function () {
    ParamArray = {
        'listID': ListID,
        'recordPerPage': $('#ddlListCount').val()
    };
    CallEncrypt(CallGetParam());
    window.location.href = '/DeskPlusEIP/Education/Admin/TargetList.aspx?p=' + encodeURIComponent(GetEncryptValue);
}

/***********************************************************************************************/
TargetList.LoadData = function () {
    var pageCount = $('#pager').pager.refresh(TotalItemCount, RecordPerPage);
    $('#pageInfo').text((pageCount > 0 ? CurrentPage : 0) + ' / ' + pageCount);
    TargetList.GetPageList();
}

/***********************************************************************************************/
TargetList.GetPageList = function () {
    ParamArray = {
        'listID': ListID,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };
    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var url = '/DeskPlusEIP/Education/Ws/Admin/TargetList.asmx/GetPageList';
    AjaxService(url, data, TargetList.SuccessPageList);
}

/***********************************************************************************************/
TargetList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Education/Jt/Admin/JTTargetList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
}
