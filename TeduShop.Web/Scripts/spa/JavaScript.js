/// <reference path="../plugins/angular.min.js" />

var myapp = angular.module("my-App", []);
myapp.controller('myController', myController);
/*myController.$inject["$scope", "totalTwoNumber"];*/

function myController($scope) {
    $scope.total = totalTwoNumber(1, 6);
}

function totalTwoNumber(input1, input2) {
    return input1 + input2;
}