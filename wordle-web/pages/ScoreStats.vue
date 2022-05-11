<template onload="refreshBoard">
  <v-container fluid fill-height justify-center>
    <v-row justify="center">
      <v-tooltip bottom>
        <template #activator="{ on, attrs }">
          <v-btn color="primary" nuxt to="/" fab v-bind="attrs" v-on="on">
            <v-icon> mdi-home </v-icon>
          </v-btn>
        </template>
        <span> Go Home </span>
      </v-tooltip>
    </v-row>
    <v-card>
      <v-card-title>
        <h1 class="display-1">Score Stats</h1>
      </v-card-title>
      <v-card-text>
        <v-simple-table>
          <thead>
            <tr>
              <th>Player Name</th>
              <th>Games Won</th>
              <th>Attempts Per Game</th>
              <th>Seconds Per Game</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(stat, index) in leaderboard" :key="index">
              <td>{{ stat.name }}</td>
              <td>{{ stat.gameCount }}</td>
              <td>{{ stat.averageAttempts }}</td>
              <td>{{ stat.averageSecondsPerGame }}</td>
            </tr>
          </tbody>
        </v-simple-table>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" @click="refreshBoard"> Refresh </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({})
export default class ScoreStats extends Vue {
  stats: any = []
  leaderboard: any = []
  combinedScore: number = 0

  totalScore(numGames: number, avgTries: number) {
    this.combinedScore = numGames * (7 - avgTries)
    return this.combinedScore
  }

  refreshStats() {
    this.$axios.get('/api/ScoreStats').then((response) => {
      this.stats = response.data
    })
  }
  refreshBoard() {
    this.$axios.get('/api/PlayersLeaderBoard').then((response) => {
      this.leaderboard = response.data
    })
  }

  beforeMount() {
    this.refreshBoard()
  }
}
</script>
