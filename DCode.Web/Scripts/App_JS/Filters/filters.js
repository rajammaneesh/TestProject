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

(function () {
    'use strict';

    angular.module('customFilters', [])
        .filter('num', Num)

    Num.$inject = [];

    function Num() {

        return function (input) {
            if(angular.isNumber(input))
                return parseInt(input);
            return -1;
        }

    }

})();