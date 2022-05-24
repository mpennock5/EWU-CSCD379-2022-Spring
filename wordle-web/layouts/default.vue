<template>
  <v-app dark>
    <v-app-bar :clipped-left="clipped" fixed app>
      <v-toolbar-title>
        <v-btn nuxt to="/">
          <v-icon x-large>mdi-file-word-box</v-icon>
          {{ title }}
        </v-btn>
      </v-toolbar-title>
      <v-spacer />
      <v-btn icon @click.stop="rightDrawer = !rightDrawer">
        <v-icon>mdi-menu</v-icon>
      </v-btn>
    </v-app-bar>
    <v-main>
      <Nuxt />
    </v-main>
    <v-navigation-drawer v-model="rightDrawer" :right="right" temporary fixed>
      <v-list nav dense>
        <v-list-item>
          <v-list-item-title class="d-flex justify-center">
            <v-icon x-large>mdi-file-word-box</v-icon>
          </v-list-item-title>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/">
            <v-icon>mdi-home</v-icon>
            <v-spacer></v-spacer>
            Home
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/dailywordgame">
            <v-icon>mdi-controller-classic</v-icon>
            <v-spacer></v-spacer>
            Daily Word
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/freeplay">
            <v-icon>mdi-controller-classic-outline</v-icon>
            <v-spacer></v-spacer>
            Freeplay
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/rules">
            <v-icon>mdi-book-open-variant</v-icon>
            <v-spacer></v-spacer>
            How To Play
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/leaderboard">
            <v-icon>mdi-account-details</v-icon>
            <v-spacer></v-spacer>
            Top Players
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/previousdays">
            <v-icon>mdi-equalizer</v-icon>
            <v-spacer></v-spacer>
            Hindsight 10
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block nuxt to="/about">
            <v-icon>mdi-help-circle</v-icon>
            <!-- <v-spacer></v-spacer> looks better being inline with settings -->
            About
          </v-btn>
        </v-list-item>

        <v-list-item>
          <v-btn text block>
            <SettingsDialog />
          </v-btn>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-footer app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: 'DefaultLayout',

  data() {
    return {
      clipped: false,
      right: true,
      rightDrawer: false,
      title: '!Wordle',
    }
  },
}
</script>

<!-- 
<v-tooltip bottom>
      <template #activator="{ on, attrs }">
        <v-btn color="info" nuxt to="/leaderboard" fab v-bind="attrs" v-on="on">
          <v-icon> mdi-equalizer </v-icon>
        </v-btn>
      </template>
      <span> Leader Board </span>
    </v-tooltip> -->
