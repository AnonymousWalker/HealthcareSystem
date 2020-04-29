$(document).ready(function () {
    $("table.invoice-table > tbody >tr").click(function () {
        var invoiceId = parseInt($(this).data("invoiceid"));
        var urlString = $("#ViewInvoiceURL").val();
        if (urlString != "" && invoiceId != 0) {
            window.location.href = urlString + "?invoiceId=" + invoiceId;
        }
    });

    //$("#edit-form").submit(function (e) {
        

    //})
});