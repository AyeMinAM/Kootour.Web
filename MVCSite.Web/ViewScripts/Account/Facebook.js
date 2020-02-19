function fbStatusChangeCallback(response, needToLoginWihtFb, fromPage) {
    if (!needToLoginWihtFb)
        return;
    if (response.status === 'connected') {
        loginWithFb(response,fromPage);
    } else {
        if (needToLoginWihtFb) {
            FB.login(function (response) {
                if (response.status === 'connected') {
                    FB.api('/me', { locale: 'en_US', fields: 'name, email, location, picture,first_name,last_name' }, function (response) {
                        var objForCheck = new Object();
                        objForCheck.loginId = response.email;
                        objForCheck.loginType = "2";
                        $.ajax({
                            url: '/Account/JSCheckUser',
                            type: 'post',
                            dataType: 'json',
                            data: {
                                loginId: response.id,
                                loginType: 2,
                                email: response.email,
                            },
                            success: function (json) {
                                var objForCheck = json;
                                if (objForCheck.result) {
                                    loginWithFb(response,fromPage);
                                } else {
                                    $.ajax({
                                        url: '/Account/JSRegister',
                                        type: 'post',
                                        dataType: 'json',
                                        data: {
                                            loginId: response.id != undefined ? response.id : "",
                                            loginType: "2",
                                            firstName: response.first_name != undefined ? response.first_name : "",
                                            lastName: response.last_name != undefined ? response.last_name : "",
                                            email: response.email != undefined ? response.email : "",
                                            password: '',
                                            location: response.location != undefined ? response.location.name : "",
                                            avatar: response.picture.data.url != undefined ? response.picture.data.url : "",
                                            Role:currentUserRole,
                                        },
                                        success: function (json) {
                                          
                                            var objForSign = json;
                                            if (objForSign.result) {
                                                window.location.href = objForSign.url;
                                            }
                                             else {
                                                BootstrapDialog.show({
                                                    title: 'Error',
                                                    message: objForSign.message,
                                                    buttons: [{
                                                        label: 'Close',
                                                        action: function (dialog) {
                                                            dialog.close();
                                                        }
                                                    }]
                                                });
                                            }
                                        },
                                        error: function (json) {
                                            var objForSign = json;
                                            BootstrapDialog.show({
                                                title: 'Error',
                                                message: objForSign.message,
                                                buttons: [{
                                                    label: 'Close',
                                                    action: function (dialog) {
                                                        dialog.close();
                                                    }
                                                }]
                                            });
                                        }
                                    });
                                }
                            },
                            error: function (json) {
                                var objForCheck = json;
                                BootstrapDialog.show({
                                    title: 'Error',
                                    message: objForCheck.message,
                                    buttons: [{
                                        label: 'Close',
                                        action: function (dialog) {
                                            dialog.close();
                                        }
                                    }]
                                });
                            }
                        });
                    });
                } else {
                    BootstrapDialog.show({
                        title: 'Error',
                        message: 'Facebook authorization failed, please login again.',
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                }
            }, { scope: 'public_profile,email,user_location' });
        }
    }
}

function checkLoginStateWithFb(needToLoginWihtFb,fromPage) {
    FB.getLoginStatus(function (response) {
        fbStatusChangeCallback(response, needToLoginWihtFb,fromPage);
    });
}
function loginWithFb(response,fromPage) {
    FB.api('/me', { locale: 'en_US', fields: 'name, email, location, picture,first_name,last_name' }, function (response) {
        var objForLogin = new Object();
        objForLogin.loginType = "2";
        objForLogin.loginId = response.email;
        $.ajax({
            url: '/Account/JSLogon',
            type: 'post',
            dataType: 'json',
            data: {
                loginId: response.id,
                loginType: 2,
                email: response.email,
            },
            success: function (json) {
                var objForLogin = json;
                FB.logout();//We need to logout when synchronization error happened.
                if (objForLogin.result) {
                    if (fromPage != "Tour")
                        window.location.href = objForLogin.url;
                    else
                    {
                        $('#logonModal').modal('toggle');
                        $('#BookTourForm').submit();
                    }
                } else {
                    BootstrapDialog.show({
                        title: 'Error',
                        message: objForLogin.message,
                        buttons: [{
                            label: 'Close',
                            action: function (dialog) {
                                dialog.close();
                            }
                        }]
                    });
                }
            },
            error: function (json) {
                FB.logout();//We need to logout when synchronization error happened.
                var objForLogin = json;
                BootstrapDialog.show({
                    title: 'Error',
                    message: objForLogin.message,
                    buttons: [{
                        label: 'Close',
                        action: function (dialog) {
                            dialog.close();
                        }
                    }]
                });
            }
        });
    });
}