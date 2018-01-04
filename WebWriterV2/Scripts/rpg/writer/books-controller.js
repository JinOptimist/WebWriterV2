angular.module('rpg')

    .controller('writerBooksController', [
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

            $scope.publish = function (book) {
                bookService.publishBook(book.Id).then(function () {
                    book.IsPublished = true;
                });
            }

            function loadBooks() {
                bookService.getAllForWriter().then(function (books) {
                    $scope.books = books;
                });
            }

            function init() {
                loadBooks();
            }
        }
    ]);