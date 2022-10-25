using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new { errorMessage = "Erro ao tentar obter as categorias da base de dados." });
            }
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _context.Categorias
                    .Include(x => x.Produtos)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new { errorMessage = "Erro ao tentar obter as categorias da base de dados." });
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias
                    .AsNoTracking()
                    .FirstOrDefault(x => x.CategoriaId == id);

                if (categoria is null)
                    return NotFound(new { errorMessage = $"Categoria id '{id}' não encontrada." });
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new { errorMessage = "Erro ao tentar obter a categoria da base de dados." });
            }
        }

        [HttpGet("produto/{id}")]
        public ActionResult<Categoria> GetCategoriaProduto(int id)
        {
            try
            {
                var categoria = _context.Categorias
                    .Include(x => x.Produtos)
                    .AsNoTracking()
                    .FirstOrDefault(x => x.CategoriaId == id);

                if (categoria is null)
                    return NotFound(new { errorMessage = $"Categoria id '{id}' não encontrada." });
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new { errorMessage = "Erro ao tentar obter a categoria da base de dados." });
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new { errorMessage = "Erro ao tentar inserir a categoria da base de dados." });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                    return BadRequest(new { errorMessage = $"Categoria  id '{id}' não encontrada." });

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = "Erro ao tentar editar a categoria da base de dados." });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias
                    .AsNoTracking()
                    .FirstOrDefault(x => x.CategoriaId == id);

                if (categoria is null)
                    return NotFound(new { errorMessage = $"Categoria  id '{id}' não encontrada." });

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = "Erro ao tentar excluir a categoria da base de dados." });
            }
        }
    }
}
