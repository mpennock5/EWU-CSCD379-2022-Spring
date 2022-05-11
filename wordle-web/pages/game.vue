<template>
  <v-container fluid fill-height justify-center>
    <v-row justify="center" class="mt-2">
      <v-col>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-btn color="primary" nuxt to="/" fab v-bind="attrs" v-on="on">
              <v-icon> mdi-home </v-icon>
            </v-btn>
          </template>
          <span> Go Home </span>
        </v-tooltip>
      </v-col>

      <v-col>
        <v-card flat color="transparent" class="mt-0 mb-0 pt-0 pb-0">
          <v-card-text class="text-h3 font-weight-black text-center ma-0 pa-0">
            !Wordle
          </v-card-text>
        </v-card>
      </v-col>

      <v-col>
        <div class="float-right">
          <v-dialog v-model="dialog" width="550">
            <template v-slot:activator="{ on, attrs }">
              <v-btn color="primary" dark v-bind="attrs" v-on="on" height="4em">
                <v-row>
                  <v-col>
                    <h5 class="grey--text text--lighten-1">Username</h5>
                    <br />
                    <h3>{{ username }}</h3>
                  </v-col>
                </v-row>
              </v-btn>
            </template>

            <v-card>
              <v-card-text>
                <v-form v-on:submit.prevent class="px-3">
                  <v-card-title>
                    Enter your name for the leaderboard!!!
                  </v-card-title>

                  <v-text-field
                    :rules="[rules.required, rules.counter]"
                    label="Username"
                    v-model="username"
                    outlined
                    counter
                    maxlength="25"
                  ></v-text-field>

                  <v-divider></v-divider>

                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                      color="primary"
                      text
                      @click="
                        toggleDialog()
                        nameUpdate()
                        evaluateName()
                      "
                    >
                      Save
                    </v-btn>
                  </v-card-actions>
                </v-form>
              </v-card-text>
            </v-card>
          </v-dialog>
        </div>
      </v-col>
    </v-row>

    <!-- <v-row justify="center"> Game Time: {{ gameTimeSeconds }} </v-row> -->

    <v-alert
      v-if="wordleGame.gameOver"
      prominent
      max-width="80%"
      :type="gameResult.type"
    >
      <v-row justify="center">
        <v-col>
          <p>{{ gameResult.text }} <br /></p>
        </v-col>
      </v-row>
      <v-row>
        <v-btn class="ml-2" :disabled="wordleGame.hasLost" @click="submitScore">
          Submit Score
        </v-btn>
        <v-btn class="ml-2" @click="resetGame"> Play Again </v-btn></v-row
      >
    </v-alert>

    <v-row justify="center"
      ><v-banner v-model="submittedSuccess" color="success" rounded single-line>
        Your Score has been submitted!
      </v-banner></v-row
    >

    <v-row justify="center">
      <game-board :wordleGame="wordleGame" />
    </v-row>
    <v-row justify="center">
      <keyboard :wordleGame="wordleGame" />
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator'
import { WordsService } from '~/scripts/wordsService'
import { GameState, WordleGame } from '~/scripts/wordleGame'
import KeyBoard from '@/components/keyboard.vue'
import GameBoard from '@/components/game-board.vue'
import { Word } from '~/scripts/word'
import { Stopwatch } from 'ts-stopwatch'

@Component({ components: { KeyBoard, GameBoard } })
export default class Game extends Vue {
  word: string = WordsService.getRandomWord()
  wordleGame = new WordleGame(this.word)
  username: string = this.evaluateName()
  dialog: boolean = false
  submittedSuccess: boolean = false
  gameTimeSeconds: number = 0
  stopwatch = new Stopwatch()
  rules = {
    required: (value: any) => !!value || 'Required.',
    counter: (value: string | any[]) =>
      value.length <= 25 || 'Max 25 characters',
  }

  mounted() {
    this.startTime()
  }

  startTime() {
    this.stopwatch.start()
  }

  stopTime() {
    this.stopwatch.stop()
  }

  getTime() {
    return this.stopwatch.getTime()
  }

  resetTime() {
    this.stopwatch.reset()
  }

  getTimeSeconds() {
    return this.getTime() / 1000
  }

  resetGame() {
    this.submittedSuccess = false
    this.word = WordsService.getRandomWord()
    this.wordleGame = new WordleGame(this.word)
    this.resetTime()
    this.startTime()
  }

  submitScore() {
    //guard statement, only submit score once
    if (this.submittedSuccess) return

    if (!this.hasUniqueLocalName()) {
      this.toggleDialog()
      this.waitUntilDialogClosed()
    } else {
      this.$axios.post('/api/PlayersLeaderBoard', {
        name: this.username,
        attempts: this.wordleGame.currentGuess,
        seconds: this.getTimeSeconds().toFixed(0), //removes after numbers decimal
      })
      this.submittedSuccess = true
    }
  }

  waitUntilDialogClosed() {
    if (this.dialog) {
      //we want it to be false
      setTimeout(this.waitUntilDialogClosed, 1000) //wait 50 millisecnds then recheck
      return
    }
    this.$axios.post('/api/PlayersLeaderBoard', {
      name: this.username,
      attempts: this.wordleGame.currentGuess,
      seconds: this.getTimeSeconds().toFixed(0), //removes after numbers decimal
    })
  }

  get gameResult() {
    if (this.wordleGame.state === GameState.Won) {
      this.stopTime()
      return {
        type: 'success',
        text: 'You won the game in ' + this.getTimeSeconds() + ' seconds!',
      }
    }
    if (this.wordleGame.state === GameState.Lost) {
      this.stopTime()
      return {
        type: 'error',
        text: 'You lost... :^( The word was ' + this.word,
      }
    }
    return { type: '', text: '' }
  }

  getLetter(row: number, index: number) {
    const word: Word = this.wordleGame.words[row - 1]
    if (word !== undefined) {
      return word.letters[index - 1]?.char ?? ''
    }
    return ''
  }

  hasUniqueLocalName(): boolean {
    if (
      localStorage.getItem('UsernameLocal') == null ||
      localStorage.getItem('UsernameLocal') == 'Guest'
    ) {
      return false
    } else return true
  }

  evaluateName(): string {
    if (localStorage.getItem('UsernameLocal') == null) {
      return 'Guest'
    }
    return localStorage.getItem('UsernameLocal') + ''
  }

  toggleDialog() {
    this.dialog = !this.dialog
    console.log('the word is: ' + this.word) //for cheating AKA time saving
  }

  nameUpdate() {
    localStorage.setItem('UsernameLocal', this.username)
  }
}
</script>
