angular.module('rpg')

    .controller('bookController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService',
        function ($scope, $routeParams, $location, $cookies, bookService,
            eventService, CKEditorService, userService, genreService) {

            $scope.bookHasCycle = true;
            $scope.book = null;//ContainsCycle
            $scope.wait = true;
            init();

            
            function init() {
                var bookId = $routeParams.bookId;
                if (bookId) {
                    loadBook(bookId);
                    loadEndingEvents(bookId);
                    loadNotAvailableEvents(bookId);
                }

                userService.getCurrentUser().then(function (data) {
                    $scope.user = data;
                    loadBooks();
                });

                genreService.getAll().then(function (data) {
                    $scope.allGenres = data;
                });
            }
        }
    ]);