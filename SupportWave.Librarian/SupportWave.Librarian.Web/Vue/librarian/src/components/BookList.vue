<template>
    <div class="container">
      <h2>Books</h2>
      <button class="btn btn-primary mb-3" @click="showAddBookForm">Add Book</button>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Title</th>
            <th>Author</th>
            <th>Published Date</th>
            <th>ISBN</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="book in books" :key="book.id">
            <td>{{ book.title }}</td>
            <td>{{ book.author }}</td>
            <td>{{ book.publishedDate }}</td>
            <td>{{ book.isbn }}</td>
            <td>
              <button class="btn btn-info btn-sm" @click="editBook(book)">Edit</button>
              <button class="btn btn-danger btn-sm" @click="deleteBook(book.id)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
      <BookForm
        v-if="isFormVisible"
        :bookData="currentBook"
        :isEditMode="isEditMode"
        @save="saveBook"
        @cancel="hideForm"
      />
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  import BookForm from './BookForm.vue';
  axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
  export default {
    components: {
      BookForm
    },
    data() {
      return {
        books: [],
        currentBook: null,
        isFormVisible: false,
        isEditMode: false
      }
    },
    created() {
      this.fetchBooks();
    },
    methods: {
      async fetchBooks() {
        try {
          const response = await axios.get('/Book');
          // eslint-disable-next-line
          debugger;
          this.books = response.data;
        } catch (error) {
          console.error('Error fetching books:', error);
        }
      },
      showAddBookForm() {
        this.currentBook = { title: '', author: '', publishedDate: '', isbn: '' };
        this.isFormVisible = true;
        this.isEditMode = false;
      },
      editBook(book) {
        this.currentBook = { ...book };
        this.isFormVisible = true;
        this.isEditMode = true;
      },
      async saveBook(book) {
        if (this.isEditMode) {
          try {
            await axios.put(`/Book/${book.id}`, book);
          } catch (error) {
            console.error('Error updating book:', error);
          }
        } else {
          try {
            const response = await axios.post('/api/Book', book);
            this.books.push(response.data);
          } catch (error) {
            console.error('Error creating book:', error);
          }
        }
        this.hideForm();
        this.fetchBooks();
      },
      async deleteBook(id) {
        try {
          await axios.delete(`/api/Book/${id}`);
          this.fetchBooks();
        } catch (error) {
          console.error('Error deleting book:', error);
        }
      },
      hideForm() {
        this.isFormVisible = false;
      }
    }
  }
  </script>
  