var OnClickBackButton = function (toUrl) {
    BootstrapDialog.show({
        title: 'Confirm to leave',
        message: "All changes you made on this page will be lost. Are you sure you want to leave?",
        buttons: [{
            label: 'Leave',
            cssClass: 'btn btn-primary',
            action: function (dialog) {
                dialog.close();
                location.href = toUrl;
            }
        },
        {
            label: 'Cancel',
            cssClass: 'btn btn-primary',
            action: function (dialog) {
                dialog.close();
            }
        },
        ]
    });
};
var ensureNumberInputNotEmpty = function () {
    var durVal = $(this).val();
    if (durVal === "") {
        $(this).val(0);
    }
};