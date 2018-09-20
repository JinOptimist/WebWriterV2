angular.module('services')
    .service('stateTypeService', ['httpHelper', function (httpHelper) {
        return {
            add: add,
            remove: remove
        };

        function add(stateType, bookId) {
            var url = '/api/book/AddState';
            var data = angular.toJson(stateType);
            return httpHelper.post(url, data);
        }

        function remove(stateTypeId) {
            var url = '/api/book/removeState';
            var data = {
                stateTypeId: stateTypeId
            };
            return httpHelper.get(url, data);
        }
    }]);