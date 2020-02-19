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
					var flg;
					var workDayFormatted = moment(workDayArray[i], "YYYYMMDD").format('YYYY-MM-DD');
					if (moment().isBefore(workDayFormatted)) {
						workDayBackColor = '#5cb85c';
						flg = true;
					} else {
						workDayBackColor = '#666666';
						flg = false;
					}
					eventsForSchedule.push({
						overlap: false,
						start: workDayFormatted,
						rendering: 'background',
						color: workDayBackColor,
						canCancel: flg
					});
				}

				for (i in userOrderEntityList) {
					userOrderEntity = userOrderEntityList[i];
					var index = $.inArray(userOrderEntity.reservationDate, workDayArray);
					eventsForSchedule[index].color = '#666666';
					eventsForSchedule[index].canCancel = false;
				}

				for (i in eventsForSchedule) {
					var eventsForScheduleEntity = eventsForSchedule[i];
					if (eventsForScheduleEntity.canCancel) {
						var canCancelDayFormatted = eventsForScheduleEntity.start;
						var event = {
								title: 'Cancel this day',
								start: canCancelDayFormatted,
								className: 'label-important',
						}
						eventsArray.push(event);
					}
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
						BootstrapDialog.show({
							title: 'Cancel work day',
						    message: "Do you make sure to cancel this work day",
						    buttons: [{
						   		label: 'Cancel it',
						        action: function(dialog) {
						        	dialog.close();
									$.ajax({

									});
						        }
						    }]
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
