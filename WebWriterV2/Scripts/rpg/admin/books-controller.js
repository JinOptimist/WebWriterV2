angular.module('rpg')

    .controller('adminBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'tagService',
        function ($scope, $routeParams, $location, $cookies, bookService, tagService) {

            $scope.books = null;
            $scope.resources = resources;

            init();

            $scope.remove = function (book, index) {
                if (!confirm(resources.Writer_ConfirmRemovingBook.format(book.Name))) {
                    return false;
                }
                bookService.remove(book.Id)
                    .then(function () {
                        $scope.books.splice(index, 1);
                    });
            }

            function loadBooks() {
                bookService.getAllForAdmin().then(function (books) {
                    $scope.books = books;
                });
            }

            function init() {
                loadBooks();
            }
        }
    ]);