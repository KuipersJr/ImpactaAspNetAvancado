﻿using System;
using System.Web;
using System.Web.Mvc;
using Loja.Mvc.Helpers;

namespace Loja.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DefinirLinguagemPadrao();
            return View();
        }

        private void DefinirLinguagemPadrao()
        {
            if (Request.Cookies[Cookie.LinguagemSelecionada.ToString()] != null) return;

            var linguagem = CulturaHelper.LinguagemPadrao;

            if (Request.UserLanguages != null && Request.UserLanguages[0] != string.Empty)
            {
                linguagem = Request.UserLanguages[0];
            }

            var linguagemSelecionadaCookie = new HttpCookie(Cookie.LinguagemSelecionada.ToString(), linguagem);
            linguagemSelecionadaCookie.Expires = DateTime.MaxValue;

            Response.Cookies.Add(linguagemSelecionadaCookie);
        }

        public ActionResult DefinirLinguagem(string linguagem)
        {
            //if (Request.Cookies["linguagemSelecionada"] != null)
            //{
            //    Response.Cookies["linguagemSelecionada"].Value = linguagem;
            //}
            //else
            //{
            //    DefinirLinguagemPadrao();
            //}

            //if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.ToString());

            //return RedirectToAction("Index");

            Response.Cookies[Cookie.LinguagemSelecionada.ToString()].Value = linguagem;

            return Redirect(Request.UrlReferrer.ToString());
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}