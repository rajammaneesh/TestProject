//dCodeApp.controller('layoutController', ['$scope', '$http', '$rootScope', 'UserContextService', function ($scope, $http, $rootScope, userContextService) {
//    $scope.tabName = null;
//}]);

(function () {
    'use strict';
    angular.module('dCodeApp')
        .controller('LayoutController', LayoutController);

    LayoutController.$inject = ['$scope', '$http', '$rootScope', '$location', 'UserContextService'];

    function LayoutController($scope, $http, $rootScope, $location, UserContextService) {
        $scope.tabName = null;
        $scope.userContext = null;
        $rootScope.permissionsCount = null;
        $scope.gamificationStats = null;
        $scope.gamificationBannerContent = null;

        $scope.navigateToProfile = function () {
            location.href = '/Profile/Profile';
        }

        $scope.showPopup = { isVisible: false }

        $scope.IsRequestor = function () {
            if ($scope.userContext != null)
                return $scope.userContext.Role == 1;
            else
                return false;
        }

        $scope.isCoreRoleRequestor = function () {
            if ($scope.userContext != null) {
                return $scope.userContext.IsCoreRoleRequestor;
            }
            return false;
        }

        $scope.open = function () {
            $scope.showPopup.isVisible = true;
        }

        $scope.cancel = function () {
            location.href = '/requestor/Dashboard'
        }

        $scope.submit = function () {
            $scope.SwitchContext();
        }


        $scope.SwitchContext = function (value) {
            // location.href = '/Common/SwitchRole'
            $http({
                url: "/Common/SwitchRoleFromLayout",
                method: "POST",
                data: { roleFromUI: value }
            }).success(function (data, status, headers, config) {
                var test = data;
                if (data.Role == 2) {
                    location.href = '/contributor/Dashboard';
                }
                else if (data.Role == 1) {
                    location.href = '/requestor/Dashboard';
                }
            }).error(function (error) {
            });
        }

        $scope.GetGamificationStats = function () {
            $http({
                url: "/Common/GetGamificationStats",
                method: "GET"
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    $scope.gamificationStats = data;
                }
            }).error(function (error) {
                $scope.gamificationStats = null;
            });
        }

        $scope.GetGamificationBanner = function () {
            $http({
                url: "/Common/GetBannerMessage",
                method: "GET"
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    $scope.gamificationBannerContent = data;
                }
            }).error(function (error) {
                $scope.gamificationBannerContent = null;
            });
        }

        $scope.ResetSuccess = function () {
            $scope.successMessage = null;
            $scope.errorMessage = null;
            $scope.suggestionValue = null;
        }
        $scope.insertSuggestion = function () {
            if ($scope.suggestionValue != null) {
                $http({
                    url: "/Common/InsertNewSuggestion",
                    method: "POST",
                    data: { suggestion: $scope.suggestionValue }
                }).success(function (data, status, headers, config) {
                    if (data != undefined) {
                        if (data != null) {
                            $scope.successMessage = null;
                            $scope.errorMessage = null;
                            if (data > 0) {
                                $scope.successMessage = "Thank you for your feedback!";
                                $scope.suggestionValue = null;
                            }
                            else {
                                $scope.errorMessage = "Sorry cannot post suggestions now! Please try again.";
                                $scope.suggestionValue = null;
                            }
                        }
                    }
                }).error(function (error) {
                });
            }
            else { $scope.errorMessage = "Please enter a few words."; }
        }

        $scope.GetPermissionsCount = function () {
            $http({
                url: "/Requestor/GetPermissionsCount",
                method: "GET",
            }).success(function (data, status, headers, config) {
                if (data != undefined) {
                    if (data != null) {
                        $rootScope.permissionsCount = data;
                    }
                }
            }).error(function (error) {
            });
        }

        $scope.SetModalLoaded = function () {
            $http({
                url: "/Common/SetModalLoaded",
                method: "GET",
            }).success(function (data, status, headers, config) {
            }).error(function (error) {
            });
        }

        //Load page only after usercontext loads
        UserContextService.InitializeUserContext().then(function (data) {
            $scope.onLoad();
        });

        $scope.$on('updateBanner', function (event, args) {
            $scope.GetGamificationStats();
            $scope.GetGamificationBanner();
        });

        $scope.onLoad = function () {
            $scope.userContext = $rootScope.userContext;
            $scope.GetPermissionsCount();
            $scope.GetGamificationStats();
            $scope.GetGamificationBanner();

            $scope.SetModalLoaded();

            $rootScope.$on('cfpLoadingBar:completed', function () {
                $('#gamificationModal').modal('show');
                $('#gamificationModal').attr('id', 'gamificationModal1');
            });
        }
    }
})();




(function () {
    'use strict';
    angular.module('dCodeApp')
        .controller('IndexController', IndexController);

    IndexController.$inject = ['$scope', '$http', '$rootScope', '$location', 'UserContextService'];

    function IndexController($scope, $http, $rootScope, $location, UserContextService) {
        $scope.user = [];
        $scope.loginOptions = ['Requestor', 'Contributor'];
        $scope.userMockOption = { login: null, department: null };
        $scope.department = [];

        $scope.PopulateDepartment = function () {
            $http({
                method: 'GET',
                url: '/Common/GetServiceLines',
                async: true,
            }).success(function (data, status, config) {
                if (data != null) {
                    angular.forEach(data, function (value, index) {
                        $scope.department.push(value.Name);
                    });
                }
                // handle success things
            }).error(function (data, status, config) {
                // handle error things
            });
        }
        $scope.SetUserContext = function () {
            $http({
                method: 'POST',
                url: '/Common/SetUser',
                data: {
                    firstName: $scope.user.FirstName,
                    lastName: $scope.user.LastName,
                    emailId: $scope.user.Email,
                    role: $scope.userMockOption.login,
                    managerEmailId: $scope.user.ManagerEmailId,
                    department: $scope.userMockOption.department
                },
                async: true,
            }).success(function (data, status, config) {
                if (data != null) {
                    alert('user set!');
                    location.href = "/";
                }
                // handle success things
            }).error(function (data, status, config) {
                // handle error things
            });
        }

        $scope.onLoad = function () {
            $scope.PopulateDepartment();
        }

        $scope.onLoad();
    }

})();