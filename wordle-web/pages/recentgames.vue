<template>
  <v-container fluid fill-height justify-center>
    <v-card>
      <v-card-title class="display-3 justify-center" style="word-break: normal;">
        Recent Games
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
            <tr
              v-for="(game, date) in returnData"
              :key="date"
              @click="linkTo(game.date)"
            >
              <td>{{ displayDate(game.date) }}</td>

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
        <p></p>
        <v-row justify="center">
          <v-btn color="primary" @click="GetLast10DailyWords">
            Update Recent Games
          </v-btn>
        </v-row>
        <p></p>
      </v-card-text>
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

  linkTo(date: any) {
    let convertedDate = this.convertDate(date)
    localStorage.setItem('Year', convertedDate.getFullYear().toString())
    localStorage.setItem('Month', (convertedDate.getMonth() + 1).toString())
    localStorage.setItem('Day', convertedDate.getDate().toString())
    this.$router.push({ name: 'freeplay' })
  }
  convertDate(date: any) {
    let d = new Date(date)
    return d
  }

  displayDate(date: any) {
    let d = this.convertDate(date)
    return d.toLocaleDateString('en-US')
  }

  GetLast10DailyWords() {
    this.title = 'Click a day below to play that game'
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
