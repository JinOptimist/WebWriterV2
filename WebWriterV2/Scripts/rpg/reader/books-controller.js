angular.module('rpg')

    .controller('readerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService) {

            $scope.books = null;
            $scope.wait = true;
            init();

            function loadBooks() {
                bookService.getAll().then(function (books) {
                    $scope.books = books;
                });
            }

            function init() {
                loadBooks();
            }
        }
    ]);