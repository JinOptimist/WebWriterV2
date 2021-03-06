﻿angular.module('services')
    .service('chapterService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            getForTravel: getForTravel,
            save: save,
            createChild: createChild,
            remove: remove,
            getBottomChapters: getBottomChapters,
            getAllChapters: getAllChapters,
            createNextChapter: createNextChapter,
            

            createLink: createLink,
            removeLink: removeLink,
            saveChapterLink: saveChapterLink,
            getLinksFromChapter: getLinksFromChapter,
            removeChapterLink: removeChapterLink,
            // TO REMOVE
            linkConditionToChapterLink: linkConditionToChapterLink,
            getAvailableDecision: getAvailableDecision,
            linkDecisionToChapterLink: linkDecisionToChapterLink,
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

        function createLink(fromId, toId) {
            var url = '/api/chapterLink/CreateLink';
            var data = {
                fromId: fromId,
                toId: toId
            };
            return httpHelper.get(url, data);
        }

        function removeLink(linkId) {
            var url = '/api/chapterLink/Remove';
            var data = {
                id: linkId,
            };
            return httpHelper.get(url, data);
        }

        function createChild(parentId) {
            var url = '/api/chapter/CreateChild';
            var data = { parentId: parentId };
            return httpHelper.get(url, data);
        }

        function createNextChapter(chapter) {
            var url = '/api/chapter/CreateNextChapter';
            var data = chapter;
            return httpHelper.post(url, data);
        }

        function getAllChapters(bookId) {
            var url = '/api/chapter/GetAllChapters';
            var data = { bookId: bookId };
            return httpHelper.get(url, data);
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

        function linkDecisionToChapterLink(chapterId, decision) {
            var url = '/api/chapterLink/LinkDecisionToChapterLink';
            var data = {
                chapterId: chapterId,
                decision: decision
            };
            return httpHelper.get(url, data);
        }

        function linkConditionToChapterLink(chapterId, condition) {
            var url = '/api/chapterLink/LinkConditionToChapterLink';
            var data = {
                chapterId: chapterId,
                condition: condition
            };
            return httpHelper.get(url, data);
        }

        function getAvailableDecision(chapterId) {
            var url = '/api/chapterLink/GetAvailableDecision';
            var data = {
                chapterId: chapterId
            };
            return httpHelper.get(url, data);
        }
    }]);