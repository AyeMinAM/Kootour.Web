$(document).ready(function(){
	$('#create_new').click(function () {
		$(this).attr("disabled", true);
		window.location.href="localhostCourse!load";
	});
});

function doEdit(v) {
	window.location.href="localhostCourseEdit!load?courseIdentiNo=" + v;
}
function doDelete(v) {
	window.location.href="localhostCourseDelete!deleteCourse?courseIdentiNo=" + v;
}
function doClone(v) {
	window.location.href="localhostCourseClone!load?courseIdentiNo=" + v;
}