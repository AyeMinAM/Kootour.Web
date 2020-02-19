$(function () {
    var widgetcalendar = $('#filter-calendar').datetimepicker({
      minDate: new Date(),
      defaultDate: new Date(),
      format: "MM/DD/YYYY"
	  }).on('dp.change', function(e) {
	  	$('#filter-calendar').data("DateTimePicker").hide();
	  });
});