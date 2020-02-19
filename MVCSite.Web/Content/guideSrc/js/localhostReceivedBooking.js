$(document).ready(function(){
	HoldOn.open({message:"Loading Calendar...",theme:"sk-dot"});
	$.ajax({
		url: 'localhostReceivedBooking/receivedBooking',
		type: 'post',
		dataType: 'json',
		success: function(json) {
			HoldOn.close();
			var obj = JSON.parse(json);
			if (obj.result == 'success') {

				var eventsForSchedule = new Array();
				var eventsArray = new Array();

				var workDayArray = obj.data.workDayList;
				var userOrderEntityList = obj.data.userOrderEntityList;

				for (i in workDayArray) {
					var workDayBackColor;
					var workDayFormatted = moment(workDayArray[i], "YYYYMMDD").format('YYYY-MM-DD');
					if (moment().isBefore(workDayFormatted)) {
						workDayBackColor = '#5cb85c';
					} else {
						workDayBackColor = '#666666';
					}
					eventsForSchedule.push({
						overlap: false,
						start: workDayFormatted,
						rendering: 'background',
						color: workDayBackColor
					});
				}

				for (n in userOrderEntityList) {
					var userOrderEntity = userOrderEntityList[n];
					var orderDayStyle;
					var orderDayFormatted = moment(userOrderEntity.reservationDate, "YYYYMMDD").format('YYYY-MM-DD')

					if (moment().isBefore(orderDayFormatted)) {
						orderDayStyle = 'label-important';
					} else {
						orderDayStyle = 'label-info';
					}

					var event = {
							title: 'Have been booked',
							start: orderDayFormatted,
							className: orderDayStyle,
							courseIdentiNo:userOrderEntity.courseIdentiNo,
							localhostIdentiNo:userOrderEntity.localhostIdentiNo,
							touristIdentiNo:userOrderEntity.touristIdentiNo
					}
					eventsArray.push(event);
				}

				var calendar = $('#calendar').fullCalendar({
			        loading: function(isLoading) {
			            if(isLoading) {
			            	var options = {
			            		    message:"Loading Calendar...",
			            		    theme:"sk-dot"
			            		};
		            		HoldOn.open(options);
			            } else {

			            }
			        },
					 buttonHtml: {
						prev: '<i class="ace-icon fa fa-chevron-left"></i>',
						next: '<i class="ace-icon fa fa-chevron-right"></i>'
					},

					header: {
						left: 'prev,next,today',
						center: 'title',
						right: ''
					},
					events: eventsForSchedule.concat(eventsArray),
					editable: false,
					droppable: false, // this allows things to be dropped onto the calendar !!!
					selectable: false,
					eventClick: function(calEvent, jsEvent, view) {
						$.ajax({
							url: 'localhostReceivedBooking/fetchOrderBookingDetail',
							type: 'post',
							dataType: 'json',
							data: {
								courseIdentiNo:calEvent.courseIdentiNo,
								touristIdentiNo:calEvent.touristIdentiNo
								},
							success: function(json) {
								var obj = JSON.parse(json);
								if (obj.result == 'success') {
									//display a modal
									var includeHtml = '<h5>Includes:</h5>';
									var excludeHtml = '<h5>Excludes:</h5>';
									if (obj.data.courseInExclusionEntityList != undefined && obj.data.courseInExclusionEntityList != null) {
										var x = 1;
										var y = 1;
										for (n in obj.data.courseInExclusionEntityList) {
											if (obj.data.courseInExclusionEntityList[n].inExclusionType == 1) {
												includeHtml = includeHtml +
												'<h5>' + (x++) + '.' + obj.data.courseInExclusionEntityList[n].inExclusion + '</h5>';
											} else {
												excludeHtml = excludeHtml +
												'<h5>' + (y++) + '.' + obj.data.courseInExclusionEntityList[n].inExclusion + '</h5>';
											}
										}
									}

									var modal =
										'<div class="modal fade">'+
										'<div class="modal-dialog">'+
										'<div class="modal-content">'+
										'<div class="modal-body">'+
										'<button type="button" class="close" data-dismiss="modal" style="margin-top:-10px;">&times;</button>'+
										'<h4 class="lighter block kootour-main-color">Booking Information</h4>'+
										'<h5>Tour Name: ' + obj.data.courseEntity.fullName + '</h5>'+
										'<h5>Person Or Group: ' + obj.data.courseEntity.personOrGroup + '</h5>'+
										'<h5>Tour Location: ' + obj.data.courseEntity.tourLocation + '</h5>'+
										'<h5>Meetup Location: ' + obj.data.courseEntity.meetupLocation + '</h5>'+
										'<h5>Duration: ' + obj.data.courseEntity.duration + ' '+ obj.data.courseEntity.durationUnit +'(s)</h5>'+
										includeHtml +
										excludeHtml +
										'<h5>Order Name: ' + obj.data.touristEntity.firstName + ' ' + obj.data.touristEntity.lastName + '</h5>'+
										'</div>'+
										'<div class="modal-footer">'+
										'<button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">OK</button>'+
										'</div>'+
										'</div>'+
										'</div>'+
										'</div>';

									var modal = $(modal).appendTo('body');
									modal.modal('show').on('hidden', function(){
										modal.remove();
									});
								} else {
									alert("error");
								}
							},
							error: function(res) {
								alert("error");
							}
						});
					}
				});
			} else {
				BootstrapDialog.show({
					title: 'Error',
				    message: "TODO",
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
			HoldOn.close();
			BootstrapDialog.show({
				title: 'Error',
			    message: "TODO",
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
