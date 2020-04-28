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
                $("#patient-table").html(result);
            }
        });
    });

    

    $("#patient-table").on("click","tbody > tr",function () {
        var patientId = $(this).data("patientid");
        var action = $("#ActionType").val();
        if (action != "") {
            var urlString;
            if (action == "make-apt") {
                urlString = $("#MakeAppointmentURL").val();
                window.location.href = urlString + "?patientid=" + patientId;
            }
            else if (action == "view") {
                urlString = $("#PatientRecordURL").val();
                window.location.href = urlString + "?patientid=" + patientId;
            }
            else if (action == "input") {
                urlString = $("#InputRecordURL").val();
                window.location.href = urlString + "?patientid=" + patientId;
            }
        }
    });

});


