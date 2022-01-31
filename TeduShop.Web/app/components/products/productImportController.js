(function (app) {
    app.controller('productImportController', productImportController);
    productImportController.$inject = ['apiService','$http','authenticationService', '$scope', 'notificationService', '$state', 'commonService'];

    function productImportController(apiService, $scope, notificationService, $state, commonService) {

        $scope.files = [];
        $scope.categoryId = 0;

        //listen for the file selected event
        $scope.$on('fileSelected', function (event, args) {
            $scope.$apply(function () {
                //add the file object to the scope's files collection
                $scope.files.push(args.file);
            });
        });
        $scope.ImportProduct = ImportProduct; //su kien cho nut

        function ImportProduct() {
            apiService.post('api/Product/import', $scope.Product,
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
        loadProductCategory();
    }
})(angular.module('tedushop.products'));