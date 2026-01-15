type ApiResponse<T> = | {status: "success", data: unknown[], errors:unknown, code: number} 
                  | {status: "error",data: unknown[], errors:unknown, code: number} 
                  |{status: "loading", data: unknown[], errors:[], code: number} 

// 🔹 Qué demuestra un candidato que las usa
// ✅ Entiende modelado de estados
// ✅ Piensa en correctitud, no solo en compilar
// ✅ Sabe usar TS como herramienta de diseño, no solo de tipos


// 🟥 Nivel 3: Deep Readonly (inmutabilidad real)
// type DeepReadonly<T> =
//   T extends (...args: any[]) => any
//     ? T
//     : T extends readonly (infer R)[]
//       ? readonly DeepReadonly<R>[]
//       : T extends object
//         ? { readonly [K in keyof T]: DeepReadonly<T[K]> }
//         : T;


// Uso:

// const u: DeepReadonly<User> = { ... };

// u.profile.age = 31; // ❌ ahora sí

// 🎯 Qué evalúa un entrevistador aquí

// No que memorices el tipo, sino que:

// Sepas que Readonly<T> no es profundo

// Sepas cuándo importa (estado, cache, store)

// Entiendas el trade-off DX vs seguridad

// 📌 Respuesta senior:

// “Uso deep readonly solo donde el costo lo justifica, como state global o cache.”

// 2️⃣ 🔥 Ejercicio de entrevista: romper un mal diseño
// ❌ Diseño ingenuo (MUY común)
// type ApiResponse<T> = {
//   loading?: boolean;
//   data?: T;
//   error?: string;
// };

// 🧨 Problemas reales (rompamos esto)
// Estado imposible #1
// {
//   loading: true,
//   data: { id: "1" }
// }


// 👉 ¿Spinner o data?

// Estado imposible #2
// {
//   data: { id: "1" },
//   error: "Unauthorized"
// }


// 👉 ¿Éxito o error?

// Estado imposible #3
// {}


// 👉 ¿Qué renderizas?

// 🔥 Tarea de entrevista

// “Refactoriza este tipo para que estos estados no puedan existir.”

// ✅ Solución correcta (discriminated union)
// type ApiError = {
//   message: string;
//   code?: string;
// };

// type ApiResponse<T> =
//   | { readonly status: "loading" }
//   | { readonly status: "success"; readonly data: Readonly<T> }
//   | { readonly status: "error"; readonly errors: readonly ApiError[] };

// 🎯 Qué demuestra el candidato

// Modela estados, no flags

// Usa TS para prevenir bugs

// Piensa en UI + backend

// 3️⃣ 🔥 Comparar solución ingenua vs correcta (bugs reales)
// ❌ Ingenua en React
// if (res.loading) return <Spinner />;
// if (res.error) return <Error />;
// return <User data={res.data} />;

// Bugs:

// res.data puede ser undefined

// Spinner + data

// Error + data

// Cannot read property 'x' of undefined

// ✅ Correcta con discriminated union
// switch (res.status) {
//   case "loading":
//     return <Spinner />;
//   case "error":
//     return <Error errors={res.errors} />;
//   case "success":
//     return <User data={res.data} />;
// }

// Beneficios:

// Exhaustividad

// No undefined

// UI consistente

// TS te avisa si falta un caso

// 📌 Esto es pensamiento senior real.

// 4️⃣ 🔥 Integración con React Query / TanStack Query

// TanStack Query ya aplica estos principios, por eso encaja tan bien.

// 🧠 Estado interno de React Query
// status: "pending" | "success" | "error"
// data?: T
// error?: unknown


// 👉 Es una discriminated union implícita.

// Uso correcto
// const { data, error, status } = useQuery({
//   queryKey: ["user"],
//   queryFn: fetchUser,
// });

// if (status === "pending") return <Spinner />;
// if (status === "error") return <Error error={error} />;
// return <User data={data} />;

// 🚫 Error común de juniors
// if (isLoading) { ... }
// if (data) { ... }
// if (error) { ... }


// 👉 Tres flags = estados imposibles

// 🧠 Versión mental correcta

// “React Query ya modela estados, yo solo los consumo correctamente.”

// Bonus: Forzar inmutabilidad con React Query
// useQuery<DeepReadonly<User>>(...)


// Evita mutar datos cacheados:

// data.name = "x"; // ❌

// 🧠 Resumen final (mentalidad de entrevista)
// Tema	Qué demuestra
// readonly vs shallow vs deep	Conocimiento real
// Refactor de mal diseño	Pensamiento crítico
// Evitar estados imposibles	Diseño correcto
// React Query integration	Experiencia en producción
// 🎯 Frase que identifica a un buen senior

// “Modelo estados de forma que los bugs no puedan existir, y dejo que el compilador me proteja.”