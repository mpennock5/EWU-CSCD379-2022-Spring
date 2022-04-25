<template>
  <img class="W-logo" alt="WLogo" src="/w-logo-noBG.png" />
</template>

<style>
.W-logo {
  height: 1em;
  width: 1em;
  transform: rotateY(560deg);
  animation: turn 3.5s ease-out forwards 1s;
}

@keyframes turn {
  100% {
    transform: rotateY(0deg);
  }
}
</style>
