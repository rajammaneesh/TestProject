(function () {
    'use strict';
    angular.module('dCodeApp')
    .controller('commonController', commonController);

    commonController.$inject = ['$scope', '$http', '$rootScope', 'UserContextService'];

    function commonController($scope, $http, $rootScope, UserContextService) {
        $scope.user = [];
        $scope.loginOptions = ['Requestor', 'Contributor'];
        $scope.userMockOption = { login: null };

        $scope.SetUserContext = function () {
            $http({
                method: 'POST',
                url: '/Common/SetUser',
                data: {
                    firstName: $scope.user.FirstName,
                    lastName: $scope.user.LastName,
                    emailId: $scope.user.Email
                },
                async: true,
            }).success(function (data, status, config) {
                if (data != null) {
                    alert('user set!');
                }
                // handle success things
            }).error(function (data, status, config) {
                // handle error things
            });
        }
        $scope.Apply = function () {
            var reqObj = $scope.task;
            $http({
                url: "/Common/Apply",
                method: "POST",
                data: {
                    taskId: reqObj.TaskId,
                    firstName: reqObj.FirstName,
                    lastName: reqObj.LastName,
                    emailId: reqObj.EmailId,
                    mgrEmail: reqObj.ManagerEmailId
                }
            }).success(function (data, status, config) {
                var test = data;
                if (data != null) {
                    alert('Applicant applied!');
                }
            }).error(function (error) {
            });
        }
    }
})();