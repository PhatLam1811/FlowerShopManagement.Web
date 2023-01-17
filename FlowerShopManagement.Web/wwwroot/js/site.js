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

            //$.notify("Access granted", "success", { position: "right" });

        }
    })
}

function showPartialView(url) {

    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (res) {
            $("#pw").html(res);
        },
        error: function (err) {
            console.log(err);
            alert(err);
        }
    })
}

function showPartialView1(form) {
    var obj = new FormData(form);
    $.ajax({
        type: "POST",
        url: form.action,
        data: obj,
        contentType: false,
        processData: false,
        success: function (res) {

            $("#pw").html(res);
            console.log(res);

        },
        error: function (err) {
            console.log(err);
            alert(err);
        }
    })
    return false;
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

function OpenFormDialog(form) {
    var obj = new FormData(form);
    console.log(obj);

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: obj,
            contentType: false,
            processData: false,
            success: function (res) {

                //$('#picked-items').html(res);
                $("#form-modal .modal-body").html(res);
                $("#form-modal .modal-title").html('');
                $("#form-modal").modal('show');
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

function OpenGetPage(url) {
    try {
        $.ajax({
            type: "GET",
            url: url,

            success: function (res) {
                $("#main").html(res);
            },
            error: function (err) {
                console.log(err);
                alert(err);
            }
        })
        return false;
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

function jQueryAjaxPickCustomer(url, id) {
    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { phone: id },
            success: function (res) {
                $("#form-modal .modal-body").html('');
                $("#form-modal .modal-title").html('');
                $("#form-modal").modal('hide');
                $('#picked-cus').html(res);
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

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: obj,
            contentType: false,
            processData: false,
            success: function (res) {
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

function jQueryAjaxReloadPickingTableWithId(url, id) {

    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { id: id },
            success: function (res) {
                $('#picked-items').html(res);
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

function FindDistricts(selectTag, url) {
    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { city: selectTag.options[selectTag.selectedIndex].value },
            success: function (res) {
                $('#district').empty();
                $('#ward').empty();
                $.each(res, function (index, key) {
                    $('#district').append($('<option>', {

                        text: key
                    }));
                });
                $('#district').removeClass("disabled");
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

function FindWards(selectTag, url) {

    try {
        $.ajax({
            type: 'POST',
            url: url,
            data: { city: $('#city').find(":selected").text(), district: selectTag.options[selectTag.selectedIndex].text },
            success: function (res) {

                $('#ward').empty();

                $.each(res, function (index, key) {
                    $('#ward').append($('<option>', {

                        text: key
                    }));
                });
                $('#ward').removeClass("disabled");

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

function callWithId(url, id) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function (res) {
            if (res.isValid) {
                //$('#hihi').html(res.html);
                //$.notify("Added to your wishlist", "success", { position: "right" });
            }
            else {
                //$.notify("Error", "warn", { position: "right" });
            }

        }
    })
}

function callGetWithId(url, id) {
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        success: function (res) {
            if (res.isValid) {
                //$('#hihi').html(res.html);
                //$.notify("Added to your wishlist", "success", { position: "right" });
            }
            else {
                //$.notify("Error", "warn", { position: "right" });
            }

        }
    })
}

function callPost(url) {
    $.ajax({
        type: "POST",
        url: url,
        success: function (res) {
            $('#hix').html(res);
        }
    })
}

function updateAmount(url, url2, id, amount) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id, amount: amount },
        success: function (res) {
            $.ajax({
                type: "POST",
                url: url2,
                success: function (res) {
                    $('#hix').html(res);
                }
            })
        }
    })
}

function updateSelection(url, url2, id, isSelected) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id, isSelected: isSelected },
        success: function (res) {
            $.ajax({
                type: "POST",
                url: url2,
                success: function (res) {
                    $('#hix').html(res);
                }
            })
        }
    })
}

function confirmRemove() {
    if (confirm("Press")) {
        return true;
    }
    return false;
}

function removeCartItem(url, url2, id) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function () {
            $.ajax({
                type: "POST",
                url: url2,
                success: function (res) {
                    $('#hix').html(res);
                }
            })
        }
    })
}

function showAlert(title) {
    alert(title);
}

function removeAddress(url, name, phone, address) {
    $.ajax({
        type: "POST",
        url: url,
        data: { name: name, phone: phone, address: address },
        success: function (res) {
            $("#pw").html(res);
            console.log(res);

        }
    })
}

function addToCart(url, id, amount) {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id, amount: amount },
        success: function () {
        }
    })
}

function buyNow(url, id, amount) {
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id, amount: amount },
        success: function (res) {
        }
    })
}

function addAddress(form) {
    var obj = new FormData(form);
    $.ajax({
        type: "POST",
        url: form.action,
        data: obj,
        contentType: false,
        processData: false,
        success: function (res) {
            $("#form-modal .modal-body").html('');
            $("#form-modal .modal-title").html('');
            $("#form-modal").modal('hide');
            $("#pw").html(res);
            console.log(res);
        },
        error: function (err) {
            console.log(err);
            alert(err);
        }
    })
    return false;
}