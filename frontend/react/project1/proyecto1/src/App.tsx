// App.tsx
import React, {
  useState,
  useEffect,
  useMemo,
  useDeferredValue,
  useCallback,
} from 'react';

// --- Tipos de datos ---
type Pokemon = {
  name: string;
  url: string;
};

type PokeApiResponse = {
  count: number;
  results: Pokemon[];
};

const PAGE_SIZE = 12;

// --- Componente principal ---
const App: React.FC = () => {
  // Estado básico de la carga de datos
  const [pokemons, setPokemons] = useState<Pokemon[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  // Estado de UI
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [searchTerm, setSearchTerm] = useState<string>("");

  // useEffect -> trae datos cuando el componente se monta
  useEffect(() => {
    const fetchPokemons = async () => {
      try {
        setIsLoading(true);
        setError(null);

        // Simula la lectura de DB: aquí usamos la PokeAPI
        const response = await fetch(
          "https://pokeapi.co/api/v2/pokemon?limit=200&offset=0"
        );

        if (!response.ok) {
          throw new Error("Error al cargar los pokemons");
        }

        const data: PokeApiResponse = await response.json();
        setPokemons(data.results);
      } catch (err: any) {
        setError(err.message ?? "Error inesperado");
      } finally {
        setIsLoading(false);
      }
    };

    fetchPokemons();
  }, []);

  // useDeferredValue -> “suaviza” el filtro para que la UI no se trabe
  const deferredSearchTerm = useDeferredValue(searchTerm);

  // useMemo -> calcula los filtrados solo cuando cambian pokemons o el término de búsqueda
  const filteredPokemons = useMemo(() => {
    const term = deferredSearchTerm.trim().toLowerCase();
    if (!term) return pokemons;

    return pokemons.filter((p) => p.name.toLowerCase().includes(term));
  }, [pokemons, deferredSearchTerm]);

  // Cálculos de paginación
  const totalPages = useMemo(() => {
    return Math.max(1, Math.ceil(filteredPokemons.length / PAGE_SIZE));
  }, [filteredPokemons.length]);

  // Asegura que currentPage nunca quede fuera de rango
  useEffect(() => {
    setCurrentPage((prev) => {
      if (prev > totalPages) return totalPages;
      if (prev < 1) return 1;
      return prev;
    });
  }, [totalPages]);

  // useMemo -> calcula la “rebanada” de pokemons para la página actual
  const paginatedPokemons = useMemo(() => {
    const startIndex = (currentPage - 1) * PAGE_SIZE;
    const endIndex = startIndex + PAGE_SIZE;
    return filteredPokemons.slice(startIndex, endIndex);
  }, [filteredPokemons, currentPage]);

  // useCallback -> handler estable para cambiar de página
  const handlePageChange = useCallback((page: number) => {
    setCurrentPage(page);
  }, []);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
    setCurrentPage(1); // resetea a la primera página al buscar
  };

  return (
    <div style={styles.app}>
      <h1>Pokédex (React + Hooks)</h1>

      {/* Buscador */}
      <div style={styles.searchContainer}>
        <label htmlFor="search">Buscar Pokémon:</label>
        <input
          id="search"
          type="text"
          value={searchTerm}
          onChange={handleSearchChange}
          placeholder="Ej. pikachu"
          style={styles.searchInput}
        />
      </div>

      {/* Estado de carga / error */}
      {isLoading && <p>Cargando pokemons...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {/* Lista paginada */}
      {!isLoading && !error && (
        <>
          <PokemonGrid pokemons={paginatedPokemons} />

          <Pagination
            currentPage={currentPage}
            totalPages={totalPages}
            onPageChange={handlePageChange}
          />
        </>
      )}
    </div>
  );
};

// --- Componente para mostrar la grilla de pokemons ---
interface PokemonGridProps {
  pokemons: Pokemon[];
}

const PokemonGrid: React.FC<PokemonGridProps> = ({ pokemons }) => {
  if (pokemons.length === 0) {
    return <p>No hay pokemons para mostrar.</p>;
  }

  return (
    <div style={styles.grid}>
      {pokemons.map((pokemon) => (
        <PokemonCard key={pokemon.name} pokemon={pokemon} />
      ))}
    </div>
  );
};

// --- Tarjeta individual de Pokemon ---
interface PokemonCardProps {
  pokemon: Pokemon;
}

const PokemonCard: React.FC<PokemonCardProps> = ({ pokemon }) => {
  // Podemos derivar el ID a partir de la URL, solo como ejemplo
  const id = useMemo(() => {
    const parts = pokemon.url.split("/").filter(Boolean);
    return parts[parts.length - 1];
  }, [pokemon.url]);

  const imageUrl = `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`;

  return (
    <article style={styles.card}>
      <img src={imageUrl} alt={pokemon.name} style={styles.image} />
      <h3 style={styles.cardTitle}>{pokemon.name}</h3>
      <p style={styles.cardSubtitle}>ID: {id}</p>
    </article>
  );
};

// --- Componente de paginación ---
interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onPageChange,
}) => {
  if (totalPages <= 1) return null;

  const canGoPrev = currentPage > 1;
  const canGoNext = currentPage < totalPages;

  const handlePrev = () => {
    if (canGoPrev) onPageChange(currentPage - 1);
  };

  const handleNext = () => {
    if (canGoNext) onPageChange(currentPage + 1);
  };

  // Renderiza un pequeño rango de páginas (ej. actual ± 2)
  const pagesToShow = getVisiblePages(currentPage, totalPages);

  return (
    <div style={styles.paginationContainer}>
      <button onClick={handlePrev} disabled={!canGoPrev}>
        {"<"} Prev
      </button>

      {pagesToShow.map((page, index) =>
        typeof page === "number" ? (
          <button
            key={index}
            onClick={() => onPageChange(page)}
            disabled={page === currentPage}
            style={{
              fontWeight: page === currentPage ? "bold" : "normal",
            }}
          >
            {page}
          </button>
        ) : (
          <span key={index} style={{ padding: "0 4px" }}>
            ...
          </span>
        )
      )}

      <button onClick={handleNext} disabled={!canGoNext}>
        Next {">"}
      </button>
    </div>
  );
};

