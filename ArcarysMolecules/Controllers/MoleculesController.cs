using Arcarys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;

namespace ArcarysMolecules.Controllers
{
    public class MoleculesController : ApiController
    {
        // GET: api/MoleculesAPI
        public IHttpActionResult GetAllMolecules()
        {
            var molecules = ReadStorage();

            if (molecules == null)
            {
                molecules = new List<Molecule>();
            }

            return Ok(molecules);
        }
        
        public IHttpActionResult PostNewMolecule(Molecule molecule)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var moleculesExistentes = ReadStorage();
            molecule.Id = moleculesExistentes.Count + 1;
            moleculesExistentes.Add(molecule);
            WriteStorage(moleculesExistentes);

            return Ok();
        }

        [Route("api/molecules/{name}")]
        [HttpGet]
        public IHttpActionResult GetMoleculeByName(string name)
        {
            var molecules = ReadStorage();

            if (String.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var mol = molecules.Where(a => a.Name.Contains(name)).ToList();
            if (mol.Count > 0)
            {
                return Ok(mol.First());
            }
            else
            {
                return NotFound();
            }
        }

        private List<Molecule> ReadStorage()
        {
            var file = HostingEnvironment.MapPath(@"~/App_Data/moleculasStorage.txt");
            if (!File.Exists(file))
            {              
                FileStream fs = new FileStream(file, FileMode.CreateNew);
                fs.Close();
            }

            var fileContents = System.IO.File.ReadAllText(file);
            List<Molecule> molecules = new List<Molecule>();
            using (StreamReader sr = new StreamReader(file))
            {
                molecules = JsonConvert.DeserializeObject<List<Molecule>>(sr.ReadToEnd());
            }

            if (molecules == null)
            {
                molecules = new List<Molecule>();
            }

            return molecules;
        }

        private void WriteStorage(List<Molecule> newMolecules)
        {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(HostingEnvironment.MapPath(@"~/App_Data/moleculasStorage.txt")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, newMolecules);
            }
        }

    }

  
}
