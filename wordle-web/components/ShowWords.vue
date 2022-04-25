<template>
  <div>
    <v-row justify="center">
      <v-dialog v-model="dialog" scrollable max-width="300px">
        <template #activator="{ on, attrs }">
          <v-btn
            :disabled="wordleGame.gameOver"
            v-bind="attrs"
            @click="validWords()"
            v-on="on"
          >
            Show Available Words - {{ count }}
          </v-btn>
        </template>
        <v-card>
          <v-card-title>Available Words - {{ count }}</v-card-title>
          <v-divider></v-divider>
          <v-card-text style="height: 300px">
            <v-radio-group v-model="dialog" column>
              <div v-for="item in items" :key="item">
                <v-radio
                  :label="item"
                  :value="item"
                  @click="setInput(item)"
                ></v-radio>
              </div>
            </v-radio-group>
          </v-card-text>
          <v-divider></v-divider>
          <v-card-actions>
            <v-btn text @click="dialog = false"> Close </v-btn>
            <v-btn color="primary" text @click="inputWord(availableWordsInput)">
              Select
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { WordleGame } from '~/scripts/wordleGame'
import { WordsService } from '~/scripts/wordsService'

@Component({})
export default class ShowWords extends Vue {
  @Prop({ required: true })
  wordleGame!: WordleGame

  @Prop({ required: true })
  readonly count!: number

  dialog = false
  availableWordsInput = 'words'
  regexWord = '?????'
  items: string[] = []

  toggleDialog() {
    this.dialog = !this.dialog
  }

  clearLetters() {
    while (this.wordleGame.currentWord.length > 0) {
      this.wordleGame.currentWord.removeLetter()
    }
  }

  inputWord(word: string) {
    this.clearLetters()
    for (let i = 0; i < word.length; i++) {
      this.wordleGame.currentWord.addLetter(word.charAt(i))
    }
    this.dialog = false
  }

  setInput(text: string) {
    this.availableWordsInput = text
  }

  validWords() {
    this.regexWord = this.wordleGame.currentWord.text
    this.items = WordsService.availableWords(this.regexWord)
  }
}
</script>
