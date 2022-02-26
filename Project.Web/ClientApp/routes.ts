import { RouteConfig } from 'vue-router';
import { Component } from 'vue-property-decorator';

let routes: RouteConfig[] = [    
    { path: '/', component: require('./components/login/login.vue.html').default, meta: { authenticated: false } }, // NO AUTH
    //{ path: '/example', name: "example", component: require('./components/example/example.vue.html').default, meta: { authenticated: true, permission: "Example_Use" } },6
];

export default routes;