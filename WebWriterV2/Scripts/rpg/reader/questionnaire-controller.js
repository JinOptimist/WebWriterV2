angular.module('rpg')

    .controller('questionnaireController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService', 'userService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService, userService) {

            $scope.questionnaire = null;
            $scope.userId = null;
            $scope.resources = resources;
            $scope.answer = {};
            $scope.questionnaireAreDone = false;

            init();

            $scope.saveAnswers = function () {
                var answers = [];
                var thereIsQuestionWithoutAnswers = false;
                $scope.questionnaire.Questions.forEach(function (q) {
                    var answersCount = answers.length;
                    if (q.AllowMultipleAnswers) {
                        q.Answers.forEach(function (a) { answers.push({ Id: a }) });
                    } else if (q.AnswerId) {
                        answers.push({ Id: q.AnswerId });
                    }

                    if (answersCount == answers.length) {
                        // if count of answer doesn't change, question doesn't have answer.
                        q.withoutAnswer = true;
                        thereIsQuestionWithoutAnswers = true;
                    } else {
                        q.withoutAnswer = false;
                    }
                });

                if (thereIsQuestionWithoutAnswers) {
                    return;
                }

                var questionnaireResult = {
                    QuestionnaireId: $scope.questionnaire.Id,
                    UserId: $scope.userId,
                    QuestionAnswers: answers
                };

                questionnaireService.saveQuestionnaireResult(questionnaireResult).then(function () {
                    $scope.answer = {
                        answers: answers,
                        result: 'good'
                    };
                    $scope.questionnaireAreDone = true;
                });
            }

            $scope.toggleSelection = function (question, answerId) {
                var idx = question.Answers.indexOf(answerId);
                if (idx > -1) {
                    question.Answers.splice(idx, 1);
                } else {
                    question.Answers.push(answerId);
                }
            }

            function loadQuestionnaire(questionnaireId) {
                questionnaireService.getQuestionnaire(questionnaireId).then(function (questionnaire) {
                    $scope.questionnaire = questionnaire;
                    $scope.questionnaire.Questions.forEach(function (q) {
                        q.Answers = [];
                    })
                });
            }

            function init() {
                var questionnaireId = $routeParams.questionnaireId;
                loadQuestionnaire(questionnaireId);

                $scope.userId = userService.getCurrentUserId();
            }
        }
    ]);