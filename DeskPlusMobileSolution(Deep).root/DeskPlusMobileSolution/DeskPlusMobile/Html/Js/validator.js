// ------------------------------------------------------------------------------------------------
// - 폼체크 기능 (from ZEOSPACE) 
// ------------------------------------------------------------------------------------------------
/**
* 폼체크 클래스 - checkForm 없을 경우 id로 체크함 - alertFunc 없을 경우 alert() 경고창
* 
* @param {form object} checkForm 체크할 폼
* @param {function object} alertFunc 경고창을 표시할 함수
*/
Validator = function (checkForm, alertFunc) {
    this.checkForm = checkForm;
    this.validateObjectArray = new Array();
    this.alertFunc = alertFunc;  // 경고창 함수
}

/**
* 유효성 객체
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.ValidateObject = function (identifier, errorMessage, focusFlag) {
    this.identifier = identifier;
    this.errorMessage = errorMessage;
    this.focusFlag = focusFlag;
}

/**
* id 또는 name으로 엘리먼트 객체를 획득
* 
* @param {string} identifier id 또는 name
* @return {element object} 엘리먼트 객체
*/
Validator.prototype.getElement = function (identifier) {
    if (this.checkForm && this.checkForm.nodeName == "FORM") {
        return this.checkForm[identifier];
    } else {
        return document.getElementById(identifier);
    }
}

/**
* 유효성 검사
* 
* @return {boolean} 유효성 검사결과
*/
Validator.prototype.validate = function () {
    for (var i = 0; i < this.validateObjectArray.length; i++) {
        var validateObject = this.validateObjectArray[i];
        if (!validateObject.validate()) {
            if (this.alertFunc) this.alertFunc(validateObject.errorMessage);
            else alert(validateObject.errorMessage);
            if (validateObject.focusFlag) {
                this.getElement(validateObject.identifier).focus();
            }
            this.validateObjectArray = new Array();
            return false;
        }
    }
    this.validateObjectArray = new Array();
    return true;
}

