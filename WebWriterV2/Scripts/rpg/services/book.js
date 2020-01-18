angular.module('services')
    .service('bookService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            getWithChapters: getWithChapters,
            getWithChaptersV2: getWithChaptersV2,
            getAll: getAll,
            getAllForWriter: getAllForWriter,
            getAllForAdmin: getAllForAdmin,
            getWithChaptersRoadmap: getWithChaptersRoadmap,
            saveBook: saveBook,
            getRootChapter: getRootChapter,
            remove: remove,
            publishBook: publishBook,
            addView: addView,
            addCoAuthor: addCoAuthor,
            addComment: addComment,
            removeComment: removeComment,
            removeCoAuthor: removeCoAuthor,
            like: like
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

        function getWithChaptersV2(bookId) {
            var url = '/api/book/GetWithChaptersV2?id=' + bookId;
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

        function getAllForAdmin() {
            var url = '/api/book/getAllForAdmin';
            return httpHelper.get(url);
        }

        function getWithChaptersRoadmap(bookId) {
            var url = '/api/book/GetWithChaptersRoadmap/' + bookId;
            return httpHelper.get(url);
        }

        function publishBook(bookId, newValue) {
            var url = '/api/book/PublishBook';
            var data = {
                bookId: bookId,
                newValue: newValue
            };
            return httpHelper.get(url, data);
        }

        function addView(bookId) {
            var url = '/api/book/addView';
            var data = {
                bookId: bookId
            };
            return httpHelper.get(url, data);
        }

        function addCoAuthor(email, bookId) {
            var url = '/api/book/AddCoAuthor';
            var data = {
                email: email,
                bookId: bookId
            };
            return httpHelper.get(url, data);
        }

        function addComment(bookId, text) {
            var url = '/api/book/AddComment';
            var data = {
                bookId: bookId,
                text: text
            };
            return httpHelper.get(url, data);
        }

        function removeComment(bookCommentId) {
            var url = '/api/book/removeComment';
            var data = {
                bookCommentId: bookCommentId
            };
            return httpHelper.get(url, data);
        }

        function removeCoAuthor(email, bookId) {
            var url = '/api/book/removeCoAuthor';
            var data = {
                email: email,
                bookId: bookId
            };
            return httpHelper.get(url, data);
        }

        function like(bookId) {
            var url = '/api/book/Like?id=' + bookId;
            return httpHelper.get(url);
        }
    }]);