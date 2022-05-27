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
    <v-row justify="center">
      <v-card>
        <v-card-title>
          <h2>Rules of !Wordle</h2>
        </v-card-title>

        <v-card-text>
          <p>
            1. The player has to guess the word in 6 attempts or less. <br><br>
            2. Every word entered should be in the word list. <br><br>
            3. If a letter is in the word and in the correct position, the color will turn green. <br><br>
            4. If a letter is in the word but placed incorrectly, it will turn yellow. <br><br>
            5. If a letter is not in the word it will turn gray. <br><br>
            6. Letters can be used multiple times. <br><br>
            7. The ? can be entered as a wildcard character to show the list of available words.
          </p>
        </v-card-text>
      </v-card>
    </v-row>
  </v-container>
</template>

<script>
import Vue from 'vue'
import Component from 'vue-class-component'

@Component
export default class HowToPlay extends Vue {}
</script>
