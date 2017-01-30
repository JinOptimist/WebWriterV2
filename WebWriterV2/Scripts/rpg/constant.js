angular
    .module('AppConst',[])
    .constant("ConstCookies", (function () {
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
