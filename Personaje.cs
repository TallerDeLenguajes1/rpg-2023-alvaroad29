using System;
using System.Collections.Generic;
using System.Text.Json;


namespace EspacioPersonaje
{
    public enum Tipo{humano,deidad,hada,ogro,nigromante,mago};
    
    public class Personaje  //clase que contiene los datos y caracteristicas del personaje
    {
        //datos
        private Tipo tipo;
        private string? nombre;
        private string? apodo;
        private DateTime fechaNac;
        private int edad;

        //Propiedades (datos)
        public Tipo Tipo { get => tipo; set => tipo = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apodo { get => apodo; set => apodo = value; }
        public DateTime FechaNac { get => fechaNac; set => fechaNac = value; }
        public int Edad { get => edad; set => edad = value; }


        //caracteristicas
        private int velocidad;
        private int destreza;
        private int fuerza;
        private int nivel;
        private int armadura;
        private int salud;

        //Propiedades(caracteristicas)
        public int Velocidad { get => velocidad; set => velocidad = value; }
        public int Destreza { get => destreza; set => destreza = value; }
        public int Fuerza { get => fuerza; set => fuerza = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Armadura { get => armadura; set => armadura = value; }
        public int Salud { get => salud; set => salud = value; }

        //metodos
        public void mostrarPersonaje()
        {
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Apodo: {Apodo}");
            Console.WriteLine($"Tipo: {Tipo}");
            Console.WriteLine($"Fecha de nacimiento: {FechaNac.ToShortDateString()}");
            Console.WriteLine($"Edad: {Edad}");
            Console.WriteLine($"Velocidad: {Velocidad}");
            Console.WriteLine($"Destreza: {Destreza}");
            Console.WriteLine($"Fuerza: {Fuerza}");
            Console.WriteLine($"Nivel: {Nivel}");
            Console.WriteLine($"Armadura: {Armadura}");
            Console.WriteLine($"Salud: {Salud}");
        }
    }

    public class FabricaDePersonajes //clase que genera personajes aleatorios
    {
        string[] nombres = { "Thaus", "Racid", "Hyr", "Red", "Flint", "Cecile" };
        string[] apodos = { "Ace", "Shadow", "Raptor", "Luna", "Blaze", "Spike" };
        public Personaje generarPersonaje() //metodo que retorna el personaje con los datos cargados
        {
            Personaje personaje = new Personaje(); //instancio el personaje
            Random aleatorio = new Random();

            //generar datos
            personaje.Tipo = (Tipo)aleatorio.Next(0,Enum.GetValues(typeof(Tipo)).Length); //devuelve un array con los enum y obtengo su longitud
            personaje.Nombre = nombres[aleatorio.Next(nombres.Length)];
            personaje.Apodo = apodos[aleatorio.Next(apodos.Length)];
            personaje.Edad = aleatorio.Next(0,300); //genero aleatoriamente la edad
            personaje.FechaNac = DateTime.Now.AddYears(-personaje.Edad); //de la fecha actual resto la edad para pbtener la fecha de nacimiento

            //generar caracterisitcas
            personaje.Velocidad = aleatorio.Next(1,11);//indInferior,indSuperior + 1
            personaje.Destreza = aleatorio.Next(1,6);
            personaje.Fuerza = aleatorio.Next(1,11);
            personaje.Nivel = aleatorio.Next(1,11);
            personaje.Armadura = aleatorio.Next(1,11);
            personaje.Salud = 100;

            return personaje;
        }
    }

    public class PersonajesJson
    {
        //guarda(lista,nombreArichivo)
        public  void GuardarPersonajes(List<Personaje> listaDePersonajes,string nombreArchivo)
        {
            //SERIALIZA PARA GUARDARLO EN UN ARCHIVO JSON

            string Personajesjson = JsonSerializer.Serialize(listaDePersonajes); //mando objeto y lo convierte a string con un formato json
            File.WriteAllText(nombreArchivo,Personajesjson); //escribe y pisa lo anterior?
        }

        //leer 
        public List<Personaje> LeerPersonajes(string nombreArchivo)
        {
            //DESERIALIZA  un archivo JSON a un objeto
            //es importante q el nombre de las propiedades de la clase tienen q ser iguales a las etiquetas de json
            if (File.Exists(nombreArchivo))
            {
                string jsonString = File.ReadAllText(nombreArchivo); //lee el json y lo guardo en un string
                List<Personaje> listPer = JsonSerializer.Deserialize<List<Personaje>>(jsonString); //<tipo de dato>(variable donde esta la informacion (string)), pasa el string a una lista
                return listPer;
            }
            else
            {
                System.Console.WriteLine($"El archivo {nombreArchivo} NO EXISTE");
                return null;
            }
        }
        //existe()
        public bool Existe(string nombreArchivo)
        {
            //VERIFICO QUE EXISTA EL ARCHIVO
            return File.Exists(nombreArchivo) && new FileInfo(nombreArchivo).Length > 0;
        }
    }
}