angular.module('rpg')

    .controller('writerBooksController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'tagService',
        function ($scope, $routeParams, $location, $cookies, bookService, tagService) {

            $scope.books = null;
            $scope.newBook = null;
            $scope.listOfFilters = [];
            $scope.filter = {};
            $scope.resources = resources;

            init();

            $scope.showCreateBook = function () {
                $scope.newBook = {};
            }

            $scope.switchToEditModeBookForNameOrDesc = function (book) {
                book.actionsActive = false;
                book.isEdit = true;
            }

            $scope.toggleActionsBlock = function ($event, book) {
                book.actionsActive = !book.actionsActive;
                $event.stopPropagation();
            }

            $scope.bookNameTyped = function ($event) {
                // 'enter'.keyEvent === 13
                if ($event.which !== 13) { 
                    return false;
                }

                $scope.createBook();
            }

            $scope.remove = function (book, index) {
                if (!confirm(resources.Writer_ConfirmRemovingBook.format(book.Name))) {
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

            $scope.togglePublish = function (book) {
                bookService.publishBook(book.Id, !book.IsPublished).then(function () {
                    book.IsPublished = !book.IsPublished;
                });
            }

            $scope.updateBookNameAndDesc = function (book) {
                bookService.saveBook(book).then(function (newBook) {
                    book.isEdit = false;
                });
            }

            $scope.documentKeyPressed = function (e) {
                // 'esc'.which == 27
                if (e.which === 27) {
                    $scope.cancelCreationNewBook();
                }
            }

            $scope.cancelCreationNewBook = function () {
                $scope.newBook = null;
            }

            $scope.tagAdd = function (e, book) {
                // 'enter'.which == 13
                if (e.which === 13) {
                    tagService.addForBook(book.newTag.Name, book.Id).then(function (tag) {
                        if (!book.Tags) {
                            book.Tags = [];
                        }

                        if (!book.Tags.some(x => x.Id === tag.Id)) {
                            book.Tags.push(tag);
                        }

                        book.newTag = {};
                    });
                }
            }

            $scope.removeTag = function (book, tag, tagIndex) {
                tagService.removeTagFromBook(book.Id, tag.Id).then(function () {
                    book.Tags.splice(tagIndex, 1);
                });
            }

            $scope.createBook = function() {
                if (!$scope.newBook.name) {
                    return false;
                }

                $scope.newBook.showBook = !$scope.newBook.showBook;
                var book = {
                    Name: $scope.newBook.name,
                    Desc: $scope.newBook.desc
                };
                bookService.saveBook(book).then(function (newBook) {
                    $scope.books.push(newBook);
                });

                $scope.newBook = null;
            }

            function loadBooks() {
                bookService.getAllForWriter().then(function (books) {
                    $scope.books = books;
                });
            }

            function initListOfFilters() {
                $scope.listOfFilters = [
                    { displayName: 'All', active: true, restriction: {} },
                    { displayName: 'Published', restriction: { IsPublished: true } }];
            }

            function init() {
                loadBooks();
                initListOfFilters();

                document.onkeydown = function (e) {
                    $scope.$apply($scope.documentKeyPressed(e));
                };

                document.onclick = function () {
                    $scope.$apply($scope.books && $scope.books.forEach(x => x.actionsActive = false));
                }
            }
        }
    ]);