function willBeTooLong(target, max) {
    return $(target).val().length < max || target.selectionEnd != target.selectionStart;
}


function phoneKeyPress(event) {
    return event.charCode === 0 || (/\d/.test(String.fromCharCode(event.charCode)) && willBeTooLong(event.target, 10));
}
function isValidPhoneNumber(number) {
     return number.length == 10;
}

function snnKeyPress(event) {
    return event.charCode === 0 || (/\d/.test(String.fromCharCode(event.charCode)) && willBeTooLong(event.target, 9));
}
function isValidSSN(ssn) {
    return ssn.length == 9;
}

function capacityKeyPress(event) {
    return event.charCode === 0 || (/\d/.test(String.fromCharCode(event.charCode)) && willBeTooLong(event.target, 2));
}
function isValidCapacity(capacity) {
    return Number(capacity) > 0;
}

function zipKeyPress(event) {
    return event.charCode === 0 || (/\d/.test(String.fromCharCode(event.charCode)) && willBeTooLong(event.target, 5));
}
function isValidZip(zip) {
    return zip.length == 5;
}

function plateKeyPress(event) {
    return event.charCode === 0 || (/[0-9a-zA-Z]/.test(String.fromCharCode(event.charCode)) && willBeTooLong(event.target, 6));
}
function isValidPlate(plate) {
    return plate.length == 6;
}

function licenseKeyPress(event) {
    var value = $(event.target).val();
    var needAlphabet = (value.length == 0 || (event.target.selectionStart == 0 && event.target.selectionEnd != 0));

    return event.charCode === 0 ||
        ((needAlphabet && /[a-zA-z]/.test(String.fromCharCode(event.charCode)) ||
        !needAlphabet && event.target.selectionStart != 0 && /\d/.test(String.fromCharCode(event.charCode))) && willBeTooLong(event.target, 14));
}
function isValidLicense(license) {
    return license.length == 14;
}