/**
* 값을 입력했는지의 여부를 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkRequired = function (identifier, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        return (checkElement.value).replace(/^\s*/, "").replace(/\s*$/, "") != "";
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값의 길이가 maxLength보다 작거나 같은지 검사
* 
* @param {string} identifier id 또는 name
* @param {int} maxLength 체크값
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkMaxLength = function (identifier, maxLength, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var stringValue = checkElement.value;
        alert("callme " + stringValue.length <= maxLength);
        return (stringValue.length <= maxLength);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값의 길이가 minLength보다 크거나 같은지 검사
* 
* @param {string} identifier id 또는 name
* @param {int} minLength 체크값
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkMinLength = function (identifier, minLength, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var stringValue = checkElement.value;
        return (minLength <= stringValue.length);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값의 길이가 minLength와 maxLength사이에 있는지 검사
* 
* @param {string} identifier id 또는 name
* @param {int} minLength 체크값
* @param {int} maxLength 체크값
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkBetweenLength = function (identifier, minLength, maxLength, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var stringValue = checkElement.value;
        return (minLength <= stringValue.length && stringValue.length <= maxLength);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값의 바이트 길이가 maxLength보다 작거나 같은지 검사
* 
* @param {string} identifier id 또는 name
* @param {int} maxLength 체크값
* @param {int} kbyte 한글바이트수
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkMaxLengthByte = function (identifier, maxLength, kbyte, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var stringValue = checkElement.value;
        var nbytes = 0;
        for (i = 0; i < stringValue.length; i++) {
            var ch = stringValue.charAt(i);
            if (escape(ch).length > 4) {
                nbytes += kbyte;
            } else if (ch == '\n') {
                if (stringValue.charAt(i - 1) != '\r') {
                    nbytes += 1;
                }
            } else if (ch == '<' || ch == '>') {
                nbytes += 4;
            } else {
                nbytes += 1;
            }
        }
        return (nbytes <= maxLength);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값의 바이트 길이가 minLength보다 작거나 같은지 검사
* 
* @param {string} identifier id 또는 name
* @param {int} minLength 체크값
* @param {int} kbyte 한글바이트수
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkMinLengthByte = function (identifier, minLength, kbyte, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var stringValue = checkElement.value;
        var nbytes = 0;
        for (i = 0; i < stringValue.length; i++) {
            var ch = stringValue.charAt(i);
            if (escape(ch).length > 4) {
                nbytes += kbyte;
            } else if (ch == '\n') {
                if (stringValue.charAt(i - 1) != '\r') {
                    nbytes += 1;
                }
            } else if (ch == '<' || ch == '>') {
                nbytes += 4;
            } else {
                nbytes += 1;
            }
        }
        return (nbytes >= minLength);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값의 바이트 길이가 minLength와 maxLength사이에 있는지 검사
* 
* @param {string} identifier id 또는 name
* @param {int} minLength 체크값
* @param {int} maxLength 체크값
* @param {int} kbyte 한글바이트수
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkBetweenLengthByte = function (identifier, minLength, maxLength, kbyte, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var stringValue = checkElement.value;
        var nbytes = 0;
        for (i = 0; i < stringValue.length; i++) {
            var ch = stringValue.charAt(i);
            if (escape(ch).length > 4) {
                nbytes += kbyte;
            } else if (ch == '\n') {
                if (stringValue.charAt(i - 1) != '\r') {
                    nbytes += 1;
                }
            } else if (ch == '<' || ch == '>') {
                nbytes += 4;
            } else {
                nbytes += 1;
            }
        }
        return (minLength <= nbytes && nbytes <= maxLength);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값이 정규표현식에 해당하는 지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} regex 정규표현식
* @param {boolean} includeFlag 정규표현식 해당여부 포함되어야할때 true
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkRegex = function (identifier, regex, includeFlag, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var str = checkElement.value;
        if (str.length == 0) return true;
        if (includeFlag) return str.search(regex) != -1;
        else return str.search(regex) == -1;
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 값이 알파벳과 0~9사이의 숫자만 포함하는 지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkAlphaNum = function (identifier, errorMessage, focusFlag) {
    this.checkRegex(identifier, /^[a-zA-Z0-9]+$/, true, errorMessage, focusFlag);
}

/**
* 값이 0~9 사이의 문자만 포함하는 지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkOnlyNumber = function (identifier, errorMessage, focusFlag) {
    this.checkRegex(identifier, /^[0-9]+$/, true, errorMessage, focusFlag);
}

/**
* 값이 올바른 이메일 주소인지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkEmail = function (identifier, errorMessage, focusFlag) {
    this.checkRegex(identifier, /^((\w|[\-\.])+)@((\w|[\-\.])+)\.([A-Za-z]+)$/, true, errorMessage, focusFlag);
}

/**
* 올바른 주소인지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkUrl = function (identifier, errorMessage, focusFlag) {
    this.checkRegex(identifier, /^(?:(?:ftp|https?):\/\/)?(?:[a-z0-9](?:[-a-z0-9]*[a-z0-9])?\.)+(?:com|edu|biz|org|gov|int|info|mil|net|name|museum|coop|aero|[a-z][a-z])\b(?:\d+)?(?:\/[^;"'<>()\[\]{}\s\x7f-\xff]*(?:[.,?]+[^;"'<>()\[\]{}\s\x7f-\xff]+)*)?/, true, errorMessage, focusFlag);
}

/**
* 특수문자 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkSpecialChar = function (identifier, errorMessage, focusFlag) {
    this.checkRegex(identifier, /[~!@\#$%^&*\()\-=+_']/, false, errorMessage, focusFlag);
}

/**
* 공백 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkWhiteSpace = function (identifier, errorMessage, focusFlag) {
    this.checkRegex(identifier, /\s/g, false, errorMessage, focusFlag);
}

/**
* <select>에서 선택한 옵션의 인덱스가 firstIdx보다 크거나 같은 지 검사한다.
* 
* @param {string} identifier id 또는 name
* @param {int} firstIdx 옵션인덱스
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkSelected = function (identifier, firstIdx, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, focusFlag);
    validateObject.validate = function () {
        var idx = checkElement.selectedIndex;
        return idx >= firstIdx;
    }
    this.validateObjectArray.push(validateObject);
}

/**
* checkbox나 radio 입력 요소가 최소한 1개 이상 선택됐는지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkAtLeastOneChecked = function (identifier, errorMessage, focusFlag) {
    var validateObject = new this.ValidateObject(null, errorMessage, false);
    validateObject.validate = function () {
        var ele = document.getElementsByName(identifier);
        for (var idxe = 0; idxe < ele.length; idxe++) {
            if (ele[idxe].checked == true) return true;
        }
        return false;
    }
    this.validateObjectArray.push(validateObject);
}

/**
* checkboxlist 입력 요소가 1개 이상 선택됐는지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkboxListChecked = function (identifier, errorMessage, focusFlag) {

    var validateObject = new this.ValidateObject(null, errorMessage, false);
    validateObject.validate = function () {
        var res = $("[id^=" + identifier + "]:checkbox:checked").length > 0;
        return res;
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 특정 문자열이 포함되었는지 검사
* 
* @param {string} identifier id 또는 name
* @param {string} checkContainText 포함 문자열
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkContains = function (identifier, checkContainText, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, false);
    validateObject.validate = function () {
        return checkElement.value.indexOf(checkContainText) == -1;
    }
    this.validateObjectArray.push(validateObject);
}

/**
* 입력된 숫자가 지정된 범위의 숫자인지 체크
* 
* @param {string} identifier id 또는 name
* @param {int} minNum 지정 최소 수
* @param {int} maxNum 지정 최대 수
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkBetweenNum = function (identifier, minNum, maxNum, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, false);
    validateObject.validate = function () {
        var number = checkElement.value;
        return (number >= minNum && number <= maxNum);
    }
    this.validateObjectArray.push(validateObject);
}

/**
* isNaN 함수로 숫자여부 체크
* 
* @param {string} identifier id 또는 name
* @param {string} errorMessage 에러메시지
* @param {boolean} focusFlag 포커스여부
*/
Validator.prototype.checkIsNumber = function (identifier, errorMessage, focusFlag) {
    var checkElement = this.getElement(identifier);
    var validateObject = new this.ValidateObject(identifier, errorMessage, false);
    validateObject.validate = function () {
        var val = checkElement.value;
        return !isNaN(val);
    }
    this.validateObjectArray.push(validateObject);
}