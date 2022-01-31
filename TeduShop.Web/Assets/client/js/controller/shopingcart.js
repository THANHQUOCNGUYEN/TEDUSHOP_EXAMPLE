var cart = {
    init: function () {
        cart.loadData();

        cart.registerEvent();
    },

    registerEvent: function () {
        //validate form thanh toán
        $('#frmPayment').validate({
            rules: {
                name: "required",
                Address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "yêu cầu nhập tên",
                Address: "phải nhập địa chỉ",
                email: {
                    required: "phải nhập email",
                    email: "email chưa đúng định dạng"
                },
                phone: {
                    required: "phải nhập số điên thoại",
                    number: "Số điện thoại phải là số",
                }
            }
        });


        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });

        $('.txtQuantity').off('keyup').on('keyup', function () { //su kien change cua textbox
            var quantity = parseInt($(this).val());
            var productId = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + productId).text(amount);
            } //la so
            else //neu khong la so
            {
                $('#amount_' + productId).text(0);
            }

            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));

            //mỗi lần keyup ta phải update nó lên server
            cart.updateAll();
            //gan vao text
            cart.getBagCart();
        });


        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });


        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();

        });

        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });

        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked')) {
                cart.getLoginUser();
            }
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });

        //thanh toan don hang
        $('#btnCreateOrder').off('click ').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                //neu thoa validation thi moi thuc hien function Order
                cart.createOrder();
            }
        });
        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'NL') {
                $('.boxContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() == 'ATM_ONLINE') {
                $('.boxContent').hide();
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });
    },

    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });

        return total;
    },
	
    createOrder: function () {
        //văng doi tuong qua controller:back-end de tien hanh xu ly
        
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[GroupName="bankcode"]:checked').prop('id'),
            Status:false//chua duoc xu lu
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {
                        window.location.href = response.urlCheckout;//chuyen ve trang thanh toan
                    }
                    else {
                        //thanh toan thanh cong
                        $('#divCheckout').hide();
                        cart.deleteAll();

                        //chú ý set timeout
                        setTimeout(function () {
                            $('#cartContent').html('Cảm ơn bạn đã đặt hàng thành công.Chúng tôi sẽ liên hệ sớm nhất.', 2000);
                        });
                    }

                }
                else {
                    //lỗi kết nối tài khoản ngân lượng,đảm bảo lỗi này không xảy ra
                    alert(response.message)
                }
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                    $('#txtMessage').val(user.messages);
                }
            }
        });
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.Price,
                            Quantity: item.Quantity,
                            Amount: item.Quantity * item.Product.Price //tong tien
                        });
                    });

                    $('#cartBody').html(html);


                    if (html == '') {
                        $('#cartContent').html('<strong style="color:red">Không có sản phẩm nào trong giỏ hàng</strong>');
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                    cart.registerEvent(); //sau cai nay la 1 su phai co de buding no a
                }
            }
        })
    },

    updateAll: function () {
        //lấy danh sách các thay đổi của textbox số lượng truyền qua kia để xử lý
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });

        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('update ok');
                }
            }
        });
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
cart.init();