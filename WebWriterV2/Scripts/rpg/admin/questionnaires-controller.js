angular.module('rpg')

    .controller('adminQuestionnairesController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService', 'userService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService, userService) {

            $scope.questionnaires = null;
            $scope.user = null;
            $scope.resources = resources;

            init();

            $scope.addQuestionnaire = function () {
                if (!$scope.questionnaires) {
                    $scope.questionnaires = [];
                }

                $scope.questionnaires.push({
                    Name: resources.Admin_Questionnaire_DefaultName,
                    isEdit: true,
                    Questions: []
                });
            }
            $scope.saveQuestionnaire = function (questionnaire, index) {
                questionnaireService.saveQuestionnaire(questionnaire).then(function (savedQuestionnaire) {
                    $scope.questionnaires[index] = savedQuestionnaire;
                    savedQuestionnaire.isEdit = false;
                });
            }
            $scope.removeQuestionnaire = function (questionnaire, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestionnaire.format(questionnaire.Name))) {
                    return false;
                }
                questionnaireService.removeQuestionnaire(questionnaire.Id).then(function () {
                    $scope.questionnaires.splice(index, 1);
                });
            }

            $scope.addQuestion = function (questionnaire) {
                questionnaire.Questions.push({
                    Text: resources.Admin_Questionnaire_Question_DefaultName,
                    Order: questionnaire.Questions.length + 1,
                    QuestionnaireId: questionnaire.Id,
                    VisibleIf: [],
                    QuestionAnswers: [],
                    isEdit: true,
                });
            }
            $scope.saveQuestion = function (questionnaire, question, index) {
                questionnaireService.saveQuestion(question).then(function (savedQuestion) {
                    questionnaire.Questions[index] = savedQuestion;
                    savedQuestion.isEdit = false;
                });
            }
            $scope.removeQuestion = function (questionnaire, question, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestion.format(question.Order))) {
                    return false;
                }
                questionnaireService.removeQuestion(question.Id).then(function () {
                    questionnaire.Questions.splice(index, 1);
                });
            }

            $scope.addQuestionAnswers = function (question) {
                question.QuestionAnswers.push({
                    Text: resources.Admin_Questionnaire_QuestionAnswer_DefaultName,
                    Order: question.QuestionAnswers.length + 1,
                    QuestionId: question.Id,
                    isEdit: true
                });
            }
            $scope.saveQuestionAnswer = function (questionAnswer, index, question) {
                questionnaireService.saveQuestionAnswer(questionAnswer).then(function (savedQuestionAnswer) {
                    question.QuestionAnswers[index] = savedQuestionAnswer;
                    savedQuestionAnswer.isEdit = false;
                });
            }
            $scope.removeQuestionAnswer = function (question, questionAnswer, index) {
                if (!confirm(resources.Admin_Questionnaire_ConfirmRemovingQuestionAnswer.format(questionAnswer.Order, question.Order))) {
                    return false;
                }
                questionnaireService.removeQuestionAnswer(questionAnswer.Id).then(function () {
                    question.QuestionAnswers.splice(index, 1);
                });
            }

            function loadQuestionnaires() {
                questionnaireService.getAllQuestionnaire().then(function (questionnaires) {


                    questionnaires.forEach(function (questionnaire) {
                        for (var i = 1; i < questionnaire.Questions.length; i++) {
                            var question = questionnaire.Questions[i];
                            var questionPrev = questionnaire.Questions[i - 1];
                            question.answerFromPrevQuestion = questionPrev.QuestionAnswers;
                        }
                    });

                    $scope.questionnaires = questionnaires;
                });
            }

            function init() {
                loadQuestionnaires();

                var userId = userService.getCurrentUserId();
                if (userId) {
                    userService.getById(userId).then(function (data) {
                        $scope.user = data;
                    });
                } else {
                    $scope.user = null;
                }
            }
        }
    ]);