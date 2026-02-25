using Incidentapi_mounaa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Incidentapi_mounaa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;
        private static readonly string[] AllowedSeverities =
        { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses =
        { "OPEN", "IN_PROGRESS", "RESOLVED" };

        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        {
            if (!AllowedSeverities.Contains(incident.Severity))
                return BadRequest("Invalid severity value");

            incident.Id = _nextId++;
            incident.CreatedAt = DateTime.Now;
            incident.Status = "OPEN";

            _incidents.Add(incident);

            return Ok(incident);

        }
        [HttpGet("get-all")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }
        [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        {
            Incident incident;
            try
            {
                incident = _incidents.First(i => i.Id == id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }


            return Ok(incident);
        }
        [HttpPut("update-status/{id}")]

        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
                return NotFound();

            if (!AllowedStatuses.Contains(status))
                return BadRequest("Invalid status value");

            incident.Status = status;

            return Ok(incident);
        }
        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            // Search for the incident by id
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            // If incident does not exist
            if (incident == null)
                return NotFound();

            // Rule: A CRITICAL incident cannot be deleted if it is still OPEN
            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
                return BadRequest("Cannot delete a CRITICAL incident that is still OPEN");

            // Remove the incident
            _incidents.Remove(incident);

            return NoContent();
        }
        [HttpGet("filter-by-status/{status}")]
        public IActionResult FilterByStatus(string status)
        {
            var result = _incidents
                .Where(i => i.Status.Contains(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }
        [HttpGet("filter-by-severity/{severity}")]
        public IActionResult FilterBySeverity(string severity)
        {
            var result = _incidents
                .Where(i => i.Severity.Contains(severity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }





    }
}
   