using EspacioPersonaje;
using System;
using System.Collections.Generic;
using System.Text.Json;
// See https://aka.ms/new-console-template for more information

PersonajesJson guardarPersonajes = new PersonajesJson(); 
string nombreArchivo = "Personajes.json";
List<Personaje> listaPersonajes; 
if (guardarPersonajes.Existe(nombreArchivo)) //verifico si el archivo existe y tiene datos
{
    //si existe, cargo una lista con los datos del archivo
    System.Console.WriteLine("----------------------EXISTE------------------------");
    listaPersonajes = guardarPersonajes.LeerPersonajes(nombreArchivo);
}
else
{
    System.Console.WriteLine("----------------------NO EXISTE------------------------");
    //si no existe, genero 10 personajes y los guardo
    FabricaDePersonajes fp = new FabricaDePersonajes();
    Personaje nuevo;
    listaPersonajes = new List<Personaje>();
    for (int i = 0; i < 10; i++)
    {
        nuevo = fp.generarPersonaje();
        listaPersonajes.Add(nuevo);
    }
    guardarPersonajes.GuardarPersonajes(listaPersonajes,nombreArchivo);
}

mostrarPersonajes(listaPersonajes);











/*----------------- METODOS -------------------*/

static void mostrarPersonajes(List<Personaje> lista)
{
    foreach (var item in lista)
    {
        item.mostrarPersonaje();
        System.Console.WriteLine("#############");
    }
}