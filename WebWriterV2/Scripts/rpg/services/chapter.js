angular.module('services')
    .service('chapterService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            getForTravel: getForTravel,
            save: save,
            remove: remove,
            getBottomChapters: getBottomChapters,
            createNextChapter: createNextChapter,

            saveChapterLink: saveChapterLink,
            getLinksFromChapter: getLinksFromChapter,
            removeChapterLink: removeChapterLink
        };

        function save(chapter) {
            var url = '/api/chapter/Save';
            var data = angular.toJson(chapter);
            return httpHelper.post(url, data);
        }

        function get(chapterId) {
            var url = '/api/chapter/get?id=' + chapterId;
            return httpHelper.get(url);
        }

        function getForTravel(chapterId) {
            var url = '/api/chapter/GetForTravel?id=' + chapterId;
            return httpHelper.get(url);
        }

        function createNextChapter(chapter) {
            var url = '/api/chapter/CreateNextChapter';
            var data = angular.toJson(chapter);
            return httpHelper.post(url, data);
        }

        function remove(chapterId) {
            var url = '/api/chapter/Remove?id=' + chapterId;
            return httpHelper.get(url);
        }

        function getBottomChapters(chapter) {
            var url = '/api/chapter/GetChapterBottom';
            var data = angular.toJson(chapter);
            return httpHelper.post(url, data);
        }

        function saveChapterLink(chapterLink) {
            var url = '/api/chapterLink/save';
            var data = angular.toJson(chapterLink);
            return httpHelper.post(url, data);
        }

        function removeChapterLink(chapterLinkId) {
            var url = '/api/chapterLink/remove?id=' + chapterLinkId;
            return httpHelper.get(url);
        }

        function getLinksFromChapter(chapterId) {
            var url = '/api/chapterLink/GetLinksFromChapter?chapterId=' + chapterId;
            return httpHelper.get(url);
        }
    }]);