import { registerPlugins } from "@/plugins";
import App from "./App.vue";
import { createApp } from "vue";

const Appp = createApp(App);

registerPlugins(Appp);

Appp.mount("#appp");
