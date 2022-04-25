<template>
  <v-card
    class="my-5 pa-5"
    style="background: radial-gradient(circle, rgba(186,186,186,1) 0%, rgba(55,55,55,1) 100%)"
  >
    <v-row v-for="(charRow, i) in chars" :key="i" no-gutters justify="center">
      <v-col v-for="char in charRow" :key="char" cols="1">
        <v-container class="text-center">
          <v-btn
            :color="letterColor(char)"
            :disabled="wordleGame.gameOver"
            elevation="24"
            @click="setLetter(char)"
          >
            {{ char }}
          </v-btn>
        </v-container>
      </v-col>
    </v-row>
    <v-row>
      <v-col class="d-flex justify-start">
        <v-btn :disabled="wordleGame.gameOver" @click="guessWord">
          Guess
        </v-btn>
      </v-col>
      <v-col class="d-flex justify-center">
        <ShowWords :wordleGame="wordleGame" :count="watchNumber" />
      </v-col>
      <v-col class="d-flex justify-end">
        <v-btn :disabled="wordleGame.gameOver" icon @click="removeLetter">
          <v-icon>mdi-backspace</v-icon>
        </v-btn>
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
// import { useSound } from '@vueuse/sound'
import ShowWords from './ShowWords.vue'
import { Letter, LetterStatus } from '~/scripts/letter'
import { WordleGame } from '~/scripts/wordleGame'
import { WordsService } from '~/scripts/wordsService'

@Component
export default class KeyBoard extends Vue {
  @Prop({ required: true })
  wordleGame!: WordleGame

  showWords!: ShowWords

  watchProperty: boolean = true
  watchNumber: number = 0
  // buttonSound = useSound('~/assets/button.mp3')

  playSound() {
    // this doesnt work
    // this.buttonSound.play()
  }

  @Watch('watchProperty')
  onPropertyChanged() {
    // play the button clicked sound here
    this.playSound()
    this.watchNumber = WordsService.availableWords(
      this.wordleGame.currentWord.text
    ).length
  }

  update() {
    this.watchProperty = !this.watchProperty
  }

  chars = [
    ['q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p'],
    ['a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l'],
    ['z', 'x', 'c', 'v', 'b', 'n', 'm', '?'],
  ]

  setLetter(char: string) {
    this.wordleGame.currentWord.addLetter(char)
    this.update()
  }

  removeLetter() {
    this.wordleGame.currentWord.removeLetter()
    this.update()
  }

  guessWord() {
    if (
      this.wordleGame.currentWord.length ===
      this.wordleGame.currentWord.maxLetters
    ) {
      this.wordleGame.submitWord()
    }
  }

  letterColor(char: string): string {
    if (this.wordleGame.correctChars.includes(char)) {
      return Letter.getColorCode(LetterStatus.Correct)
    }
    if (this.wordleGame.wrongPlaceChars.includes(char)) {
      return Letter.getColorCode(LetterStatus.WrongPlace)
    }
    if (this.wordleGame.wrongChars.includes(char)) {
      return Letter.getColorCode(LetterStatus.Wrong)
    }

    return Letter.getColorCode(LetterStatus.Unknown)
  }
}
</script>
