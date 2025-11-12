UI = f(state)
react s eapoya de al idea de que la UI es una funcion del estado.
cuando el edo cambia tambien  cambia la ui


React no manipula el DOM directamente.
Crea una representacion en memoria ( Virttual DOM)
DOM anterior vs DOM nuevo → genera el mínimo cambio necesario.\



Un componente wse re-renderiza cuando cambian : N
sus props
su edo interno 
su context 
su componente padre cambia (dependiendo optimizaciones)


Hooks 

useState



useEffect 
llamafdaas http 
suscripciones 
listeners
timers
logs
actualizar algo fuera del react tree.



Performance y reedenrizacion 
Técnica   	            Para qué sirve
React.memo	            Evitar renders innecesarios en componentes
useCallback	            Evitar recrear funciones
useMemo	                Evitar recálculos costosos
react-virtualized / 
tanstack-virtual	        Evitar renderizar listas gigantes
Suspense + concurrent mode	    Cargas progresivas fluidas

En lugar de usar useEffect y un fetch es mejor usar useQuery
Cache

Refetch automático

Retry automático

Manejo de loading/error listo

Integración con UI real de flujo\





Cual es la diferencia entre props y estados? 












Arquitectura Senior

Un Senior en React debe:

Entender por qué usar un router y cuándo

Diseñar componentes reusables

Crear design systems

Evitar props-drilling usando Context de forma correcta

Conocer tanstack query para estado remoto

Saber qué NO meter en Redux

Saber cuándo usar Signals (si migras después