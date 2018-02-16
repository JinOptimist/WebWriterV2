angular.module('services')
    .service('tagService', ['httpHelper', function (httpHelper) {
        return {
            addForBook: addaddForBook,
            removeTagFromBook: removeTagFromBook
        };

        function addaddForBook(tagName, bookId) {
                var url = '/api/book/addTag';
                var data = {
                    tagName: tagName,
                    bookId: bookId
                };
                return httpHelper.get(url, data);
        }

        function removeTagFromBook(bookId, tagId) {
            var url = '/api/book/removeTag';
            var data = {
                bookId: bookId,
                tagId: tagId
            };
            return httpHelper.get(url, data);
        }
    }]);