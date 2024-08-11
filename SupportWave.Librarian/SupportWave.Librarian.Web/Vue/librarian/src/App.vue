<template>
  <div id="app" class="container mt-5">
    <BookList />
  </div>
</template>

<script>
import BookList from './components/BookList.vue';

export default {
  name: 'App',
  components: {
    BookList
  }
}
</script>

<style>
body {
  background-color: #f8f9fa;
}
</style>
