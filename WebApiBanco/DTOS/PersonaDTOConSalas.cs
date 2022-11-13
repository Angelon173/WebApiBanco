namespace WebApiBanco.DTOS
{
    public class PersonaDTOConSalas : GetPersonaDTO
    {
        public List<SalasDTO> Salas { get; set; }
    }
}