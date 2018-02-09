angular.module('rpg')

    .controller('readerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'travelService',
        function ($scope, $routeParams, $location, $cookies, bookService, travelService,
            eventService, CKEditorService, userService, genreService) {

            $scope.books = null;
            $scope.wait = true;
            init();

            $scope.goToTravel = function (bookId) {
                travelService.getByBook(bookId).then(function (travel) {
                    $location.path('/ar/reader/travel/' + travel.Id + '/' + travel.Chapter.Id);
                });
            }

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