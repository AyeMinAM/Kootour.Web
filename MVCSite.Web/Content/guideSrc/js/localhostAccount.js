$(document).ready(function() {
	$('[data-rel=tooltip]').tooltip();

	$('#localhost_account_form').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: true,
		ignore: "",
		rules: {
			firstName: {
				required: true,
				minlength: 5,
				maxlength: 20
			},
			lastName: {
				required: true,
				minlength: 5,
				maxlength: 20
			},
			oldPassword: {
				required: true,
				minlength: 5,
				maxlength: 20
			},
			newPassword: {
				required: true,
				minlength: 5,
				maxlength: 20,
			},
			rePassword: {
				required: true,
				minlength: 5,
				maxlength: 20,
				equalTo: "#newPassword"
			},
			email: {
				required: true,
				email:true ,
				minlength: 5,
				maxlength: 20
			},

		},

		messages: {
		},

		highlight: function (e) {
			$(e).closest('.form-validator').removeClass('has-info').addClass('has-error');
		},

		success: function (e) {
			$(e).closest('.form-validator').removeClass('has-error');//.addClass('has-info');
			$(e).remove();
		},

		submitHandler: function (form) {
			var obj = new Object();
			obj.firstName = $("#firstName").val();
			obj.lastName = $("#lastName").val();
			obj.oldPassword = $("#oldPassword").val();
			obj.newPassword = $("#newPassword").val();
			obj.email = $("#email").val();
			obj.phone = $("#phone").val();
			obj.memo = $('#editor').summernote('code');
			$.ajax({
				url: 'localhostAccountJsonAction/updateAccount',
				type: 'post',
				dataType: 'json',
				data: {"jsonFromWeb": JSON.stringify(obj)},
				success: function(json) {
					var obj = JSON.parse(json);
					if (obj.result == 'success') {
						BootstrapDialog.show({
							title: 'Success',
						    message: obj.message + "<br>Please re-login use your new information.",
						    buttons: [{
						   		label: 'Close',
						        action: function(dialog) {
						        	window.location.href="localhostLogin!load";
						        }
						    }]
						});
					} else {
						BootstrapDialog.show({
							title: 'Error',
						    message: obj.message,
						    buttons: [{
						   		label: 'Close',
						        action: function(dialog) {
						        	dialog.close();
						        }
						    }]
						});
					}
				},
				error: function(res) {
					BootstrapDialog.show({
						title: 'Error',
					    message: obj.message,
					    buttons: [{
					   		label: 'Close',
					        action: function(dialog) {
					        	dialog.close();
					        }
					    }]
					});
				}
			});
		},

		invalidHandler: function (form) {
			BootstrapDialog.show({
				title: 'Error',
			    message: 'You missed some fields. They have been highlighted below.',
			    buttons: [{
			   		label: 'Close',
			        action: function(dialog) {
			        	dialog.close();
			        }
			    }]
			});
		}
	});
	$('#editor').summernote({
		 height: 200,
		  toolbar: [
		            ['style', ['bold', 'italic', 'underline', 'clear']],
		            ['font', ['strikethrough', 'superscript', 'subscript']],
		            ['fontsize', ['fontsize']],
		            ['para', ['ul', 'ol', 'paragraph']],
		            ['height', ['height']]
		          ]
	});

    $("#input-dim-1").fileinput({
    	language: 'en', //设置语言
    	showCaption: false,//是否显示标题
    	browseClass: "btn btn-primary", //按钮样式
    	maxFileCount: 1,
        uploadUrl: "localhostCourseUpload/uploadAvatar",
        allowedFileExtensions: ["jpg", "png", "jpeg"],
        maxFileSize: 5000,
    });

    $("#input-dim-2").fileinput({
    	language: 'en', //设置语言
    	showCaption: false,//是否显示标题
    	browseClass: "btn btn-primary", //按钮样式
    	dropZoneTitle: "Drag & drop videos here &hellip;",
    	maxFileCount: 3,
        uploadUrl: "localhostAccountUpload/uploadVideo",
        allowedFileExtensions: ["mp4"],
        maxFileSize: 50000
    });
});
