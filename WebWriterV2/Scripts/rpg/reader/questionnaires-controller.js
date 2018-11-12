angular.module('rpg')

    .controller('questionnairesController', [
        '$scope', '$routeParams', '$location', '$cookies', 'questionnaireService', 'userService',
        function ($scope, $routeParams, $location, $cookies, questionnaireService, userService) {

            $scope.questionnaires = null;
            $scope.user = null;
            $scope.resources = resources;

            init();

            //$scope.remove = function (questionnaire, index) {
            //    if (!confirm(resources.Reader_Questionnaires_ConfirmRemovingQuestionnaire.format(questionnaire.Name))) {
            //        return false;
            //    }
            //    questionnaireService.remove(questionnaire.Id)
            //        .then(function () {
            //            $scope.questionnaires.splice(index, 1);
            //        });
            //}

            function loadQuestionnaires() {
                questionnaireService.getAll().then(function (questionnaires) {
                    $scope.questionnaires = questionnaires;
                });
            }

            $scope.addQuestionnaire = function () {
                if (!$scope.questionnaires) {
                    $scope.questionnaires = [];
                }

                $scope.questionnaires.push({
                    Name: resources.Reader_Questionnaire_DefaultName,
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

            $scope.addQuestion = function (questionnaire) {
                questionnaire.Questions.push({
                    Text: resources.Reader_Questionnaire_Question_DefaultName,
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

            $scope.addQuestionAnswers = function (question) {
                question.QuestionAnswers.push({
                    Text: resources.Reader_Questionnaire_QuestionAnswer_DefaultName,
                    Order: question.QuestionAnswers.length + 1,
                });
            }

            //Text = question.Text;
            //Order = question.Order;
            //VisibleIf = question.VisibleIf.Select(x => x.Id).ToList();
            //QuestionAnswers


            //$scope.save = function (questionnaire, index) {
            //    questionnaireService.save(questionnaire).then(function (savedQuestionnaire) {
            //        $scope.questionnaires[index] = savedQuestionnaire;
            //    });
            //}

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