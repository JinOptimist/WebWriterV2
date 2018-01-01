angular.module('rpg')

    .controller('booksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService) {

            $scope.books = [];
            $scope.wait = true;
            $scope.newBook = {name: ''};
            init();

            $scope.createBook = function () {
                $scope.newBook.showBook = !$scope.newBook.showBook;
                $scope.books.push({ name: $scope.newBook.name });

                $scope.newBook.name = '';
            }

            function loadBooks() {
                var books = [];
                books.push({ name: 'aa 1' });
                books.push({ name: 'aa 2' });
                return books;
            }

            function init() {
                $scope.books = loadBooks();
            }
        }
    ]);