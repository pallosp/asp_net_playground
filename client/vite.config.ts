import react from '@vitejs/plugin-react-swc'
import {defineConfig} from 'vite'

const backend = 'http://localhost:5252'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': backend,
    },
  },
})
