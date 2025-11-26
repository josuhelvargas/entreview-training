package com.example.app;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;//Se utiliza para acceder a las propiedades definidas en el arfhcivo properties.

public class App {
    public static void main(String[] args) {
        System.out.println("App iniciando...");
        Properties p = new Properties();
        try (InputStream is = App.class.getClassLoader().getResourceAsStream("config.properties")) { //obtencion de info del archivo de propiedades
            if (is != null) {
                p.load(is);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        String message = p.getProperty("app.message", "Mensaje por defecto");
        String env = p.getProperty("app.env", "unknown");
        System.out.printf("Entorno: %s%nMensaje: %s%n", env, message);
    }
}

//Se define propeidad en resources/config.properties -> se declara el valor en el pom.xml profiles>profile>properties>app.message
