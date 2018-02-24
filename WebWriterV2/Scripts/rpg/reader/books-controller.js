angular.module('rpg')

    .controller('readerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'travelService',
        function ($scope, $routeParams, $location, $cookies, bookService, travelService,
            eventService, CKEditorService, userService, genreService) {

            $scope.books = null;
            $scope.filterObj = {};
            $scope.listOfFilters = [];
            $scope.wait = true;
            init();

            $scope.goToTravel = function (bookId) {
                travelService.getByBook(bookId).then(function (travel) {
                    $location.path('/ar/reader/travel/' + travel.Id + '/' + travel.Chapter.Id);
                });
            }

            $scope.chooseFilter = function (filter) {
                $scope.listOfFilters.forEach(x => x.active = false);
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