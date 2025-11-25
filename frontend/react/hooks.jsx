// Hooks: 2 implementaciones de cada uno + “cómo funciona adentro”

// Nota interna: en cada render React recorre una lista de hooks ligada al Fiber del componente. 
// Cada llamada a hook crea/lee un nodo de esa lista en el mismo orden. Cambiar el orden rompe la correspondencia.

// 1) useState
// A) Implementación típica (paginador)
function Pager({ total }: { total: number }) {
  const [page, setPage] = useState(1);
  const pageSize = 20;

  const next = () => setPage(p => Math.min(p + 1, Math.ceil(total / pageSize)));
  const prev = () => setPage(p => Math.max(p - 1, 1));

  return (
    <div>
      <button onClick={prev}>Prev</button>
      <span>Página {page}</span>
      <button onClick={next}>Next</button>
    </div>
  );
}

//B) Con lazy init + función set (evitar trabajo caro en cada render)
function Chart({ raw }: { raw: number[] }) {
  const [stats, setStats] = useState(() => heavyCompute(raw)); // solo 1er render

  const recompute = () => setStats(prev => ({ ...prev, refreshedAt: Date.now() }));

  return <button onClick={recompute}>Recalcular</button>;
}


// Internals (simple):
// En render phase, React lee el hook.state desde el Fiber.
// setState encola una update en hook.queue con un payload (valor o función).
// En el siguiente render, React reduce la queue (aplica actualizaciones), calcula el nuevo state y si cambia, marca el Fiber para re-render (con su lane / prioridad).



// 2) useEffect
// A) Efecto pasivo para petición HTTP (sin lib)
function Products() {
  const [items, setItems] = useState<string[]>([]);
  useEffect(() => {
    let alive = true;
    fetch('/api/products').then(r => r.json()).then(d => { if (alive) setItems(d); });
    return () => { alive = false; }; // cleanup
  }, []); // solo cuando monta
  return <ul>{items.map(i => <li key={i}>{i}</li>)}</ul>;
}

//B) Sincronizar DOM externo / suscripción
function ResizeWatcher() {
  const [w, setW] = useState(window.innerWidth);
  useEffect(() => {
    const h = () => setW(window.innerWidth);
    window.addEventListener('resize', h);
    return () => window.removeEventListener('resize', h);
  }, []);
  return <div>Width: {w}</div>;
}


// Internals:

// Se registra en render, pero se ejecuta en commit (después de pintar).

// React compara deps (Object.is por índice). Si alguna cambió, agenda el efecto; si no, lo salta.

// Si había cleanup, lo ejecuta antes de correr el nuevo efecto o al desmontar.

// 3) useRef
// A) Guardar un valor mutable que no re-renderiza
function Stopwatch() {
  const start = useRef<number | null>(null);
  const tickId = useRef<number | null>(null);

  const startTimer = () => {
    start.current = Date.now();
    tickId.current = window.setInterval(() => {
      // leer start.current sin provocar renders
    }, 100);
  };
  const stopTimer = () => { if (tickId.current) clearInterval(tickId.current); };

  return <button onClick={startTimer}>Start</button>;
}

//B) Acceso a un DOM node (imperativo)
function AutoFocusInput() {
  const inputRef = useRef<HTMLInputElement>(null);
  useEffect(() => { inputRef.current?.focus(); }, []);
  return <input ref={inputRef} />;
}


// Internals:

// En render, React devuelve el mismo objeto { current } ligado al Fiber: no cambia entre renders.

// Mutar ref.current no causa re-render. Es para “caja fuerte” de cosas mutables.

// 4) useMemo
// A) Derivar lista filtrada costosa
function Search({ items, q }: { items: string[]; q: string }) {
  const result = useMemo(() => expensiveSearch(items, q), [items, q]);
  return <ul>{result.map(x => <li key={x}>{x}</li>)}</ul>;
}

//B) Memorizar columnas (evita recrear objetos en props)
const columns = (t: (k: string) => string) => ([
  { key: 'name', header: t('name') },
  { key: 'price', header: t('price') },
]);

function Grid({ t }: { t: any }) {
  const cols = useMemo(() => columns(t), [t]);
  return <Table columns={cols} />;
}


// Internals:

// Guarda par (depsHash, value) en el nodo de hook. Si deps no cambian, retorna el valor previo sin recomputar.

// No desencadena render por sí mismo; optimiza el trabajo dentro del render.

// 5) useCallback
// A) Evitar re-crear handlers que rompen memo/useEffect
function List({ onSelect }: { onSelect: (id: string) => void }) {
  const [selected, setSelected] = useState<string | null>(null);
  const handleClick = useCallback((id: string) => {
    setSelected(id);
    onSelect(id);
  }, [onSelect]); // estable mientras onSelect no cambie
  // ...
}

//B) Depedencias de un useEffect con funciones
function Poller({ interval }: { interval: number }) {
  const [count, setCount] = useState(0);
  const tick = useCallback(() => setCount(c => c + 1), []);
  useEffect(() => {
    const id = setInterval(tick, interval);
    return () => clearInterval(id);
  }, [tick, interval]);
  return <div>{count}</div>;
}


// Internals:

// Igual que useMemo, pero guarda una función estable si las deps no cambian.

// No evita renders; evita identidad nueva innecesaria en props/dep arrays.

// ¿Qué hook usar en cada caso?

// Paginador

// Mínimo: useState para page y pageSize.

// Si hay reglas complejas: useReducer para transiciones (NEXT, PREV, GOTO).

// Si la página deriva una lista filtrada: useMemo(items, page, pageSize).

// Control de un campo de formulario (input, checkbox)

// Local y simple: useState o con RHF, usar useForm + Controller.

// Validación/refs: useRef para acceder al DOM si hace falta foco.

// Gestionar el estado de un formulario completo

// Complejo (muchos campos, validación, submit): react-hook-form (internamente usa refs + registro, evita renders).

// Sin librería: useReducer (estado como objeto, acciones por campo) + useMemo para reglas derivadas.

// Hacer una llamada HTTP y poblar elementos

// Sencillo: useEffect (fetch) + useState (datos, loading, error).

// Producción: React Query (useQuery) ➜ cache, reintentos, estados, invalidación (mejor DX y consistencia).

// Actualizar UI según selecciones (sin HTTP)

// useState para selección(es).

// useMemo para derivar cálculos (totales, items elegidos) sin trabajo innecesario.

// useCallback para pasar handlers estables a hijos memoizados.

// Notas internas que te hacen “senior”

// Orden de hooks importa: React mapea llamadas a una lista por índice. No los metas dentro de if.

// setState es asíncrono y puede batchear varias updates en el mismo tick (desde React 18, por defecto).

// Comparación de deps en useEffect/useMemo/useCallback es Object.is posicional; arrays/objetos nuevos rompen memorias si no los fijas.

// Refs atraviesan renders sin cambiar identidad; perfectos para temporizadores, abortControllers, medir tiempos, etc.

// useMemo/useCallback no siempre mejoran: sólo cuando evitan trabajo caro o evitan romper memoización en hijos.