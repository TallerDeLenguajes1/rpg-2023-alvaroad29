using System.Text.Json;


namespace EspacioPersonaje
{

    public class Personaje  //clase que contiene los datos y caracteristicas del personaje
    {
        //datos
        private string tipo;
        private string nombre;
        private DateTime fechaNac;
        private int edad;

        //Propiedades (datos)
        public string Tipo { get => tipo; set => tipo = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public DateTime FechaNac { get => fechaNac; set => fechaNac = value; }
        public int Edad { get => edad; set => edad = value; }


        //caracteristicas
        private int velocidad;
        private int destreza;
        private int fuerza;
        private int nivel;
        private int armadura;
        private float salud;

        //Propiedades(caracteristicas)
        public int Velocidad { get => velocidad; set => velocidad = value; }
        public int Destreza { get => destreza; set => destreza = value; }
        public int Fuerza { get => fuerza; set => fuerza = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Armadura { get => armadura; set => armadura = value; }
        public float Salud { get => salud; set => salud = value; }

        //metodos
        public void mostrarPersonaje()
        {
            System.Console.WriteLine("DATOS");
            Console.WriteLine($"⋄ Nombre: {Nombre}");
            Console.WriteLine($"⋄ Tipo: {Tipo}");
            Console.WriteLine($"⋄ Fecha de nacimiento: {FechaNac.ToShortDateString()}");
            Console.WriteLine($"⋄ Edad: {Edad}\n");
            System.Console.WriteLine("ESTADISTICAS");
            Console.WriteLine($"⋄ Velocidad: {Velocidad}");
            Console.WriteLine($"⋄ Destreza: {Destreza}");
            Console.WriteLine($"⋄ Fuerza: {Fuerza}");
            Console.WriteLine($"⋄ Nivel: {Nivel}");
            Console.WriteLine($"⋄ Armadura: {Armadura}");
            Console.WriteLine($"⋄ Salud: {Salud}");
        }
    }

    public class FabricaDePersonajes //clase que genera personajes aleatorios
    {
        private List<string> nombresUtilizados = new List<string>();
        string[] nombres = { "Thaus", "Racid", "Hyr", "Red", "Flint", "Cecile","Gadull","Pix","Marcus","Geoff","Ace", "Shadow", "Raptor", "Luna", "Blaze", "Spike" };

        string[] tipos = {"humano","deidad","hada","ogro","nigromante","mago","elfo","orco","gnomo","vampiro","enano","dragon","goblin"};
        public Personaje generarPersonaje() //metodo que retorna el personaje con los datos cargados
        {
            Personaje personaje = new Personaje(); //instancio el personaje
            Random aleatorio = new Random();

            //generar datos
            personaje.Tipo = tipos[aleatorio.Next(tipos.Length)]; 

            string nombreGenerado;
            do
            {
                nombreGenerado = nombres[aleatorio.Next(nombres.Length)];
            } while (nombresUtilizados.Contains(nombreGenerado));
            personaje.Nombre = nombreGenerado;
            nombresUtilizados.Add(nombreGenerado);

            personaje.Edad = aleatorio.Next(0,300); //genero aleatoriamente la edad
            personaje.FechaNac = DateTime.Now.AddYears(-personaje.Edad); //de la fecha actual resto la edad para obtener la fecha de nacimiento

            //generar caracterisitcas
            personaje.Velocidad = aleatorio.Next(3,11);//indInferior,indSuperior + 1
            personaje.Destreza = aleatorio.Next(3,6);
            personaje.Fuerza = aleatorio.Next(3,11);
            personaje.Nivel = aleatorio.Next(3,11);
            personaje.Armadura = aleatorio.Next(3,11);
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
            File.WriteAllText(nombreArchivo,Personajesjson); 
        }

        //leer 
        public List<Personaje> LeerPersonajes(string nombreArchivo)
        {
            //DESERIALIZA  un archivo JSON a un objeto
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