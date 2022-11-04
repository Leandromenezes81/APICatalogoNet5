using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations;

public partial class Populadb : Migration
{
    protected override void Up(MigrationBuilder mb)
    {
        mb.Sql(@"INSERT INTO Categorias (Nome,ImagemUrl)
                            VALUES ('Bebidas','http://www.macoratti.net/Imagens/1.jpg')");
        mb.Sql(@"INSERT INTO Categorias (Nome,ImagemUrl)
                            VALUES ('Lanches','http://www.macoratti.net/Imagens/2.jpg')");
        mb.Sql(@"INSERT INTO Categorias (Nome,ImagemUrl)
                            VALUES ('Sobremesas','http://www.macoratti.net/Imagens/3.jpg')");

        mb.Sql(@"INSERT INTO Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)
                            VALUES('Coca-Cola Diet','Refrigerante de Cola 350 ml','5.45','http://www.macoratti.net/Imagens/coca.jpg'
                                    ,50,now(),(SELECT CategoriaId FROM Categorias WHERE Nome = 'Bebidas'))");
        mb.Sql(@"INSERT INTO Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)
                            VALUES('Lanche de Atum','Lanche de Atum com maionese','8.50','http://www.macoratti.net/Imagens/coca.jpg'
                                    ,10,now(),(SELECT CategoriaId FROM Categorias WHERE Nome = 'Lanches'))");
        mb.Sql(@"INSERT INTO Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)
                            VALUES('Pudim 100 g','Pudim de leite condensado 100 g','6.75','http://www.macoratti.net/Imagens/coca.jpg'
                                    ,20,now(),(SELECT CategoriaId FROM Categorias WHERE Nome = 'Sobremesas'))");
    }

    protected override void Down(MigrationBuilder mb)
    {
        mb.Sql("DELETE FROM Categorias");
        mb.Sql("DELETE FROM Produtos");
    }
}
