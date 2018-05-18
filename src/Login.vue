<template>
    <b-button @click="auth('microsoft')">Microsoft</b-button>
</template>

<script>


export default {
  data() {
    return {

    }
  }, props: {
    helloJs: {}
  }, methods: {
    auth(provider) {
      this.$auth.authenticate(provider).then(function () {
        // Execute application logic after successful social authentication
      })
    }
  }
}
</script>

<style scoped></style>