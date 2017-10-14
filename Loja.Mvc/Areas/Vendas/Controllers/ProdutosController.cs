﻿using System.Linq;
using System.Net;
using System.Web.Mvc;
using Loja.Dominio;
using Loja.Repositorios.SqlServer.EF;
using Loja.Mvc.Helpers;
using Loja.Mvc.Areas.Vendas.Models;
using Loja.Mvc.Hubs;
using Microsoft.AspNet.SignalR;

namespace Loja.Mvc.Areas.Vendas.Controllers
{
    public class ProdutosController : Controller
    {
        // ToDo: design pattern Unity of Work.
        private readonly LojaDbContext _db = new LojaDbContext();
        private readonly IHubContext _leilaoHub = GlobalHost.ConnectionManager.GetHubContext<LeilaoHub>();

        //[HandleError(ExceptionType = typeof(DivideByZeroException), View = "ZeroError")]
        public ActionResult Index()
        {
            return View(Mapeamento.Mapear(_db.Produtos.ToList()));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = _db.Produtos.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(Mapeamento.Mapear(produto));
        }

        public ActionResult Create()
        {
            return View(Mapeamento.Mapear(new Produto(), _db.Categorias.ToList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProdutoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _db.Produtos.Add(Mapeamento.Mapear(viewModel, _db));
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Produto produto = _db.Produtos.Find(id);

            if (produto == null)
            {
                return HttpNotFound();
            }

            return View(Mapeamento.Mapear(produto, _db.Categorias.ToList()));
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProdutoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var produto = _db.Produtos.Find(viewModel.Id);

                Mapeamento.Mapear(viewModel, produto, _db);

                //db.Entry(produto).State = EntityState.Modified;
                //db.Entry(produto).CurrentValues.SetValues(viewModel);
                //produto.Categoria = db.Categorias.Find(viewModel.CategoriaId);

                _db.SaveChanges();

                _leilaoHub?.Clients.All.atualizarListaProdutos();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Produtos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Produto produto = _db.Produtos.Find(id);

            if (produto == null)
            {
                return HttpNotFound();
            }

            return View(Mapeamento.Mapear(produto));
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = _db.Produtos.Find(id);
            _db.Produtos.Remove(produto);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();                
            }
            base.Dispose(disposing);
        }
    }
}
