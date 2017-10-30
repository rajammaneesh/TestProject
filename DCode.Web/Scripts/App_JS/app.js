(function () {
    'use strict';

    angular.module('dCodeApp', ['ngTouch', 'ui.router', 'infinite-scroll', 'servicesModule', 'customFilters', 'angucomplete', 'angucomplete-alt', 'dCodeDirectives', 'cfp.loadingBar', 'angular-loading-bar', 'ui.bootstrap'])
    .config(config)

    config.$inject = ['$stateProvider', '$locationProvider', '$httpProvider', "$urlRouterProvider", 'cfpLoadingBarProvider'];

    function config($stateProvider, $locationProvider, $httpProvider, $urlRouterProvider, cfpLoadingBarProvider) {
        cfpLoadingBarProvider.latencyThreshold = 100;
        cfpLoadingBarProvider.spinnerTemplate = '<div class="backdrop"><span class="loading">Loading...</span></div>';
        
        $httpProvider.interceptors.push('AuthHttpResponseInterceptor');

        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

        // bundling multiple http responses' $digest cycles on load of controllers
        $httpProvider.useApplyAsync(true);

        //$httpProvider.defaults.headers.post['__RequestVerificationToken'] = $(':input:hidden[name*="RequestVerificationToken"]').val();
    }



})();

