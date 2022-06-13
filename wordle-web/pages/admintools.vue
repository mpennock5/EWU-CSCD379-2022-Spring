<template>
  <v-container fluid fill-height justify-center>
    <v-card :loading="dataInTransit" width="80%">
      <v-card-title>
        <v-spacer></v-spacer>
        <h1>!Wordle Editor</h1>
        <v-spacer></v-spacer>
      </v-card-title>


      <!-- action success or fail response -->
      <v-row align-content="center" justify="center">
        <v-alert v-if="actionVisible">
          {{ actionResult.text }}
          <v-btn
            class="pa-0 ma-0"
            color="red darken-2"
            @click="actionVisible = !actionVisible"
          >
            <v-icon>mdi-close-circle</v-icon>
          </v-btn>
        </v-alert>
      </v-row>

      <!-- search and action buttons -->
      <v-card-text class="mt-4">
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
                counter="5"
                min="5"
                max="5"
                :append-icon="'mdi-plus-circle-outline'"
                @click:append="addWord(wordToAdd)"
              ></v-text-field>
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
              <td style="text-align: center">{{ word.item2 }}</td>
              <td style="text-align: right">
                <v-btn @click="commonFlag(word.item1, true)">common+</v-btn>
                <v-btn @click="commonFlag(word.item1, false)">common-</v-btn>
                <v-btn @click="deleteWord(word.item1)">delete</v-btn>
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
import { Component, Vue, Watch } from 'vue-property-decorator'

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
  loginAccount = sessionStorage.getItem('userAccount')
  actionVisible: boolean = false
  actionSuccess: boolean = true

  //startup
  mounted() {
    this.searchWords()
    this.checkAuthorizedAddRemove()
    this.checkAuthorizedCommon()
  }

  @Watch('searchTerm')
  searchTermUpdate() {
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

  addWord(word: string) {
    //check permissions
    //if valid submit new word to controller
    if (sessionStorage.getItem('AddRemove') == 'true') {
      this.$axios
        .post('/api/Word/AddWord', word)
        .then((result) => {
          if (result.status == 200) {
            this.actionSuccess = true
            this.actionVisible = true
            this.searchWords()
          } else {
            this.actionSuccess = false
            this.actionVisible = true
          }
        })
        .catch((error) => {
          console.log(error)
          this.actionSuccess = false
          this.actionVisible = true
        })
    } else {
      this.actionSuccess = false
      this.actionVisible = true
    }
    //if response is favorable flash success popup
    //else flash fail popup and a reason
    //re-run search
  }

  deleteWord(word: string) {
    //check permissions
    //if valid submit delete request to controller
    if (sessionStorage.getItem('AddRemove') == 'true') {
      this.$axios
        .post('/api/Word/DeleteWord', word)
        .then((result) => {
          if (result.status == 200) {
            this.actionSuccess = true
            this.actionVisible = true
            this.searchWords()
          } else {
            this.actionSuccess = false
            this.actionVisible = true
          }
        })
        .catch((error) => {
          console.log(error)
          this.actionSuccess = false
          this.actionVisible = true
        })
    } else {
      this.actionSuccess = false
      this.actionVisible = true
    }
    //if response is favorable flash success popup
    //else flash fail popup and a reason
    //re-run search
  }

  // {
  // params: {
  //   pageSize: this.currentPageSize,
  //   currentPage: this.currentPage,
  //   query: this.searchTerm,
  // }}

  commonFlag(word: string, common: boolean) {
    if (sessionStorage.getItem('common')) {
      //change flag
      this.$axios
        .post('/api/Word/SetCommonWord', null, {
          params: { target: word, common },
        })
        .then((result) => {
          if (result.status == 200) {
            this.actionSuccess = true
            this.actionVisible = true
            this.searchWords()
          }
        })
        .catch((error) => {
          console.log(error)
          this.actionSuccess = false
          this.actionVisible = true
        })
    } else {
      this.actionSuccess = false
      this.actionVisible = true
    }
  }

  checkAuthorizedCommon() {
    //check permissions to modify common flag (logged in)
    this.$axios
      .get('Token/test')
      .then((result) => {
        if (result.status == 200) {
          sessionStorage.setItem('common', 'true')
        }
      })
      .catch((error) => {
        console.log(error)
        return false
      })
    sessionStorage.setItem('common', 'false')
  }

  checkAuthorizedAddRemove() {
    //check permissions to add/remove words (21+ & MOTU)
    this.$axios
      .get('Token/testrandomadmin')
      .then((result) => {
        if (result.status == 200) {
          sessionStorage.setItem('AddRemove', 'true')
        }
      })
      .catch((error) => {
        console.log(error)
        return false
      })
    sessionStorage.setItem('AddRemove', 'false')
  }

  get actionResult() {
    if (this.actionVisible == true) {
      if (this.actionSuccess == true) {
        return { type: 'success', text: 'Success' }
      }

      if (this.actionSuccess == false) {
        return { type: 'error', text: 'Unauthorized' }
      }
    }
    return { type: '', text: '' }
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
