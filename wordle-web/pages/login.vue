<template>
  <v-container fluid fill-height>
    <v-container>
      <v-card class="pa-5">
        <v-card-title>Login</v-card-title>
        <div>
          <v-col>
            <v-text-field v-model="email" label="Email" required outlined>
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="password"
              label="Password"
              required
              outlined
              :type="passwordVisible ? 'text' : 'password'"
              :append-icon="passwordVisible ? 'mdi-eye' : 'mdi-eye-off'"
              @click:append="passwordVisible = !passwordVisible"
            ></v-text-field>
          </v-col>

          <v-spacer></v-spacer>
          <v-btn @click="login()" class="mb-3"> Login </v-btn>
          <v-spacer></v-spacer>

          <v-alert :type="loginResult.type">
            {{ loginResult.text }}
          </v-alert>
        </div>
      </v-card>
    </v-container>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { JWT } from '~/scripts/jwt'
@Component({})
export default class Login extends Vue {
  email: string = ''
  password: string = ''
  passwordVisible: boolean = false
  dialog: boolean = false
  loginSuccess = false
  loginLoading = true

  login() {
    sessionStorage.clear()
    this.loginLoading = true
    this.$axios
      .post('Token/GetToken', {
        username: this.email,
        password: this.password,
      })
      .then((result) => {
        if (result.status == 200) {
          JWT.setToken(result.data.token, this.$axios)
        //   this.$axios.defaults.headers.common.Authorization =
        //     'Bearer ' + result.data.token
          sessionStorage.setItem('userAccount', this.email)
          this.loginSuccess = true
          this.loginLoading = false
        } else {
          this.loginSuccess = false
          this.loginLoading = false
        }
      })
      .catch((error) => {
        this.loginSuccess = false
        this.loginLoading = false
        console.log('error thrown from login')
      })
  }

  get loginResult() {
    if (this.loginLoading == false) {
      if (this.loginSuccess == true) {
        return { type: 'success', text: 'Login Successful' }
      }

      if (this.loginSuccess == false) {
        return { type: 'error', text: 'Login Failed' }
      }
    }
    return { type: '', text: '' }
  }
}
</script>
