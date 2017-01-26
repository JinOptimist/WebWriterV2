angular
    .module('rpg', [])
    .constant("cookies", (function () {
        var baseApiUrl = 'http://localhost:8086';
        return {
            // placeholders
            userId: "userId",
            isWriter: "isWriter",
            isAdmin: "isAdmin"
        };
    })())
    .constant("authConst", {
        AuthCookieName: 'Authorization'
    });