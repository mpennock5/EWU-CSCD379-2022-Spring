<template>
  <v-app class="overflow-hidden">
    <v-app-bar fixed app>
      <router-link to="/" class="text-h4 font-weight-black no-decoration">
        <WLogo /> Wordle </router-link
      ><v-spacer />
      <settings-dialog />
      <v-app-bar-nav-icon @click="drawer = true"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-main>
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
    <v-footer app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>

    <v-navigation-drawer v-model="drawer" absolute temporary right height="150">
      <v-list nav dense>
        <v-list-item nuxt to="/">
          <v-list-item-icon>
            <v-icon>mdi-home</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Home</v-list-item-title>
        </v-list-item>

        <v-list-item nuxt to="/game">
          <v-list-item-icon>
            <v-icon>mdi-controller-classic</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Game</v-list-item-title>
        </v-list-item>

        <v-list-item nuxt to="/about">
          <v-list-item-icon>
            <v-icon>mdi-information-outline</v-icon>
          </v-list-item-icon>
          <v-list-item-title>About</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
  </v-app>
  <!-- <v-app>
    <v-app-bar fixed app>
      <router-link to="/" class="text-h4 font-weight-black no-decoration">
        <WLogo /> Wordle
      </router-link>
      <v-spacer />
      <settings-dialog />
    </v-app-bar>
    <v-main>
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
    <v-footer app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app> -->
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import SettingsDialog from '@/components/settings-dialog.vue'

@Component({ components: { SettingsDialog } })
export default class DefaultLayout extends Vue {
  drawer: boolean = false
}
</script>

<style>
.no-decoration {
  text-decoration: none;
}
</style>
