/* Utility types   */
// Idea general

//     Los utility types son tipos genéricos ya construidos por TypeScript para:

//     hacer propiedades opcionales

//     volverlas obligatorias

//     congelarlas como solo lectura

//     escoger solo algunas

//     excluir algunas

//     construir objetos tipo diccionario

//     extraer tipos de funciones y clases

// Piensa en ellos como funciones, pero para tipos.


//Partial: convierte propiedades en opcionales. 

type User = {
    id:number, 
    name: string, 
    active: boolean
};

type PartialUser = Partial<User>;

//equivale a
type PartialUserOther = {
    id?:number, 
    name?:string, 
    active?:boolean
}

//Required<T> :coinvierte todaas las propiedades en obnligatorias.

type Config = {
  host?: string;
  port?: number;
};

function buildConfig(config: Partial<Config>): Required<Config> {
  return {
    host: config.host ?? "localhost",
    port: config.port ?? 3000
  };
}

const finalConfig = buildConfig({});


// Relación útil: Partial + Required

// Este es un patrón muy común:

// recibes algo parcial

// completas valores por defecto

// devuelves algo 






//Readonly<T> (todfas las propeidades son de xsolo lectura)
type AppState = Readonly<{
  user: string;
  token: string;
}>;

//Pick<T, K> Escoge solo ciertas propeidades de T
type UserEmployee = {
  id: number;
  name: string;
  email: string;
  active: boolean;
};

type UserPreview = Pick<UserEmployee, "id" | "name">;

// ¿Para qué sirve?

// Muy útil cuando:

// necesitas una vista reducida

// quieres enviar menos datos

// modelas DTOs

// reutilizas parte de un tipo grande


// 5) Omit<T, K>
// ¿Qué hace?
// Crea un tipo con todas las propiedades de T, excepto las de K.

type UserAuthenticated = {
  id: number;
  name: string;
  password: string;
};

type SafeUser = Omit<UserAuthenticated, "password">;
// ¿Para qué sirve?

// Súper común para:

// remover campos sensibles

// crear DTOs públicos

// excluir propiedades de UI o backend


// 6) Record<K, T>
// ¿Qué hace?

// Construye un tipo de objeto donde:

// las llaves son K

// los valores son T

// Ejemplo
type Roles = "admin" | "user" | "guest";

type Permissions = Record<Roles, boolean>;

// Resultado:

// type Permissions = {
//   admin: boolean;
//   user: boolean;
//   guest: boolean;
// };

// ¿Para qué sirve?

// Sirve para modelar:

// diccionarios

// mapas simples

// tablas de lookup

// configuraciones por clave



// 7) ReturnType<T>
// ¿Qué hace?

// Extrae el tipo de retorno de una función.

// Ejemplo
function getUser() {
  return { id: 1, name: "Ana" };
}

type UserResponse = ReturnType<typeof getUser>;
// ¿Para qué sirve?

// Muy útil cuando:

// no quieres duplicar tipos

// quieres derivar el tipo desde una función real

// frameworks generan tipos desde funciones factory



// 8) Parameters<T>
// ¿Qué hace?

// Extrae los parámetros de una función como una tupla.

// Ejemplo
function createUser(name: string, age: number, active: boolean) {
  return { name, age, active };
}

type Params = Parameters<typeof createUser>;

// Resultado:

// type Params = [name: string, age: number, active: boolean]

// o conceptualmente:

//type Params = [string, number, boolean]
// ¿Para qué sirve?

// Sirve mucho para:

// wrappers

// decorators

// forwarders

// reusar firmas de funciones






// 9) InstanceType<T>
// ¿Qué hace?

// Extrae el tipo de la instancia creada por una clase.

// Ejemplo
class UserService {
  getUser() {
    return "Ana";
  }
}

type UserServiceInstance = InstanceType<typeof UserService>;

// Resultado:

// type UserServiceInstance = UserService







// resumen:
// Cómo pensar cada utility type mentalmente
// Partial<T>

// “haz todo opcional”

// Required<T>

// “haz todo obligatorio”

// Readonly<T>

// “haz todo solo lectura”

// Pick<T, K>

// “quédate solo con estas propiedades”

// Omit<T, K>

// “quita estas propiedades”

// Record<K, T>

// “crea un objeto indexado por K con valores T”

// ReturnType<T>

// “dime qué devuelve esta función”

// Parameters<T>

// “dime qué recibe esta función”

// InstanceType<T>

// “dime qué tipo produce esta clase al hacer new”