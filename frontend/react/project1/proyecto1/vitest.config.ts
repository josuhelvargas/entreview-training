import { defineConfig } from 'vitest/config';
import react from '@vitejs/plugin-react'


export default defineConfig({
 plugins: [react()],
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],//antes de correr olos test ejecuta este archivo
    globals: true, // opcional, si quieres usar describe/it/expect sin importarlos
  },
});