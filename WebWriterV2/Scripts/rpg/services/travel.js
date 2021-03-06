﻿angular.module('services')
    .service('travelService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            remove: remove,
            getByBook: getByBook,
            getByBookAndReset: getByBookAndReset,
            choice: choice,
            getByUserId: getByUserId,
            travelIsEnded: travelIsEnded
        };

        function get(id, stepId) {
            var url = '/api/travel/get';
            var data = {
                id: id,
                stepId: stepId
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

        function getByBookAndReset(bookId) {
            var url = '/api/travel/getByBookAndReset';
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

        function travelIsEnded(id) {
            var url = '/api/travel/travelIsEnded';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
    }]);