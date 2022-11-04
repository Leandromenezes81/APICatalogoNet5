using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        try
        {
            return await _context.Produtos
                .Include(x => x.Categoria)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Erro ao tentar obter os produtos da base de dados.");
        }
    }

    //[BindRequired] => https://localhost:44306/api/produtos/1?nome=Refrigerante
    //[FromQuery] => https://localhost:44306/api/produtos/1?id=2
    [HttpGet("{id}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> Get([FromQuery] int id)
    {
        try
        {
            var produto = await _context.Produtos
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.ProdutoId == id);

            if (produto is null)
                return NotFound(new { errorMessage = $"Produto id '{id}' não encontrado." });
            return produto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = "Erro ao tentar obter os produtos da base de dados." });
        }
    }

    [HttpPost]
    public ActionResult Post([FromBody] Produto produto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = "Erro ao tentar inserir o produto da base de dados." });
        }
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Produto produto)
    {
        try
        {
            if (id != produto.ProdutoId)
                return BadRequest(new { errorMessage = $"Produto  id '{id}' não encontrado." });

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = "Erro ao tentar inserir o produto da base de dados." });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult<Produto> Delete(int id)
    {
        try
        {
            var produto = _context.Produtos
                .AsNoTracking()
                .FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound(new { errorMessage = $"Produto  id '{id}' não encontrado." });

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return produto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = "Erro ao tentar excluir o produto da base de dados." });
        }
    }

}
