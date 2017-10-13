﻿using Loja.Mvc.Helpers;
using Loja.Repositorios.SqlServer.EF;
using System.Linq;
using System.Web.Mvc;

namespace Loja.Mvc.Areas.Vendas.Controllers
{
    public class LeiloesController : Controller
    {
        private readonly LojaDbContext _db = new LojaDbContext();

        public ActionResult Index()
        {
            return View(Mapeamento.Mapear(_db.Produtos.Where(p => p.EmLeilao).ToList()));
        }
    }
}