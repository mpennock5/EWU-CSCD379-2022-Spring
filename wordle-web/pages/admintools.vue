<template>
  <v-container fluid fill-height justify-center>
    <v-card :loading="dataInTransit" width="80%">
      <v-card-title>
        <h1>Game Words</h1>
      </v-card-title>
      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="searchTerm"
              append-icon="mdi-magnify"
              label="Search"
              single-line
              hide-details
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
        </v-row>
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
  currentPageSize: number = 10
  currentPage: number = 1
  pageSizeOptions = [10, 25, 50, 100]
  page: any = []

  //methods
  searchWords(){
    this.dataInTransit = true;
    this.$axios
      .get('/api/Word/GetWordsPerPage', {
        params: {
          pageSize: this.currentPageSize,
          currentPage: this.currentPage,
          query: this.searchTerm,
        },
      })
      .then((response) => {
        this.page = response.data
      })
  }
}
</script>
