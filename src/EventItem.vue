<template>
    <b-card no-body >
        <h4 v-b-toggle="event.id" class="card-title">{{event.id}}</h4>
        <b-collapse :id="event.id">
            <pre><code class="hljs json">{{ event }}</code></pre>
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