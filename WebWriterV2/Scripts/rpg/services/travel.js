angular.module('services')
    .service('travelService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            remove: remove,
            getByBook: getByBook,
            choice: choice,
            getByUserId: getByUserId,
            travelIsEnd: travelIsEnd
        };

        function get(id, chapterId) {
            var url = '/api/travel/get';
            var data = {
                id: id,
                chapterId: chapterId
            };
            return httpHelper.get(url, data);
        }

        function remove(id) {
            var url = '/api/travel/remove';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }

        function getByBook(bookId) {
            var url = '/api/travel/getByBook';
            var data = {
                bookId: bookId
            };
            return httpHelper.get(url, data);
        }

        function choice(travelId, linkItemId) {
            var url = '/api/travel/choice';
            var data = {
                travelId: travelId,
                linkItemId: linkItemId
            };
            return httpHelper.get(url, data);
        }

        function getByUserId(userId) {
            var url = '/api/travel/getByUserId';
            var data = {
                userId: userId
            };
            return httpHelper.get(url, data);
        }

        function travelIsEnd(id) {
            var url = '/api/travel/travelIsEnd';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
    }]);