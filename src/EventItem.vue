<template>
    <b-card no-body class="mt-2">
        <h4 v-b-toggle="event.id" class="card-title">#{{event.id}} - {{event.time}}</h4>
        <b-collapse :id="event.id">
            <pre class="prettyprint lang-js"><code>{{ event.data }}</code></pre>
        </b-collapse>
    </b-card>
</template>

<script>

export default {
  props: ['event'],
  data() {
    return {

    }
  }
}
</script>

<style scoped></style>