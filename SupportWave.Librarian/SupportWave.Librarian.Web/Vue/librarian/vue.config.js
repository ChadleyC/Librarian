const { defineConfig } = require('@vue/cli-service')
// module.exports = defineConfig({
//   transpileDependencies: true
// })

module.exports = {
  transpileDependencies: true,
  devServer: {
    proxy: {
      '^/Book': {
        target: 'http://localhost:5238/api',
        ws: true,
        changeOrigin: true
      },
    }
  }
}