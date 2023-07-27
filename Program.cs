using EspacioPersonaje;
using EspacioAscii;


PersonajesJson guardarPersonajes = new PersonajesJson();  //instancio para usar guardar,leer y existe
string nombreArchivo = "Personajes.json";
List<Personaje> listaPersonajes = new List<Personaje>(); //instancio la lista de personajes

if (guardarPersonajes.Existe(nombreArchivo)) //verifico si el archivo existe y tiene datos
{
    //existe el archivo, elijo si usarlos o generar nuevos
    int eleccion;
    System.Console.WriteLine("Hay personajes cargados");
    listaPersonajes = guardarPersonajes.LeerPersonajes(nombreArchivo);
    mostrarPersonajes(listaPersonajes);

    do
    {
        System.Console.WriteLine("Continuar con estos [0] / Nuevos personajes [1]");
    } while (!int.TryParse(Console.ReadLine(),out eleccion) || eleccion < 0 || eleccion > 1);

    if (eleccion == 1)
    {
        listaPersonajes.Clear();
        guardarPersonajesJson(listaPersonajes,nombreArchivo,guardarPersonajes);
    }

}
else
{
    //si no existe, genero los personajes y los guardo
    guardarPersonajesJson(listaPersonajes,nombreArchivo,guardarPersonajes);
}


/*====================== Desarrollo del juego ========================*/
asciiApi asc = new asciiApi(); //para usar la api

Personaje jugador1,jugador2;

int indice1,indice2;

Random aleatorio = new Random();
List<Personaje> listaAux = new List<Personaje>(); //lista auxiliar donde guardo los ganadores
int bandera = 0;
int ronda = 1;

menu();
Console.ReadKey(); //empieza cuando apreto

while (bandera != 1)
{
    System.Console.WriteLine("\n");
    string rondaNum = $"     Ronda {ronda}  ";
    System.Console.WriteLine(asc.obtenerTextoAscii(rondaNum,"Digital"));
    System.Console.WriteLine(asc.obtenerTextoAscii("   combatientes","Digital"));

    mostrarPersonajes(listaPersonajes);

    while (listaPersonajes.Count != 0) 
    {
        if (listaPersonajes.Count != 1) 
        {
            //Elijo los personajes
            indice1 = aleatorio.Next(0,listaPersonajes.Count);
            jugador1 = listaPersonajes[indice1];
            listaPersonajes.RemoveAt(indice1);

            indice2 = aleatorio.Next(0,listaPersonajes.Count);
            jugador2 = listaPersonajes[indice2];
            listaPersonajes.RemoveAt(indice2);

            System.Console.WriteLine("\n");
            string vs = $"{jugador1.Nombre} vs {jugador2.Nombre}";
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(asc.obtenerTextoAscii(vs,"Calvin S"));
            Console.ResetColor();

            //selecciona quien atacara primero
            int quienComienza = aleatorio.Next(1,3); 
            Personaje atacante,defensor,temporal;

            if (quienComienza == 1)
            {
                System.Console.WriteLine($" ▷ {jugador1.Nombre} comienza ◁\n");
                atacante = jugador1;
                defensor = jugador2;
            }
            else
            {
                System.Console.WriteLine($" ▷ {jugador2.Nombre} comienza ◁\n");
                atacante = jugador2;
                defensor = jugador1;
            }

            //juego
            while (jugador1.Salud > 0 && jugador2.Salud > 0)
            {
                realizarAtaque(atacante,defensor); //realizo el ataque
                System.Console.WriteLine("\n");

                //intercambio los roles
                temporal = atacante;
                atacante = defensor;
                defensor = temporal;
            }

            string ganador;
            if (jugador1.Salud > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                ganador = $" ► ► Ganador :【 {jugador1.Nombre} 】◄ ◄";
                System.Console.WriteLine(ganador);
                realizarMejora(jugador1);
                listaAux.Add(jugador1);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                ganador = $" ► ► Ganador :【 {jugador2.Nombre} 】◄ ◄";
                System.Console.WriteLine(ganador);
                realizarMejora(jugador2);
                listaAux.Add(jugador2);
                Console.ResetColor();
            }
        }
        else // si tiene un personaje pasa directamente
        {
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"\n【 {listaPersonajes[0].Nombre} esta bendecido, pasa a la siguiente ronda 】");
            Console.ResetColor();
            listaAux.Add(listaPersonajes[0]);
            listaPersonajes.RemoveAt(0);
        }
    }

    ronda++;

    foreach (var item in listaAux) //agrego todos los item de listaAux a la lista principal
    {
        listaPersonajes.Add(item);
    }

    if (listaAux.Count != 1) //limpio la lista
    {
        listaAux.Clear(); 
    }else //si tiene un elemento entonces es el ultimo en pie
    {
        bandera = 1;
    }
}

