angular.module('services')
    .service('bookService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            getWithChapters: getWithChapters,
            getAll: getAll,
            getAllForWriter: getAllForWriter,
            saveBook: saveBook,
            getRootChapter: getRootChapter,
            remove: remove,
            publishBook: publishBook
        };

        function saveBook(book) {
            var url = '/api/book/Save';
            var data = angular.toJson(book);
            return httpHelper.post(url, data);
        }

        function remove(bookId) {
            var url = '/api/book/Remove?id=' + bookId;
            return httpHelper.get(url);
        }

        function get(bookId) {
            var url = '/api/book/get?id=' + bookId;
            return httpHelper.get(url);
        }

        function getRootChapter(bookId) {
            var url = '/api/book/getRootChapter?id=' + bookId;
            return httpHelper.get(url);
        }

        function getWithChapters(bookId) {
            var url = '/api/book/GetWithChapters?id=' + bookId;
            return httpHelper.get(url);
        }

        function getAll() {
            var url = '/api/book/getAll';
            return httpHelper.get(url);
        }

        function getAllForWriter() {
            var url = '/api/book/GetAllForWriter';
            return httpHelper.get(url);
        }

        function publishBook(bookId) {
            var url = '/api/book/PublishBook';
            var data = {
                bookId: bookId,
                newValue: true
            };
            return httpHelper.get(url, data);
        }
    }]);