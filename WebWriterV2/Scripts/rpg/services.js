
var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

angular.module('services', ['ngRoute', 'underscore']) //, ['common', 'search', 'masha', 'ui.ace']
    .constant('_',
        window._
    )
    .run(['$http', 'sexService', 'raceService', function ($http, sexService, raceService) {
        $http({
            method: 'GET',
            url: '/Rpg/GetListSex',
            headers: { 'Accept': 'application/json' }
        }).success(function (response) {
            var sexList = angular.fromJson(response);
            sexService.setList(sexList);
        });

        $http({
            method: 'GET',
            url: '/Rpg/GetListRace',
            headers: { 'Accept': 'application/json' }
        }).success(function (response) {
            var raceList = angular.fromJson(response);
            raceService.setList(raceList);
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
                    .success(function(response) {
                            currentQuest = angular.fromJson(response);
                            deferred.resolve(currentQuest);
                        });
                return deferred.promise;
            },
            setQuest: function (value) {
                currentQuest = value;
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
    .service('sharedHeroes', ['$http', '$q', function ($http, $q) {
        var listHeroes = null;
        return {
            getListHeroes: function () {
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
                    .success(function(response) {
                            listHeroes = angular.fromJson(response);
                            deferred.resolve(listHeroes);
                        });
                return deferred.promise;
            },
            addHero: function (newHero) {
                newHero.Sex = newHero.Sex - 0;
                newHero.Race = newHero.Race - 0;

                listHeroes.push(newHero);
            }
        };
    }])
    .service('raceService', ['_', function (_) {
        var raceList = [];

        function updateRaceList(newList) {
            _.each(newList, function (item) {
                switch (item.value) {
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
                        item.src = "/Content/rpg/dragon.jpg";
                        break;
                    case 5:
                        item.src = "/Content/rpg/gnom.jpg";
                        break;
                }
            });
            return newList;
        }

        return {
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
    .service('sexService', ['_', function (_) {
        var sexList = [];

        function updateSexList(newList) {
            _.each(newList, function (item) {
                switch (item.value) {
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
    }]);