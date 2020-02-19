$(document).ready(function() {
	$('[data-rel=tooltip]').tooltip();

	$('#localhost_bank_form').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: true,
		ignore: "",
		rules: {
			bankName: {
				minlength: 5,
				maxlength: 50
			},
			bankBranch: {
				minlength: 5,
				maxlength: 50
			},
			bankAccountNo: {
				minlength: 5,
				maxlength: 20
			},
			bankAccountType: {
				minlength: 5,
				maxlength: 20
			},
			bankAccountOwner: {
				minlength: 5,
				maxlength: 50
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
			obj.bankName = $("#bankName").val();
			obj.bankBranch = $("#bankBranch").val();
			obj.bankAccountNo = $("#bankAccountNo").val();
			obj.bankAccountType = $("#bankAccountType").val();
			obj.bankAccountOwner = $("#bankAccountOwner").val();
			$.ajax({
				url: 'localhostBankInformationJsonAction/updateBankInformation',
				type: 'post',
				dataType: 'json',
				data: {"jsonFromWeb": JSON.stringify(obj)},
				success: function(json) {
					var obj = JSON.parse(json);
					if (obj.result == 'success') {
						BootstrapDialog.show({
							title: 'Success',
						    message: obj.message,
						    buttons: [{
						   		label: 'Close',
						        action: function(dialog) {
						        	window.location.href="localhostCourseList!load";
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
});
