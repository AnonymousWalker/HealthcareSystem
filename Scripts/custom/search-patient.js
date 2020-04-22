$(document).ready(function () {

    $("#search-btn").click(function () {
        var firstName = $("#fname-input").val();
        var lastName = $("#lname-input").val();
        var phone = $("#phone-input").val();
        var urlString = $("#SearchURL").val();

        $.ajax({
            url: urlString,
            type: 'post',
            data: { firstName, phone, lastName },
            success: function (result) {
                $("#result-table").html(result);
            }
        });
    });

    

    $("#result-table").on("click","tbody > tr",function () {
        var patientId = $(this).data("patientid");
        var action = $("#ActionType").val();
        if (action != "") {
            var urlString = (action == "view")? $("#PatientRecordURL").val() : $("#InputRecordURL").val();
            window.location.href = urlString + "?patientid=" + patientId;
        }
    });

});


