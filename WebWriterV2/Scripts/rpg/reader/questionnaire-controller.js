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
                var fullAnswers = getFullAnswers();

                if ($scope.questionnaire.Questions.some(a => a.withoutAnswer)) {
                    return;
                }

                var questionnaireResult = {
                    QuestionnaireId: $scope.questionnaire.Id,
                    UserId: $scope.userId,
                    QuestionAnswers: fullAnswers.baseAnswers,
                    QuestionOtherAnswers: fullAnswers.otherAnswers,
                };

                questionnaireService.saveQuestionnaireResult(questionnaireResult).then(function () {
                    $scope.answer = {
                        answers: fullAnswers.baseAnswers,
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

            $scope.requiredAnswersWasChecked = function (question) {
                var fullAnswers = getFullAnswers();
                return requiredAnswersWasChecked(question, fullAnswers.baseAnswers);
            }

            function requiredAnswersWasChecked(question, answers) {
                var visibleIf = question.VisibleIf[0];
                return answers.some(a => a.Id == visibleIf);
            }

            function getFullAnswers() {
                var baseAnswers = [];
                var otherAnswers = [];
                $scope.questionnaire.Questions.forEach(function (question) {
                    // Check do we can see the question. If we can't, skip the question
                    if (question.VisibleIf && question.VisibleIf.length > 0 && !requiredAnswersWasChecked(question, baseAnswers)) {
                        question.withoutAnswer = false;
                        return;
                    }

                    var baseAnswersCount = baseAnswers.length;
                    var otherAnswersCount = otherAnswers.length;
                    if (question.AllowMultipleAnswers) {
                        question.Answers.forEach(function (a) { baseAnswers.push({ Id: a }) });
                    } else if (question.AnswerId) {
                        baseAnswers.push({ Id: question.AnswerId });
                    }

                    if (question.EnableOtherAnswer && question.OtherAnswer) {
                        otherAnswers.push({
                            AnswerText: question.OtherAnswer,
                            QuestionId: question.Id,
                        });
                    }

                    // if count of answer doesn't change, question doesn't have answer.
                    question.withoutAnswer = baseAnswersCount == baseAnswers.length
                        && otherAnswersCount == otherAnswers.length;
                });
                return {
                    baseAnswers: baseAnswers,
                    otherAnswers: otherAnswers
                };
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