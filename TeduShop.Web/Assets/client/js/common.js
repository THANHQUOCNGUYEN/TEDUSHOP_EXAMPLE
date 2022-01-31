var common = {
    init: function () {
        common.registerEvents();
        common.getBagCart();
    },
    registerEvents: function () {
        $('#btnLogout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#frmLogout').submit();
        });
        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                datatype: 'json',
                success: function (response) {
                    if (response.status) {
                       
                        common.getBagCart();
                    }
                }
            });
           
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            common.getBagCart();
        });
        //history loading
        

        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: '/Product/GetListProductByName',
                    dataType: 'json',
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);

                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li class='auto-complete'>")
                .append("<i class='fa fa-search' aria-hidden=true></i>"+'&nbsp;'+item.label)
                .appendTo(ul);
        };

    },
    getBagCart: function () {
        $.ajax({
            url: '/Home/BagCart',
            dataType: 'json',
            type: 'post',
            success: function (result) {
                if (result.status) {
                    $('.bagcart').text(result.data);
                }
            }
        });
    }
}
common.init();
