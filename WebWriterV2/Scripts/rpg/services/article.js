angular.module('services')
    .service('articleService', ['httpHelper', function (httpHelper) {
        return {
            save: save,
            getAll: getAll,
            remove: remove,
        };

        function save(article) {
            var url = '/api/article/Save';
            var data = angular.toJson(article);
            return httpHelper.post(url, data);
        }

        function getAll() {
            var url = '/api/article/getAll';
            return httpHelper.get(url);
        }

        function remove(id) {
            var url = '/api/article/remove';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
    }]);