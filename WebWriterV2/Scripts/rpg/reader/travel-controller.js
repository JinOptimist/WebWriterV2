angular.module('rpg')

    .controller('travelController', [
        '$scope', '$routeParams', '$location', '$cookies', '$window', 'bookService', 'chapterService',
        'eventService', 'CKEditorService', 'userService', 'genreService', 'travelService',
        function ($scope, $routeParams, $location, $cookies, $window, bookService, chapterService,
            eventService, CKEditorService, userService, genreService, travelService) {

            $scope.travel = null;

            $scope.wait = true;
            init();

            $scope.choice = function (linkItemId) {
                travelService.choice($scope.travel.Id, linkItemId).then(function (chapter) {
                    $scope.travel.Chapter = chapter;
                    $window.scrollTo(0, document.getElementById('travel'));
                });
            }

            $scope.travelIsEnd = function (){
                travelService.travelIsEnd($scope.travel.Id).then(function () {
                    // go to profile or MainPage or BookEndPage
                });
            }

            //function loadChapter(chapterId) {
            //    chapterService.getForTravel(chapterId).then(function (chapter) {
            //        $scope.chapter = chapter;
            //    });

            //    loadChapterLinks(chapterId);
            //}

            //function loadBook(bookId) {
            //    //bookService.getRoot(bookId).then(function (books) {
            //    //    $scope.books = books;
            //    //});
            //}

            //function loadChapterLinks(chapterId) {
            //    chapterService.getLinksFromChapter(chapterId).then(function (chapterLinks) {
            //        $scope.chapterLinks = chapterLinks;
            //    });
            //}

            function loadTravel(travelId) {
                travelService.get(travelId).then(function (travel) {
                    $scope.travel = travel;
                });
            }

            function init() {
                var travelId = $routeParams.travelId;
                loadTravel(travelId);

                //var chapterId = $routeParams.chapterId;
                //if (chapterId) {
                //    loadChapter(chapterId);
                //} else {
                //    var bookId = $routeParams.bookId;
                //    loadBook(bookId);
                //}
            }
        }
    ]);