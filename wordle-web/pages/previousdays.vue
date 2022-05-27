<template>
  <v-container fluid fill-height justify-center>
    <v-card>
      <v-card-title class="display-3 justify-center">
        Leader Board
      </v-card-title>
      <v-card-text class="text-center">
        {{ title }}
      </v-card-text>
      <v-card-text>
        <v-simple-table>
          <thead>
            <tr>
              <th style="text-align: center">Date</th>
              <th style="text-align: center">Total Plays</th>
              <th style="text-align: center">Avg. Score</th>
              <th style="text-align: center">Avg. Time</th>
              <th style="text-align: center">Already Played?</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(game, date) in returnData" :key="date">
              <td>{{ game.date }}</td>

              <td style="text-align: center">
                {{ game.totalPlays }}
              </td>

              <td style="text-align: center">
                {{ game.averageScore }}
              </td>

              <td style="text-align: center">
                {{ game.averageTime }}
              </td>

              <td style="text-align: center">
                {{ game.hasPlayed }}
              </td>
            </tr>
          </tbody>
        </v-simple-table>
      </v-card-text>

      <v-card-actions>
        <v-btn color="primary" @click="GetLast10DailyWords">
          Get recent Words
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({})
export default class leaderboard extends Vue {
  returnData: any = []
  title: string = ''

  created() {
    this.GetLast10DailyWords()
  }

  GetLast10DailyWords() {
    this.title = 'Previous Games!'
    this.$axios
      .get('/api/DateWord/GetLast10DailyWords', {
        params: { name: localStorage.getItem('userName') },
      })
      .then((response) => {
        this.returnData = response.data
      })
  }
}
</script>
