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
    //System.Console.WriteLine("----------------------EXISTE------------------------");
    listaPersonajes = guardarPersonajes.LeerPersonajes(nombreArchivo);
}
else
{
    //System.Console.WriteLine("----------------------NO EXISTE------------------------");
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

//mostrarPersonajesCompleto(listaPersonajes);


/*====================== Desarrollo del Gameplay ========================*/
Personaje jugador1,jugador2;

int indice1,indice2;

//Listo los personajes (solo nombre, apodo y tipo)
System.Console.WriteLine("=== Personajes disponibles ===");
mostrarPersonajes(listaPersonajes);


do
{
    System.Console.WriteLine("[ JUGADOR 1]");
    System.Console.WriteLine("Indice del personaje seleccionado: ");
} while (!int.TryParse(Console.ReadLine(),out indice1) || indice1 < 0 || indice1 > listaPersonajes.Count - 1);
//asigno personaje
jugador1 = listaPersonajes[indice1];
System.Console.WriteLine($"[Jugador 1] selecciono a {jugador1.Nombre}");

do
{
    System.Console.WriteLine("[ JUGADOR 2]");
    System.Console.WriteLine("Indice del personaje seleccionado: ");
} while (!int.TryParse(Console.ReadLine(),out indice2) || indice2 < 0 || indice2 > listaPersonajes.Count - 1);

//asigno los personaje
jugador2 = listaPersonajes[indice2];
System.Console.WriteLine($"[Jugador 2] selecciono a {jugador2.Nombre}");
// listaPersonajes.RemoveAt(indice1);
// listaPersonajes.RemoveAt(indice2-1);

int bandera = 1;


while (bandera != 0) //?
{
    System.Console.WriteLine("======== START GAME ========");
    System.Console.WriteLine($"[Jugador 1] : {jugador1.Nombre}");
    System.Console.WriteLine($"           VS                  ");
    System.Console.WriteLine($"[Jugador 2] : {jugador2.Nombre}");

    Random aleatorio = new Random();
    int quienComienza = aleatorio.Next(1,3); //selecciona quien atacara primero
    Personaje atacante,defensor,temporal;

    if (quienComienza == 1)
    {
        System.Console.WriteLine("[Jugador 1] comienza");
        atacante = jugador1;
        defensor = jugador2;
    }
    else
    {
        System.Console.WriteLine("[Jugador 2] comienza");
        atacante = jugador2;
        defensor = jugador1;
    }

    while (jugador1.Salud > 0 && jugador2.Salud > 0)
    {
        realizarAtaque(atacante,defensor); //realizo el ataque
        System.Console.WriteLine("\n");
        //intercambio los roles
        temporal = atacante;
        atacante = defensor;
        defensor = temporal;
    }

    if (jugador1.Salud > 0)
    {
        System.Console.WriteLine("========== GANADOR [Jugador 1] ========");
        //jugador1.mostrarPersonaje();
        realizarMejora(jugador1);
        listaPersonajes.Remove(jugador2);
    }
    else
    {
        System.Console.WriteLine("========== GANADOR [Jugador 2] ========");
        //jugador1.mostrarPersonaje();
        listaPersonajes.Remove(jugador1);
        realizarMejora(jugador2);
        jugador1 = jugador2;
    }

    if (listaPersonajes.Count != 0)
    {
        System.Console.WriteLine("=== Personajes restantes ===");
        mostrarPersonajes(listaPersonajes);
        do
        {
            System.Console.WriteLine("[ JUGADOR 2]");
            System.Console.WriteLine("Indice del personaje seleccionado: ");
        } while (!int.TryParse(Console.ReadLine(),out indice2) || indice2 < 0 || indice2 > listaPersonajes.Count - 1);

        //asigno los personaje
        jugador2 = listaPersonajes[indice2];
        System.Console.WriteLine($"[Jugador 2] selecciono a {jugador2.Nombre}");
        listaPersonajes.RemoveAt(indice2);
    }else
    {
        bandera = 0;
    }
}

System.Console.WriteLine("################### ULTIMO EN PIE ###################");
jugador1.mostrarPersonaje();














/*----------------- METODOS -------------------*/

static void mostrarPersonajesCompleto(List<Personaje> lista)
{
    foreach (var item in lista)
    {
        item.mostrarPersonaje();
        System.Console.WriteLine("#############");
    }
}

static void mostrarPersonajes(List<Personaje> lista)
{

    System.Console.WriteLine($"{"Indice".PadRight(7)} || {"Nombre".PadRight(10)}  || {"Tipo".PadRight(10)}");
    for (int i = 0; i < lista.Count; i++)
    {
        //PadRight genera un espaciadp uniforme, completa con espacios hasta llegar a 10
        System.Console.WriteLine($"{i.ToString().PadRight(7)} || {lista[i].Nombre.PadRight(10)} || {lista[i].Tipo.ToString().PadRight(10)}");
    }
}

static void realizarAtaque(Personaje ataca,Personaje defiende)
{
    int ataque = ataca.Destreza * ataca.Fuerza * ataca.Nivel;
    Random rm = new Random();
    int efectividad = rm.Next(1,101);
    int defensa = defiende.Armadura * defiende.Velocidad;
    const int ajuste = 500;
    int danio = ((ataque * efectividad) - defensa) / ajuste;
    if (defiende.Salud - danio >= 0)
    {
        defiende.Salud -= danio;
    }else
    {
        defiende.Salud = 0;
    }

    System.Console.WriteLine($"{ataca.Nombre} ataca a {defiende.Nombre}");
    System.Console.WriteLine($"Daño realizado : {danio}");
    System.Console.WriteLine($"Salud Restante de {defiende.Nombre}: {defiende.Salud}");
}

static void realizarMejora(Personaje ganador)
{
    System.Console.WriteLine($"Mejora aplicada a {ganador.Nombre}");
    Random rm = new Random();
    int eleccion= rm.Next(0,6);
    int mejora = 0;
    switch (eleccion)
    {
        case 0:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($"Mejora de velocidad de {mejora}");
            ganador.Velocidad += mejora;
            break;

        case 1:
            mejora = rm.Next(1,3);
            System.Console.WriteLine($"Mejora de destreza de {mejora}");
            ganador.Velocidad += mejora;
            break;

        case 2:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($"Mejora de velocidad de {mejora}");
            ganador.Destreza += mejora;
            break;

        case 3:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($"Mejora de Fuerza de {mejora}");
            ganador.Fuerza += mejora;
            break;

        case 4:
            mejora = 1;
            System.Console.WriteLine($"Mejora de Nivel de {mejora}");
            ganador.Nivel += mejora;
            break;

        case 5:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($"Mejora de Armadura de {mejora}");
            ganador.Armadura += mejora;
            break;

        case 6:
            mejora = rm.Next(1,21);
            System.Console.WriteLine($"Mejora de Salud de {mejora}");
            ganador.Salud += mejora;
            break;
    }
}