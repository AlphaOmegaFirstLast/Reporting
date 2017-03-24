
var apiPath = "http://localhost:54958/api/Test/";

//----------------- document onReady --------------------
$(document).ready(function () {
 
    window.onerror = function (msg, url, line, col, error) {
        globalErrorHandler(msg, url, line, col, error);
    };
});
//-------------------------------------------------------------
function globalErrorHandler(msg, url, line, col, error) {

    // Note that col & error are only in the HTML 5 
    var extra = (!col ? '' : '\ncolumn: ' + col) + (!error ? '' : '\nerror: ' + error);

    $('#dvMessage').text("javascript error: " + msg + "\nurl: " + url + "\nline: " + line + extra);

    // TODO: Report this error via ajax so you can keep track of js issues

    var suppressErrorAlert = true;

    // If you return true, then error alerts (like in older versions of Internet Explorer) will be suppressed.
    return suppressErrorAlert;
}
//-------------------------------------------------------------
function httpErrorHandler(response) {

    $('#dvMessage').text("http error: " + JSON.stringify(response, null, 3));

    // TODO: Log this error via ajax so you can keep track of js issues

    var suppressErrorAlert = true;

    // If you return true, then error alerts (like in older versions of Internet Explorer) will be suppressed.
    return suppressErrorAlert;
}
//-------------------------------------------------------------
function ShowInfo(msg , obj) {
    var info = $('#dvMessage').html();
    $('#dvMessage').html(info + " <br/> " + "Info: " + msg + JSON.stringify(obj, null, 3));

    // TODO: Log this error via ajax so you can keep track of js issues

    var suppressErrorAlert = true;

    // If you return true, then error alerts (like in older versions of Internet Explorer) will be suppressed.
    return suppressErrorAlert;
}
//------------------------------------------------------------------------------------------------------------
