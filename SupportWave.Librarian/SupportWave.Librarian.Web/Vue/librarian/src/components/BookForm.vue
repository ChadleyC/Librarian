<template>
    <div class="container">
      <h2>{{ isEditMode ? 'Edit Book' : 'Add Book' }}</h2>
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="title">Title</label>
          <input type="text" class="form-control" id="title" v-model="book.title" required>
        </div>
        <div class="form-group">
          <label for="author">Author</label>
          <input type="text" class="form-control" id="author" v-model="book.author" required>
        </div>
        <div class="form-group">
          <label for="publishedDate">Published Date</label>
          <input type="date" class="form-control" id="publishedDate" v-model="book.publishedDate" required>
        </div>
        <div class="form-group">
          <label for="isbn">ISBN</label>
          <input type="text" class="form-control" id="isbn" v-model="book.isbn" required>
        </div>
        <button type="submit" class="btn btn-primary">{{ isEditMode ? 'Update' : 'Add' }} Book</button>
        <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
      </form>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      bookData: {
        type: Object,
        default: () => ({
          title: '',
          author: '',
          publishedDate: '',
          isbn: ''
        })
      },
      isEditMode: {
        type: Boolean,
        default: false
      }
    },
    data() {
      return {
        book: { ...this.bookData }
      }
    },
    watch: {
      bookData(newVal) {
        this.book = { ...newVal };
      }
    },
    methods: {
      handleSubmit() {
        this.$emit('save', { ...this.book });
        this.book = { title: '', author: '', publishedDate: '', isbn: '' };
      }
    }
  }
  </script>
  