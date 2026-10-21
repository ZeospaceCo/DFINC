AddTrustedSite = {}

$(function () {
    $(document).ready(function () {

        // 저장
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();

            AddTrustedSite.Confirm();
            //txtRepeat
        });
        //닫기

        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

    });
});

/*************************
작성자: 
작성일 
용도: 저장
**************************/
AddTrustedSite.Confirm = function () {
    //등록할 도메인 명
    var strBool = false;

    // 도메인에 대한 모든 사이트를 신뢰하는 사이트로 등록
    //strBool = DPTrustedSites.AddTrustedSite(0, domainName);

    // HTTP로 접근할 때를 신뢰하는 사이트로 등록
    strBool = form1.DPTrustedSites.AddTrustedSite(1, domainName);

    // HTTPS로 접근할 때를 신뢰하는 사이트로 등록
    //strBool = DPTrustedSites.AddTrustedSite(2, domainName);

    if (strBool) {
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    } else {
        alert("등록 오류.");
    }

}

