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

            // Traer el departamento
            Departamento departamento = await this.repo.FindDepartamentoAsync(idDepartamento);

            // Traer los empleados y el total de registros
            ModelEmpleadosDepartamento model = await this.repo.GetEmpleadosDepartamentoOutAsync(posicion.Value, idDepartamento);

            

            // Asignar los datos a la vista
            ViewData["Departamento"] = departamento;
            ViewData["Empleado"] = model.Empleado;
            ViewData["TotalEmpleados"] = model.NumeroRegistros;
            ViewData["Posicion"] = model.Posicion;

            return View(model);
        }


    }
}
