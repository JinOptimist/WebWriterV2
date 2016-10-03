
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('services', ['ngRoute', 'underscore']) //, ['common', 'search', 'masha', 'ui.ace']
    .constant('_',
        window._
    )
    .run(['$http', 'sexService', 'raceService', 'heroService', function ($http, sexService, raceService, heroService) {
        $http({
            method: 'GET',
            url: '/Rpg/GetDefaultHero',
            headers: { 'Accept': 'application/json' }
        }).success(function (rawHero) {
            var hero = angular.fromJson(rawHero);
            heroService.setDefaultHero(hero);
        });
    }])
    .service('questService', ['$http', '$q', function ($http, $q) {
        var currentQuest = null;

        return {
            getQuest: function () {
                var deferred = $q.defer();
                if (currentQuest) {
                    deferred.resolve(currentQuest);
                    return deferred.promise;
                }

                $http({
                    method: 'GET',
                    url: '/Rpg/GetOneQuest',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        currentQuest = angular.fromJson(response);
                        deferred.resolve(currentQuest);
                    });
                return deferred.promise;
            },
            setQuest: function (value) {
                currentQuest = value;
            },
            setExecutor: function (executor) {
                currentQuest.Executor = executor;
            }
        };
    }])
    .service('eventService', ['$http', '$q', function ($http, $q) {
        function getPromise(currentEventId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'GET',
                    url: '/Rpg/GetEventChildren?id=' + currentEventId,
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    var event = angular.fromJson(response);
                    resolve(event);
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        function getAllEvents(questId) {
            return $q(function (resolve, reject) {
                $http({
                    method: 'GET',
                    url: '/Rpg/GetAllEvents?questId=' + questId,
                    headers: { 'Accept': 'application/json' }
                }).success(function (response) {
                    resolve(angular.fromJson(response));
                },
                function () {
                    reject(Error("Sorry :( we have fail"));
                });
            });
        }

        return {
            getEventChildrenPromise: function (currentEventId) {
                return getPromise(currentEventId);
            },
            getAllEvents: getAllEvents
        };
    }])
    .service('heroService', ['$http', '$q', function ($http, $q) {
        var listHeroes = null;
        var selectedHero = null;
        var defaultHero = {};
        return {
            loadListHeroes: function () {
                var deferred = $q.defer();

                if (listHeroes) {
                    deferred.resolve(listHeroes);
                    return deferred.promise;
                }

                $http({
                    method: 'GET',
                    url: '/Rpg/GetHeroes',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        listHeroes = angular.fromJson(response);
                        deferred.resolve(listHeroes);
                    });
                return deferred.promise;
            },
            removeHero: function (hero) {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: '/Rpg/RemoveHero?id=' + hero.Id,
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        deferred.resolve(response);
                    });
                return deferred.promise;
            },
            addHero: function (newHero) {
                newHero.Sex = newHero.Sex - 0;
                newHero.Race = newHero.Race - 0;

                listHeroes.push(newHero);
            },
            getAllHeroes: function () {
                return listHeroes;
            },
            saveHero: function (newHero) {
                var deferred = $q.defer();
                $http({
                    method: 'POST',
                    url: '/Rpg/SaveHero',
                    data: { jsonHero: angular.toJson(newHero) },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        deferred.resolve(response);
                    });

                return deferred.promise;
            },
            selectHero: function (hero) {
                selectedHero = hero;
            },
            getSelectedHero: function () {
                return selectedHero;
            },
            loadEnemy: function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: '/Rpg/GetEnemy',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var enemy = angular.fromJson(response);
                        deferred.resolve(enemy);
                    });
                return deferred.promise;
            },
            getDefaultHero: function () {
                return defaultHero;
            },
            setDefaultHero: function (hero) {
                defaultHero = hero;
            },
            loadDefaultHero: function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: '/Rpg/GetDefaultHero',
                    headers: { 'Accept': 'application/json' }
                }).success(function (rawHero) {
                    var hero = angular.fromJson(rawHero);
                    defaultHero = hero;
                    deferred.resolve(defaultHero);
                });
                return deferred.promise;
            }
        };
    }])
    .service('raceService', ['$http', '$q', '_', function ($http, $q, _) {
        var raceList;

        function updateRaceList(newList) {
            _.each(newList, function (item) {
                switch (item.Value) {
                    case 1:
                        item.src = "/Content/rpg/human.jpg";
                        break;
                    case 2:
                        item.src = "/Content/rpg/elf.jpg";
                        break;
                    case 3:
                        item.src = "/Content/rpg/orc.jpg";
                        break;
                    case 4:
                        item.src = "/Content/rpg/gnom.jpg";
                        break;
                    case 5:
                        item.src = "/Content/rpg/dragon.jpg";
                        break;
                }
            });
            return newList;
        }

        return {
            loadRaceList: function () {
                var deferred = $q.defer();
                if (raceList) {
                    deferred.resolve(raceList);
                    return deferred.promise;
                }

                $http({
                    method: 'GET',
                    url: '/Rpg/GetListRace',
                    headers: { 'Accept': 'application/json' }
                })
                .success(function (response) {
                    raceList = angular.fromJson(response);
                    deferred.resolve(raceList);
                });
                return deferred.promise;
            },
            getRaceList: function () {
                var copyRace = raceList.map(function (value) {
                    return angular.copy(value);
                });
                return copyRace;//clone array
            },
            getRaceWord: function (raceValue) {
                var filtered = raceList.filter(function (race) { return race.value == raceValue });
                if (filtered.length === 1)
                    return filtered[0].name;
                return "?";
            },
            setList: function (newRaceList) {
                raceList = updateRaceList(newRaceList);
            }
        };
    }])
    .service('sexService', ['$http', '$q', '_', function ($http, $q, _) {
        var sexList;

        function updateSexList(newList) {
            _.each(newList, function (item) {
                switch (item.Value) {
                    case 1:
                        item.src = "/Content/rpg/man.jpg";
                        break;
                    case 2:
                        item.src = "/Content/rpg/woman.jpg";
                        break;
                    case 3:
                        item.src = "/Content/rpg/unknown.jpg";
                        break;
                }
            });
            return newList;
        }

        return {
            loadSexList: function () {
                var deferred = $q.defer();
                if (sexList) {
                    deferred.resolve(sexList);
                    return deferred.promise;
                }

                $http({
                    method: 'GET',
                    url: '/Rpg/GetListSex',
                    headers: { 'Accept': 'application/json' }
                })
                .success(function (response) {
                    sexList = angular.fromJson(response);
                    deferred.resolve(sexList);
                });
                return deferred.promise;
            },
            getSexList: function () {
                var copySex = sexList.map(function (value) {
                    return angular.copy(value);
                });
                return copySex; // clone array
            },
            getSexWord: function (sexValue) {
                var filtered = sexList.filter(function (sex) { return sex.value == sexValue });
                if (filtered.length === 1)
                    return filtered[0].name;
                return "?";
            },
            setList: function (newSexList) {
                sexList = updateSexList(newSexList);
            }
        };
    }])
    .service('guildService', ['$http', '$q', '_', function ($http, $q, _) {
        var currentGuild = null;

        var guildPromise = $q(function (resolve, reject) {
            if (currentGuild) {
                resolve(currentGuild);
                return;
            }

            $http({
                method: 'GET',
                url: '/Rpg/GetGuildInfo',
                headers: { 'Accept': 'application/json' }
            }).success(function (response) {
                currentGuild = angular.fromJson(response);
                resolve(currentGuild);
            },
            function () {
                reject(Error("Sorry :( we have fail"));
            });
        });

        return {
            getGuild: function () {
                return currentGuild;
            },
            setGuild: function (value) {
                currentGuild = value;
            },
            getGuildPromise: guildPromise
        };
    }])
    .service('traningRoomService', ['$http', '$q', '_', function ($http, $q, _) {
        var currentRoom = null;
        var currentHero = null;

        return {
            getRoom: function () {
                return currentRoom;
            },
            getHero: function () {
                return currentHero;
            },
            chooseRoom: function (selectedRoom, hero) {
                currentRoom = selectedRoom;
                currentHero = hero;
            }
        };
    }])
    .service('skillService', ['$http', '$q', '_', function ($http, $q, _) {
        var allSkills = [];

        function getState(hero, stateName) {
            return _.find(hero.State, function (state) { return state.StateType.Name == stateName; });
        }

        return {
            loadSkillEffect: function (skillId) {
                return $q(function (resolve, reject) {
                    $http({
                        method: 'GET',
                        url: '/Rpg/GetSkillEffect?skillId=' + skillId,
                        headers: { 'Accept': 'application/json' }
                    }).success(function (response) {
                        resolve(angular.fromJson(response));
                    },
                    function () {
                        reject(Error("Sorry :( we have fail"));
                    });
                });
            },
            isEnough: function (hero, skill) {
                // return null if ok, and stat name id you have lack
                for (var i = 0; i < skill.SelfChanging.length; i++) {
                    var selfChange = skill.SelfChanging[i];
                    var stat = getState(hero, selfChange.StateType.Name);//_.where(hero.State, { Value: selfChange.Value });
                    if (stat)
                        if (stat.Number + selfChange.Number < 1) {
                            return "Not enough " + stat.StateType.Name;
                        }
                }

                return null;
            },
            loadAll: function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: '/Rpg/GetSkills',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        allSkills = angular.fromJson(response);
                        deferred.resolve(allSkills);
                    });
                return deferred.promise;
            },
            loadSkillsSchool: function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: '/Rpg/GetSkillsSchool',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var skillSchools = angular.fromJson(response);
                        deferred.resolve(skillSchools);
                    });
                return deferred.promise;
            },
            saveSkill: function (skill) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/SaveSkill',
                    data: { jsonSkill: angular.toJson(skill) },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var skill = angular.fromJson(response);
                        deferred.resolve(skill);
                    });
                return deferred.promise;
            },
            removeSkill: function (skill) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/RemoveSkill',
                    data: { skillId: skill.Id },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var skill = angular.fromJson(response);
                        deferred.resolve(skill);
                    });
                return deferred.promise;
            }
        };
    }])
    .service('stateService', ['$http', '$q', '_', function ($http, $q, _) {
        var states = [];
        return {
            loadAllTypes: function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: '/Rpg/GetStateTypes',
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        states = angular.fromJson(response);
                        deferred.resolve(states);
                    });
                return deferred.promise;
            },
            save: function (state) {
                return '+';
            }
        };
    }])
    .service('characteristicService', ['$http', '$q', '_', function ($http, $q, _) {
        var characteristicTypes = [];
        return {
            loadAllCharacteristic: function() {
                var deferred = $q.defer();

                $http({
                        method: 'GET',
                        url: '/Rpg/GetCharacteristicTypes',
                        headers: { 'Accept': 'application/json' }
                    })
                    .success(function(response) {
                        characteristicTypes = angular.fromJson(response);
                        deferred.resolve(characteristicTypes);
                    });
                return deferred.promise;
            },
            removeCharacteristicType: function (characteristicType) {
                var deferred = $q.defer();

                $http({
                        method: 'POST',
                        url: '/Rpg/RemoveCharacteristicType',
                        data: { characteristicTypeId: characteristicType.Id },
                        headers: { 'Accept': 'application/json' }
                    })
                    .success(function(response) {
                        deferred.resolve(response);
                    });
                return deferred.promise;
            },
            save: function(characteristicType) {
                var deferred = $q.defer();

                $http({
                    method: 'POST',
                    url: '/Rpg/SaveCharacteristicType',
                    data: { jsonCharacteristicType: angular.toJson(characteristicType) },
                    headers: { 'Accept': 'application/json' }
                })
                    .success(function (response) {
                        var savedCharacteristicType = angular.fromJson(response);
                        deferred.resolve(savedCharacteristicType);
                    });
                return deferred.promise;
            }
        };
    }]);