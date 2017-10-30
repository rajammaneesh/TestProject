(function () {
    'use strict';

    angular.module('customFilters', [])
        .filter('yesNo', YesNo)

    YesNo.$inject = [];

    function YesNo() {

        return function (input) {
            return input=="true" ? 'Yes' : 'No';
        };
        
    }

})();