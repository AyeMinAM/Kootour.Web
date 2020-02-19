$(function () {
    var widgetcalendar = $('.datetimepicker').datetimepicker({
        minDate: new Date(),
        format: "MM/DD/YYYY"
    }).on('dp.change', function(e) {
        $('.datetimepicker').data("DateTimePicker").hide();
    });
});
