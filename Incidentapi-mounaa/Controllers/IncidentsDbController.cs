using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Incidentapi_mounaa.Models;

namespace Incidentapi_mounaa.Controllers
{
    public class IncidentsDbController : Controller
    {
        private readonly IncidentsDbContext _context;

        private static readonly string[] AllowedSeverities = {"LOW", "MEDIUM","HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses = { "OPEN", "IN_PROGRESS","RESOLVED" };

        public IncidentsDbController(IncidentsDbContext context)
        {
            _context = context;
        }
        // GET: api/IncidentsDb/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound();
            return incident;
        }

        // POST: api/IncidentsDb
        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            incident.CreatedAt = DateTime.Now;
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }

        // PUT: api/IncidentsDb/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, Incident incident)
        {
            if (id != incident.Id)
                return BadRequest();

            _context.Entry(incident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/IncidentsDb/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound();

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: Incidents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Incidents.ToListAsync());
        }

        // GET: Incidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,Severity,CreatedAt")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                incident.Severity = incident.Severity.ToUpper();

                incident.Status = "OPEN";
                incident.CreatedAt = DateTime.Now;

                _context.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,Severity,CreatedAt")] Incident incident)
        {
            if (id != incident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: api/IncidentsDb/filter-by-status
        [HttpGet("getbystatusasync/{status}")]
        public async Task<ActionResult<IEnumerable<Incident>>> FilterByStatus([FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Le paramètre 'status' est requis.");

            var filtered = await _context.Incidents
                .Where(i => i.Status.ToUpper() == status.ToUpper())
                .ToListAsync();

            return Ok(filtered);
        }

        // GET: api/IncidentsDb/filter-by-severity
        [HttpGet("getbyseverityasync/{severity}")]
        public async Task<IActionResult> FilterBySeverity(string severity)
        {
            if (string.IsNullOrWhiteSpace(severity))
                return BadRequest("Le paramètre 'severity' est requis.");

            var filtered = await _context.Incidents
                .Where(i => i.Severity.ToUpper() == severity.ToUpper())
                .ToListAsync();

            return Ok(filtered);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident != null)
            {
                _context.Incidents.Remove(incident);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<ActionResult<List<Incident>>> GetIncidents()
        {
            var incidents = await _context.Incidents.ToListAsync();
            return incidents; 
        }
    }
}
