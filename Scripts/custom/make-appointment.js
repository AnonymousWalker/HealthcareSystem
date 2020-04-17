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

    $("#datepicker").change(function (e) {
        var dateString = $(this).val();
        var urlString = $("#GetAppointmentURL").val();
        $.ajax({
            url: urlString,
            type: "get",
            data: { dateString },
            success(response) {
                $(".ap-table-body").html(response);
                disableBookedButton();
            }
        });
    });

    disableBookedButton();

});

// handle "book" button 
function makeAppointment(self) {
    if ($(self).text() === "Booked") return;
    var urlString = $("#MakeAppointmentURL").val();
    var doctorId = $(self).parent().siblings(".selector-td").children().val();
    var patientId = parseInt($("#PatientId").val());
    var time = $(self).parent().siblings(".time-td").text();
    var date = $("#datepicker").val();
    var datetime = date + " " + time;
    $.ajax({
        url: urlString,
        type: "post",
        data: { patientId, doctorId, time: datetime },
        success(response) {
            $(".ap-table-body").html(response);
            disableBookedButton();
        }
    });
}

function disableBookedButton() {
    var isDisabled = false;
    $(".doctor-selector").each(function () {
        $self = $(this);
        //loop thru each <option> tag to check if all are disabled
        $self.children().each(function () {
            if ($(this).prop("disabled") == false) {
                isDisabled = false;
                return false;
            } else {
                isDisabled = true;
            }
        });


        if (isDisabled) {
            var $aTag = $(this).parent().siblings(".action-td").children();
            $aTag.text("Booked");
            $aTag.css({ "background-color": "#989496", "cursor": "not-allowed" });
            $aTag.click(function (e) {
                e.preventDefault();
            });
        }
    });
}