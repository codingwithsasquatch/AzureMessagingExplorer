<template>
    <b-button @click="auth('microsoft')">Microsoft</b-button>
</template>

<script>
export default {
  data() {
    return {

    }
  }, methods: {
    auth(network) {
      const hello = this.hello;
      hello(network).login().then(() => {
        const authRes = hello(network).getAuthResponse();
        /*
          performs operations using the token from authRes
        */
        hello(network).api('me').then(function (json) {
          const profile = json;
          /*
            performs operations using the user info from profile
          */
        });
      })
    }
  }
}
</script>

<style scoped></style>