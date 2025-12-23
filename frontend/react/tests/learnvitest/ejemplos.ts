// uí tienes un ejemplo completo(componente + módulos + test) que ilustra AAA, cuándo usar vi.mock vs vi.spyOn vs vi.fn, async, RTL, y timers.Después te explico el código línea por línea(por secciones).

//   Escenario: Un componente React < LoginForm /> llama a una API login().

// Si funciona: muestra “Welcome” y llama track()(analytics).

// Si falla: muestra error y hace console.error.

//   Además: el submit está “debounced” 300ms para ilustrar timers.

// 1) Código del módulo externo(API) — src / api / auth.ts
// src/api/auth.ts
export type LoginResult = { ok: true; userName: string } | { ok: false; message: string };

export async function login(email: string, password: string): Promise<LoginResult> {
  // En la vida real esto llamaría a fetch/axios.
  // Aquí lo dejamos "real", pero en tests se mockea con vi.mock.
  return { ok: false, message: "Not implemented" };
}









// 2) Código del módulo externo(Analytics) — src / analytics.ts
// src/analytics.ts
export function track(eventName: string, payload?: Record<string, unknown>) {
  // En la vida real enviaría a un servicio (Segment/Adobe/etc.)
  // En tests lo mockeamos para verificar "se llamó con X".
  void eventName;
  void payload;
}








// 3) Componente React — src / LoginForm.tsx
// src/LoginForm.tsx
import React, { useState } from "react";
import { login } from "./api/auth";
import { track } from "./analytics";

export function LoginForm({ onSuccess }: { onSuccess?: (userName: string) => void }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [status, setStatus] = useState<"idle" | "loading" | "success" | "error">("idle");
  const [message, setMessage] = useState<string>("");

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    setStatus("loading");
    setMessage("");

    // Debounce artificial para ilustrar timers en tests:
    await new Promise((r) => setTimeout(r, 300));

    const result = await login(email, password);

    if (result.ok) {
      track("login_success", { userName: result.userName });
      setStatus("success");
      setMessage(`Welcome, ${result.userName}`);
      onSuccess?.(result.userName);
    } else {
      console.error("Login failed:", result.message);
      setStatus("error");
      setMessage(result.message);
    }
  }

  return (
    <form onSubmit= { handleSubmit } aria - label="login-form" >
      <label>
      Email
      < input
  aria - label="email"
  value = { email }
  onChange = {(e) => setEmail(e.target.value)
}
        />
  </label>

  <label>
Password
  < input
aria - label="password"
type = "password"
value = { password }
onChange = {(e) => setPassword(e.target.value)}
        />
  </label>

  < button type = "submit" > Sign in </button>

{ status === "loading" && <p role="status" > Loading...</p> }
{ message && <p role="alert" > { message } </p> }
</form>
  );
}











// 4) Test con Vitest + RTL — src / LoginForm.test.tsx

// Requiere entorno jsdom y @testing-library / react, @testing - library / jest - dom.

// src/LoginForm.test.tsx
import React from "react";
import { describe, it, expect, vi, afterEach } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";

// 1) vi.mock: Se usa para dependencias externas importadas (API/analytics)
vi.mock("./api/auth", () => ({
  login: vi.fn(),
}));

vi.mock("./analytics", () => ({
  track: vi.fn(),
}));

import { LoginForm } from "./LoginForm";
import { login } from "./api/auth";
import { track } from "./analytics";

