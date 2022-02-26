import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
import Notifications from 'vue-notification'
import store from './store/store';
import axios from 'axios';
import { EventBus } from './event-bus';
import routes from './routes';
import 'vuetify/dist/vuetify.min.css';
import 'font-awesome/css/font-awesome.css';
import Vuetify from 'vuetify';
import VueI18n from 'vue-i18n';
import moment from 'moment';
import VueGoogleCharts from 'vue-google-charts'

Vue.use(VueGoogleCharts)
// @ts-ignore
import VueDragscroll from 'vue-dragscroll';

import Vue2TouchEvents from 'vue2-touch-events'
Vue.use(Vue2TouchEvents);
Vue.use(VueDragscroll);

Vue.prototype.moment = moment;

Vue.use(VueRouter);
Vue.use(Notifications);
Vue.use(VueI18n);
const vuetifyOptions = {}

Vue.use(Vuetify)

const vueRouter = new VueRouter({
    routes: routes,
});

const i18n = new VueI18n({
    locale: 'fi', // set locale
    messages: {
        'en': require('./lang/en.json'),
        'fi': require('./lang/fi.json')
    },
})

const translate = (key: string) => {
    if (!key) {
        return '';
    }
    return i18n.t(key);
};

vueRouter.beforeEach(async (to: any, from: any, next: any) => {
    if (to.matched.some((record: any) => record.meta.authenticated)) {

        EventBus.$emit('CHECK_AND_INIT_HUBCONNECTION');
        let auth: boolean = store.getters['authentication/isAuthenticated'].isAuthenticated.toString() == "true";

        if (!auth) {
            next({
                path: '/login'
            });
        }

        else {
           if (to.matched.some((record: any) => record.meta.permission)) {
                let permission: boolean = store.getters['authentication/permissions'].permissions.indexOf(to.meta.permission) > -1

                if (!permission) {
                    next({
                        path: '/login'
                    });
                }

                else {
                    next();
                }
            }
            else {
                alert(next);
                next();
            }
        }
    }

    else {
        next();
    }
});

new Vue({
    el: '#app-root',
    router: vueRouter,
    i18n,
    vuetify: new Vuetify({
        icons: {
            iconfont: 'md', // 'mdi' || 'mdiSvg' || 'md' || 'fa' || 'fa4'
        },
    }),
    store: store,

    render: h => h(require('./components/app/app.vue.html').default),
});

export { i18n, translate }; 
