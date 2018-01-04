var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('services', ['ngRoute', 'ngCookies', 'underscore', 'AppConst'])
    .constant('_',
        window._
    )
    .service('bookService', ['httpHelper', function (httpHelper) {
        return {
            //new method
            get: get,
            getWithChapters: getWithChapters,
            getAll: getAll,
            getAllForWriter: getAllForWriter,
            saveBook: saveBook,
            getRootChapter: getRootChapter,

            //old method
            changeRootEvent: changeRootEvent,
            removeBook: removeBook,
            importBook: importBook,
            bookCompleted: bookCompleted,
            publishBook: publishBook
        };

        function saveBook(book) {
            var url = '/api/book/Save';
            var data = angular.toJson(book);
            return httpHelper.post(url, data);
        }

        function bookCompleted(bookId) {
            var url = '/api/book/BookCompleted';
            var data = {
                bookId: bookId
            };
            return httpHelper.get(url, data);
        }

        function publishBook(bookId) {
            var url = '/api/book/PublishBook';
            var data = {
                bookId: bookId,
                newValue: true
            };
            return httpHelper.get(url, data);
        }

        function changeRootEvent(bookId, eventId) {
            var url = '/api/book/ChangeRootEvent';
            var data = {
                bookId: bookId,
                eventId: eventId
            };
            return httpHelper.get(url, data);
        }

        function get(bookId) {
            var url = '/api/book/get?id=' + bookId;
            return httpHelper.get(url);
        }

        function getRootChapter(bookId) {
            var url = '/api/book/getRootChapter?id=' + bookId;
            return httpHelper.get(url);
        } 

        function getWithChapters(bookId) {
            var url = '/api/book/GetWithChapters?id=' + bookId;
            return httpHelper.get(url);
        }

        function getAll() {
            var url = '/api/book/getAll';
            return httpHelper.get(url);
        }

        function getAllForWriter() {
            var url = '/api/book/GetAllForWriter';
            return httpHelper.get(url);
        }

        function removeBook(bookId) {
            var url = '/api/book/Remove?id=' + bookId;
            return httpHelper.get(url);
        }

        function importBook(bookJson) {
            var url = '/api/book/ImportBook';
            var data = {
                jsonBook: bookJson
            };
            return httpHelper.get(url, data);
        }
    }])
    .service('chapterService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            save: save
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
    }])
    .service('eventService', ['httpHelper', function (httpHelper) {
        return {
            get: get,
            getEvents: getEvents,
            save: save,
            remove: remove,
            saveEventLink: saveEventLink,
            removeEventLink: removeEventLink,
            addSkill: addSkill,
            removeSkill: removeSkill,
            addState: addState,
            removeState: removeState,
            addReqState: addReqState,
            removeReqState: removeReqState,
            getEndingEvents: getEndingEvents,
            getNotAvailableEvents: getNotAvailableEvents,
            addRequirementThing: addRequirementThing,
            removeRequirementThing: removeRequirementThing,
            addThingChanges: addThingChanges,
            removeThingChanges: removeThingChanges,
            getEventForTravel: getEventForTravel,
            eventChangesApplyToHero: eventChangesApplyToHero,
            getEventForTravelWithHero: getEventForTravelWithHero,
            createNextChapter: createNextChapter
        };

        function getEventForTravel(eventId, heroId) {
            var url = '/Rpg/GetEventForTravel';
            var data = {
                eventId: eventId,
                heroId: heroId
            };
            return httpHelper.post(url,data);
        }

        function getEventForTravelWithHero(eventId, hero, applyChanges) {
            var url = '/Rpg/GetEventForTravelWithHero';
            var data = {
                eventId: eventId,
                heroJson: angular.toJson(hero),
                applyChanges: applyChanges
            };
            return httpHelper.post(url,data);
        }

        function eventChangesApplyToHero(eventId, heroId) {
            var url = '/Rpg/EventChangesApplyToHero';
            var data = {
                eventId: eventId,
                heroId: heroId
            };
            return httpHelper.post(url, data);
        }

        function get(eventId) {
            var url = '/Rpg/GetEvent?id=' + eventId;
            return httpHelper.post(url);
        }

        function getEvents(bookId) {
            var url = '/Rpg/GetEvents?bookId=' + bookId;
            return httpHelper.post(url);
        }

        function getEndingEvents(bookId) {
            var url = '/Rpg/GetEndingEvents?bookId=' + bookId;
            return httpHelper.post(url);
        }

        function getNotAvailableEvents(bookId) {
            var url = '/Rpg/GetNotAvailableEvents?bookId=' + bookId;
            return httpHelper.post(url);
        }

        function save(event, bookId) {
            var url = '/Rpg/SaveEvent';
            var data = {
                jsonEvent: angular.toJson(event),
                bookId: bookId
            };
            return httpHelper.post(url,data);
        }

        function remove(eventId) {
            var url = '/Rpg/RemoveEvent';
            var data = {
                eventId: eventId
            };
            return httpHelper.post(url, data);
        }

        function saveEventLink(eventLink) {
            var url = '/Rpg/SaveEventLink';
            var data = {
                jsonEventLink: angular.toJson(eventLink)
            };
            return httpHelper.post(url, data);
        }

        function removeEventLink(eventLinkId) {
            var url = '/Rpg/RemoveEventLink';
            var data = {
                eventLinkId: eventLinkId
            };
            return httpHelper.post(url, data);
        }

        function addSkill(eventId, skillId) {
            var url = '/Rpg/AddSkillToEvent';
            var data = {
                eventId: eventId,
                skillId: skillId
            };
            return httpHelper.post(url, data);
        }

        function removeSkill(eventId, skillId) {
            var url = '/Rpg/RemoveSkillToEvent';
            var data = {
                eventId: eventId,
                skillId: skillId
            };
            return httpHelper.post(url, data);
        }

        function addState(eventId, stateTypeId, stateValue) {
            var url = '/Rpg/AddStateToEvent';
            var data = {
                eventId: eventId,
                stateTypeId: stateTypeId,
                stateValue: stateValue
            };
            return httpHelper.post(url, data);
        }

        function removeState(stateId) {
            var url = '/Rpg/RemoveStateFromEvent';
            var data = {
                stateId: stateId
            };
            return httpHelper.post(url, data);
        }

        function addReqState(eventId, stateTypeId, reqType, stateValue) {
            var url = '/Rpg/AddReqStateToEvent';
            var data = {
                eventId: eventId,
                stateTypeId: stateTypeId,
                reqType: reqType,
                stateValue: stateValue
            };
            return httpHelper.post(url, data);
        }

        function removeReqState(stateId) {
            var url = '/Rpg/RemoveReqStateFromEvent';
            var data = {
                stateId: stateId
            };
            return httpHelper.post(url, data);
        }

        function addRequirementThing(eventId, thingSampleId, count) {
            var url = '/Rpg/AddRequirementThingToEvent';
            var data = {
                eventId: eventId,
                thingSampleId: thingSampleId,
                count: count
            };
            return httpHelper.post(url, data);
        }

        function removeRequirementThing(eventId, thingId) {
            var url = '/Rpg/RemoveRequirementThingFromEvent';
            var data = {
                eventId: eventId,
                thingId: thingId
            };
            return httpHelper.post(url, data);
        }

        function addThingChanges(eventId, thingSampleId, count) {
            var url = '/Rpg/AddThingChangesToEvent';
            var data = {
                eventId: eventId,
                thingSampleId: thingSampleId,
                count: count
            };
            return httpHelper.post(url, data);
        }

        function removeThingChanges(eventId, thingId) {
            var url = '/Rpg/RemoveThingChangesFromEvent';
            var data = {
                eventId: eventId,
                thingId: thingId
            };
            return httpHelper.post(url, data);
        }

        function createNextChapter(eventId) {
            var url = '/Rpg/CreateNextChapter';
            var data = {
                eventId: eventId
            };
            return httpHelper.post(url, data);
        }
    }])
    .service('heroService', ['httpHelper', function (httpHelper) {
        var maxHpStateName = "MaxHp";
        var maxMpStateName = "MaxMp";
        var hpStateName = "Hp";
        var mpStateName = "Mp";

        var listHeroes = null;
        var selectedHero = null;
        var defaultHero = {};

        function updateHeroState(heroOrigin, heroNew) {
            for (var i = 0; i < heroNew.State.length; i++) {
                var newStat = heroNew.State[i];
                var st = heroOrigin.State.find(function (stat) {
                    return stat.StateType.Id === newStat.StateType.Id;
                });
                st.Number = newStat.Number;
            }
        }

        return {
            load: function (heroId) {
                var url = '/Rpg/GetHero';
                var data = {
                    heroId: heroId
                };
                return httpHelper.post(url, data);
            },
            loadListHeroes: function () {
                var url = '/Rpg/GetHeroes';
                var data = {
                    heroId: heroId
                };
                return httpHelper.post(url, data);
            },
            removeHero: function (hero) {
                var url = '/Rpg/RemoveHero?id=' + hero.Id;
                var data = {
                    heroId: heroId
                };
                return httpHelper.post(url, data);
            },
            addHero: function (newHero) {
                newHero.Sex = newHero.Sex - 0;
                newHero.Race = newHero.Race - 0;

                listHeroes.push(newHero);
            },
            getAllHeroes: function () {
                return listHeroes;
            },
            selectHero: function (hero) {
                selectedHero = hero;
            },
            getSelectedHero: function () {
                return selectedHero;
            },
            getDefaultHero: function () {
                return defaultHero;
            },
            setDefaultHero: function (hero) {
                defaultHero = hero;
            },
            updateHeroState: updateHeroState
        };
    }])
    .service('requirementTypeService', ['httpHelper', '_', function (httpHelper, _) {
        return {
            load: function () {
                var url = '/Rpg/GetListRequirementType';
                return httpHelper.post(url);
            },
        };
    }])
    .service('stateService', ['_', 'httpHelper', function (_, httpHelper) {
        return {
            loadTypesAvailbleForUser: loadTypesAvailbleForUser,
            loadTypesAvailbleForEdit: loadTypesAvailbleForEdit,
            add: add,
            edit: edit,
            remove: remove,
            changeState: changeState
        };

        function loadTypesAvailbleForUser() {
            var url = '/Rpg/GetStateTypesAvailbleForUser';
            return httpHelper.post(url);
        }

        function loadTypesAvailbleForEdit() {
            var url = '/Rpg/GetStateTypesAvailbleForEdit';
            return httpHelper.post(url);
        }

        function changeState(stateId, delta) {
            var url = '/Rpg/ChangeState';
            var data = {
                stateId: stateId,
                delta: delta
            };
            return httpHelper.post(url, data);
        }

        function add(newState) {
            var url = '/Rpg/AddState';
            var data = {
                name: newState.Name,
                desc: newState.Desc,
                hideFromReader: newState.HideFromReader
            };
            return httpHelper.post(url, data);
        }

        function edit(state) {
            var url = '/Rpg/EditStateType';
            var data = {
                jsonStateType: angular.toJson(state)
            };
            return httpHelper.post(url, data);
        }

        function remove(stateId) {
            var url = '/Rpg/RemoveState';
            var data = {
               stateId: stateId,
            };
            return httpHelper.post(url, data);
        }
    }])
    .service('thingService', ['httpHelper', '_', function (httpHelper, _) {
        return {
            loadAllSamples: loadAllSamples,
            add: add,
            remove: remove,
        };

        function loadAllSamples() {
            var url = '/Rpg/GetThingSamples';
            return httpHelper.post(url);
        }

        function add(name, desc) {
            var url = '/Rpg/AddThing';
            var data= {
                name: name,
                desc: desc
            };
            return httpHelper.post(url,data);
        }

        function remove(thingId) {
            var url = '/Rpg/RemoveThing';
            var data = {
                thingId: thingId
            };
            return httpHelper.post(url, data);
        }
    }])
    .service('genreService', ['httpHelper', '_', function (httpHelper, _) {
        return {
            getAll: getAll,
            add: add,
            remove: remove,
        };

        function getAll() {
            var url = '/Rpg/GetGenres';
            return httpHelper.post(url);
        }

        function add(newGenre) {
            var url = '/Rpg/AddGenre';
            var data = {
                name: newGenre.Name,
                desc: newGenre.Desc
            };
            return httpHelper.post(url, data);
        }

        function remove(genreId) {
            var url = '/Rpg/RemoveGenre';
            var data = {
                genreId: genreId
            };
            return httpHelper.post(url, data);
        }
    }])
    .service('userService', ['$cookies', '$q', 'httpHelper', 'ConstCookies',
        function ($cookies, $q, httpHelper, ConstCookies) {
            var currentUser = null;

            return {
                login: login,
                logout: logout,
                register: register,
                getById: getById,
                addBookmark: addBookmark,
                removeAccount: removeAccount,
                becomeWriter: becomeWriter,
                getCurrentUser: getCurrentUser,
                uploadAvatar: uploadAvatar
            };

            function getCurrentUser() {
                var deferred = $q.defer();

                if (currentUser) {
                    deferred.resolve(currentUser);
                } else {
                    var userId = $cookies.get(ConstCookies.userId);
                    getById(userId).then(function (data) {
                        currentUser = data;
                        deferred.resolve(currentUser);
                    });
                }

                return deferred.promise;
            }

            function login(user) {
                var url = '/api/user/Login';
                var data = {
                    username: user.Name,
                    password: user.Password
                };
                return httpHelper.get(url, data);
            }

            function logout() {
                $cookies.remove(ConstCookies.userId);
                $cookies.remove(ConstCookies.isAdmin);
                $cookies.remove(ConstCookies.isWriter);
                currentUser = null;
            }

            function register(user) {
                var url = '/Rpg/Register';
                var userJson = angular.toJson(user);
                var data = {
                    userJson: userJson
                };
                return httpHelper.post(url, data);
            }

            function getById(userId) {
                var url = '/Rpg/GetUserById';
                var data = {
                    userId: userId
                };
                return httpHelper.post(url, data);
            }

            function addBookmark(eventId, heroJson) {
                var url = '/Rpg/AddBookmark';
                var data = {
                    eventId: eventId,
                    heroJson: heroJson
                };
                return httpHelper.post(url, data);
            }

            function removeAccount(userId) {
                var url = '/Rpg/RemoveUser';
                var data = {
                    userId: userId
                };
                return httpHelper.post(url, data);
            }

            function becomeWriter() {
                var url = '/Rpg/BecomeWriter';
                return httpHelper.post(url);
            }

            function uploadAvatar(imageData) {
                var url = '/Rpg/UploadAvatar';
                var data = {
                    data: imageData
                };
                return httpHelper.post(url, data);
            }
        }])
    .service('evaluationService', ['httpHelper', function (httpHelper) {
        return {
            save: save
        };

        function save(evaluation) {
            var url = '/Rpg/SaveEvaluation';
            var data = {
                evaluationJson: angular.toJson(evaluation)
            };
            return httpHelper.post(url, data);
        }

        //function bookCompleted(bookId) {
        //    var url = '/Rpg/BookCompleted';
        //    var data = {
        //        bookId: bookId
        //    };
        //    return httpHelper.post(url, data);
        //}

        //function get(bookId) {
        //    var url = '/Rpg/GetBook?id=' + bookId;
        //    return httpHelper.post(url);
        //}

        //function removeBook(bookId) {
        //    var url = '/Rpg/RemoveBook?id=' + bookId;
        //    return httpHelper.post(url);
        //}
    }])
    //CKEditor
    .service('CKEditorService', ['$http', '$q', '_', function ($http, $q, _) {
        function reloadEditor(editorName, newData) {
            var editor = CKEDITOR.instances[editorName];
            if (editor) {
                editor.destroy(true);
            }
            editor = CKEDITOR.replace(editorName, {width: 900, height: 600});
            editor.setData(newData);
        }

        function setData(editorName, data) {
            var editor = CKEDITOR.instances[editorName];
            editor.setData(data);
        }

        function getData(editorName) {
            var editor = CKEDITOR.instances[editorName];
            return editor.getData();
        }

        return {
            reloadEditor: reloadEditor,
            setData: setData,
            getData: getData
        };
    }])
    // Login
    .factory('authInterceptorService', ['$q', '$location', '$cookies', '_',
        function ($q, $location, $cookies, _) {
            function rebook(rebook) {
                // TODO check permission to get access to admin panel
                // goToLogin();
                return rebook;
            }

            function rebookError(rejection) {
                // do something on error
                return $q.reject(rejection);
            }

            function response(response) {
                // do something on success
                return response;
            }

            function responseError(rejection) {
                // do something on error
                return $q.reject(rejection);
            }

            return {
                rebook: rebook,
                rebookError: rebookError,
                response: response,
                responseError: responseError
            };
        }])
    .service('httpHelper', ['$q', '$http', function ($q, $http) {
        return {
            get: get,
            post: post
        };

        function get(url, data, success, error) {
            url = generateUrl(url, data);
            return call(url, null, success, error, 'GET');
        }

        function post(url, data, success, error) {
            return call(url, data, success, error, 'POST', 'application/json; charset=utf-8');
        }

        function call(url, data, success, error, method, contentType) {
            if (!success) {
                success = defaultSuccess;
            }

            if (!error) {
                error = defaultError;
            }

            var deferred = $q.defer();
            var requestModel = {
                method: method,
                url: url,
                headers: { 'Accept': 'application/json' }
            };
            if (data)
                requestModel.data = data;
            if (contentType)
                requestModel.contentType = contentType;

            $http(requestModel).then(success, error);

            return deferred.promise;

            function defaultSuccess(response) {
                deferred.resolve(angular.fromJson(response.data));
            }

            function defaultError() {
                deferred.reject(Error("Sorry :( we have fail"));
            }
        }

        function generateUrl(url, data) {
            if (!data) {
                return url;
            }

            var baseUrl = url;
            var paramStr = '?';
            var properties = Object.keys(data);
            for (var i = 0; i < properties.length; i++){
                var propertyName = properties[i];
                var propertyValue = data[propertyName];
                paramStr += propertyName + '=' + propertyValue + '&';
            }

            return baseUrl + paramStr;
        }
    }]);