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
    public class UsuariosController : Controller
    {
        private readonly ProyectDbContext _context;

        public UsuariosController(ProyectDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.USUARIO.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSUARIO = await _context.USUARIO
                .FirstOrDefaultAsync(m => m.USUARIO_ID == id);
            if (uSUARIO == null)
            {
                return NotFound();
            }

            return View(uSUARIO);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("USUARIO_ID,CUI,PRIMERNOMBRE,SEGUNDONOMBRE,TERCERNOMBRE,PRIMERAPELLIDO,SEGUNDOAPELLIDO,APELLIDOCASADA,FECHA_NACIMIENTO,TELEFONO,DIRECCION,PERSONA_NIT,PERSONA_DIRECCION,PERSONA_TELEFONOCASA,PERSONA_TELEFONOMOVIL,FOTOGRAFIA,DPI_PDF,USUARIO_CORREO,ESTADO_TINY,ROL_ID,CREADO_POR,FECHA_CREACION,MODIFICADO_POR,FECHA_MODIFICACION,ELIMINADO,ELIMINADO_POR,FECHA_ELIMINACION")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uSUARIO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uSUARIO);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSUARIO = await _context.USUARIO.FindAsync(id);
            if (uSUARIO == null)
            {
                return NotFound();
            }
            return View(uSUARIO);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("USUARIO_ID,CUI,PRIMERNOMBRE,SEGUNDONOMBRE,TERCERNOMBRE,PRIMERAPELLIDO,SEGUNDOAPELLIDO,APELLIDOCASADA,FECHA_NACIMIENTO,TELEFONO,DIRECCION,PERSONA_NIT,PERSONA_DIRECCION,PERSONA_TELEFONOCASA,PERSONA_TELEFONOMOVIL,FOTOGRAFIA,DPI_PDF,USUARIO_CORREO,ESTADO_TINY,ROL_ID,CREADO_POR,FECHA_CREACION,MODIFICADO_POR,FECHA_MODIFICACION,ELIMINADO,ELIMINADO_POR,FECHA_ELIMINACION")] USUARIO uSUARIO)
        {
            if (id != uSUARIO.USUARIO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uSUARIO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!USUARIOExists(uSUARIO.USUARIO_ID))
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
            return View(uSUARIO);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSUARIO = await _context.USUARIO
                .FirstOrDefaultAsync(m => m.USUARIO_ID == id);
            if (uSUARIO == null)
            {
                return NotFound();
            }

            return View(uSUARIO);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uSUARIO = await _context.USUARIO.FindAsync(id);
            if (uSUARIO != null)
            {
                _context.USUARIO.Remove(uSUARIO);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool USUARIOExists(int id)
        {
            return _context.USUARIO.Any(e => e.USUARIO_ID == id);
        }
    }
}
