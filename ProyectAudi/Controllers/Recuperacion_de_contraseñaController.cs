using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectAudi.Data;
using ProyectAudi.Models;

namespace ProyectAudi.Controllers
{
    public class Recuperacion_de_contraseñaController : Controller
    {
        private readonly ProyectDbContext _context;

        public Recuperacion_de_contraseñaController(ProyectDbContext context)
        {
            _context = context;
        }

        // GET: RECUPERACION_CONTRASENA
        public async Task<IActionResult> Index()
        {
            var proyectDbContext = _context.RECUPERACION_CONTRASENA.Include(r => r.CREDENCIAL);
            return View(await proyectDbContext.ToListAsync());
        }

        // GET: RECUPERACION_CONTRASENA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rECUPERACION_CONTRASENA = await _context.RECUPERACION_CONTRASENA
                .Include(r => r.CREDENCIAL)
                .FirstOrDefaultAsync(m => m.RECUPERACION_ID == id);
            if (rECUPERACION_CONTRASENA == null)
            {
                return NotFound();
            }

            return View(rECUPERACION_CONTRASENA);
        }

        // GET: RECUPERACION_CONTRASENA/Create
        public IActionResult Create()
        {
            ViewData["CREDENCIAL_ID"] = new SelectList(_context.CREDENCIAL, "CREDENCIAL_ID", "CREDENCIAL_ID");
            return View();
        }

        // POST: RECUPERACION_CONTRASENA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RECUPERACION_ID,CREDENCIAL_ID,TOKEN,FECHA_SOLICITUD,FECHA_EXPIRACION,USADO")] RECUPERACION_CONTRASENA rECUPERACION_CONTRASENA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rECUPERACION_CONTRASENA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CREDENCIAL_ID"] = new SelectList(_context.CREDENCIAL, "CREDENCIAL_ID", "CREDENCIAL_ID", rECUPERACION_CONTRASENA.CREDENCIAL_ID);
            return View(rECUPERACION_CONTRASENA);
        }

        // GET: RECUPERACION_CONTRASENA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rECUPERACION_CONTRASENA = await _context.RECUPERACION_CONTRASENA.FindAsync(id);
            if (rECUPERACION_CONTRASENA == null)
            {
                return NotFound();
            }
            ViewData["CREDENCIAL_ID"] = new SelectList(_context.CREDENCIAL, "CREDENCIAL_ID", "CREDENCIAL_ID", rECUPERACION_CONTRASENA.CREDENCIAL_ID);
            return View(rECUPERACION_CONTRASENA);
        }

        // POST: RECUPERACION_CONTRASENA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RECUPERACION_ID,CREDENCIAL_ID,TOKEN,FECHA_SOLICITUD,FECHA_EXPIRACION,USADO")] RECUPERACION_CONTRASENA rECUPERACION_CONTRASENA)
        {
            if (id != rECUPERACION_CONTRASENA.RECUPERACION_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rECUPERACION_CONTRASENA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RECUPERACION_CONTRASENAExists(rECUPERACION_CONTRASENA.RECUPERACION_ID))
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
            ViewData["CREDENCIAL_ID"] = new SelectList(_context.CREDENCIAL, "CREDENCIAL_ID", "CREDENCIAL_ID", rECUPERACION_CONTRASENA.CREDENCIAL_ID);
            return View(rECUPERACION_CONTRASENA);
        }

        // GET: RECUPERACION_CONTRASENA/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rECUPERACION_CONTRASENA = await _context.RECUPERACION_CONTRASENA
                .Include(r => r.CREDENCIAL)
                .FirstOrDefaultAsync(m => m.RECUPERACION_ID == id);
            if (rECUPERACION_CONTRASENA == null)
            {
                return NotFound();
            }

            return View(rECUPERACION_CONTRASENA);
        }

        // POST: RECUPERACION_CONTRASENA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rECUPERACION_CONTRASENA = await _context.RECUPERACION_CONTRASENA.FindAsync(id);
            if (rECUPERACION_CONTRASENA != null)
            {
                _context.RECUPERACION_CONTRASENA.Remove(rECUPERACION_CONTRASENA);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RECUPERACION_CONTRASENAExists(int id)
        {
            return _context.RECUPERACION_CONTRASENA.Any(e => e.RECUPERACION_ID == id);
        }
    }
}
