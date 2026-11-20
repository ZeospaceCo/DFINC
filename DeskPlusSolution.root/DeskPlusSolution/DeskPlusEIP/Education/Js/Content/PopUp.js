EducationPopUp = {};

EducationPopUp.Close = function () {
    if (parent != window && parent.disablePopup) parent.disablePopup();
    else self.close();
};

EducationPopUp.Hide = function (expiredays) {
    if (!/^EDU_[0-9]+_[0-9]+_[0-9]+$/.test(EducationPopUpCookieName)) return;
    // 게시판 팝업과 동일하게 로컬 자정까지 보이지 않도록 설정합니다.
    var today = new Date();
    var expires = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 1 + expiredays);
    document.cookie = EducationPopUpCookieName + '=yes; path=/; expires=' + expires.toUTCString() + ';';
    EducationPopUp.Close();
};

(function () {
    var actions = [
        { id: 'educationPopupWeek', days: 7 },
        { id: 'educationPopupToday', days: 0 },
        { id: 'educationPopupNever', days: 3650 }
    ];
    for (var i = 0; i < actions.length; i++) {
        (function (action) {
            var button = document.getElementById(action.id);
            if (button) button.onclick = function () { EducationPopUp.Hide(action.days); };
        })(actions[i]);
    }
    var closeButton = document.getElementById('educationPopupClose');
    if (closeButton) closeButton.onclick = EducationPopUp.Close;
})();
