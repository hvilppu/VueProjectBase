import Vue from 'vue';
import { Component, Prop, Watch } from 'vue-property-decorator';

@Component
export default class PageLoading extends Vue {

    @Prop({ default: null })
    eventBus: any | null;

    @Prop({ default: 'show-loading' })
    eventShow: string | undefined;

    @Prop({ default: 'hide-loading' })
    eventHide: string | undefined;
    
    private showing: boolean = false;
    private label: string = "loading...";

    showLoading(label?: string) {
        if (label)
            this.label = label;

        this.showing = true
    }

    hideLoading() {
        this.showing = false
    }

    registerBusMethods() {
        this.eventBus.$on(this.eventShow, this.showLoading);
        this.eventBus.$on(this.eventHide, this.hideLoading);
    }

    mounted() {
        if (this.eventBus)
            this.registerBusMethods();
    }
}