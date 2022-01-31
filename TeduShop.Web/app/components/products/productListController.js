(function (app) {
    //các controller và module phải viết đúng theo định dạng để không bị mất thời gian
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox']; //$inject cac function
    function productListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.productCategories = []; //phan nay thua

        //khai báo biến 
        $scope.page = 0; //page
        $scope.pagesCount = 0;  //tong so trang    
        $scope.getProducts = getProducts; //Đây giống như hàm void
        $scope.keyword = ''; //khai bao

        $scope.search = search;
        $scope.deleteProduct = deleteProduct;//phải truyền xuống scode thì nó mới nhận được
        $scope.exportPdf = exportPdf;
        function deleteProduct(id) {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                //gom 4 phuong thuc nghen
                apiService.del('api/product/delete', config, function () {
                    notificationService.displaySuccess('xóa thành công');
                    search(); //gọi lại phương thức search
                }, function () {
                    notificationService.displayError('Xoá không thành công')
                })
            });

        }

        function exportPdf(productID) {
            var config = {
                params:{
                    id: productID
                }
               
            }
            apiService.get('/api/product/ExportPdf', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);
            });
        }

        //goi hàm search cho button click
        function search() {
            getProducts();
        }
        function getProducts(page) {
            page = page || 0; //neu page = null thi page  = 0
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 2
                }
            }
            //config la co tham so ,neu khong truyen tham so thi coi nhu khong co config
            apiService.get('/api/product/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                else {
                    notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount + 'bản ghi');
                }
                $scope.products = result.data.Items; //danh sach hien tai
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages; //tong so trang
                $scope.totalCount = result.data.TotalCount;// tong so ban ghi
            }, function () {
                console.log('Load product failed .'); //ghi log ra console
            });
        }

        //load du lieu len comboxbox
      
        $scope.getProducts();
    }
})(angular.module('tedushop.products'));