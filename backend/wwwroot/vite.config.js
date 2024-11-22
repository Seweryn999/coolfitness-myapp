import { defineConfig } from "vite";

export default defineConfig({
  root: "src", // Określa katalog źródłowy
  build: {
    outDir: "../dist", // Gdzie mają trafiać pliki po zbudowaniu
  },
});
