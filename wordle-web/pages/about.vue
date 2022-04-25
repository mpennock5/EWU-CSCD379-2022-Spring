<template>
  <v-container fluid fill-height justify-center>
    <v-row class="mb-10 mt-5" justify="center">
      <v-tooltip bottom>
        <template #activator="{ on, attrs }">
          <v-btn color="primary" nuxt to="/" fab v-bind="attrs" v-on="on">
            <v-icon> mdi-home </v-icon>
          </v-btn>
        </template>
        <span> Go Home </span>
      </v-tooltip>
    </v-row>

    <v-row justify="center">
      <v-card width="70%">
        <v-expansion-panels accordion>
          <v-expansion-panel>
            <v-expansion-panel-header color="primary"
              >About this Class</v-expansion-panel-header
            >
            <v-expansion-panel-content class="mt-5">
              <h3>CSCD 379 .NET WEB APPLICATION DEVELOPMENT.</h3>
              <h4>4 Credits</h4>
              <br />
              <p></p>
              <h4>Pre-requisites:</h4>
              CSCD 327 with a grade ≥C+. (DESN 368 or XHTML/HTML knowledge
              (highly recommended) or permission of the instructor.) This course
              examines the fundamental principles and techniques associated with
              the development of multi-tier web applications, using the .NET
              Framework. Topics include web standards, portability, and
              usability. Programming projects are required.
            </v-expansion-panel-content>
          </v-expansion-panel>
          <v-expansion-panel>
            <v-expansion-panel-header color="primary"
              >Meg!</v-expansion-panel-header
            >
            <v-expansion-panel-content class="mt-5">
              Check out her website
              <v-btn href="https://www.meghanwoodford.com/" color="primary"
                >here</v-btn
              >
            </v-expansion-panel-content>
          </v-expansion-panel>
        </v-expansion-panels>
      </v-card>
    </v-row>
  </v-container>
</template>

<script></script>