describe("<LoginForm />", () => {
  afterEach(() => {
    // Limpia llamadas entre tests (sin perder implementación mockeada)
    vi.clearAllMocks();
    // Si usas spies (vi.spyOn), esto restaura implementaciones originales
    vi.restoreAllMocks();
    // Si activas fake timers en algún test, buena práctica volver a real
    vi.useRealTimers();
  });

  it("renders success flow: calls login, tracks event, shows welcome, calls onSuccess", async () => {
    // ---------- Arrange ----------
    // A) vi.fn: callback recibido por props (dependencia inyectada)
    const onSuccess = vi.fn();

    // B) Controlas la implementación del mock de módulo (login)
    vi.mocked(login).mockResolvedValue({ ok: true, userName: "Josue" });

    render(<LoginForm onSuccess={ onSuccess } />);

    const user = userEvent.setup();

    // ---------- Act ----------
    await user.type(screen.getByLabelText("email"), "josue@mail.com");
    await user.type(screen.getByLabelText("password"), "123456");
    await user.click(screen.getByRole("button", { name: /sign in/i }));

    // Debounce: el componente espera 300ms antes de llamar login.
    // Para no esperar tiempo real, usamos fake timers.
    vi.useFakeTimers();
    // Importante: "avanzamos el reloj" para resolver el setTimeout(300)
    await vi.advanceTimersByTimeAsync(300);

    // ---------- Assert ----------
    // 1) Interacción con dependencia externa (API)
    expect(login).toHaveBeenCalledTimes(1);
    expect(login).toHaveBeenCalledWith("josue@mail.com", "123456");

    // 2) Interacción con dependencia externa (analytics)
    expect(track).toHaveBeenCalledTimes(1);
    expect(track).toHaveBeenCalledWith("login_success", { userName: "Josue" });

    // 3) Resultado observable (UI)
    expect(screen.getByRole("alert")).toHaveTextContent("Welcome, Josue");

    // 4) Callback inyectado (vi.fn)
    expect(onSuccess).toHaveBeenCalledTimes(1);
    expect(onSuccess).toHaveBeenCalledWith("Josue");
  });

  it("renders error flow: shows error message and logs to console.error", async () => {
    // ---------- Arrange ----------
    vi.mocked(login).mockResolvedValue({ ok: false, message: "Invalid credentials" });

    // vi.spyOn: lo usamos para interceptar un método real (console.error)
    const consoleSpy = vi.spyOn(console, "error").mockImplementation(() => { });

    render(<LoginForm />);

    const user = userEvent.setup();

    // ---------- Act ----------
    await user.type(screen.getByLabelText("email"), "bad@mail.com");
    await user.type(screen.getByLabelText("password"), "wrong");
    await user.click(screen.getByRole("button", { name: /sign in/i }));

    vi.useFakeTimers();
    await vi.advanceTimersByTimeAsync(300);

    // ---------- Assert ----------
    // Verificas UI
    expect(screen.getByRole("alert")).toHaveTextContent("Invalid credentials");

    // Verificas side effect (log)
    expect(consoleSpy).toHaveBeenCalledTimes(1);
    expect(consoleSpy).toHaveBeenCalledWith("Login failed:", "Invalid credentials");
  });
});







// Explicación detallada(por “puntos anteriores”)
// A) ¿Dónde está AAA ?

//   En ambos tests:

// Arrange: defines inputs, mocks y render.

//   Act: userEvent(type / click) + avanzar timers.

//     Assert: esperas llamadas y verificas UI / side - effects.

// Esto hace el test legible y sistemático.

//   B) ¿Cuándo usar vi.mock ?

//     Usas vi.mock("./api/auth") y vi.mock("./analytics") porque:

// Son dependencias externas(IO / side effects / no deterministas).

// Se importan como módulos en el SUT.

// Necesitas control total del resultado(ok: true / false) y verificar llamadas.

//   Regla: vi.mock = reemplazar módulo completo(o su export ) por algo controlado.

//     C) ¿Cuándo usar vi.fn ?

// const onSuccess = vi.fn() porque:

//   Es un callback que el componente recibe por props.

//   No es un “módulo importado”, sino una dependencia inyectada.

//   Quieres verificar: “¿se llamó con el username correcto?”

//   Regla: vi.fn = dependencia inyectada o función simple mockeable.

//   D) ¿Cuándo usar vi.spyOn?

//   vi.spyOn(console, "error") porque:

// console.error ya existe(es real).

// No quieres mockear “todo console”.

// Solo quieres interceptar ese método y confirmar que se llamó.

//   Regla: vi.spyOn(obj, method) = observar / interceptar una función real puntual.

//     E) ¿Cómo funciona el async aquí ?

//       login es async y el submit tiene un await setTimeout(300).

// Por eso:

// el click dispara handleSubmit

// handleSubmit entra en estado loading

// espera 300ms(debounce)

// luego llama login

// luego actualiza UI

// En tests, si no controlas el tiempo, sería más lento y / o flaky.

//   F) ¿Por qué fake timers(vi.useFakeTimers) ?

//     Porque el componente espera 300ms reales.

// Con fake timers:

// el test corre rápido

// no depende del reloj real

// controlas exactamente el “momento” en que ocurre la llamada

// Regla: usa fake timers cuando tu SUT usa setTimeout / setInterval / debounce / throttle.

//   G) Limpieza sistemática

// En afterEach hacemos:

// vi.clearAllMocks() para que cada test arranque limpio(conteo de llamadas).

//   vi.restoreAllMocks() para revertir spyOn.

//     vi.useRealTimers() por si algún test deja timers falsos activos.

// Esto evita contaminación y flakiness.

//   H) ¿Dónde entra React Testing Library ?

//     Vitest solo:

// corre tests

// ofrece mocks

// ofrece expect

// RTL:

// monta el componente(render)

// te da queries(screen.getBy...)

// te ayuda a probar “como usuario” (userEvent)

// Regla: en UI, aserta lo observable(texto, roles, estado visible), no implementación interna.