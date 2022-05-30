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
          src="Daily !wordle.png"
          class="center"
          style="width: 40rem; height: 8rem"
        />
      </v-row>

      <v-row justify="center">
        <v-icon>mdi-timer</v-icon>
        <div v-text="timeField" class="mt-1 ml-1"></div>
      </v-row>

      <v-row justify="center">
        <v-alert
          v-if="wordleGame.gameOver"
          width="20rem"
          :type="gameResult.type"
        >
          <v-row>
            {{ gameResult.text }}
          </v-row>
          <v-row>
            <v-btn
              class="ma-1"
              @click="endGameSave"
              :disabled="hasLost"
            >
              Submit Score?
            </v-btn>
            <v-btn class="ma-1" nuxt to="/freeplay"> Go To Freeplay? </v-btn>
          </v-row>

          <v-row justify="center"
            ><v-banner
              v-model="submittedSuccess"
              rounded

            >
              Your Score has been submitted!
            </v-banner></v-row
          >
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
import { Stopwatch } from 'ts-stopwatch'

@Component({ components: { KeyBoard, GameBoard } })
export default class DailyGame extends Vue {
  // ? need this for closing button
  dialog: boolean = false
  playerName: string = ''
  timeField: string = '1'
  word: string = 'abcde'
  wordleGame: WordleGame = new WordleGame(this.word!)
  isLoaded: boolean = false
  hasLost: boolean = true
  submittedSuccess: boolean = false
  stopwatch = new Stopwatch()

  mounted() {
    this.startTime()
    this.showTime()
    setTimeout(() => {
      this.isLoaded = true
    }, 500)
    this.retrieveUserName()
    console.log(this.word)
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
  showTime() {
    this.timeField = this.formatTime()
    setTimeout(() => {
      this.showTime()
    }, 50)
  }
  resetTime() {
    this.stopwatch.reset()
  }
  getTimeSeconds() {
    return this.stopwatch.getTime() / 1000
  }

  formatTime(): string {
    let time = this.stopwatch.getTime()
    // let ftms = ((time % 1000) / 10).toFixed(0)
    let fts = Math.floor((time / 1000) % 60)
    let ftm = Math.floor((time / (1000 * 60)) % 60)
    let fth = Math.floor((time / (1000 * 60 * 60)) % 24)

    let Shours = fth < 10 ? '0' + fth : fth
    let Sminutes = ftm < 10 ? '0' + ftm : ftm
    let Sseconds = fts < 10 ? '0' + fts : fts

    let text = Shours + ':' + Sminutes + ':' + Sseconds //+ ':' + ftms
    return text
    //adding milliseconds makes the timer bounce around on the page
  }

  get gameResult() {
    if (this.wordleGame.state === GameState.Won) {
      this.stopTime()
      this.hasLost = false
      if (
        this.playerName.toLocaleLowerCase() !== 'guest' &&
        this.playerName !== ''
      ) {
      } else {
        this.dialog = true
      }
      return { type: 'success', text: 'You won! :^)' }
    }
    if (this.wordleGame.state === GameState.Lost) {
      this.stopTime()
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

  endGameSave() {
    //guard statement, only submit score once
    if (this.submittedSuccess) return

    let today = new Date()
    this.$axios.post('/api/DateWord/Post', {
      Year: today.getFullYear(),
      Month: today.getMonth() + 1,
      Day: today.getDate(),
      Player: localStorage.getItem('userName'),
      Score: this.wordleGame.words.length,
      TimeSeconds: Math.floor(this.getTimeSeconds()),
    })
    this.hasLost = true //disable button
    this.submittedSuccess = true
  }

  getDailyWord() {
    let today = new Date()
    let tempString = 'blank'
    this.$axios
      .get<string>('/api/DateWord/GetWordByDate', {
        params: {
          year: today.getFullYear(),
          month: today.getMonth() + 1,
          day: today.getDate(),
        },
      })
      .then((response) => {
        tempString = response.data
        this.word = response.data
        this.wordleGame = new WordleGame(response.data)
        return response.data
      })
  }

  beforeMount() {
    this.getDailyWord()
  }
}
</script>
