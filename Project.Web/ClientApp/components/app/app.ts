import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { EventBus } from '../../event-bus';
import store from '../../store/store';
import * as signalR from "@microsoft/signalr";
import { ChatMessageViewModel } from '../../viewmodels/chatmessageviewmodel';
import { projectService } from '../../services/project-service';
import routes from '../../routes';
import { translate } from '../../boot';
import version from "raw-loader!./../../versionFile.txt"
import { UserDto } from '../../dtos/UserDto';
import { UserInfoDto } from '../../dtos/UserInfoDto';

@Component({
    components: {
        PageLoading: require('../pageloading/pageloading.vue.html').default
    }
})
export default class AppComponent extends Vue {

    private version: string = "v.0.1.11";

    private connection?: signalR.HubConnection;
    private eventbus: any = EventBus;
    private gotAlerts: boolean = false; 
    public clipped = false;
    public drawer = false;
    public fixed = false;
    public get items() {
        return [
            { icon: 'lock_open', title: translate("LOG"), to: '/login', authenticated: false, },
        ];
    }
    public show = true;
    public miniVariant = false;
    public right = true;

    public name = '';
    public langs: any = ['en', 'fi']
    public newCompanyId: number = 0;
    public messages: ChatMessageViewModel[] = [];
    public currentMessage: string = "";

    setPasswordCpa: boolean = false;
    setPasswordButton: boolean = false;

    changeLogCp: boolean = false;
    newPassword: string = "";

    private alert: boolean = false;
    private alertText: string = "";
    private alertTimeout: number = 20000; 

    public logo: string = require("../../assets/project.png");
    public defaultImageSource: string = require("../../assets/banner.png");

    private info: boolean = false;
    public infoSnackbarColor: string = "green";
    public infoSnackbarTimeout: number = 6000;
    public infoSnackbarText: string = "Place holder";

    get title(): string { return translate("TITLE").toString() };

    public selectedUsers: number[] = [];
    public users: UserInfoDto[] = [];

    get versionText(): string {
        return version;
    }

    get messageHeaders(): any[] {
        return [
            { text: translate("NAME"), value: 'Name', width: '200px' },
            { text: translate("MESSAGE"), value: 'Message' },
        ]
    }

    get footerProps(): any {
        return { 'items-per-page-options': [10, 25, 50, -1], 'items-per-page-text': translate("SHOW"), 'items-per-page-all-text': translate("ALL") };
    }

    get newPasswordValid() {
        return this.newPassword.length >= 4;
    }

    get userName() {
        return this.$store.getters['authentication/name'] as string;
    }

    get companyName() {
        return this.$store.getters['authentication/companyName'] as string;
    }

    get auth(): boolean {

        return store.getters['authentication/isAuthenticated'].isAuthenticated.toString() == "true";
    }

    userHavePermission(permission: string): boolean {

        if (!store.getters['authentication/permissions'].permissions) {
            return false;
        }

        return store.getters['authentication/permissions'].permissions.indexOf(permission) > -1;
    }

    get isUserAdmin(): boolean {
        return store.getters['authentication/permissions'].permissions.indexOf('Users_Admin') > -1;
    }

    get isTouchDevice() {
        return 'ontouchstart' in window || 'onmsgesturechange' in window;
    }

    get isPoolAdmin(): boolean {
        return store.getters['authentication/permissions'].permissions.indexOf('Pool_Admin') > -1;
    }

    getItemColor(item: any) {
        if (item.name != undefined) {
            return "color:indianred";
        }

        return "color:white";
    }

    getItemClass(item: any) {
        if (item.name != undefined) {
            return "pulse-button";
        }

        return "";
    }

    forceUpdate() {
        this.$nextTick(() => {
            this.$forceUpdate();
        });
    }
     
    dialogCp: boolean = false;

    showPw: boolean = false;

    setNewPassword() {

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

        EventBus.$emit('DISCONNECT_HUBCONNECTION');
        this.deleteAllCookies();
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

    speechBubbleClass(): string {

        if (this.messages.length == 0)
            return "background-color: white; ml-2 mr-2";

        return "background-color: green; ml-2 mr-2";
    }

    sendMessage() {
        if (this.currentMessage == "")
            return;

        EventBus.$emit('SEND_MESSAGE', this.$store.getters['authentication/name'].name, this.currentMessage, this.selectedUsers);
    }

    afterMessage() {
        this.currentMessage = "";
    }


    //type: error, warn, success
    showNotification(text: string, title: string, type: string) {

        let notificationDuration: number = 3000;

        if (type == 'error' || type == 'warn')
            notificationDuration = -1;

        this.$notify({
            group: 'appNotifications',
            title: title,
            text: text,
            type: type,
            duration: notificationDuration
        });
    }

    DisconnectSignalR() {
        if (this.connection) {
            this.connection.send("disconnectClientMessage");
            this.connection.stop();
        }
    }

    InitSignalR() {

        if (this.connection && this.connection.state.toString().toUpperCase() != "DISCONNECTED") {
            return;
        }

        console.info("InitSignalR");

        if (this.connection == null) {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/hub")
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Debug)
                .build();

            this.connection.on("messageReceived", (username: string, message: string) => {
                EventBus.$emit('MESSAGE_RECEIVED', username, message);
                let chatMessageViewModel: ChatMessageViewModel = new ChatMessageViewModel();
                chatMessageViewModel.Name = username;
                chatMessageViewModel.Message = message;
                this.messages.push(chatMessageViewModel);
            });

        }

        this.connection.start().catch(err => alert(err));

    }

