angular.module('services')
    .service('questionnaireService', ['httpHelper', function (httpHelper) {
        return {
            getQuestionnaire: getQuestionnaire,
            getAllQuestionnaire: getAllQuestionnaire,
            getForWriter: getForWriter,
            getAllQuestionnaireResults: getAllQuestionnaireResults,
            saveQuestionnaire: saveQuestionnaire,
            saveQuestion: saveQuestion,
            saveQuestionAnswer: saveQuestionAnswer,
            saveQuestionnaireResult: saveQuestionnaireResult,
            removeQuestionnaire: removeQuestionnaire,
            removeQuestion: removeQuestion,
            removeQuestionAnswer: removeQuestionAnswer,
            removeQuestionnaireResults: removeQuestionnaireResults,
            sendQuestionnaireResultsToEmail: sendQuestionnaireResultsToEmail
        };

        function getQuestionnaire(id) {
            var url = '/api/questionnaire/get';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }

        function getAllQuestionnaire() {
            var url = '/api/questionnaire/getAll';
            return httpHelper.get(url);
        }

        function getForWriter(userId) {
            var url = '/api/questionnaire/getForWriter';
            var data = {
                userId: userId
            };
            return httpHelper.get(url, data);
        }

        function getAllQuestionnaireResults() {
            var url = '/api/questionnaire/getAllQuestionnaireResults';
            return httpHelper.get(url);
        }

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
        function saveQuestionAnswer(questionAnswer) {
            var url = '/api/questionnaire/SaveQuestionAnswer';
            var data = angular.toJson(questionAnswer);
            return httpHelper.post(url, data);
        }
        function saveQuestionnaireResult(questionnaireResult) {
            var url = '/api/questionnaire/SaveQuestionnaireResult';
            var data = angular.toJson(questionnaireResult);
            return httpHelper.post(url, data);
        }

        function removeQuestionnaire(id) {
            var url = '/api/questionnaire/remove';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
        function removeQuestion(id) {
            var url = '/api/questionnaire/removeQuestion';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
        function removeQuestionAnswer(id) {
            var url = '/api/questionnaire/removeQuestionAnswer';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
        function removeQuestionnaireResults(id) {
            var url = '/api/questionnaire/removeQuestionnaireResults';
            var data = {
                id: id
            };
            return httpHelper.get(url, data);
        }
        
        function sendQuestionnaireResultsToEmail(id, email) {
            var url = '/api/questionnaire/sendQuestionnaireResultsToEmail';
            var data = {
                id: id,
                email: email
            };
            return httpHelper.get(url, data);
        }
        
    }]);