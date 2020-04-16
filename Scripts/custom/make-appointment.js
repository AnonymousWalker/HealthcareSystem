$(document).ready(function () {
    if ($("#datepicker").val() === "") {
        var today = new Date();
        var year = today.getFullYear() 
        var month = today.getMonth() + 1;
        var day = today.getDate();
        if (month < 10) month = "0" + month;
        if (day < 10) day = "0" + day;
        var dateString = year + "-" + month + "-" + day;
        $("#datepicker").val(dateString);
    }
});