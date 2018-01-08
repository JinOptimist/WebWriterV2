angular.module('rpg')

    .controller('writerBookController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, bookService, chapterService,
            eventService, CKEditorService, userService, genreService) {

            var chapterWidth = 270;
            var chapterHeight = 300;
            var offsetWidth = 20;
            var offsetHeight = 22;

            var addButtonWidth = 70;
            var addButtonHeight = 30;

            $scope.bookHasCycle = true;
            $scope.book = null;
            $scope.wait = true;
            init();

            $scope.addChapter = function (level) {
                var chapter = {
                    Name: 'Header',
                    Desc: 'Desc',
                    Level: level,
                    BookId: $scope.book.Id,
                    IsRootChapter: $scope.book.RootEventId < 0
                };
                chapterService.save(chapter).then(function (savedChapter) {
                    $location.path('/ar/writer/chapter/' + savedChapter.Id);
                });
            }

            $scope.remove = function (chapter, index) {
                chapterService.remove(chapter.Id)
                    .then(function () {
                        chapter.parent.Chapters.splice(index, 1);
                        init();
                    });
            }

            function loadBook(bookId) {
                bookService.getWithChapters(bookId).then(function (book) {
                    $scope.book = book;
                    $scope.book.Levels.forEach(level => {
                        level.dynamicStyle = calcLevelWidthStyle;
                        level.Chapters.forEach(chapter => {
                            chapter.dynamicStyle = calcChapterStyle;
                            chapter.parent = level;
                        });
                    });
                });
            }

            function calcLevelWidthStyle() {
                var level = this;
                var style = {};
                var countChapter = level.Chapters.length;

                if (level.isWide) {
                    style.width = countChapter * (chapterWidth + 2) + addButtonWidth + 'px';
                } else {
                    style.width = chapterWidth + (countChapter - 1) * offsetWidth + addButtonWidth + 'px';
                }
                if (!level.isWide) {
                    style.height = addButtonHeight + chapterHeight + (countChapter - 1) * offsetHeight + 'px';
                }
                return style;
            }

            function calcChapterStyle() {
                var chapter = this;
                var level = chapter.parent;
                if (level.isWide) {
                    return {};
                }

                var index = level.Chapters.indexOf(chapter);
                return {
                    'position': 'relative',
                    'top': -1 * index * (chapterHeight - offsetHeight) + 'px',
                    'left': index * offsetWidth + 'px'
                };
            }

            function init() {
                var bookId = $routeParams.bookId;
                loadBook(bookId);
            }
        }
    ]);