using WebApiBanco.Validaciones;


namespace WebApiBanco.Entidades
{
    public class Clase
    {
        public int Id { get; set; }

        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public string Puesto { get; set; }

        public int PersonaId { get; set; }
        public Persona Persona { get; set; }
    }
}