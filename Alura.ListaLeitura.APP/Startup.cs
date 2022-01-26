using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var biulder = new RouteBuilder(app);
            biulder.MapRoute("Livros/ParaLer", LivrosParaLer);
            biulder.MapRoute("Livros/Lidos", Lendo);
            biulder.MapRoute("Livros/Lendo", Lidos);
            biulder.MapRoute("Livros/Todos", Todos);
            biulder.MapRoute("Cadastro/NovoLivro/{nome}/autor", NovoLivroParaLer);
            biulder.MapRoute("Livro/Detalhes/{id:int}", ExibeDetalhes);

            var rotas = biulder.Build();

            app.UseRouter(rotas);
            //app.Run(Roteamento);
        }

        private Task ExibeDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.First(l => l.Id == id);
            return context.Response.WriteAsync(livro.Detalhes());
        }

        private Task NovoLivroParaLer(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.GetRouteValue("nome")),
                Autor = Convert.ToString(context.GetRouteValue("autor")),
            };
            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
        }

        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {
                {"/Livros/ParaLer",LivrosParaLer},
                {"/Livros/Lidos",Lendo},
                {"/Livros/Lendo",Lidos},
                {"/Livros/Todos",Todos}

            };
            
            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {   var metodo = caminhosAtendidos[context.Request.Path]
                return metodo.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 404;
                return context.Response.WriteAsync("Caminho Inexistente.");
            }
                
        }
        public Task LivrosParaLer(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }
        public Task Lendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }
        public Task Lidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }
        public Task Todos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Todos.ToString());
        }
    }
}