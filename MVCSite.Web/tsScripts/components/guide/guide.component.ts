
import { Component, ViewChild, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { jqxNotificationComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxnotification';
import { jqxDataAdapterComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxdataadapter';
import { jqxGridComponent } from '../../jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxgrid';

@Component({
    selector: 'guide-panel',
    template: `
                <!--Notifications-->
                <angularNotification #newGuidesNotification>
                    <div>
                        Welcome.
                    </div>
                </angularNotification>

                <!--Grid-->
                <angularGrid #guideGridReference 
                    (OnBindingcomplete)="guideGridBindingcomplete($event)"
                    (OnCellclick)="guideGridCellClick($event)">
                </angularGrid> 
    `
})
export class GuideComponent implements AfterViewInit {
    public newUserNum : number;
    @ViewChild('newGuidesNotification') newGuidesNotification: jqxNotificationComponent;
    @ViewChild('guideGridReference') guideGrid: jqxGridComponent;


    ngAfterViewInit(): void {
        this.newGuidesNotification.createWidget(this.newGuidesNotificationSettings);
        this.guideGrid.createWidget(this.guideGridSettings);
    }

    newGuidesNotificationSettings: jqwidgets.NotificationOptions =
    {
        width: 250, position: "top-right", opacity: 0.9,
        autoOpen: true, animationOpenDelay: 800,
        autoClose: false, autoCloseDelay: 10000,
        template: "info"
    };


    guideGridSource =
    {
        datatype: "json",
        datafields: [
            { name: 'ID', type: 'string' }
            , { name: 'LastName', type: 'string' }
            , { name: 'FirstName', type: 'string' }
            , { name: 'Email', type: 'string' }
            , { name: 'Address', type: 'string' }
            , { name: 'Mobile', type: 'string' }
            , { name: 'IsConfirmed', type: 'string' }
            , { name: 'EnterTime', type: 'string' }

            //{ name: 'QuantityPerUnit', type: 'int' },
            //{ name: 'UnitPrice', type: 'float' },
            //{ name: 'UnitsInStock', type: 'float' },
            //{ name: 'Discontinued', type: 'bool' }
        ],
        //root: "Products",
        //record: "Product",
        //id: 'ProductID',
        //url: "../tsScripts/sampledata/products.xml"

        url: "../Admin/GetGuides"
    };
    guideGridDataAdapter = new $.jqx.dataAdapter(this.guideGridSource
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
    cellsrenderer = (row, columnfield, value, defaulthtml, columnproperties, rowdata) => {
        if (value < 20) {
            return '<span style="margin: 4px; float: ' + columnproperties.cellsalign + '; color: #ff0000;">' + value + '</span>';
        }
        else {
            return '<span style="margin: 4px; float: ' + columnproperties.cellsalign + '; color: #008000;">' + value + '</span>';
        }
    };

    IDcellsrenderer = (row, columnfield, value, defaulthtml, columnproperties, rowdata) => {

    };

    guideGridInitrowdetails = (index: number, parentElement: any, gridElement: any, record: any): void => {
        this.guideGridSource;
    }
    guideGridSettings: jqwidgets.GridOptions = {
        width: "100%",
        source: this.guideGridDataAdapter,

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
        //editable: true,
        initrowdetails: this.guideGridInitrowdetails,
        selectionmode: 'singlecell',
        //selectionmode: 'multiplecellsadvanced',
        columns:
        [
            { text: 'ID', datafield: 'ID', cellsalign: 'left', align: 'center', width: 40 }
            , { text: 'Last Name', datafield: 'LastName', cellsalign: 'left', align: 'center', width: 120 }
            , { text: 'First Name', datafield: 'FirstName', cellsalign: 'left', align: 'center', width: 120 }

            , { text: 'Address', datafield: 'Address', cellsalign: 'right', align: 'center', width: 250 }
            , { text: 'Email', datafield: 'Email', cellsalign: 'center', align: 'center', width: 250 }
            , { text: 'E-Confirmed', datafield: 'IsConfirmed', cellsalign: 'center', align: 'center', width: 100 }
            , {
                text: 'Phone', datafield: 'Mobile', cellsalign: 'left', align: 'center', width: 96
                //, columntype: 'numberinput',
                //validation: (cell: any, value: any): any =>
                //{
                //    return true;
                //},
                //createeditor: function (row: any, cellvalue: any, editor: any): void
                //{
                //    editor.jqxNumberInput({digits:10});
                //}

            }

            , { text: 'Signup Date', datafield: 'EnterTime', cellsalign: 'left', align: 'center', width: 95 }

            //, { text: 'Last Active', datafield: 'EnterTime', cellsalign: 'left', align: 'center', width: 100 }
            //,{
            //    text: 'Unit Price', columngroup: 'ProductDetails', datafield: 'UnitPrice', align: 'right', cellsalign: 'right', cellsformat: 'c2', width: 200
            //},
            //{
            //    text: 'Units In Stock', datafield: 'UnitsInStock', cellsalign: 'right', cellsrenderer: this.cellsrenderer, width: 100
            //},
            //{
            //    text: 'Discontinued', columntype: 'checkbox', datafield: 'Discontinued'
            //}
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

    guideGridCellClick(event: any): void {
        //alert(event.args.datafield);
        //let args = event.args;
        //let dataField = args.dataField;
        let dataField = event.args.datafield;
        let value = event.args.value;
        switch (dataField){
            case "ID":
                window.open("../Tourist/Guider/" + value);
                break;
            case "FirstName":
                //window.open("../Admin/Console/Tour/" + "88");
                break;
        }

        //if (dataField === "ID") {
        //    window.open("../Tourist/Guider/" + value);
        //}
    }


    guideGridBindingcomplete(event: any): void {
        var data = this.guideGrid.getrowdata(5);
        this.newUserNum = 0;
        for (var d in data) {
            this.newUserNum++;
        }
        //alert(this.newUserNum);
    }
}


