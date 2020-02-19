import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http, Response } from '@angular/http';
import { Headers, RequestOptions } from '@angular/http';
import { jqxNotificationComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxnotification';
import { jqxDataAdapterComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxdataadapter';
import { jqxGridComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxgrid';


@Component({
    selector: 'tour-panel',
    template: `                
               <angularGrid #tourGridReference 
                (OnCellclick)="tourGridOnCellclick($event)"
                (OnCellbeginedit)="tourGridOnCellbeginedit($event)" 
                (OnCellendedit)="tourGridOnCellendedit($event)">
               </angularGrid> 

                <!--Notifications-->
                <angularNotification #newFeaturedToursNotification>
                    <div>
                        A tour can be selected as featured ONLY IF it's published already.<br/>
                        Let system administrator restart this site to make this change in effect.
                    </div>
                </angularNotification>
    `
})
export class TourComponent implements AfterViewInit {
    @ViewChild('newFeaturedToursNotification') newFeaturedToursNotification: jqxNotificationComponent;
    @ViewChild('tourGridReference') tourGrid: jqxGridComponent;

    constructor(private route: ActivatedRoute, private http: Http) {
        this.http = http;
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            
        });
    }

    ngOnDestory() {

    }

    ngAfterViewInit(): void {
        this.newFeaturedToursNotification.createWidget(this.newFeaturedToursNotificationSettings);
        this.tourGrid.createWidget(this.tourGridSettings);
    }
    newFeaturedToursNotificationSettings: jqwidgets.NotificationOptions =
    {
        width: 250, position: "bottom-right", opacity: 1,
        autoOpen: false, animationOpenDelay: 800,
        autoClose: true, autoCloseDelay: 30000,
        showCloseButton: false,
        template: "info"
    };

    tourGridSource =
    {
        datatype: "json",
        datafields: [
            { name: 'ID', type: 'string' }
            , { name: 'Name', type: 'string' }
            , { name: 'IsFeatured', type: 'bool' }
            , { name: 'Status', type: 'string' }
            , { name: 'City', type: 'string' }
            , { name: 'UserID', type: 'string' }
            , { name: 'EnterTime', type: 'string' }
            , { name: 'ModifyTime', type: 'string' }
            , { name: 'BookingType', type: 'string' }

            //, { name: 'FirstName', type: 'string' }
            //, { name: 'Email', type: 'string' }
            //, { name: 'Address', type: 'string' }
            //, { name: 'Mobile', type: 'string' }
            //, { name: 'IsConfirmed', type: 'string' }
            //, { name: 'EnterTime', type: 'string' }

            //{ name: 'QuantityPerUnit', type: 'int' },
            //{ name: 'UnitPrice', type: 'float' },
            //{ name: 'UnitsInStock', type: 'float' },
            //{ name: 'Discontinued', type: 'bool' }
        ],
        //root: "Products",
        //record: "Product",
        //id: 'ProductID',
        //url: "../tsScripts/sampledata/products.xml"

        url: "../Admin/GetTours"
    };

    //userIDColumnFilter = function () {
    //    var filtergroup = new $.jqx.filter();
    //    var filter_or_operator = 1;
    //    var filtervalue = '99';
    //    console.log("Filter value is " + filtervalue);
    //    var filtercondition = 'equal';
    //    var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
    //    filtergroup.addfilter(filter_or_operator, filter);
    //    return filtergroup;
    //} ();

    tourGridDataAdapter = new $.jqx.dataAdapter(this.tourGridSource
        //, {
        //loadServerData: function (serverdata, source, callback) {
        //    $.ajax({
        //        dataType: source.datatype,
        //        url: "../Admin/GetUsers",
        //        data: serverdata,
        //        success: function (data, status, xhr) {
        //            //var xmldata = xmldata || $(source.root + " " + source.record, data);
        //            //if (!xmldata) {
        //            //    xmldata = [];
        //            //}
        //            //var dataArray = new Array();
        //            //for (var i = 0; i < xmldata.length; i++) {
        //            //    var record = xmldata[i];
        //            //    var datarow = {};
        //            //    for (var j = 0; j < source.datafields.length; j++) {
        //            //        var datafield = source.datafields[j];
        //            //        var value = '';
        //            //        if (undefined == datafield || datafield == null) {
        //            //            continue;
        //            //        }
        //            //        if (datafield.map) {
        //            //            value = $(datafield.map, record).text();
        //            //        }
        //            //        // searches by both selectors when necessary.
        //            //        if (value == '') {
        //            //            value = $(datafield.name, record).text();
        //            //        }
        //            //        datarow[datafield.name] = value;
        //            //    }
        //            //    dataArray[dataArray.length] = datarow;
        //            //}
        //            //// send the loaded records to the jqxDataAdapter plug-in.
        //            //callback({ records: dataArray });
        //        }
        //    });
        //    }
        //}
    );
    //endcelledit(rowBoundIndex: number, dataField: string, confirmChanges: boolean): void {
        
    //}
    //cellBeginEditEvent(event: any): void {
    //    alert();
    //    let args = event.args;
    //    //let begineditLog = document.getElementById('cellbegineditevent');
    //    //begineditLog.innerText = "Event Type: cellbeginedit, Column: " + args.datafield + ", Row: " + (1 + args.rowindex) + ", Value: " + args.value;
        
    //}
    //cellEndEditEvent(event: any): void {
    //    let args = event.args;
    //    //let endeditLog = document.getElementById('cellendeditevent');
    //    //endeditLog.innerText = "Event Type: cellendedit, Column: " + args.datafield + ", Row: " + (1 + args.rowindex) + ", Value: " + args.value;
    //    alert(args.value);
    //}

    tourGridOnCellclick(event: any): void {
        //alert(event.args.datafield);
        //let args = event.args;
        //let dataField = args.dataField;
        let dataField = event.args.datafield;
        let value = event.args.value;
        let rowIndex = event.args.rowindex;
        let tourId = this.tourGrid.getcellvalue(rowIndex, "ID");
        //alert("inside tourGridCellClick");
        switch (dataField) {

            case "":
                window.location.href = "../Admin/TourType/" + tourId;
                break;

            case "ID":
                var status = this.tourGrid.getcellvalue(rowIndex, "Status");
                if (status === 0) {
                    alert("Tour " + value + " is not complete yet.");
                    break;
                }
                window.open("../Tourist/Tour/" + value);
                break;

            case "UserID":
                window.open("../Tourist/Guider/" + value);
                break;

        }
    }

    tourGridOnCellbeginedit(event: any): void {
        //console.log("OnCellbeginedit");
        let args = event.args;
        //console.log(args.datafield + "  " + args.value);
        //let endeditLog = document.getElementById('cellendeditevent');
        //endeditLog.innerText = "Event Type: cellendedit, Column: " + args.datafield + ", Row: " + (1 + args.rowindex) + ", Value: " + args.value;
    }

    tourGridOnCellendedit(event: any): void {
        //console.log("OnCellendedit");
        let args = event.args;

        let rowIndex = args.rowindex;
        let tourId = this.tourGrid.getcellvalue(rowIndex, "ID");
        //console.log(args.datafield + "  " + args.value);
        
        let data = { id: tourId, status: args.value };
        let url = "/Admin/TourSetFeaturedTour";
        //this.http.post(url,
        //    JSON.stringify(data));
        
            //,
            //{ headers: { 'Content-Type': 'application/json' } });
        
        this.http.post(url, data)
            //.map(res => res.json()) // convert json
            .subscribe(
                () => this.newFeaturedToursNotification.open()
            );
        //let endeditLog = document.getElementById('cellendeditevent');
        //endeditLog.innerText = "Event Type: cellendedit, Column: " + args.datafield + ", Row: " + (1 + args.rowindex) + ", Value: " + args.value;
    }
    

    StatusCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {

        switch (value) {
            case 0:
                return '<div class="tour-panel-Status-div" style="float: ' + columnproperties.cellsalign + ';padding-left:5px;">Incomplete</div>';
        }

        let tourId = $("tour-panel").children("angulargrid").children("div").jqxGrid('getcellvalue', row, "ID"); 

        var options = '';
        switch (value) {
            case 1:
                options =
                    '<option class="tour-panel-Status-published-option" value="5">Published</option>' +
                    '<option class="tour-panel-Status-complete-option" value="1" selected>Complete</option>' +
                    '<option class="tour-panel-Status-inactive-option" value="3">Inactive</option>' +
                    '<option class="tour-panel-Status-active-option" value="2">Active</option>';
                break;
            case 2:
                options =
                    '<option class="tour-panel-Status-published-option" value="5">Published</option>' +
                    '<option class="tour-panel-Status-complete-option" value="1">Complete</option>' +
                    '<option class="tour-panel-Status-inactive-option" value="3">Inactive</option>' +
                    '<option class="tour-panel-Status-active-option" value="2" selected>Active</option>';
                break;
            case 3:
                options =
                    '<option class="tour-panel-Status-published-option" value="5">Published</option>' +
                    '<option class="tour-panel-Status-complete-option" value="1">Complete</option>' +
                    '<option class="tour-panel-Status-inactive-option" value="3" selected>Inactive</option>' +
                    '<option class="tour-panel-Status-active-option" value="2">Active</option>';
                break;
            case 5:
                options =
                    '<option class="tour-panel-Status-published-option" value="5" selected>Published</option>' +
                    '<option class="tour-panel-Status-complete-option" value="1">Complete</option>' +
                    '<option class="tour-panel-Status-inactive-option" value="3">Inactive</option>' +
                    '<option class="tour-panel-Status-active-option" value="2">Active</option>';
                break;
        }

        return '<select class="tour-panel-Status-selector" onchange="var option = this.options[this.selectedIndex];var newStatus=option.value; var tourId=' + tourId + '; UpdateTourStatus(tourId,newStatus);">'
            + options
            + '</select>';
    }


    EditCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<button class="tour-panel-Edit-button">Edit</button>';
    }
    BookingTypeCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        switch (value) {
            case 0:
                return '<div class="tour-panel-BookingType-div">Per Person</div>';
            case 1:
                return '<div class="tour-panel-BookingType-div">Per Group</div>';
        }
    }

    tourGridSettings: jqwidgets.GridOptions = {
        width: "100%",
        source: this.tourGridDataAdapter,

        showfilterrow: true,
        filterable: true,

        columnsresize: true,
        pageable: true,
        pagesize: 100,
        //pagermode: 'simple',
        autoheight: true,
        sortable: true,
        altrows: true,
        //enabletooltips: true,
        editable: true,
        //initrowdetails: this.tourGridInitrowdetails,
        //selectionmode: 'singlecell',
        selectionmode: 'multiplecellsadvanced',
        columns:
        [
            {
                text: 'ID', datafield: 'ID', cellsalign: 'left', align: 'center', width: 40
                //, filter: this.userIDColumnFilter
            }
            , { text: 'Tour Name', datafield: 'Name', cellsalign: 'left', align: 'center', width: 250 }

            //, { text: 'City', datafield: 'City', cellsalign: 'center', align: 'center', width: 120 }
            //, {
            //    text: 'Status', datafield: 'Status', cellsalign: 'left', align: 'center', width: 100
            //    ,cellsrenderer: this.StatusCellsRenderer
            //}
            , {
                text: 'Status', datafield: 'Status', cellsalign: 'left', align: 'center', width: 100, editable: false
                //, columntype: 'dropdownlist'
                ,cellsrenderer: this.StatusCellsRenderer
            }
            , { text: 'Featured', datafield: 'IsFeatured', columntype: 'checkbox', cellsalign: 'center', align: 'center', width: 60 }

            , {
                text: 'Edit', datafield: '', cellsalign: 'left', align: 'center', width: 40
                ,cellsrenderer: this.EditCellsRenderer
            }
            , { text: 'Guide ID', datafield: 'UserID', cellsalign: 'center', align: 'center', width: 60 }
            , { text: 'Last Modified', datafield: 'ModifyTime', cellsalign: 'left', align: 'center', width: 95 }
            , { text: 'Creation', datafield: 'EnterTime', cellsalign: 'left', align: 'center', width: 95 }
            , {
                text: 'Booking Type', datafield: 'BookingType', cellsalign: 'center', align: 'center', width: 90
                ,cellsrenderer: this.BookingTypeCellsRenderer
            }
        ]
    };
}


