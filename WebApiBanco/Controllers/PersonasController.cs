using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBanco.Entidades;
using WebApiBanco.Filtros;
using WebApiBanco.Services;
using WebApiBanco;

namespace WebApiBanco.Controllers
{
    [ApiController]
    [Route("api/personas")]  

    public class PersonasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<PersonasController> logger;
        private readonly IWebHostEnvironment env;
        private readonly string nuevosAutorizos = "nuevosAutorizos.txt";
        private readonly string AutorizacionesConsultados = "AutorizacionesConsultados.txt";

        public PersonasController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<PersonasController> logger,
            IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            throw new NotImplementedException();
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                PersonasControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                PersonasControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                PersonasControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
     
        public async Task<ActionResult<List<Persona>>> GetAlumnos()
        {
          
            throw new NotImplementedException();
            logger.LogInformation("obtenemos loistado de personas");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
            return await dbContext.Personas.Include(x => x.salas).ToListAsync();
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("primero")] 
        public async Task<ActionResult<Persona>> PrimerPersona()
        {
            return await dbContext.Personas.FirstOrDefaultAsync();
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("primero2")] 
        public ActionResult<Persona> PrimerPersonaD()
        {
            return new Persona { Nombre = "Angelon" };
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("{param?}")] 
        public async Task<ActionResult<Persona>> Get(int id, string param)
        {
            var persona = await dbContext.Personas.FirstOrDefaultAsync(x => x.Id == id);



            if (persona == null)
            {
                return NotFound();
            }

            return persona;
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("Obtener persona por medio de su  nombre=/{nombre}")]
        public async Task<ActionResult<Persona>> Get([FromRoute] string nombre)
        {
            var persona = await dbContext.Personas.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (persona == null)
            {
                logger.LogError("No se encuentra la persona ");
                return NotFound();
            }

            var ruta = $@"{env.ContentRootPath}\wwwroot\{AutorizacionesConsultados}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(persona.Id + " " + persona.Nombre); }

            return persona;
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Persona persona)
        {
  
            var existePersonaMismoNombre = await dbContext.Personas.AnyAsync(x => x.Nombre == persona.Nombre);

            if (existePersonaMismoNombre)
            {
                return BadRequest("Ya existe un autor ");
            }
            dbContext.Add(persona);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPut("{id:int}")] 
        public async Task<ActionResult> Put(Persona persona, int id)
        {
            var exist = await dbContext.Personas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if (persona.Id != id)
            {
                return BadRequest("El id de la persona no coincide con el establecido ");
            }

            dbContext.Update(persona);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Personas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Persona()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
