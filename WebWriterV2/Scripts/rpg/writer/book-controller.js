angular.module('rpg')

    .controller('writerBookController', [
        '$scope', '$routeParams', '$location', '$cookies', 'bookService', 'chapterService',
        function ($scope, $routeParams, $location, $cookies, bookService, chapterService,
            eventService, CKEditorService, userService, genreService) {

            var chapterWidth = 270;
            var chapterHeight = 310;

            var offsetWidth = 20;
            var offsetHeight = 25;

            var addButtonWidth = 70;
            var addButtonHeight = 30;

            $scope.bookHasCycle = true;
            $scope.book = null;
            $scope.wait = true;
            init();

            $scope.onChapterHover = function (chapter) {
                if (chapter) {
                    var highlightToChapterIds = [];
                    chapter.LinksFromThisEvent.forEach(function (link) {
                        highlightToChapterIds.push(link.ToId);
                    });

                    var highlightFromChapterIds = [];
                    chapter.LinksToThisEvent.forEach(function (link) {
                        highlightFromChapterIds.push(link.FromId);
                    });
                }

                highlightChapter(highlightToChapterIds, highlightFromChapterIds);
            }

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

            $scope.toggleWide = function(level){
                level.isWide = !level.isWide;
                recalcLevelStyle(level);
                level.Chapters.forEach(function (chapter) {
                    recalcChapterStyle(chapter);
                });
                
            }

            function loadBook(bookId) {
                bookService.getWithChapters(bookId).then(function (book) {
                    $scope.book = book;
                    $scope.book.Levels.forEach(level => {
                        recalcLevelStyle(level)
                        level.Chapters.forEach(chapter => {
                            chapter.parent = level;
                            recalcChapterStyle(chapter);
                        });
                    });
                });
            }

            function recalcLevelStyle(level) {
                var style = {};
                var countChapter = level.Chapters.length;

                if (level.isWide) {
                    style.width = countChapter * (chapterWidth + 2) + addButtonWidth + 'px';
                } else {
                    style.width = chapterWidth + (countChapter - 1) * offsetWidth + addButtonWidth + 'px';
                }
                if (!level.isWide) {
                    style.height = chapterHeight + (countChapter - 1) * offsetHeight + 'px';
                }
                level.dynamicStyle = style;
            }

            function recalcChapterStyle(chapter) {
                var level = chapter.parent;
                if (level.isWide) {
                    chapter.dynamicStyle = {};
                    return false;
                }

                var index = level.Chapters.indexOf(chapter);
                chapter.dynamicStyle = {
                    'position': 'relative',
                    'top': -1 * index * (chapterHeight - offsetHeight) + 'px',
                    'left': index * offsetWidth + 'px',
                };
            }

            function highlightChapter(nextChapterIds, prevChapterIds) {
                $scope.book.Levels.forEach(function (level) {
                    level.Chapters.forEach(function (chapter) {
                        chapter.isNext = nextChapterIds && nextChapterIds.indexOf(chapter.Id) > -1;
                        chapter.isPrev = prevChapterIds && prevChapterIds.indexOf(chapter.Id) > -1;
                    });
                });
            }

            function init() {
                var bookId = $routeParams.bookId;
                loadBook(bookId);
            }
        }
    ]);