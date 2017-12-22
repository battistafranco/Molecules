
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Collections;
using Arcarys.Models;
using System.Net.Http.Headers;

namespace ArcarysMolecules.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Molecule> molecules = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:16302/api/");
                //HTTP GET
                var responseTask = client.GetAsync("molecules");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Molecule>>();
                    readTask.Wait();

                    molecules = readTask.Result;
                }
                else
                {
                    molecules = Enumerable.Empty<Molecule>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(molecules);
        }

        public ActionResult Create()
        {
            FillDDL();
            return View();
        }

        [HttpPost]
        public ActionResult Create(MoleculeVM molecule)
        {
            if (molecule.AtomsIds.Length > 0 && molecule.LinksIds.Length > 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:16302/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    Molecule newMol = new Molecule();
                    newMol.Name = molecule.Name;
                    newMol.Atoms = GetMockedAtomos().Where(a => molecule.AtomsIds.Contains(a.Id)).ToList();
                    newMol.Links = GetMockedEnlaces().Where(a => molecule.LinksIds.Contains(a.Id)).ToList();
                                        
                    var postTask = client.PostAsJsonAsync<Molecule>("api/molecules", newMol);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError(string.Empty, "Oops, ocurrió un error! Vuelva a intentarlo..");

                FillDDL();
                return View(molecule);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "La cantidad de átomos y/o enlaces seleccionados no debe ser nulo.");

                FillDDL();
                return View(molecule);
            }
           
        }

        public ActionResult Details(string name)
        {
            Molecule molecules = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:16302/api/");
                var responseTask = client.GetAsync("molecules/" + name);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Molecule>();
                    readTask.Wait();

                    molecules = readTask.Result;
                }
                else
                {

                    molecules = new Molecule();

                    ModelState.AddModelError(string.Empty, "Oops, ocurrió un error! Vuelva a intentarlo..");
                }
            }
            return View(molecules);
        }
                
        public ActionResult ValenceCheck()
        {
            FillDDL();
            return View();
        }

        [HttpPost]
        public ActionResult ValenceCheck(Atom atom)
        {
            var realVal = GetMockedAtomos().Where(a => a.Id == atom.Id).First();
            if (realVal.Valence != atom.Valence)
            {
                ModelState.AddModelError(string.Empty, String.Format("Advertencia! La valencia standard del {0} debe ser {1}.", realVal.Element, realVal.Valence));
            }
            else
            {
                ViewBag.Message = "Valencia verificada correctamente!";
                ModelState.AddModelError(string.Empty, "Valencia verificada correctamente!");
            }
            FillDDL();
            return View(atom);
        }

        public void FillDDL()
        {
            ViewBag.Atomos = new SelectList(GetMockedAtomos(), "Id", "Element");
            ViewBag.Enlaces = new SelectList(GetMockedEnlaces(), "Id", "Order");
        }

        //Métodos para mockear átomos y enlaces
        private List<Link> GetMockedEnlaces()
        {
            List<Link> links = new List<Link>();
            Link a1 = new Link();
            a1.Order = Order.Simple;
            a1.Id = 2;
            a1.IsHydrogenLink = false;

            Link a2 = new Link();
            a2.Order = Order.Doble;
            a2.Id = 1;
            a2.IsHydrogenLink = false;

            Link a3 = new Link();
            a3.Order = Order.Triple;
            a3.Id = 3;
            a3.IsHydrogenLink = false;

            links.Add(a1);
            links.Add(a2);
            links.Add(a3);

            return links;
        }

        private List<Atom> GetMockedAtomos()
        {
            List<Atom> atomos = new List<Atom>();
            Atom a1 = new Atom();
            a1.Element = "Carbono";
            a1.Id = 2;
            a1.Valence = 2;

            Atom a2 = new Atom();
            a2.Element = "Hidrogeno";
            a2.Id = 1;
            a2.Valence = 1;

            Atom a3 = new Atom();
            a3.Element = "Oxigeno";
            a3.Id = 3;
            a3.Valence = 4;

            atomos.Add(a1);
            atomos.Add(a2);
            atomos.Add(a3);

            return atomos;
        }

    }
}