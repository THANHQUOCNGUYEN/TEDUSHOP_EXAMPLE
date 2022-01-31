/// <reference path="../plugins/angular.min.js" />

var myApp = angular.module('my-App', [])
myApp.controller("schoolController", schoolController);
myApp.service('Validator', Validator);// giong nghu 1 cau ham

myApp.directive("teduShopDirective", teduShopDirective)

schoolController.$inject['$scope', 'Validator']
function schoolController($scope, Validator) {
   
    $scope.checkNumber = function () {
        $scope.message = Validator.checkNumber($scope.num);
    }

    $scope.num = 1;
}   

function Validator($window) {

    return {
        checkNumber: checkNumber
    }
    function checkNumber(input) {
        if (input % 2 == 0) {
            return 'This is even';
        }
        else {
            return 'This is old';
        }
    }
}

function teduShopDirective() {
    return {
        templateUrl: "/Scripts/spa/teduShopDirective.html"
    }
}