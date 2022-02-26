import Vue from 'vue';
import Vuex from 'vuex';
import authentication from './modules/authentication';
import createPersistedState from 'vuex-persistedstate'

Vue.use(Vuex);

export default new Vuex.Store({
    plugins: [createPersistedState({
        storage: window.sessionStorage,
    })],
    modules: {
        authentication: {
            namespaced: true,
            state: authentication.state,
            mutations: authentication.mutations,
            getters: authentication.getters,
            actions: authentication.actions,
        }
    }
});




