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
                var book = {
                    Name: $scope.newBook.name,
                    Desc: $scope.newBook.name
                };
                bookService.saveBook(book).then(function (newBook) {
                    $scope.books.push(newBook);
                })
                

                $scope.newBook.name = '';
            }

            $scope.goToBook = function (bookId) {
                $location.path('/ar/writer/book/' + bookId);
            }

            function loadBooks() {

                bookService.getAllForWriter().then(function (books) {
                    $scope.books = books;
                });

                //var books = [];
                //books.push({ name: 'aa 1' });
                //books.push({ name: 'aa 2' });
                //return books;
            }

            function init() {
                loadBooks();
            }
        }
    ]);