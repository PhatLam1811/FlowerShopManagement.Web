// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showContent2(url, title) {

    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
            //$.notify("I'm over here !");
            //$.notify("Access granted", "success", { position: "right" });

        }
    })
}

function OpenPostDialog(url, title) {
    try {
        $.ajax({
            type: "POST",
            url: url,

            success: function (res) {

                $("#form-modal .modal-body").html(res);
                $("#form-modal .modal-title").html(title);
                $("#form-modal").modal('show');
                //$.notify("I'm over here !");
                //$.notify("Access granted", "success", { position: "right" });
            },
            error: function (err) {
                console.log(err);
                alert(err);
            }
        })
    }

    catch (e) {
        console.log(e);
        alert(e);
    }
   
}

function EditPage(url, title, id) {

    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },

    })
}

function jQueryAjaxReloadViewAll(url) {
    try {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (res) {
                if (res.isValid) {
                    $('#hihi').html(res.htmlViewAll);
                    $('#pagination').html(res.htmlPagination);
                }
            },
            error: function (err) {
                alert(err);

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;

    } catch (ex) {

        alert(ex);
        return false;
    }
}

function jQueryAjaxReloadPickingTable(form) {
    var obj = new FormData(form);
    console.log(obj);

    debugger;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: obj,
            contentType: false,
            processData: false,
            success: function (res) {
                alert(res);

                $('#picked-items').html(res);
                $("#form-modal .modal-body").html('');
                $("#form-modal .modal-title").html('');
                $("#form-modal").modal('hide');
            },
            error: function (err) {
                alert("some error happens! ");

                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;

    } catch (ex) {

        alert(ex);
        return false;
    }
}

function jQueryAjaxSearch(form) {
    var obj = new FormData(form);
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: obj,
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#hihi').html(res.htmlViewAll);
                    $('#pagination').html(res.htmlPagination);
                }
            },
            error: function (err) {

                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
        return false;

    }
    //to prevent default form submit event


}