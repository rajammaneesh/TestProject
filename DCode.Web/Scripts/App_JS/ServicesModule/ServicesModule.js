
(function () {
    'use strict';

    angular
        .module('servicesModule',[])
        .factory('UserContextService', UserContextService)
        .factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);

    UserContextService.$inject = ['$http', '$rootScope', '$q'];

    function UserContextService($http, $rootScope, $q) {
        return {
            InitializeUserContext: function () {
                var deferred = $q.defer();
                $http({
                    method: 'GET',
                    url: "/Common/GetCurrentUserContext",
                }).success(function (response) {

                    if (response != null && response != "" && response.d != "") {
                        $rootScope.userContext = response;
                        deferred.resolve('');
                    }
                    else
                        deferred.reject();
                }).error(function (data, status, headers, config) {

                    deferred.reject(status + ";" + config.url + ";" + data.Message);
                });
                return deferred.promise;
            }
        };
    }

    AuthHttpResponseInterceptor.$inject = ['$q', '$location'];

    function AuthHttpResponseInterceptor($q, $location) {
        return {
            response: function (response) {
                if (response.status === 401) {
                    console.log("Response 401");
                }
                return response || $q.when(response);
            },
            responseError: function (rejection) {
                if (rejection.status === 401) {
                    console.log("Response Error 401", rejection);
                    $location.path('/login').search('returnUrl', $location.path());
                }
                return $q.reject(rejection);
            }
        };
    }



})();