(function (app) {
    app.controller('productEditController', productEditController);
    productEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService','$stateParams'];

    function productEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.Product = {};
        $scope.moreImages = [];
        //lay trong scope
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }

        $scope.GetSeoTitle = GetSeoTitle;
        //load detail

        function loadProductDetail() {
            apiService.get('api/Product/getbyid/' + $stateParams.id, null, function (result) {
                $scope.Product = result.data;

                $scope.moreImages = JSON.parse($scope.Product.MoreImages); //chuyển về dạng json
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function GetSeoTitle() {
            $scope.Product.Alias = commonService.getSeoTitle($scope.Product.Name);
        }


        $scope.UpdateProduct = UpdateProduct; //su kien cho nut 


        //update la put
        function UpdateProduct() {
            $scope.Product.MoreImages = JSON.stringify($scope.moreImages); //chuyen qua dang String
            apiService.put('api/Product/update', $scope.Product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('cập nhật không thành công.');
                }
            )
        }
        //Doc du lieu cho danh muc cha

        function loadProductCategory() {
            apiService.get('/api/ProductCategory/getallparents', null, function (result) {
                $scope.ProductCategories = result.data;

            }, function () {
                console.log('Cannot get list parent');
            });
        }


        //su kien cho nút chọn ảnh
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.Product.Image = fileUrl;
                })

            }

            finder.popup();
        }
        
        //ham load du lieu san pham,xử lý sự kiệ cho nút click

        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                //cho nó cập nhật ngay
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })
            }

            finder.popup();
        }

        //load danh mục cha
        loadProductCategory();
        //load danh mục chi tiết sản phẩm
        loadProductDetail();
    }
})(angular.module('tedushop.products'));