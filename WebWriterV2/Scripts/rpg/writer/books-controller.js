angular.module('rpg')

    .controller('writerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService) {

            $scope.books = [];
            $scope.wait = true;
            $scope.newBook = null;
            $scope.listOfFilters = [];
            $scope.filter = {};

            init();

            $scope.showCreateBook = function () {
                $scope.newBook = { };
            }

            $scope.createBook = function ($event) {
                if ($event.which !== 13) { // 'Enter'.keyEvent === 13
                    return false;
                }

                if (!$scope.newBook.name) {
                    $scope.newBook = null;
                    return false;
                }

                $scope.newBook.showBook = !$scope.newBook.showBook;
                var book = {
                    Name: $scope.newBook.name,
                    Desc: $scope.newBook.name
                };
                bookService.saveBook(book).then(function (newBook) {
                    $scope.books.push(newBook);
                });

                $scope.newBook = null;
            }

            $scope.remove = function (book, index) {
                if (!confirm('You try delete "' + book.Name + '". Are you sure?')) {
                    return false;
                }
                bookService.remove(book.Id)
                    .then(function () {
                        $scope.books.splice(index, 1);
                    });
            }

            $scope.filterBooks = function (filter) {
                $scope.filter = filter.restriction;
                $scope.listOfFilters.forEach(filter => filter.active = false);
                filter.active = true;
            }

            $scope.publish = function (book) {
                bookService.publishBook(book.Id).then(function () {
                    book.IsPublished = true;
                });
            }

            $scope.keyPressed = function (e) {
                // 'esc'.which == 27
                if (e.which === 27) {
                    $scope.newBook = null;
                }
            }

            function loadBooks() {
                bookService.getAllForWriter().then(function (books) {
                    $scope.books = books;
                });
            }

            function initListOfFilters() {
                $scope.listOfFilters = [
                    { displayName: 'All', active: true, restriction: {} },
                    { displayName: 'Published', restriction: { IsPublished: true }}];
            }

            function init() {
                loadBooks();
                initListOfFilters();

                document.onkeydown = function (e) {
                    $scope.$apply($scope.keyPressed(e));
                };
            }
        }
    ]);