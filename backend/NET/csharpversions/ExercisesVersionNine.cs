using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

record Alumno{
    public string Nombre { get; init; }
    public int Edad { get; init; }
}

record ValidationObject<T>(T Value, List<string> Errors);

var alumnoValido = new ValidationObject <Alumno>(
    new Alumno { Nombre = "Ana", Edad = 20 },
    new List<string>()
);

var alumnoValido2 = alumnoValido with {
    Errors = alumnoValido.Errors.Append("Edad no válida").ToList()
};


bool EsFinDeSemana(DayOfWeek dia) => 
    dia is DayOfWeek.Saturday or DayOfWeek.Sunday;


List<int> numeros = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
