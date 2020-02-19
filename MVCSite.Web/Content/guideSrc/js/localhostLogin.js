$(document).ready(function(){
	$('#localhost_login_form').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: true,
		ignore: "",
		rules: {
			loginId: {
				required: true,
				email:true ,
				minlength: 5,
				maxlength: 20
			},
			password: {
				required: true,
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
			obj.loginId = $("#loginId1").val();
			obj.password = $("#password1").val();
			$.ajax({
				url: 'localhostLoginJsonAction/login',
				type: 'post',
				dataType: 'json',
				data: {"jsonFromWeb": JSON.stringify(obj)},
				success: function(json) {
					var obj = JSON.parse(json);
					if (obj.result == 'success') {
						window.location.href="localhostCourseList!load";
					} else {
						BootstrapDialog.show({
							title: 'Error',
						    message: obj.data,
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
					    message: obj.data,
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

	$('#localhost_reg_form').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: false,
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
			email: {
				required: true,
				email:true ,
				minlength: 5,
				maxlength: 20
			},
			reemail: {
				required: true,
				email:true ,
				minlength: 5,
				maxlength: 20
			},
			password: {
				required: true,
				minlength: 5,
				maxlength: 20
			},
			repassword: {
				required: true,
				minlength: 5,
				maxlength: 20,
				equalTo: "#password"
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
			obj.email = $("#email").val();
			obj.password = $("#password2").val();
			$.ajax({
				url: 'localhostLoginJsonAction/regWithEmail',
				type: 'post',
				dataType: 'json',
				data: {"jsonFromWeb": JSON.stringify(obj)},
				success: function(json) {
					var obj = JSON.parse(json);
					if (obj.result == 'success') {
						BootstrapDialog.show({
							closable: false,
							title: 'Error',
						    message: 'Register success!',
						    buttons: [{
						   		label: 'Go to Next Page',
						        action: function(dialog) {
						        	dialog.close();
						        	window.location.href="localhostCourseList!load";
						        }
						    }]
						});
					} else {
						BootstrapDialog.show({
							title: 'Error',
						    message: obj.data,
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
					    message: obj.data,
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
});