// Devuelve un array de páginas y "..." para grandes cantidades
function getVisiblePages(current: number, total: number): (number | string)[] {
  const pages: (number | string)[] = [];
  const delta = 2;

  const start = Math.max(1, current - delta);
  const end = Math.min(total, current + delta);

  if (start > 1) {
    pages.push(1);
    if (start > 2) pages.push("...");
  }

  for (let i = start; i <= end; i++) {
    pages.push(i);
  }

  if (end < total) {
    if (end < total - 1) pages.push("...");
    pages.push(total);
  }

  return pages;
}

// --- Estilos inline simples para no depender de CSS externo ---
const styles: Record<string, React.CSSProperties> = {
  app: {
    maxWidth: "960px",
    margin: "0 auto",
    padding: "16px",
    fontFamily: "system-ui, sans-serif",
    color: "#f1f1f1",
    backgroundColor: "#20232a",
    minHeight: "100vh",
  },
  searchContainer: {
    marginBottom: "16px",
    display: "flex",
    gap: "8px",
    alignItems: "center",
  },
  searchInput: {
    flex: 1,
    padding: "8px",
    borderRadius: "4px",
    border: "1px solid #555",
  },
  grid: {
    display: "grid",
    gridTemplateColumns: "repeat(auto-fill, minmax(120px, 1fr))",
    gap: "12px",
    marginBottom: "16px",
  },
  card: {
    backgroundColor: "#282c34",
    borderRadius: "8px",
    padding: "8px",
    textAlign: "center",
    boxShadow: "0 2px 4px rgba(0,0,0,0.2)",
  },
  image: {
    width: "80px",
    height: "80px",
    imageRendering: "pixelated",
  },
  cardTitle: {
    margin: "8px 0 4px",
    textTransform: "capitalize",
  },
  cardSubtitle: {
    margin: 0,
    fontSize: "0.8rem",
    color: "#aaa",
  },
  paginationContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    gap: "4px",
    marginBottom: "16px",
  },
};

export default App;
