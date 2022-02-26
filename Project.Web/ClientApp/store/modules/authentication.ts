import { EventBus } from '../../event-bus';
import Vue from 'vue';


const state = {
    isAuthenticated: Boolean,
    name: String,
    permissions: Array<String>(),
};

const getters = {
    isAuthenticated: (isAuthenticated: boolean) => isAuthenticated as boolean,
    name: (name: string) => name as string,
    permissions: (permissions: string[]) => permissions as string[],
};

const actions = {
    authenticate: ({ commit, dispatch }: { commit: any, dispatch: any }) => {
        commit('authenticated', true);
    },
    unauthenticate: ({ commit, dispatch }: { commit: any, dispatch: any }) => {
        commit('authenticated', false);
    },
    setusername: ({ commit, dispatch }: { commit: any, dispatch: any }, data: any) => {
        commit('username', data);
    },
    setpermissions: ({ commit, dispatch }: { commit: any, dispatch: any }, data: any) => {
        commit('permissions', data);
    }
};

const mutations = {
    authenticated: (s: any, response: any) => {
        Vue.set(s, "isAuthenticated", response);
    },
    username: (s: any, response: any) => {
        Vue.set(s, "name", response);
    },
    permissions: (s: any, response: any) => {
        Vue.set(s, "permissions", response);
    }
};

export default {
    state,
    getters,
    actions,
    mutations,
};