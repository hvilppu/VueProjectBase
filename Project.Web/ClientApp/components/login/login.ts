import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { projectService } from '../../services/project-service';
import store from '../../store/store';
import { UserViewModel } from '../../viewmodels/userviewmodel';
import { EventBus } from '../../event-bus';

@Component
export default class LogInComponent extends Vue {

    responsemodel: UserViewModel = new UserViewModel();

    reloaded: boolean = false;
    public userName: string = "";
    public passWord: string = "";
    public userNamePassWordInvalid: boolean = false;
    public auth: boolean = false;
    public logging: boolean = false;
    public pageLoaded: boolean = false;
    public workTimeEntry: boolean = false;
    
    mounted() {
        this.auth = store.getters['authentication/isAuthenticated'].isAuthenticated.toString() == "true";
        this.pageLoaded = true;
    }

    unAuthenticate() {
        this.$store.dispatch('authentication/unauthenticate').then(() => {
        }).catch((err) => {
            alert(err);
        }).then(() => {
        });

        this.$store.dispatch('authentication/setusername', null).then(() => {
        }).catch((err) => {
            alert(err);
        }).then(() => {
        });

        this.$store.dispatch('authentication/setcompanyname', null).then(() => {
        }).catch((err) => {
            alert(err);
        }).then(() => {
        });

        this.$store.dispatch('authentication/setcompanyid', null).then(() => {
        }).catch((err) => {
            alert(err);
        }).then(() => {
        });

        this.auth = store.getters['authentication/isAuthenticated'].isAuthenticated.toString() == "true";
        EventBus.$emit('DISCONNECT_HUBCONNECTION');
        this.deleteAllCookies();
        projectService.logOut(this.workTimeEntry);
    }

    deleteAllCookies() {
        var cookies = document.cookie.split(";");

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        }
    }

    userHavePermission(permission: string): boolean {

        if (!store.getters['authentication/permissions'].permissions) {
            return false;
        }

        return store.getters['authentication/permissions'].permissions.indexOf(permission) > -1;
    }

    login() {
        this.pageLoaded = false;
        this.logging = true;
        if (this.auth) {
            this.unAuthenticate();
            return;
        }

        if (!this.userName || !this.passWord) {
            this.userNamePassWordInvalid = true;
            this.pageLoaded = true;
            this.logging = false;
            return;
        }

        this.userNamePassWordInvalid = false;

        projectService.logIn(this.userName, this.passWord, this.workTimeEntry)
            .subscribe((result: any) => {

                //alert(document.cookie); //Just to check cookie
                this.responsemodel = result as UserViewModel;

                if (document.cookie.indexOf('login=') === -1 || !this.responsemodel.name) {
                    this.userNamePassWordInvalid = true;
                    this.unAuthenticate();
                    this.pageLoaded = true;
                    this.logging = false;
                    document.cookie = "";
                }

                else {
                    EventBus.$emit('CHECK_AND_INIT_HUBCONNECTION');
                    EventBus.$emit('SET_HEARTBEAT');

                    if (this.responsemodel.needSetPassword) {
                        EventBus.$emit('SET_PASSWORD');
                    };

                    this.userNamePassWordInvalid = false;
                    this.$store.dispatch('authentication/setusername', this.responsemodel.name);
                    this.$store.dispatch('authentication/setpermissions', this.responsemodel.permissions);
                    this.$store.dispatch('authentication/authenticate').then(() => {
                    }).catch((err) => {
                        alert(err);
                    }).then(() => {
                        if (this.userHavePermission("PoolsOverview_Use")) {
                            this.$router.push({ name: 'poolsoverview' });
                        }
                        else if (this.userHavePermission("CheckpointAreas_Use")) {
                            this.$router.push({ name: 'checkpointareas', params: { objectParameter: "first" } });
                        }
                        else if (this.userHavePermission("Common_Reports")) {
                            this.$router.push({ name: 'reports' });
                        }
                    });
                }

            }, (error: any) => {
                if (error.operation && error.operation == "re-load" && !this.reloaded) {
                    this.reloaded = true;
                    this.pageLoaded = true;
                    this.logging = false;
                }
            }
            );
    }
}
