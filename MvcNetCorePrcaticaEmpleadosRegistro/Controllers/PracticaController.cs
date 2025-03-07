using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MvcNetCorePrcaticaEmpleadosRegistro.Models;
using MvcNetCorePrcaticaEmpleadosRegistro.Repositories;

namespace MvcNetCorePrcaticaEmpleadosRegistro.Controllers
{
    public class PracticaController : Controller
    {
        private RepositoryHospital repo;

        public PracticaController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Departamento> d = await this.repo.GetDepartamentosAsync();
            return View(d);
        }

        public async Task<IActionResult> Details(int? posicion, int idDepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            ModelEmpleadosDepartamento model = await this.repo.GetEmpleadosDepartamentoOutAsync(posicion.Value, idDepartamento);
            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["POSICION"] = posicion;
            int siguiente = posicion.Value + 1;
            if (siguiente > model.NumeroRegistros)
            {
                siguiente = model.NumeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.NumeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["DEPARTAMENTO"] = model.Departamento.IdDepartamento;
            return View(model);
        }
    }
}