System.Console.WriteLine("\n");
Console.ForegroundColor = ConsoleColor.Yellow;
System.Console.WriteLine("╔══════════════════════════════════╗");
System.Console.WriteLine((asc.obtenerTextoAscii(" ULTIMO EN PIE","Calvin S ")));
System.Console.WriteLine("╚══════════════════════════════════╝");
System.Console.WriteLine((asc.obtenerTextoAscii(listaAux[0].Nombre,"Calvin S")));
Console.ResetColor();

listaAux[0].mostrarPersonaje();














/*================================ METODOS =========================================*/

static void mostrarPersonajes(List<Personaje> lista)
{
    System.Console.WriteLine("╔═════════╦════════════╦════════════╗");
    System.Console.WriteLine($"║ {"Indice".PadRight(7)} ║ {"Nombre".PadRight(9)}  ║ {"Tipo".PadRight(10)} ║" );
    for (int i = 0; i < lista.Count; i++)
    {
        //PadRight genera un espaciadp uniforme, completa con espacios hasta llegar a 10
        System.Console.WriteLine("╠═════════╬════════════╬════════════╣");
        System.Console.WriteLine($"║ {i.ToString().PadRight(7)} ║ {lista[i].Nombre.PadRight(10)} ║ {lista[i].Tipo.ToString().PadRight(10)} ║");
    }
    System.Console.WriteLine("╚═════════╩════════════╩════════════╝");
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

    System.Console.WriteLine($"【 {ataca.Nombre} 】ataca a【 {defiende.Nombre} 】");
    System.Console.WriteLine($" ϟ Daño realizado : {danio}");
    System.Console.WriteLine($" ❤ Salud Restante de【 {defiende.Nombre} 】: {defiende.Salud}");
}

static void realizarMejora(Personaje ganador)
{
    System.Console.WriteLine($" Mejora aplicada a【 {ganador.Nombre} 】");
    Random rm = new Random();
    int eleccion= rm.Next(0,6);
    int mejora = 0;
    switch (eleccion)
    {
        case 0:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($" Mejora de velocidad de {mejora}");
            ganador.Velocidad += mejora;
            break;

        case 1:
            mejora = rm.Next(1,3);
            System.Console.WriteLine($" Mejora de destreza de {mejora}");
            ganador.Velocidad += mejora;
            break;

        case 2:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($" Mejora de Fuerza de {mejora}");
            ganador.Fuerza += mejora;
            break;

        case 3:
            mejora = rm.Next(1,6);; 
            System.Console.WriteLine($" Mejora de Nivel de {mejora}");
            ganador.Nivel += mejora;
            break;

        case 4:
            mejora = rm.Next(1,6);
            System.Console.WriteLine($" Mejora de Armadura de {mejora}");
            ganador.Armadura += mejora;
            break;

        case 5:
            mejora = rm.Next(1,21);
            System.Console.WriteLine($" Mejora de Salud de {mejora}");
            ganador.Salud += mejora;
            break;
    }
}

//menu del principio
void menu()
{
    string texto1 = asc.obtenerTextoAscii("simulacion","Delta Corps Priest 1");
    string texto2 = asc.obtenerTextoAscii("       de ","Delta Corps Priest 1");
    string texto3 = asc.obtenerTextoAscii("  batalla  ","Delta Corps Priest 1");
    System.Console.WriteLine(texto1);
    System.Console.WriteLine(texto2);
    System.Console.WriteLine(texto3);
    string texto4 = asc.obtenerTextoAscii("  presione una tecla para comenzar","Calvin S");
    System.Console.WriteLine(texto4);
}


static void guardarPersonajesJson(List<Personaje> listaPersonajes,string nombreArchivo,PersonajesJson guardarPersonajes)
{
    FabricaDePersonajes fp = new FabricaDePersonajes();
    Personaje nuevo;
    int cantPersonajes;
    do
    {
        System.Console.WriteLine("Ingrese la cantidad de personajes (entre 2 y 10): ");  
    } while (!int.TryParse(Console.ReadLine(),out cantPersonajes) || cantPersonajes < 2 || cantPersonajes > 10);

    for (int i = 0; i < cantPersonajes; i++)
    {
        nuevo = fp.generarPersonaje();
        listaPersonajes.Add(nuevo);
    }
    guardarPersonajes.GuardarPersonajes(listaPersonajes,nombreArchivo);
}
