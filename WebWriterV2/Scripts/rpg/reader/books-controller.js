angular.module('rpg')

    .controller('readerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'travelService', 'userService',
        function ($scope, $routeParams, $location, $cookies, bookService, travelService, userService) {

            $scope.books = null;
            $scope.user = null;
            $scope.filterObj = {};
            $scope.listOfFilters = [];
            $scope.resources = resources;

            init();

            $scope.goToTravel = function (book) {
                goToTravelAndCanReset(book, false);
            };

            $scope.resetTravel = function (book) {
                if (confirm(resources.Writer_ConfirmRemovingTravel.format(book.Name))) {
                    goToTravelAndCanReset(book, true);
                }
            };

            $scope.chooseFilter = function (filter) {
                $scope.listOfFilters.forEach(function (x) { x.active = false; });//x => x.active = false
                filter.active = true;
                $scope.filterObj = filter.filterObj;
            };

            $scope.like = function (book) {
                bookService.like(book.Id).then(function () {
                    console.log('User like book: ' + book.Id);
                });

                book.UserLikedIt = !book.UserLikedIt;
                if (book.UserLikedIt) {
                    book.Likes++;
                } else {
                    book.Likes--;
                }
            };

            $scope.toggleComment = function (book) {
                book.showComments = !book.showComments;
            };

            $scope.addComment = function (book) {
                bookService.addComment(book.Id, book.newCommentText)
                    .then(function (bookComment) {
                        book.BookComments.push(bookComment);
                        book.newCommentText = '';
                    });
            };

            $scope.removeComment = function (book, comment, index) {
                bookService.removeComment(comment.Id).then(function () {
                    book.BookComments.splice(index, 1);
                });
            };

            function goToTravelAndCanReset(book, isResetTravel) {
                var userId = userService.getCurrentUserId();
                // if guest try read book or user start book from the very begining add one view for book
                if (!userId || !book.IsReaded) {
                    bookService.addView(book.Id);
                }

                if (userId) {
                    var promis;
                    if (isResetTravel) {
                        promis = travelService.getByBookAndReset(book.Id);
                    } else {
                        promis = travelService.getByBook(book.Id);
                    }
                    promis.then(function (travel) {
                        $location.path('/ar/reader/travel/' + travel.Id + '/' + travel.CurrentStepId);
                    });
                } else {
                    $location.path('/ar/reader/travel-guest/' + book.RootChapterId);
                }
            }

            function loadBooks() {
                bookService.getAll().then(function (books) {
                    $scope.books = books;
                });
            }

            function loadUser() {
                var userId = userService.getCurrentUserId();
                if (!userId)
                    return;

                userService.getById(userId).then(function (user) {
                    $scope.user = user;
                });
            }

            function initFilters() {
                $scope.listOfFilters.push(generateFilterItem(resources.ReaderBooks_AllBooks));
                $scope.listOfFilters.push(generateFilterItem(resources.ReaderBooks_ReadBooks, 'IsReaded', true));
                $scope.listOfFilters.push(generateFilterItem(resources.ReaderBooks_NewBooks, 'IsReaded', false));
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
                loadUser();
                loadBooks();
                initFilters();
            }
        }
    ]);