using Incidentapi_mounaa.Controllers;
using Incidentapi_mounaa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTests
{
    public class IncidentsTests
    {
        private IncidentsDbContext GetDbContext() 
        {
            var options = new DbContextOptionsBuilder<IncidentsDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;

            return new IncidentsDbContext(options);
        }

        [Fact]
        public async Task GetIncidents_WhenDataExists_ReturnsAllIncidents()
        {
            var context = GetDbContext();
            context.Incidents.AddRange(
            new Incident { Title = "Incident1", Status = "OPEN", Severity = "HIGH" },
            new Incident { Title = "Incident2", Status = "CLOSED", Severity = "LOW" }
            );
            context.SaveChanges();
            var controller = new IncidentsDbController(context);
            var result = await controller.GetIncidents();
            var incidents = Assert.IsType<List<Incident>>(result.Value);
            Assert.Equal(2, incidents.Count);
        }

    }
}
