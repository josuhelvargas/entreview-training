//Antes de java 8 el modelo de desarrollo con java erea suammente imperativo y tenai mucho boilerplate.

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

public class Java8Example {
    public static void main(String[] args) {
        List<String> list = Arrays.asList("Apple", "Banana", "Avocado", "Orange");
        List<String> result = new ArrayList<>();

        for (String s : list) {
            if (s.startsWith("A")) {
                result.add(s.toUpperCase());
            }

            // No tenia forma de ahcer lambdas entonces se usaban interfaces
            // Collections.sort(list, new Comparator<String>() {
            //     @Override
            //     public int compare(String s1, String s2) {
            //         return s1.compareTo(s2);
            //     }
            // });
        }
    }
}




// Java 5 infoturdjo Future, ConcurrentHashMap
// APIs antiguas y dolorosas de usar : Date, Calendar, Collections, IOs





// 6. JVM antes de Java 8
// 🔸 PermGen (Permanent Generation)
// Contenía:
// Metadatos de clases
// Pools de strings
// Tamaño fijo → errores frecuentes: