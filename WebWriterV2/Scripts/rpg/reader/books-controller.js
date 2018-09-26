angular.module('rpg')

    .controller('readerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'travelService', 'userService',
        function ($scope, $routeParams, $location, $cookies, bookService, travelService, userService) {

            $scope.books = null;
            $scope.filterObj = {};
            $scope.listOfFilters = [];
            $scope.resources = resources;

            init();

            $scope.goToTravel = function (book) {
                var userId = userService.getCurrentUserId();
                // if guest try read book or user start book from the very begining add one view for book
                if (!userId || !book.IsReaded) {
                    bookService.addView(book.Id);
                }

                if (userId) {
                    travelService.getByBook(book.Id).then(function (travel) {
                        $location.path('/ar/reader/travel/' + travel.Id + '/' + travel.CurrentStepId);
                    });
                } else {
                    $location.path('/ar/reader/travel-guest/' + book.RootChapterId);
                }
            }

            $scope.chooseFilter = function (filter) {
                $scope.listOfFilters.forEach(function (x) { x.active = false; });//x => x.active = false
                filter.active = true;
                $scope.filterObj = filter.filterObj;
            }

            function loadBooks() {
                bookService.getAll().then(function (books) {
                    $scope.books = books;
                });
            }

            function initFilters() {
                $scope.listOfFilters.push(generateFilterItem('All'));
                $scope.listOfFilters.push(generateFilterItem('Readed', 'IsReaded', true));
                $scope.listOfFilters.push(generateFilterItem('New', 'IsReaded', false));
                $scope.listOfFilters[0].active = true;
            }

            function generateFilterItem(displayName, filterName, filterValue) {
                var filterItem = { displayName: displayName, active: false };
                var filterObj = {};
                if (filterName) {
                    filterObj[filterName] = filterValue;
                }
                filterItem.filterObj = filterObj;

                return filterItem;
            }

            function init() {
                loadBooks();
                initFilters();
            }
        }
    ]);