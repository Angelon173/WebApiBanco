using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBanco.Entidades;
using WebApiBanco;

namespace WebApiBanco.Controllers
{
    [ApiController]
    [Route("Salas")] 
    public class SalasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<SalasController> log;
        private object sala;

        public SalasController(ApplicationDbContext context, ILogger<SalasController> log)
        {
            this.dbContext = context;
            this.log = log;
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [HttpGet("/listado De Salas")]
        public async Task<ActionResult<List<Sala>>> GetAll()
        {
            log.LogInformation("Obteniendo listado de las salas dispobnibles");
            return await dbContext.Salas.ToListAsync();
        }
        private object ToListAsync()
        {
            throw new NotImplementedException();
        }





        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Sala>> GetById(int id)
        {
            log.LogInformation("EL ID ES: " + id);
            return await dbContext.Salas.FirstOrDefaultAsync(x => x.Id == id);
        }




        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<ActionResult> Post(Sala sala)
        {
            var existePersona = await dbContext.Personas.AnyAsync(x => x.Id == sala.PersonaId);

            if (!existePersona)
            {
                return BadRequest($"No existe la persona con ese id: {sala.PersonaId} ");
            }

            dbContext.Add(sala);
            await dbContext.SaveChangesAsync();
            return Ok();
        }




        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Sala sala, int id)
        {
            var exist = await dbContext.Salas.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("La sala donde se esperara  no existe. ");
            }

            if (sala.Id != id)
            { 
                dbContext.Update(sala);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("El id de sala no coincide con el establecido. ");

        }




        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Salas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Sala { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}