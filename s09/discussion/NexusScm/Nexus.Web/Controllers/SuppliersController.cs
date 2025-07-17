using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;
using Nexus.Web.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nexus.Web.Controllers
{
    public class SuppliersController : Controller
    {
        //private readonly ApplicationDbContext _context;

        //public SuppliersController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IHttpClientFactory _httpClientFactory;

        public SuppliersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            //var suppliers = await _context.Suppliers.ToListAsync();

         

            var client = _httpClientFactory.CreateClient("NexusApiClient");
            var response = await client.GetAsync("api/suppliers");

            List<Supplier> suppliers = new();

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                suppliers = await JsonSerializer.DeserializeAsync<List<Supplier>>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return View(suppliers);
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);

        //    if (supplier == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(supplier);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, ContactPerson, Email")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("NexusApiClient");
                var token = HttpContext.Session.GetString("JWToken");

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.PostAsJsonAsync("api/suppliers", supplier);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create supplier. API returned an error.");
                }
            }

            return View(supplier);
        }


    }
}
