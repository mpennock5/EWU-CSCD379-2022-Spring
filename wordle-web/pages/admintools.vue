<template>
  <v-container fluid fill-height justify-center>
    <v-card :loading="dataInTransit" width="80%">
      <v-card-title>
        <v-spacer></v-spacer>
        <h1>!Wordle Editor</h1>
        <v-spacer></v-spacer>
      </v-card-title>
      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="searchTerm"
              append-icon="mdi-magnify"
              label="Search"
              outlined
            ></v-text-field>
          </v-col>

          <v-col>
            <v-select
              :items="pageSizeOptions"
              outlined
              v-model="currentPageSize"
              @input="searchWords"
              label="Words per Page"
            ></v-select>
          </v-col>

          <v-col class="fill-height">
            <div class="d-flex">
              <v-text-field
                label="Add"
                v-model="wordToAdd"
                outlined
              ></v-text-field>
              <v-btn elevation="0" icon @click="addWord()" class="mt-3">
                <v-icon x-large>mdi-plus-circle-outline</v-icon></v-btn
              >
            </div>
          </v-col>
        </v-row>

        <v-divider></v-divider>
        <!-- 
        section containing the words, common flag,
        delete and change common flag button for each word 
        -->
        <v-simple-table>
          <thead>
            <tr>
              <th>Word</th>
              <th style="text-align: center">Common</th>
              <th style="text-align: right">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(word, wordId) in words" :key="wordId">
              <td>{{ word.item1 }}</td>
              <td style="text-align: center">{{ word.item2}}</td>
              <td style="text-align: right">
              <v-btn>common+</v-btn>
              <v-btn>common-</v-btn>
              <v-btn>delete</v-btn>
              </td>
            </tr>
          </tbody>
        </v-simple-table>

        <v-divider></v-divider>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class IndexPage extends Vue {
  dataInTransit: boolean = false
  searchTerm: string = ''
  wordToAdd: string = ''
  currentPageSize: number = 10
  currentPage: number = 1
  maxPage: number = 1
  pageSizeOptions = [10, 25, 50, 100]
  totalWords: number = 0
  pageDTO: any = []
  words: any = []

  //startup
  mounted() {
    this.searchWords()
  }

  //methods
  searchWords() {
    this.dataInTransit = true
    this.$axios
      .get('/api/Word/GetWordsPerPage', {
        params: {
          pageSize: this.currentPageSize,
          currentPage: this.currentPage,
          query: this.searchTerm,
        },
      })
      .then((response) => {
        this.pageDTO = response.data
        this.totalWords = this.pageDTO.TotalItems
        this.currentPage = this.pageDTO.CurrentPage
        this.maxPage = this.pageDTO.MaxPages
        this.currentPage = this.pageDTO.PageSize
        this.words = this.pageDTO.returnable

        this.dataInTransit = false
      })
  }

  addWord() {
    //check permissions
    //if valid submit new word to controller
    //if response is favorable flash success popup
    //else flash fail popup and a reason
    //re-run search
    this.searchWords()
  }

  deleteWord() {
    //check permissions
    //if valid submit delete request to controller
    //if response is favorable flash success popup
    //else flash fail popup and a reason
    //re-run search
    this.searchWords()
  }
}
</script>

<!-- Page DTO
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int MaxPages { get; set; }
    public int PageSize { get; set; }
    public List<Tuple<string, bool>>? returnable { get; set; }
-->
