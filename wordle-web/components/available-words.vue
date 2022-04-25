<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { WordsService } from '~/scripts/wordsService'
import { Word } from '~/scripts/word'

@Component
export default class AvailableWords extends Vue {
  @Prop({ required: true })
  wordsService!: WordsService

  word!: Word

  vaildWords(): string[] {
    return WordsService.availableWords(this.word.text)
  }

  validWordCount(): number {
    return WordsService.availableWords(this.word.text).length
  }
}
</script>
