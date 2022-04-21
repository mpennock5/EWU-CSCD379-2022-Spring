import { WordsService } from '@/scripts/wordsService'

describe('Word Service', () => {
  test('Get a word', () => {
    const word = WordsService.getRandomWord()
    expect(word).not.toBeNull()
    expect(word).toHaveLength(5)
    expect(word).not.toHaveLength(4)
  })

  test('Words are private', () => {
    expect((WordsService as any).words).toBeUndefined()
  })

  test('available word filter', () => {
    var guess = "ac???"
    const words = WordsService.availableWords(guess)
    expect(words).not.toBeNull()
    expect(words).toHaveLength(3)
    expect(words).toContain("acorn")
  })
})
