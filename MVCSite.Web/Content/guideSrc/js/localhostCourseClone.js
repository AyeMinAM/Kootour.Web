$(document).ready(function() {
	$('[data-rel=tooltip]').tooltip();

	$('#fuelux-wizard-container')
	.ace_wizard({
	})
	.on('actionclicked.fu.wizard' , function(e, info){
		var flg = true;
		if(info.step == 1) {
			for (var n = 0; n < $("div[data-step='1']").find("[name]").length; n++ ) {
				var target = $("div[data-step='1']").find("[name]").get(n);
				if (!$(target).hasClass("ignore")) {
					if (!$(target).valid()) {
						flg = false;
					}
				}
			}
			return flg;
		}
		else if (info.step == 2) {
			$("#overview").val($('#editor1').summernote('code'));
			$("#itinerary").val($('#editor2').summernote('code'));
			for (var n = 0; n < $("div[data-step='2']").find("[name]").length; n++ ) {
				var target = $("div[data-step='2']").find("[name]").get(n);
				if (!$(target).hasClass("ignore")) {
					if (!$(target).valid()) {
						flg = false;
					}
				}
			}
			return flg;
		}
		else if (info.step == 3) {
			for (var n = 0; n < $("div[data-step='3']").find("[name]").length; n++ ) {
				var target = $("div[data-step='3']").find("[name]").get(n);
				if (!$(target).hasClass("ignore")) {
					if (!$(target).valid()) {
						flg = false;
					}
				}
			}
			return flg;
		}
		else if (info.step == 4) {
			for (var n = 0; n < $("div[data-step='4']").find("[name]").length; n++ ) {
				var target = $("div[data-step='4']").find("[name]").get(n);
				if (!$(target).hasClass("ignore")) {
					if (!$(target).valid()) {
						flg = false;
					}
				}
			}
			return flg;
		}
	})
	.on('finished.fu.wizard', function(e) {
		var obj = new Object();
		obj.fullName = $("#fullName").val();
		obj.historical = $("#historical").get(0).checked;
		obj.adventure = $("#adventure").get(0).checked;
		obj.leisureSports = $("#leisureSports").get(0).checked;
		obj.cultureArts = $("#cultureArts").get(0).checked;
		obj.natureRural = $("#natureRural").get(0).checked;
		obj.festivalEvents = $("#festivalEvents").get(0).checked;
		obj.nightlifeParty = $("#nightlifeParty").get(0).checked;
		obj.foodDrink = $("#foodDrink").get(0).checked;
		obj.shoppingMarket = $("#shoppingMarket").get(0).checked;
		if ($("#provideLanguages").val() != null) {
			obj.useLangId = $("#provideLanguages").val().join(";");
		} else {
			obj.useLangId = "";
		}
		obj.courseContent = $('#editor1').summernote('code');
		obj.additionalInfo = $('#editor2').summernote('code');
		obj.duration = $("#duration").val();
		obj.durationUnit = $("#durationunit-select").val();
		obj.tourLocation = $("#tourLocation").val();
		obj.meetupLocation = $("#meetupLocation").val();
		var inArray = new Array();
		for (var x in $("input[name=inclusions]")) {
			var v = $("input[name=inclusions]").eq(x).val();
			if (v != null && v != "") inArray.push(v);
		}
		obj.inclusions = inArray;
		var exArray = new Array();
		for (var x in $("input[name=exclusions]")) {
			var v = $("input[name=exclusions]").eq(x).val();
			if (v != null && v != "") exArray.push(v);
		}
		obj.exclusions = exArray;
		obj.minTouristNum = $("#minTouristNum").val();
		obj.maxTouristNum = $("#maxTouristNum").val();
		obj.personOrGroup = $('input[type="radio"][name="personOrGroup"]:checked').val();
		obj.minHourAdvance = $("#minHourAdvance").val();

		obj.courseScheduleName = $("#courseScheduleName").val();
		obj.bgnDate = $("#bgnDate").val();
		obj.endDate = $("#endDate").val();
		var workDayArray = new Array();
		if ($('input[type="checkbox"][name="workDay"]:checked').size() > 0) {
			for (var x = 0; x < $('input[type="checkbox"][name="workDay"]:checked').size(); x++) {
				var v = $('input[type="checkbox"][name="workDay"]:checked').eq(x).val();
				workDayArray.push(v);
			}
		}
		obj.workDay = workDayArray.join(";");

		var extraArray = new Array();
		for (var i = 0; i < $("[name=extraNames]").size(); i++) {
			if ($("[name=extraNames]").eq(i).val() != "" && $("[name=extraPrices]").eq(i).val() != ""
				&& $("[name=extraTimes]").eq(i).val() != "") {
				var extraObj = new Object();
				extraObj.extraOptionName = $("[name=extraNames]").eq(i).val();
				extraObj.extraPrice = $("[name=extraPrices]").eq(i).val();
				extraObj.extraTime = $("[name=extraTimes]").eq(i).val();
				extraArray.push(extraObj);
			} else {
				continue;
			}

		}
		obj.extra = extraArray;

		if (($("#discountTourists").val() != "" && $("#discountValue").val() != "") ||
				($("#discountTourists").val() != "" && $("#discountPercent").val() != "")) {
			obj.largeGroupLimit = $("#discountTourists").val();
			obj.largeDiscountValue = $("#discountValue").val();
			obj.largeDiscountPercent = $("#discountPercent").val();
		}

		obj.startHour = $("#earliestStartHour0").val();
		obj.latestStartHour = $("#lastestStartHour0").val();
		obj.retailPrice = $("#retailprice0").val();
		obj.price = $("#netprice0").html();
		obj.commision = $("#commision0").val();




//		var schedulesArray = new Array();
//		for (var z = 0; z < $("#schedulelist").children().size(); z ++) {
//			var schedulesObj = new Object();
//			var scheduleDom = $("#schedulelist").children().eq(z);
//			schedulesObj.courseScheduleName = $(scheduleDom).find("[name=courseScheduleNameItem]").data("courseschedulename");
//			schedulesObj.bgnDate = $(scheduleDom).find("[name=bgnDate]").data("bgndate");
//			schedulesObj.endDate = $(scheduleDom).find("[name=endDate]").data("enddate");
//			schedulesObj.workDay = $(scheduleDom).find("[name=workDayItem]").data("workday");
//
//			var extraArray = new Array();
//			for (var i = 0; i < $(scheduleDom).find("[name=extraOptionName]").size(); i++) {
//				var extraObj = new Object();
//				extraObj.extraOptionName = $(scheduleDom).find("[name=extraOptionName]").eq(i).data("extraoptionname");
//				extraObj.extraPrice = $(scheduleDom).find("[name=extraPrice]").eq(i).data("extraprice");
//				extraObj.extraTime = $(scheduleDom).find("[name=extraTime]").eq(i).data("extratime");
//				extraArray.push(extraObj);
//			}
//			schedulesObj.extra = extraArray;
//
//			if ($(scheduleDom).find("[name=largeGroupLimit]").size() > 0 && ($(scheduleDom).find("[name=largeDiscountValue]").size() >0 || $(scheduleDom).find("[name=largeDiscountPercent]").size() > 0)) {
//				schedulesObj.largeGroupLimit = $(scheduleDom).find("[name=largeGroupLimit]").data("largegrouplimit");
//				schedulesObj.largeDiscountValue = $(scheduleDom).find("[name=largeDiscountValue]").data("largediscountvalue");
//				schedulesObj.largeGroupPercent = $(scheduleDom).find("[name=largeDiscountPercent]").data("largediscountpercent");
//			}
//
//			var timeArray = new Array();
//			for (var i = 0; i < $(scheduleDom).find("[name=startHour]").size(); i++) {
//				var timeObj = new Object();
//				timeObj.startHour = $(scheduleDom).find("[name=startHour]").eq(i).data("starthour");
//				timeObj.latestStartHour = $(scheduleDom).find("[name=latestStartHour]").eq(i).data("lateststarthour");
//				timeObj.retailPrice = $(scheduleDom).find("[name=retailPrice]").eq(i).data("retailprice");
//				timeObj.price = $(scheduleDom).find("[name=price]").eq(i).data("price");
//				timeObj.commision = $(scheduleDom).find("[name=commision]").eq(i).data("commision");
//				timeArray.push(timeObj);
//			}
//			schedulesObj.time = timeArray;
//			schedulesArray.push(schedulesObj);
//		}
//		obj.schedules = schedulesArray;

		$.ajax({
			url: 'localhostCourseCreate/createCourse',
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
	}).on('stepclick.fu.wizard', function(e){
	//e.preventDefault();//this will prevent clicking and selecting steps
	});
//	$(".chosen-select").on('change', function(){
//		$(this).closest('.form-validator').validate().element($(this));
//	});
	jQuery.validator.addMethod("maxmincheck", function(value, element, params) {
		var destVal = $(params).val();
		return this.optional(element) || parseInt(destVal) <= parseInt(value);
	}, "Maximum People must be greater than min minimum people.");

	$('#course_form').validate({
		errorElement: 'div',
		ignore: ".ignore",
		errorClass: 'help-block',
		focusInvalid: true,
		rules: {
			fullName: {
				required: true,
				minlength: 5,
				maxlength: 100
			},
			tourType: {
				required: true,
				minlength:1,
				maxlength:3
			},
			useLangId: {
				required: true
			},
			overview: {
				required: true,
				maxlength:500
			},
			itinerary: {
				required: true,
				maxlength:5000
			},
			duration: {
				required: true,
				digits: true
			},
			tourLocation: {
				required: true,
				minlength: 5,
				maxlength: 100
			},
			meetupLocation: {
				required: true,
				minlength: 5,
				maxlength: 250
			},
			minTouristNum: {
				required: true,
				digits: true
			},
			maxTouristNum: {
				required: true,
				digits: true,
				maxmincheck: "#minTouristNum"
			},
			personOrGroup: {
				required: true
			},
			minHourAdvance: {
				required: true,
				digits: true
			},
			courseScheduleName: {
				required: true,
				minlength: 5,
				maxlength: 250
			},
			daterange: {
				required: true
			},
			workDay: {
				required: true
			},
			earliestStartHours: {
				required: true,
			},
			retailPrices: {
				required: true,
				digits: true
			},
			extraPrices: {
				digits: true
			},
			extraTimes: {
				digits: true
			},
			discountTourists: {
				digits: true
			},
			discountValue: {
				digits: true
			},
			discountPercent: {
				digits: true
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

		errorPlacement: function (error, element) {
			if(element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
				var controls = element.closest('div[class*="control-"]');
				if(controls.find(':checkbox,:radio').length > 1) controls.append(error);
				else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
			}

			else if(element.is('.select2')) {
				error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
			}
			else if(element.is('.chosen-select')) {
				error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
			}
			else if (element.is("input[name=extraTimes]") || element.is("input[name=extraPrices]") ) {
				error.insertAfter(element);
			}
			else error.insertAfter(element.parent());
		},

		submitHandler: function (form) {
			form.submit();
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

	$('#modal-wizard-container').ace_wizard();
	$('#modal-wizard .wizard-actions .btn[data-dismiss=modal]').removeAttr('disabled');

	if(!ace.vars['touch']) {

//        $("#alltype").click(function() {
//        	 $('input[data="tourtype"]').prop("checked",this.checked);
//         });
//         var $subBox = $("input[data='tourtype']");
//         $subBox.click(function(){
//             $("#alltype").prop("checked",$subBox.length == $("input[data='tourtype']:checked").length ? true : false);
//         });


		$('.chosen-select').chosen({allow_single_deselect:true});
		//resize the chosen on window resize

		$(window)
		.off('resize.chosen')
		.on('resize.chosen', function() {
			$('.chosen-select').each(function() {
				 var $this = $(this);
				 $this.next().css({'width': $this.parent().width()});
			})
		}).trigger('resize.chosen');
		//resize chosen on sidebar collapse/expand
		$(document).on('settings.ace.chosen', function(e, event_name, event_val) {
			if(event_name != 'sidebar_collapsed') return;
			$('.chosen-select').each(function() {
				 var $this = $(this);
				 $this.next().css({'width': $this.parent().width()});
			})
		});


		$('#chosen-multiple-style .btn').on('click', function(e){
			var target = $(this).find('input[type=radio]');
			var which = parseInt(target.val());
			if(which == 2) $('#provideLanguages').addClass('tag-input-style');
			 else $('#provideLanguages').removeClass('tag-input-style');
		});
	}

		$('#editor1').summernote({
			 height: 200,
			  toolbar: [
			            ['style', ['bold', 'italic', 'underline', 'clear']],
			            ['font', ['strikethrough', 'superscript', 'subscript']],
			            ['fontsize', ['fontsize']],
			            ['para', ['ul', 'ol', 'paragraph']],
			            ['height', ['height']]
			          ]
		});

		$('#editor2').summernote({
			 height: 200,
			  toolbar: [
			            ['style', ['bold', 'italic', 'underline', 'clear']],
			            ['font', ['strikethrough', 'superscript', 'subscript']],
			            ['fontsize', ['fontsize']],
			            ['para', ['ul', 'ol', 'paragraph']],
			            ['height', ['height']]
			          ]
		});

		//or change it into a date range picker
	    $('#daterange').daterangepicker({
	        locale: {
	            format: 'YYYYMMDD'
	        },
	        minDate: new Date(),
	        "showDropdowns": true,
	        "autoApply": true,
	        startDate: moment().format('YYYYMMDD'),
	        endDate: moment().add(1, 'd').format('YYYYMMDD')
	    });
    	$('#bgnDate').val(moment().format('YYYYMMDD'));
    	$('#endDate').val(moment().add(1, 'd').format('YYYYMMDD'));
	    $('#daterange').on('apply.daterangepicker', function(ev, picker) {
	    	$('#bgnDate').val(picker.startDate.format('YYYYMMDD'));
	    	$('#endDate').val(picker.endDate.format('YYYYMMDD'));
	    });

		$('input[name=date-range-picker]').daterangepicker({
			'applyClass' : 'btn-sm btn-success',
			'cancelClass' : 'btn-sm btn-default',
			locale: {
				applyLabel: 'Apply',
				cancelLabel: 'Cancel',
			}
		})
		.prev().on(ace.click_event, function(){
			$(this).next().focus();
		});

		$('#pairtimepicker0 .time').timepicker({'show2400' : true, 'timeFormat' : "H:i", 'showDuration': true});
		$('#pairtimepicker0').datepair();

		$("#add_in").click(function(){
			var tableSize = $("#inclusions-table tr").size();
			for (var i = 0; i <= 4; i++) {
				var trHtml='<tr><td><input type="text" name="inclusions" class="form-control" ></td></tr>';
				$("#inclusions-table tr:last").after(trHtml);
			}
		});

		$("#add_ex").click(function(){
			var tableSize = $("#exclusions-table tr").size();
			for (var i = 0; i <= 4; i++) {
				var trHtml='<tr><td><input type="text" name="exclusions" class="form-control" ></td></tr>';
				$("#exclusions-table tr:last").after(trHtml);
			}
		});

		$('input:radio[name="personOrGroup"]').change(function(){
			if ($(this).is(':checked') && $(this).val() == 'G') {
				$("#discount").hide();
			} else {
				$("#discount").show();
			}
		});

//		$("#add_extra").click(function(){
//			var tableSize = $("#extra-table tr").size();
//			var trHtml='<tr data-extra="true"><td><input class="form-control ignore" type="text" placeholder="e.g. Snorking" name="extraNames"></td><td><input class="form-control ignore" type="text" placeholder="20" name="extraPrices"></td><td><input class="form-control ignore" type="text" placeholder="2hrs" name="extraTimes"></td></tr>';
//			$("#extra-table tr:last").after(trHtml);
//		});

//		$("#add_start_time").click(function(){
//			var divSize = $(".start_time_child").size();
//			if (divSize == 1) {
//				var divHtml = '<div id="start_time2" class="col-xs-4 start_time_child">' +
//				'<div class="form-validator"><div class="clearfix">'+
//				'<div class="input-group" id="pairtimepicker1">'+
//				'<span class="input-group-addon">'+
//				'<i class="fa fa-clock-o"></i>'+
//				'</span>'+
//				'<input id="earliestStartHour1" class="time start" type="text" name="earliestStartHours"> to'+
//				'<input id="lastestStartHour1" class="time end" type="text" name="lastestStartHours">'+
//				'</div>'+
//				'</div></div>'+
//				'<div class="space-8"></div>'+
//				'<h5 class="block">Suggested Retail Price:</h5>'+
//				'<div class="form-validator"><div class="clearfix">'+
//				'<input type="number" id="retailprice1" name="retailPrices">'+
//				'</div></div>'+
//				'<div class="space-8"></div>'+
//				'<h5 class="block">Commision pay to Kootour:</h5>'+
//				'<select id="commision1" name="commisions">'+
//				'<option value="8">8</option>'+
//				'<option value="9">9</option>'+
//				'<option value="10">10</option>'+
//				'<option value="11">11</option>'+
//				'<option value="12" selected="selected">12</option>'+
//				'<option value="13">13</option>'+
//				'<option value="14">14</option>'+
//				'<option value="15" >15</option>'+
//				'<option value="16">16</option>'+
//				'<option value="17">17</option>'+
//				'<option value="18">18</option>'+
//				'<option value="19">19</option>'+
//				'<option value="20">20</option>'+
//				'<option value="21">21</option>'+
//				'<option value="22">22</option>'+
//				'<option value="23">23</option>'+
//				'<option value="24">24</option>'+
//				'<option value="25">25</option>'+
//				'</select>'+
//				'<label>%(Percentage)</label>'+
//				'<div class="space-8"></div>'+
//				'<h5 class="block">Net Price:</h5>'+
//				'<h5 class="block netprices" id="netprice1" >0</h5>'+
//				'<button id="delete_start_time2" type="button" class="btn btn-white kootour-btn-main"><i class="fa fa-minus-circle" aria-hidden="true"></i>&nbsp;Delete this time</button>' +
//				'</div>';
//				$(".start_time_more:last").append(divHtml);
//				$('#pairtimepicker1 .time').timepicker({'show2400' : true, 'timeFormat' : "H:i", 'showDuration': true});
//				$('#pairtimepicker1').datepair();
//				$("#retailprice1").on('input', function(){
//					var commision = $("#commision1").val()
//					var v = ($(this).val() / (1 + commision/100)).toFixed(2);
//					$("#netprice1").html(v);
//				});
//				$("#commision1").on('change', function(){
//					var commision = $(this).val()
//					var v = ($("#retailprice1").val() / (1 + commision/100)).toFixed(2);
//					$("#netprice1").html(v);
//				});
//				$("#delete_start_time2").on('click', function() {
//					$("#start_time2").remove();
//				});
//			} else if (divSize == 2) {
//				$("#add_start_time").attr("disabled", true);
//				var divHtml = '<div id="start_time3" class="col-xs-4 start_time_child">' +
//				'<div class="form-validator"><div class="clearfix">'+
//				'<div class="input-group" id="pairtimepicker2">'+
//				'<span class="input-group-addon">'+
//				'<i class="fa fa-clock-o"></i>'+
//				'</span>'+
//				'<input id="earliestStartHour2" class="time start" type="text" name="earliestStartHours"> to'+
//				'<input id="lastestStartHour2" class="time end" type="text" name="lastestStartHours">'+
//				'</div>'+
//				'</div></div>'+
//				'<div class="space-8"></div>'+
//				'<h5 class="block">Suggested Retail Price:</h5>'+
//				'<div class="form-validator"><div class="clearfix">'+
//				'<input type="number" id="retailprice2" name="retailPrices">'+
//				'</div></div>'+
//				'<div class="space-8"></div>'+
//				'<h5 class="block">Commision pay to Kootour:</h5>'+
//				'<select id="commision2" name="commisions">'+
//				'<option value="8">8</option>'+
//				'<option value="9">9</option>'+
//				'<option value="10">10</option>'+
//				'<option value="11">11</option>'+
//				'<option value="12" selected="selected">12</option>'+
//				'<option value="13">13</option>'+
//				'<option value="14">14</option>'+
//				'<option value="15" >15</option>'+
//				'<option value="16">16</option>'+
//				'<option value="17">17</option>'+
//				'<option value="18">18</option>'+
//				'<option value="19">19</option>'+
//				'<option value="20">20</option>'+
//				'<option value="21">21</option>'+
//				'<option value="22">22</option>'+
//				'<option value="23">23</option>'+
//				'<option value="24">24</option>'+
//				'<option value="25">25</option>'+
//				'</select>'+
//				'<label>%(Percentage)</label>'+
//				'<div class="space-8"></div>'+
//				'<h5 class="block">Net Price:</h5>'+
//				'<h5 class="block netprices" id="netprice2">0</h5>'+
//				'<button id="delete_start_time3" type="button" class="btn btn-white kootour-btn-main"><i class="fa fa-minus-circle" aria-hidden="true"></i>&nbsp;Delete this time</button>' +
//				'</div>';
//				$(".start_time_more:last").append(divHtml);
//				$('#pairtimepicker2 .time').timepicker({'show2400' : true, 'timeFormat' : "H:i", 'showDuration': true});
//				$('#pairtimepicker2').datepair();
//				$("#retailprice2").on('input', function(){
//					var commision = $("#commision2").val()
//					var v = ($(this).val() / (1 + commision/100)).toFixed(2);
//					$("#netprice2").html(v);
//				});
//				$("#commision2").on('change', function(){
//					var commision = $(this).val()
//					var v = ($("#retailprice2").val() / (1 + commision/100)).toFixed(2);
//					$("#netprice2").html(v);
//				});
//				$("#delete_start_time3").on('click', function() {
//					$("#start_time3").remove();
//				});
//			}
//		});

		$("#retailprice0").on('input', function(){
			var commision = $("#commision0").val()
			var v = ($("#retailprice0").val() * (1 - commision/100)).toFixed(2);
			$("#netprice0").html(v);
		});

		$("#commision0").on('change', function(){
			var commision = $(this).val()
			var v = ($("#retailprice0").val() * (1 - commision/100)).toFixed(2);
			$("#netprice0").html(v);
		});

		$("#discountValue").on('input', function(){
			$("#discountPercent").val('');
		});

		$("#discountPercent").on('input', function(){
			$("#discountValue").val('');
		});

	    $("#input-dim-1").fileinput({
	    	language: 'en', //设置语言
	    	showCaption: false,//是否显示标题
	    	browseClass: "btn btn-primary", //按钮样式
	    	maxFileCount: 5,
	        uploadUrl: "localhostReceivedBooking/uploadImage",
	        allowedFileExtensions: ["jpg", "png", "jpeg"],
	        maxFileSize: 5000,
	        uploadExtraData: {
	            img_key: "1000",
	            img_keywords: "happy, people",
	        }
	    });

//	    var panelListSize = 0;
//	    $("#addscheduleoption").click(function(){
//	    	var flg = true;
//	    	var f1 = $("[name='courseScheduleName']").valid();
//	    	var f2 = $("[name='daterange']").valid();
//	    	var f3 = $("[name='workDay']").valid();
//
//	    	var f4 = true;
//	    	for (var x = 0; x < $(".start_time_child").size(); x++) {
//	    		var f5 = $("[name='earliestStartHours']").eq(x).valid();
//	    		var f7 = $("[name='retailPrices']").eq(x).valid();
//	    		if (f4) {
//	    			f4 = f5&&f7;
//	    		}
//	    	}
//
//	    	if (f1&&f2&&f3&&f4) {
//	    		flg = true;
//	    	} else {
//	    		flg = false;
//	    	}
//
//			if (flg) {
//				panelListSize ++;
//		    	var scheduleName = $("#courseScheduleName").val();
//		    	var bgnDate = $("#bgnDate").val();
//		    	var endDate = $("#endDate").val();
//				var workDayArray = new Array();
//				if ($('input[type="checkbox"][name="workDay"]:checked').size() > 0) {
//					for (var x = 0; x < $('input[type="checkbox"][name="workDay"]:checked').size(); x++) {
//						var v = $('input[type="checkbox"][name="workDay"]:checked').eq(x).val();
//						workDayArray.push(v);
//					}
//				}
//				var workDay = workDayArray.join(";");
//
//				var extraNames = new Array();
//				for (var x = 0; x < $("input[name=extraNames]").size(); x++) {
//					var v = $("input[name=extraNames]").eq(x).val();
//					extraNames.push(v);
//				}
//
//				var extraPrices = new Array();
//				for (var x = 0; x < $("input[name=extraPrices]").size(); x++) {
//					var v = $("input[name=extraPrices]").eq(x).val();
//					extraPrices.push(v);
//				}
//
//				var extraTimes = new Array();
//				for (var x = 0; x < $("input[name=extraTimes]").size(); x++) {
//					var v = $("input[name=extraTimes]").eq(x).val();
//					extraTimes.push(v);
//				}
//
//				var discountTourists = $("#discountTourists").val();
//				var discountValue = $("#discountValue").val();
//				var discountPercent = $("#discountPercent").val();
//
//				var earliestStartHours = new Array();
//				for (var x = 0; x < $("input[name=earliestStartHours]").size(); x++) {
//					var v = $("input[name=earliestStartHours]").eq(x).val();
//					if (v != null && v != "") earliestStartHours.push(v);
//				}
//
//				var lastestStartHours = new Array();
//				for (var x = 0; x < $("input[name=lastestStartHours]").size(); x++) {
//					var v = $("input[name=lastestStartHours]").eq(x).val();
//					lastestStartHours.push(v);
//				}
//
//				var retailPrices = new Array();
//				for (var x = 0; x < $("input[name=retailPrices]").size(); x++) {
//					var v = $("input[name=retailPrices]").eq(x).val();
//					retailPrices.push(v);
//				}
//
//				var commisions = new Array();
//				for (var x = 0; x < $("select[name=commisions]").size(); x++) {
//					var v = $("select[name=commisions]").eq(x).val();
//					commisions.push(v);
//				}
//
//				var netprices = new Array();
//				for (var x = 0; x < $(".netprices").size(); x++) {
//					var v = $(".netprices").eq(x).html();
//					netprices.push(v);
//				}
//
//		    	var html= '' +
//		    	'<!-- list start -->'+
//		    	'<div class="widget-box widget-color-blue collapsed"  data="schedule">'+
//		    	'<div class="widget-header widget-header-small">'+
//		    	'<h6 class="widget-title">'+
//		    	'&nbsp;Schedule Option #'+ panelListSize +
//		    	'</h6>'+
//		    	'<div class="widget-toolbar">'+
//		    	'<a href="#" data-action="collapse">'+
//		    	'<i class="ace-icon fa fa-plus" data-icon-show="fa-plus" data-icon-hide="fa-minus"></i>'+
//		    	'</a>'+
//		    	'<a href="#" data-action="close">'+
//		    	'<i class="ace-icon fa fa-trash-o"></i>'+
//		    	'</a>'+
//		    	'</div>'+
//		    	'</div>'+
//		    	'<div class="widget-body" style="display: none;">'+
//		    	'<div class="widget-main">'+
//		    	'<!-- nelist start -->'+
//		    	'<div class="nestable nestable-'+ panelListSize +'">'+
//		    	'<ol class="dd-list">'+
//		    	'<li name="courseScheduleNameItem" class="dd-item" data-courseScheduleName="'+ scheduleName +'">'+
//		    	'<div class="dd-handle">'+
//		    	'<span>Schedule Name:</span>'+
//		    	'<span class="lighter grey">'+
//		    	'&nbsp; '+ scheduleName +
//		    	'</span>'+
//		    	'</div>'+
//		    	'</li>'+
//		    	'<li name="bgnDate" class="dd-item" data-bgnDate="'+ bgnDate +'">'+
//		    	'<div class="dd-handle">'+
//		    	'<span>Begin Date:</span>'+
//		    	'<span class="lighter grey">'+
//		    	'&nbsp; '+ bgnDate +
//		    	'</span>'+
//		    	'</div>'+
//		    	'</li>'+
//		    	'<li name="endDate" class="dd-item" data-endDate="'+ endDate +'">'+
//		    	'<div class="dd-handle">'+
//		    	'<span>End Date:</span>'+
//		    	'<span class="lighter grey">'+
//		    	'&nbsp; '+ endDate +
//		    	'</span>'+
//		    	'</div>'+
//		    	'</li>'+
//		    	'<li name="workDayItem" class="dd-item" data-workDay="'+ workDay +'">'+
//		    	'<div class="dd-handle">'+
//		    	'<span>Available Tour Days:</span>'+
//		    	'<span class="lighter grey">'+
//		    	'&nbsp; '+ workDay +
//		    	'</span>'+
//		    	'</div>'+
//		    	'</li>' +
//		    	'<li class="dd-item">'+
//		    	'<div class="dd-handle">Extra values of this schedule:</div>';
//		    	if ($("#extra-table").find('tr[data-extra="true"]').size() > 0) {
//			    	for (var x = 1; x <= $("#extra-table").find('tr[data-extra="true"]').size(); x++ ) {
//
//			    		var extraName = extraNames[x - 1];
//			    		var extraPrice = extraPrices[x - 1];
//			    		var extraTime = extraTimes[x - 1];
//			    		if (extraName == "" || extraPrice == "" || extraTime == "") {
//			    			continue;
//			    		}
//
//			    		html = html +
//				    	'<ol class="dd-list">'+
//				    	'<li class="dd-item" data-id="extravalues'+ x +'">'+
//				    	'<div class="dd-handle">'+
//				    	'Extra value of this schedule #'+ x +':'+
//				    	'</div>'+
//				    	'<ol class="dd-list">'+
//				    	'<li class="dd-item">'+
//				    	'<div name="extraOptionName" class="dd-handle" data-extraOptionName='+ extraName +'>'+
//				    	'<span>Extra Tour Option Name #'+ x +':</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ extraName +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	''+
//				    	'<li name="extraPrice" class="dd-item" data-extraPrice='+ extraPrice +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Price #'+ x +':</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ extraPrice +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	''+
//				    	'<li name="extraTime" class="dd-item" data-extraTime='+ extraTime +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Extra Time #'+ x +':</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ extraTime +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'</ol>'+
//				    	'</li>'+
//				    	'</ol>';
//			    	}
//		    	}
//		    	if (discountTourists != "") {
//		    		if (discountValue != "") {
//		    			html = html +
//				    	'<li style="display:none;" class="dd-item" data-discountFlg="1">'+
//				    	'<div class="dd-handle">'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="largeGroupLimit" class="dd-item" data-largeGroupLimit='+ discountTourists +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Discount for limit tourists:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ discountTourists +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="largeDiscountValue" class="dd-item" data-largeDiscountValue='+ discountValue +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Discount Value:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ discountValue +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>';
//		    		} else if (discountPercent != "") {
//		    			html = html +
//				    	'<li style="display:none;" class="dd-item" data-discountFlg="2">'+
//				    	'<div class="dd-handle">'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="largeGroupLimit" class="dd-item" data-largeGroupLimit='+ discountTourists +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Discount for limit tourists:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ discountTourists +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="largeDiscountPercent" class="dd-item" data-largeDiscountPercent='+ discountPercent +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Discount Percent:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ discountPercent + '%' +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>';
//		    		}
//		    	}
//
//	    		html = html +
//		    	'<li class="dd-item" data-id="times">'+
//		    	'<div class="dd-handle">Times:</div>';
//		    	for (var x = 1; x <= $(".start_time_child").size(); x++) {
//		    		var earliestStartHour = earliestStartHours[x - 1];
//		    		var lastestStartHour = lastestStartHours[x - 1];
//		    		var retailPrice = retailPrices[x - 1];
//		    		var commision = commisions[x - 1];
//		    		var netprice = netprices[x - 1];
//		    		html = html +
//			    	'<ol class="dd-list">'+
//			    	'<li class="dd-item" data-id="time'+ x +'">'+
//			    	'<div class="dd-handle">'+
//			    	'Time #'+ x +':'+
//			    	'</div>'+
//			    	'<ol class="dd-list">'+
//			    	'<li name="startHour" class="dd-item" data-startHour='+ earliestStartHour +'>'+
//			    	'<div class="dd-handle">'+
//			    	'<span>Earliest Start Time #'+ x +':</span>'+
//			    	'<span class="lighter grey">'+
//			    	'&nbsp; '+ earliestStartHour +
//			    	'</span>'+
//			    	'</div>'+
//			    	'</li>'+
//			    	'<li name="latestStartHour" class="dd-item" data-latestStartHour='+ lastestStartHour +'>'+
//			    	'<div class="dd-handle">'+
//			    	'<span>Lastest Start Time #'+ x +':</span>'+
//			    	'<span class="lighter grey">'+
//			    	'&nbsp; '+ lastestStartHour +
//			    	'</span>'+
//			    	'</div>'+
//			    	'</li>'+
//			    	'<li name="retailPrice" class="dd-item" data-retailPrice='+ retailPrice +'>'+
//			    	'<div class="dd-handle">'+
//			    	'<span>Suggested Retail Price #'+ x +':</span>'+
//			    	'<span class="lighter grey">'+
//			    	'&nbsp; '+ retailPrice +
//			    	'</span>'+
//			    	'</div>'+
//			    	'</li>'+
//			    	'<li name="commision" class="dd-item" data-commision='+ commision +'>'+
//			    	'<div class="dd-handle">'+
//			    	'<span>Commision pay to Kootour #'+ x +':</span>'+
//			    	'<span class="lighter grey">'+
//			    	'&nbsp; '+ commision +
//			    	'</span>'+
//			    	'</div>'+
//			    	'</li>'+
//			    	'<li name="price"  class="dd-item" data-price='+ netprice +'>'+
//			    	'<div class="dd-handle">'+
//			    	'<span>Net Price #'+ x +':</span>'+
//			    	'<span class="lighter grey">'+
//			    	'&nbsp; '+ netprice +
//			    	'</span>'+
//			    	'</div>'+
//			    	'</li>'+
//			    	'</ol>'+
//			    	'</li>'+
//			    	'</ol>';
//		    	}
//		    	html = html +
//		    	'</li>'+
//		    	'</ol>'+
//		    	'</div>'+
//		    	'<!-- nelist end -->'+
//		    	'</div>'+
//		    	'</div>'+
//		    	'</div>'+
//		    	'<!-- list end -->';
//		    	$("#schedulelist").append(html);
//		    	$('.nestable-' + panelListSize).nestable();
//			} else {
//				return false;
//			}
//	    });

		$.ajax({
			url: 'localhostCourseEdit/cloneCourse',
			type: 'post',
			dataType: 'json',
			data:{
				courseIdentiNo:getUrlParam('courseIdentiNo')
	        },
			success: function(json) {
				var obj = JSON.parse(json);
				if (obj.result == 'success') {
					var courseEntity = obj.data.courseEntity;
					var scheduleOptionEntity = obj.data.scheduleOptionEntity;
					var extraOptionEntityList = obj.data.extraOptionEntityList;
					var coursePictureEntityList = obj.data.coursePictureEntityList;
//					var scheduleOptionEntityList = obj.data.scheduleOptionEntityList;
					var courseInExclusionEntityList = obj.data.courseInExclusionEntityList;
					var courseExtraOptionEntityList = obj.data.extraOptionEntityList;

					$('input[name=fullName]').val(courseEntity.fullName);
					(courseEntity.historical == "1") ? $('#historical').prop("checked", true) : $('#historical').prop("checked", false);
					(courseEntity.adventure == "1") ? $('#adventure').prop("checked", true) : $('#adventure').prop("checked", false);
					(courseEntity.leisureSports == "1") ? $('#leisureSports').prop("checked", true) : $('#leisureSports').prop("checked", false);
					(courseEntity.cultureArts == "1") ? $('#cultureArts').prop("checked", true) : $('#cultureArts').prop("checked", false);
					(courseEntity.natureRural == "1") ? $('#natureRural').prop("checked", true) : $('#natureRural').prop("checked", false);
					(courseEntity.festivalEvents == "1") ? $('#festivalEvents').prop("checked", true) : $('#festivalEvents').prop("checked", false);
					(courseEntity.nightlifeParty == "1") ? $('#nightlifeParty').prop("checked", true) : $('#nightlifeParty').prop("checked", false);
					(courseEntity.foodDrink == "1") ? $('#foodDrink').prop("checked", true) : $('#foodDrink').prop("checked", false);
					(courseEntity.shoppingMarket == "1") ? $('#shoppingMarket').prop("checked", true) : $('#shoppingMarket').prop("checked", false);
					var userLangIdArray = courseEntity.useLangId.split(";")
			        $('.chosen-select').val(userLangIdArray).trigger('chosen:updated');
					$("#editor1").summernote('code', courseEntity.courseContent);
					$('#editor2').summernote('code', courseEntity.additionalInfo);
					$('input[name=duration]').val(courseEntity.duration);
					$("#durationunit-select").val(courseEntity.durationUnit);
					$('input[name=tourLocation]').val(courseEntity.tourLocation);
					$('input[name=meetupLocation]').val(courseEntity.meetupLocation);

					var inArray = new Array();
					var exArray = new Array();

					for (var n in courseInExclusionEntityList) {
						if (courseInExclusionEntityList[n].inExclusionType == "1") {
							inArray.push(courseInExclusionEntityList[n].inExclusion);
						} else {
							exArray.push(courseInExclusionEntityList[n].inExclusion);
						}
					}

					for (var n in inArray) {
						if (n < 5) {
							$("input[name=inclusions]").eq(n).val(inArray[n]);
						} else {
							var trHtml='<tr><td><input type="text" name="inclusions" class="form-control" ></td></tr>';
							var inStr = '#inclusions' + newSize;
							$("input[name=inclusions]").eq(n).val(inArray[n]);
						}
					}

					for (var n in exArray) {
						if (n < 5) {
							$("input[name=exclusions]").eq(n).val(exArray[n]);
						} else {
							var trHtml='<tr><td><input type="text" name="exclusions" class="form-control" ></td></tr>';
							$("#exclusions-table tr:last").after(trHtml);
							$("input[name=exclusions]").eq(n).val(exArray[n]);
						}
					}

					$('input[name=minTouristNum]').val(courseEntity.minTouristNum);
					$('input[name=maxTouristNum]').val(courseEntity.maxTouristNum);

					if (courseEntity.personOrGroup == "P") {
						$("#radiop").attr("checked", "checked");
						$("#discount").show();
					} else {
						$("#radiog").attr("checked", "checked");
						$("#discount").hide();
					}

					$('input[name=minHourAdvance]').val(courseEntity.minHourAdvance);

					$("#courseScheduleName").val(scheduleOptionEntity.courseScheuleName);
					$("#bgnDate").val(scheduleOptionEntity.bgnDate);
					$("#endDate").val(scheduleOptionEntity.endDate);

					var workDayArray = scheduleOptionEntity.workDay.split(";");
					for (var x in workDayArray) {
						var v = workDayArray[x];
						var element = "input[name=workDay][value=" + v + "]";
						$(element).attr("checked", 'checked');
					}
					$("#discountTourists").val(scheduleOptionEntity.largeDiscountFlg);
					$("#discountValue").val(scheduleOptionEntity.largeDiscountValue);
					$("#discountPercent").val(scheduleOptionEntity.largeDiscountPercent);
					$("#earliestStartHour0").val(scheduleOptionEntity.startHour);
					$("#lastestStartHour0").val(scheduleOptionEntity.latestStartHour);
					$("#retailprice0").val(scheduleOptionEntity.retailPrice);
					$("#netprice0").html(scheduleOptionEntity.price);
					$("#commision0").get(0).selectedIndex = scheduleOptionEntity.commision - 8;

					var extraArray = new Array();
					for (var n in extraOptionEntityList) {
						if (n <= 4) {
							$("input[name=extraNames]").eq(n).val(extraOptionEntityList[n].extraOptionName);
							$("input[name=extraPrices]").eq(n).val(extraOptionEntityList[n].extraPrice);
							$("input[name=extraTimes]").eq(n).val(extraOptionEntityList[n].extraTime);
						}
					}

//					//TODO schedule_option
//					for (var z = 0; z < scheduleOptionEntityList.length; z ++) {
//						var entity = scheduleOptionEntityList[z];
//						var scheduleCd = entity.scheduleOptionIdentiNo;
//				    	var scheduleName = entity.courseScheuleName;
//				    	var bgnDate = entity.bgnDate;
//				    	var endDate = entity.endDate;
//						var workDay = entity.workDay;
//						var discountTourists = entity.largeGroupLimit;
//						var discountValue = entity.largeDiscountValue;
//						var discountPercent = entity.largeDiscountPercent;
//						var earliestStartHour = entity.startHour;
//						var lastestStartHour = entity.latestStartHour;
//						var retailPrice = entity.retailPrice;
//						var commision = entity.commision;
//						var netprice = entity.price;
//				    	var html= '' +
//				    	'<!-- list start -->'+
//				    	'<div class="widget-box widget-color-blue collapsed"  data="schedule">'+
//				    	'<div class="widget-header widget-header-small">'+
//				    	'<h6 class="widget-title">'+
//				    	'&nbsp;Schedule Option #'+ (z+1) +
//				    	'</h6>'+
//				    	'<div class="widget-toolbar">'+
//				    	'<a href="#" data-action="collapse">'+
//				    	'<i class="ace-icon fa fa-plus" data-icon-show="fa-plus" data-icon-hide="fa-minus"></i>'+
//				    	'</a>'+
//				    	'<a href="#" data-action="close">'+
//				    	'<i class="ace-icon fa fa-trash-o"></i>'+
//				    	'</a>'+
//				    	'</div>'+
//				    	'</div>'+
//				    	'<div class="widget-body" style="display: none;">'+
//				    	'<div class="widget-main">'+
//				    	'<!-- nelist start -->'+
//				    	'<div class="nestable nestable-'+ z +'">'+
//				    	'<ol class="dd-list">'+
//				    	'<li name="courseScheduleNameItem" class="dd-item" data-courseScheduleName="'+ scheduleName +'">'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Schedule Name:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ scheduleName +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="bgnDate" class="dd-item" data-bgnDate="'+ bgnDate +'">'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Begin Date:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ bgnDate +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="endDate" class="dd-item" data-endDate="'+ endDate +'">'+
//				    	'<div class="dd-handle">'+
//				    	'<span>End Date:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ endDate +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="workDayItem" class="dd-item" data-workDay="'+ workDay +'">'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Available Tour Days:</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ workDay +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>' +
//				    	'<li class="dd-item">'+
//				    	'<div class="dd-handle">Extra values of this schedule:</div>';
//				    	if (courseExtraOptionEntityList.length > 0) {
//				    		for (var xx=0; xx < courseExtraOptionEntityList.length; xx++) {
//				    			var extraEntity = courseExtraOptionEntityList[xx];
//				    			var scheduleCdInExtra = extraEntity.scheduleOptionIdentiNo;
//				    			if (scheduleCd == scheduleCdInExtra) {
//						    		var extraName = extraEntity.extraOptionName;
//						    		var extraPrice = extraEntity.extraPrice;
//						    		var extraTime = extraEntity.extraTime;
//
//						    		html = html +
//							    	'<ol class="dd-list">'+
//							    	'<li class="dd-item" data-id="extravalues'+ xx +'">'+
//							    	'<div class="dd-handle">'+
//							    	'Extra value of this schedule :'+
//							    	'</div>'+
//							    	'<ol class="dd-list">'+
//							    	'<li class="dd-item">'+
//							    	'<div name="extraOptionName" class="dd-handle" data-extraOptionName='+ extraName +'>'+
//							    	'<span>Extra Tour Option Name :</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ extraName +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>'+
//							    	''+
//							    	'<li name="extraPrice" class="dd-item" data-extraPrice='+ extraPrice +'>'+
//							    	'<div class="dd-handle">'+
//							    	'<span>Price :</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ extraPrice +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>'+
//							    	''+
//							    	'<li name="extraTime" class="dd-item" data-extraTime='+ extraTime +'>'+
//							    	'<div class="dd-handle">'+
//							    	'<span>Extra Time :</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ extraTime +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>'+
//							    	'</ol>'+
//							    	'</li>'+
//							    	'</ol>';
//				    			}
//					    	}
//					    	if (discountTourists != "") {
//					    		if (discountValue != "") {
//					    			html = html +
//							    	'<li style="display:none;" class="dd-item" data-discountFlg="1">'+
//							    	'<div class="dd-handle">'+
//							    	'</div>'+
//							    	'</li>'+
//							    	'<li name="largeGroupLimit" class="dd-item" data-largeGroupLimit='+ discountTourists +'>'+
//							    	'<div class="dd-handle">'+
//							    	'<span>Discount for limit tourists:</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ discountTourists +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>'+
//							    	'<li name="largeDiscountValue" class="dd-item" data-largeDiscountValue='+ discountValue +'>'+
//							    	'<div class="dd-handle">'+
//							    	'<span>Discount Value:</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ discountValue +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>';
//					    		} else if (discountPercent != "") {
//					    			html = html +
//							    	'<li style="display:none;" class="dd-item" data-discountFlg="2">'+
//							    	'<div class="dd-handle">'+
//							    	'</div>'+
//							    	'</li>'+
//							    	'<li name="largeGroupLimit" class="dd-item" data-largeGroupLimit='+ discountTourists +'>'+
//							    	'<div class="dd-handle">'+
//							    	'<span>Discount for limit tourists:</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ discountTourists +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>'+
//							    	'<li name="largeDiscountPercent" class="dd-item" data-largeDiscountPercent='+ discountPercent +'>'+
//							    	'<div class="dd-handle">'+
//							    	'<span>Discount Percent:</span>'+
//							    	'<span class="lighter grey">'+
//							    	'&nbsp; '+ discountPercent + '%' +
//							    	'</span>'+
//							    	'</div>'+
//							    	'</li>';
//					    		}
//					    	}
//				    	}
//			    		html = html +
//				    	'<li class="dd-item" data-id="times">'+
//				    	'<div class="dd-handle">Times:</div>';
//			    		html = html +
//				    	'<ol class="dd-list">'+
//				    	'<li class="dd-item" data-id="time'+ z +'">'+
//				    	'<div class="dd-handle">'+
//				    	'Time :'+
//				    	'</div>'+
//				    	'<ol class="dd-list">'+
//				    	'<li name="startHour" class="dd-item" data-startHour='+ earliestStartHour +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Earliest Start Time :</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ earliestStartHour +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="latestStartHour" class="dd-item" data-latestStartHour='+ lastestStartHour +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Lastest Start Time :</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ lastestStartHour +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="retailPrice" class="dd-item" data-retailPrice='+ retailPrice +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Suggested Retail Price :</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ retailPrice +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="commision" class="dd-item" data-commision='+ commision +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Commision pay to Kootour :</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ commision +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'<li name="price"  class="dd-item" data-price='+ netprice +'>'+
//				    	'<div class="dd-handle">'+
//				    	'<span>Net Price :</span>'+
//				    	'<span class="lighter grey">'+
//				    	'&nbsp; '+ netprice +
//				    	'</span>'+
//				    	'</div>'+
//				    	'</li>'+
//				    	'</ol>'+
//				    	'</li>'+
//				    	'</ol>';
//				    	html = html +
//				    	'</li>'+
//				    	'</ol>'+
//				    	'</div>'+
//				    	'<!-- nelist end -->'+
//				    	'</div>'+
//				    	'</div>'+
//				    	'</div>'+
//				    	'<!-- list end -->';
//				    	$("#schedulelist").append(html);
//				    	$('.nestable-' + panelListSize).nestable();
//					}
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
});

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}