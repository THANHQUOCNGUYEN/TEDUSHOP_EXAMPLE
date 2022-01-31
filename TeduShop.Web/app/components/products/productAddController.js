(function (app) {
    app.controller('productAddController', ProductAddController);
    ProductAddController.$inject = ['apiService', '$scope', 'notificationService', '$state','commonService'];

    function ProductAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.Product = {
            CreatedDate: new Date(),
            Status: true,
            //Name : Danh mục 1
        }

        //lay trong scope
        $scope.ckeditorOptions = {
            languague: 'vi',
            height :'200px'
        }

        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.Product.Alias = commonService.getSeoTitle($scope.Product.Name);
        }
        $scope.AddProduct = AddProduct; //su kien cho nut 

        function AddProduct() {
            $scope.Product.MoreImages = JSON.stringify($scope.moreImages); //chuyen qua dang String
            apiService.post('api/Product/create', $scope.Product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm');
                    $state.go('products');//đưa về trang chủ
                }, function (error) {
                    notificationService.displayError('thêm mới không thành công.');
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

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.Product.Image = fileUrl;
                })
                
            }

            finder.popup();
        }

        $scope.moreImages = [];
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

        loadProductCategory();

        //goi ham loadParent
    }
})(angular.module('tedushop.products'));