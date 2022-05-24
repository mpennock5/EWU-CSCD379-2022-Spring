<template>
  <v-container fluid fill-height justify-center>
    <v-row justify="center">
      <v-btn
        fab
        v-bind="attrs"
        color="primary"
        v-on="on"
        @click="$router.go(-1)"
      >
        <v-icon>mdi-arrow-left</v-icon>
      </v-btn>
    </v-row>
  </v-container>
</template>

<script>
import Vue from 'vue'
import Component from 'vue-class-component'

@Component
export default class HowToPlay extends Vue {}
</script>
