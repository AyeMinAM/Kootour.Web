import { Component, ViewChild, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { jqxNotificationComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxnotification';
import { jqxDataAdapterComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxdataadapter';
import { jqxGridComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxgrid';



declare function UpdateUserBio(row: any,value:any): any;

@Component({
    selector: 'user-panel',
    template: `
                <!--Grid-->
                <angularGrid #userGridReference 
                    (OnBindingcomplete)="userGridOnBindingcomplete($event)"
                    (OnCellclick)="userGridOnCellclick($event)"
                    (OnCellbeginedit) = "userGridOnCellbeginedit($event)" 
                    (OnCellendedit)="userGridOnCellendedit($event)">
                </angularGrid> 

                <!--Notifications-->
                <angularNotification #newGuidesNotification>
                    <div>
                        Welcome.
                    </div>
                </angularNotification>
    `
})
export class UserComponent implements AfterViewInit {
    public newUserNum : number = 0;
    @ViewChild('newGuidesNotification') newGuidesNotification: jqxNotificationComponent;
    @ViewChild('userGridReference') userGrid: jqxGridComponent;


    ngAfterViewInit(): void {
        this.newGuidesNotification.createWidget(this.newGuidesNotificationSettings);
        this.userGrid.createWidget(this.userGridSettings);
        //this.userGrid.pincolumn("ID");
        //this.userGrid.pincolumn("LastName");
        //this.userGrid.pincolumn("FirstName");
    }

    newGuidesNotificationSettings: jqwidgets.NotificationOptions =
    {
        width: 250, position: "bottom-right", opacity: 1,
        autoOpen: true, animationOpenDelay: 800,
        autoClose: true, autoCloseDelay: 60000,
        showCloseButton: false,
        template: "info"
    };


    userGridSource =
    {
        datatype: "json",
        datafields: [
            { name: 'ID', type: 'string' }
            , { name: 'LastName', type: 'string' }
            , { name: 'FirstName', type: 'string' }
            , { name: 'Role', type: 'string' }
            , { name: 'Email', type: 'string' }
            , { name: 'Address', type: 'string' }
            , { name: 'PhoneAreaCode', type: 'string' }
            , { name: 'Mobile', type: 'string' }
            , { name: 'IsConfirmed', type: 'string' }
            , { name: 'EnterTime', type: 'string' }
            , { name: 'Bio', type: 'string' }


            //{ name: 'QuantityPerUnit', type: 'int' },
            //{ name: 'UnitPrice', type: 'float' },
            //{ name: 'UnitsInStock', type: 'float' },
            //{ name: 'Discontinued', type: 'bool' }
        ],
        //root: "Products",
        //record: "Product",
        //id: 'ProductID',
        //url: "../tsScripts/sampledata/products.xml"

        url: "../Admin/GetUsers"
    };
    userGridDataAdapter = new $.jqx.dataAdapter(this.userGridSource
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

    IDcellsrenderer = (row, columnfield, value, defaulthtml, columnproperties, rowdata) => {

    };
    RoleCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        switch (value) {
            case 0:
                return '<div class="user-panel-Role-div">Guide</div>';
            case 1:
                return '<div class="user-panel-Role-div">Traveller</div>';
        }
    }
    EmailConfirmedCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        if (value) {
            return '<div class="user-panel-EmailConfirmed-true-div">Yes</div>';
        } else {
            return '<div class="user-panel-EmailConfirmed-false-div">No</div>';
        }
    }
    userGridInitrowdetails = (index: number, parentElement: any, gridElement: any, record: any): void => {
        this.userGridSource;
    }


    EditCellsRenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        return '<button class="tour-panel-Edit-button">Edit</button>';
    }

    userGridSettings: jqwidgets.GridOptions = {
        width: "100%",
        source: this.userGridDataAdapter,

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
        initrowdetails: this.userGridInitrowdetails,
        selectionmode: 'singlecell',
        //selectionmode: 'multiplecellsadvanced',
        columns:
        [
            { text: 'ID', datafield: 'ID', cellsalign: 'left', align: 'center', width: 40 }
            , { text: 'Last Name', datafield: 'LastName', cellsalign: 'left', align: 'center', width: 100}
            , { text: 'First Name', datafield: 'FirstName', cellsalign: 'left', align: 'center', width: 90 }
            , {
                text: 'Role', datafield: 'Role', cellsalign: 'center', align: 'center', width: 70 
                , cellsrenderer: this.RoleCellsRenderer
            }
            , { text: 'Address', datafield: 'Address', cellsalign: 'right', align: 'center', width: 150 }
            , { text: 'Email', datafield: 'Email', cellsalign: 'center', align: 'center', width: 230 }
            , {
                text: 'EConfirmed', datafield: 'IsConfirmed', cellsalign: 'center', align: 'center', width:80
                , cellsrenderer: this.EmailConfirmedCellsRenderer
            }
            , { text: 'Area Code', datafield: 'PhoneAreaCode', cellsalign: 'center', align: 'left', width: 30 }
            , { text: 'Phone', datafield: 'Mobile', cellsalign: 'left', align: 'center', width: 100 }
            , { text: 'Signup Date', datafield: 'EnterTime', cellsalign: 'left', align: 'center', width: 95,editable: false }
            , { text: 'Avatar', datafield: 'AvatarUrl', cellsalign: 'left', align: 'center', width: 60, editable: false, cellsrenderer: this.EditCellsRenderer }
            , { text: 'Bio', datafield: 'Bio', cellsalign: 'left', align: 'center', width: 400 }
        ]


        //columns:
        //[
        //    {
        //        text: 'Product Name', columngroup: 'ProductDetails',
        //        datafield: 'ProductName', width: 250
        //    },
        //    {
        //        text: 'Quantity per Unit', columngroup: 'ProductDetails',
        //        datafield: 'QuantityPerUnit', cellsalign: 'right', align: 'right', width: 200
        //    },
        //    {
        //        text: 'Unit Price', columngroup: 'ProductDetails', datafield: 'UnitPrice', align: 'right', cellsalign: 'right', cellsformat: 'c2', width: 200
        //    },
        //    {
        //        text: 'Units In Stock', datafield: 'UnitsInStock', cellsalign: 'right', cellsrenderer: this.cellsrenderer, width: 100
        //    },
        //    {
        //        text: 'Discontinued', columntype: 'checkbox', datafield: 'Discontinued'
        //    }
        //],
        //columngroups:
        //[
        //    {
        //        text: 'Product Details', align: 'center', name: 'ProductDetails'
        //    }
        //]
    };

    userGridOnCellclick(event: any): void {
        //alert(event.args.datafield);
        //alert(event.args.datafield + "," + event.args.row);

        //let args = event.args;
        //let dataField = args.dataField;
        let dataField = event.args.datafield;
        let value = event.args.value;
        var id = event.args.row.bounddata.ID;
        switch (dataField){
            case "AvatarUrl":
                window.open("../Account/EditProfile/" + id);
                break;
           
        }

     //   var row = event.args.row.bounddata;
      //  UpdateUserBio(row);
    }

    userGridOnCellendedit(event: any): void {

        var row = event.args.row;
        UpdateUserBio(row, event.args.value);
    }
    
    userGridOnCellbeginedit(event: any): void {
        var row = event.args.row;
    
    }


    userGridOnBindingcomplete(event: any): void {
        //var data = this.userGrid.getrowdata(5);
        //this.newUserNum = 0;
        //for (var d in data) {
        //    this.newUserNum++;
        //}
        this.newUserNum = this.userGrid.getboundrows().length;
        //alert(this.newUserNum);
        //this.newGuidesNotification.appendContainer("#newGuidesNotification");
        //$("#newGuidesNotification div").html("ffff");
    }
}


