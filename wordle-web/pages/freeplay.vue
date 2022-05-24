<template>
  <v-container fluid fill-height>
    <v-row justify="center">
      <v-container v-if="!isLoaded">
        <v-card loading>
          <v-card-title class="justify-center">
            You're being exploited for ad revenue, please standby...
          </v-card-title>
          <PrerollAd />
        </v-card>
      </v-container>
    </v-row>

    <v-container v-if="isLoaded">
      <v-row justify="center">
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-container>
              <v-row justify="center">
                <v-btn
                  color="primary"
                  x-small
                  nuxt
                  to="/"
                  fab
                  v-bind="attrs"
                  v-on="on"
                >
                  <v-icon>mdi-home</v-icon>
                </v-btn>
              </v-row>
            </v-container>
          </template>
          <span> Go Home </span>
        </v-tooltip>
      </v-row>

      <v-row justify="center" class="ma-8">
        <v-dialog v-model="dialog" justify-end persistent max-width="600px">
          <template #activator="{ on, attrs }">
            <v-btn color="primary" dark v-bind="attrs" v-on="on">
              {{ playerName }}
            </v-btn>
          </template>
          <v-card>
            <v-card-text>
              <v-text-field
                v-model="playerName"
                type="text"
                placeholder="Guest"
              ></v-text-field>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="dialog = false">
                Close
              </v-btn>
              <v-btn
                color="blue darken-1"
                text
                @click=";(dialog = false), setUserName(playerName)"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-row>

      <v-row justify="center">
        <v-img
          contain
          src="Freeplay !wordle.png"
          class="center"
          style="width: 40rem; height: 8rem"
        />
      </v-row>

      <v-row justify="center">
        <p><v-icon>mdi-timer</v-icon> {{ displayTimer() }}</p>
      </v-row>

      <v-row justify="center">
        <v-alert
          v-if="wordleGame.gameOver"
          width="20rem"
          :type="gameResult.type"
        >
          {{ gameResult.text }}
          <v-btn class="ml-2" @click="resetGame"> Play Again? </v-btn>
        </v-alert>
      </v-row>

      <v-row justify="center">
        <game-board :wordleGame="wordleGame" />
      </v-row>

      <v-row justify="center">
        <keyboard :wordleGame="wordleGame" />
      </v-row>
    </v-container>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { WordsService } from '~/scripts/wordsService'
import { GameState, WordleGame } from '~/scripts/wordleGame'
import KeyBoard from '@/components/keyboard.vue'
import GameBoard from '@/components/game-board.vue'
import { Word } from '~/scripts/word'

@Component({ components: { KeyBoard, GameBoard } })
export default class DailyGame extends Vue {
  // ? need this for closing button
  dialog: boolean = false
  playerName: string = ''
  timeInSeconds: number = 0
  startTime: number = 0
  endTime: number = 0
  intervalID: any
  word: string = 'abcde'
  wordleGame: WordleGame = new WordleGame(this.word!)
  isLoaded: boolean = false

  mounted() {
    setTimeout(() => {
      this.isLoaded = true
      // this.word = this.getWordToday()
      console.log('on mount word = ' + this.word)
    }, 500)
    this.retrieveUserName()
    setTimeout(() => this.startTimer(), 500) // delay is because of ad loading
  }

  resetGame() {
    this.getRandomWord()
    this.timeInSeconds = 0
    this.startTimer()
  }

  get gameResult() {
    this.stopTimer()
    this.timeInSeconds = Math.floor(this.endTime - this.startTime)
    if (this.wordleGame.state === GameState.Won) {
      if (
        this.playerName.toLocaleLowerCase() !== 'guest' &&
        this.playerName !== ''
      ) {
        this.endGameSave()
      } else {
        this.dialog = true
      }
      return { type: 'success', text: 'You won! :^)' }
    }
    if (this.wordleGame.state === GameState.Lost) {
      return {
        type: 'error',
        text: `You lost... :^( The word was ${this.word}`,
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

  retrieveUserName() {
    const userName = localStorage.getItem('userName')
    if (userName == null) {
      this.playerName = 'Guest'
    } else {
      this.playerName = userName
    }
  }

  setUserName(userName: string) {
    localStorage.setItem('userName', userName)
    if (this.wordleGame.state === GameState.Won) {
      this.endGameSave()
    }
  }

  startTimer() {
    this.startTime = Date.now() / 1000
    this.intervalID = setInterval(this.updateTimer, 1000)
  }

  updateTimer() {
    this.timeInSeconds = Math.floor(Date.now() / 1000 - this.startTime)
  }

  stopTimer() {
    this.endTime = Date.now() / 1000
    clearInterval(this.intervalID)
  }

  displayTimer() {
    let text = `${
      this.timeInSeconds / 60 / 60 > 1
        ? Math.floor(this.timeInSeconds / 60 / 60) + ':'
        : ''
    }`
    text += `${
      Math.floor((this.timeInSeconds / 60) % 60) < 10
        ? '0' + Math.floor((this.timeInSeconds / 60) % 60)
        : Math.floor((this.timeInSeconds / 60) % 60)
    }:`
    text += `${
      Math.floor(this.timeInSeconds % 60) < 10
        ? '0' + Math.floor(this.timeInSeconds % 60)
        : Math.floor(this.timeInSeconds % 60)
    }`
    return text
  }

  endGameSave() {
    this.$axios.post('/api/Players', {
      name: this.playerName,
      attempts: this.wordleGame.words.length,
      seconds: this.timeInSeconds,
    })
  }

  getRandomWord() {
    let tempString = 'blank'
    this.$axios
      .get<string>('/api/DateWord/GetWordByDate', {
        params: {
            // 1008 total possible days here
          year: this.getRandomInt(2021,2023),
          month: this.getRandomInt(1,12),
          day: this.getRandomInt(1,28),
        },
      })
      .then((response) => {
        tempString = response.data
        console.log('tempString in .then: ' + tempString)
        this.word = response.data
        this.wordleGame = new WordleGame(response.data)
        return response.data
      })
  }

  beforeMount() {
    this.getRandomWord()
  }

  getRandomInt(min: number, max: number): number {
    min = Math.ceil(min)
    max = Math.floor(max)
    return Math.floor(Math.random() * (max - min + 1)) + min
  }
}
</script>
