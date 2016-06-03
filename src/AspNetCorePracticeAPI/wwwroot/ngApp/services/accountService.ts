namespace AspNetCorePracticeAPI.Services {

    export class AccountService {

        // Store access token and claims in browser session storage
        private storeUserInfo(userInfo) {
            // store user name
            this.$window.sessionStorage.setItem('userName', userInfo.userName);

            // store claims
            this.$window.sessionStorage.setItem('claims', JSON.stringify(userInfo.claims));
        }

        public getUserName() {
            return this.$window.sessionStorage.getItem('userName');
        }


        public getClaim(type) {
            var allClaims = JSON.parse(this.$window.sessionStorage.getItem('claims'));
            return allClaims ? allClaims[type] : null;
        }


        public login(loginUser) {
            return this.$q((resolve, reject) => {
                this.$http.post('/api/account/login', loginUser).then((result) => {
                        this.storeUserInfo(result.data);
                        resolve();
                }).catch((result) => {
                    var messages = this.flattenValidation(result.data);
                    reject(messages);
                });
            });
        }

        public register(userLogin) {
            return this.$q((resolve, reject) => {
                this.$http.post('/api/account/register', userLogin)
                    .then((result) => {
                        this.storeUserInfo(result.data);
                        resolve(result);
                    })
                    .catch((result) => {
                        var messages = this.flattenValidation(result.data);
                        reject(messages);
                    });
            });
        }


        public logout() {
            // clear all of session storage (including claims)
            this.$window.sessionStorage.clear();

            // logout on the server
            return this.$http.post('/api/account/logout', null);
        }

        public isLoggedIn() {
            return this.$window.sessionStorage.getItem('userName');
        }

        // associate external login (e.g., Twitter) with local user account
        public registerExternal(email) {
            return this.$q((resolve, reject) => {
                this.$http.post('/api/account/externalLoginConfirmation', { email: email })
                    .then((result) => {
                        this.storeUserInfo(result.data);
                        resolve(result);
                    })
                    .catch((result) => {
                        // flatten error messages
                        let messages = this.flattenValidation(result.data);
                        reject(messages);
                    });
            });
        }



        getExternalLogins(): ng.IPromise<{}> {
            return this.$q((resolve, reject) => {
                let url = `api/Account/getExternalLogins?returnUrl=%2FexternalLogin&generateState=true`;
                this.$http.get(url).then((result: any) => {
                    resolve(result.data);
                }).catch((result) => {
                    reject(result);
                });
            });
        }


        // checks whether the current user is authenticated on the server and returns user info
        public checkAuthentication() {
            this.$http.get('/api/account/checkAuthentication')
                .then((result) => {
                    if (result.data) {
                        this.storeUserInfo(result.data);
                    }
                });
        }

        confirmEmail(userId, code): ng.IPromise<{}> {
            return this.$q((resolve, reject) => {
                let data = {
                    userId: userId,
                    code: code
                };
                this.$http.post('/api/account/confirmEmail', data).then((result: any) => {
                    resolve(result.data);
                }).catch((result) => {
                    reject(result);
                });
            });
        }


        // extract access token from response
        parseOAuthResponse(token) {
            let results = {};
            token.split('&').forEach((item) => {
                let pair = item.split('=');
                results[pair[0]] = pair[1];
            });
            return results;
        }


        private flattenValidation(modelState) {
            let messages = [];
            for (let prop in modelState) {
                messages = messages.concat(modelState[prop]);
            }
            return messages;
        }

        constructor
        (
            private $q: ng.IQService,
            private $http: ng.IHttpService,
            private $window: ng.IWindowService
        ) {
          // in case we are redirected from a social provider
          // we need to check if we are authenticated.
          this.checkAuthentication();
        }

    }

    angular.module('AspNetCorePracticeAPI').service('accountService', AccountService);
}