    beforeDestroy() {
        EventBus.$off('SEND_MESSAGE');
        EventBus.$off('INIT_HUBCONNECTION');
        EventBus.$off('DISCONNECT_HUBCONNECTION');
        EventBus.$off('SET_HEARTBEAT');
    }

    mounted() {

        Vue.filter('toLocalDate', function (value: any) {
            return 'fi-FI';
        });
        

        Vue.filter('digits', function (value: any) {
            if (typeof value !== "number") {

                if (value == undefined)
                    return value;

                let str = value.toString();

                let strAsNumber = +str.replace(".", ",");
                if (value == 0) {
                    return value.toLocaleString('fi-FI');
                }

                value = strAsNumber;
            }

            if (value % 1 === 0) {

                return value.toLocaleString('fi-FI');
            }

            var number: number = +value.toFixed(2);

            return number.toLocaleString('fi-FI');
        });

        Vue.filter('digits0', function (value: any) {
            if (typeof value !== "number") {

                if (value == undefined)
                    return value;

                let str = value.toString();

                let strAsNumber = +str.replace(".", ",");
                if (value == 0) {
                    return value.toLocaleString('fi-FI');
                }

                value = strAsNumber;
            }

            if (value % 1 === 0) {

                return value.toLocaleString('fi-FI');
            }

            var number: number = +value.toFixed(0);

            return number.toLocaleString('fi-FI');
        });

        Vue.filter('digits3', function (value: any) {
            if (typeof value !== "number") {

                if (value == undefined)
                    return value;

                let str = value.toString();

                let strAsNumber = +str.replace(".", ",");
                if (value == 0) {
                    return value.toLocaleString('fi-FI');
                }

                value = strAsNumber;
            }

            if (value % 1 === 0) {
                return value.toLocaleString('fi-FI');
            }

            var number: number = +value.toFixed(3);

            return number.toLocaleString('fi-FI');
        });

        Vue.filter('treeChars', function (value: any) {
            if (!value)
                return "";

            if (value.length >= 3)
                return value.substring(0, 3);

            if (value.length == 2)
                return value.split('').join(' ');

            if (value.length == 1)
                return " " + value + " ";

            return ""

        });

        if (this.auth && (!this.connection || this.connection.state.toString().toUpperCase() == "DISCONNECTED")) {
            this.InitSignalR();
        }

        /* Not used yet */
        switch (performance.navigation.type) {
            case 0:
                console.info("TYPE_NAVIGATE");
                break;
            case 1:
                console.info("TYPE_RELOAD");
                // this.$router.push({ name: 'home' });
                break;
            case 2:
                console.info("TYPE_BACK_FORWARD");
                break;
            case 255:
                console.info("255");
                break;
        }

        EventBus.$off('SEND_MESSAGE');
     
        EventBus.$off('CHECK_AND_INIT_HUBCONNECTION');
        EventBus.$off('DISCONNECT_HUBCONNECTION');
        EventBus.$off('SET_HEARTBEAT');
        EventBus.$off('SET_PASSWORD');

        EventBus.$on("CHECK_AND_INIT_HUBCONNECTION", this.InitSignalR);
        EventBus.$on("DISCONNECT_HUBCONNECTION", this.DisconnectSignalR);

        EventBus.$on('SEND_MESSAGE', (username: string, message: string, selectedUsers: number[]) => {
            if (this.connection != null) {
                this.connection.send("newMessage", username, message, selectedUsers).then(() => this.afterMessage());
                let m: ChatMessageViewModel = new ChatMessageViewModel();
                m.Message = message;
                m.Name = "✉️->";
                this.messages.push(m);
            }
        });

       

        EventBus.$on("SET_HEARTBEAT", this.SetHeartBeat);
        EventBus.$on("SET_PASSWORD", () => this.setPasswordButton = true);


        this.$i18n.locale = "fi";
       
    }


    diasbleAlert() {
        this.alert = false;
    }

    SetHeartBeat() {

        setInterval(() => { this.HeartBeat() }, 60 * 1000);
        setInterval(() => {
            if (this.auth && (!this.connection || this.connection.state.toString().toUpperCase() == "DISCONNECTED")) {
                this.InitSignalR();
            }
        }, 5 * 1000);


        this.HeartBeat();
    }

    HeartBeat() {
        projectService.heartBeat();
    }

    fab: boolean = false;

    onScroll(e: any) {
        if (typeof window === 'undefined') return;
        const top = window.pageYOffset || e.target.scrollTop || 0;
        this.fab = top > 600;
    };

    toTop() {
        this.$vuetify.goTo(0);
    }
}


