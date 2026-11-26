using CercaCafe.DAL.Models;
using Microsoft.Extensions.Logging;

namespace CercaCafe.BLL.Services
{
    public class AuditoriaService
    {
        private readonly ILogger<AuditoriaService> _logger;

        public AuditoriaService(ILogger<AuditoriaService> logger)
        {
            _logger = logger;
        }

        // Registro de eventos de autenticación y accesos no autorizados
        public void RegistrarEventoSeguridad(string tipoEvento, string usuario, string ipOrigen, string detalles)
        {
            _logger.LogWarning("[AUDITORÍA SEGURIDAD] Evento: {TipoEvento} | Usuario: {Usuario} | IP: {IP} | Detalles: {Detalles} | Fecha: {Fecha}",
                tipoEvento, usuario, ipOrigen, detalles, DateTime.UtcNow);
        }

        // Registro de modificaciones en datos críticos (CRUD sensible)
        public void RegistrarCambioCritico(string entidad, string accion, string usuario, int idRegistro)
        {
            _logger.LogInformation("[AUDITORÍA DATOS] Entidad: {Entidad} | Acción: {Accion} | ID: {ID} | Usuario: {Usuario} | Fecha: {Fecha}",
                entidad, accion, idRegistro, usuario, DateTime.UtcNow);
        }
    }
}