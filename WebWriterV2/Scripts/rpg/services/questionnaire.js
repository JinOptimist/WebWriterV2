angular.module('services')
    .service('questionnaireService', ['httpHelper', function (httpHelper) {
        return {
            saveQuestionnaire: saveQuestionnaire,
            saveQuestion: saveQuestion,
            getAll: getAll,
            remove: remove,
        };

        function saveQuestionnaire(questionnaire) {
            var url = '/api/questionnaire/Save';
            var data = angular.toJson(questionnaire);
            return httpHelper.post(url, data);
        }

        function saveQuestion(question) {
            var url = '/api/questionnaire/SaveQuestion';
            var data = angular.toJson(question);
            return httpHelper.post(url, data);
        }

        function getAll() {
            var url = '/api/questionnaire/getAll';
            return httpHelper.get(url);
        }

        function remove(id) {
            var url = '/api/questionnaire/remove';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
    }]);