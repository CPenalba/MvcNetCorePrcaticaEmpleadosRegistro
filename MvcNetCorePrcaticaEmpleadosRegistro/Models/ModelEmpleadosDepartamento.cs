namespace MvcNetCorePrcaticaEmpleadosRegistro.Models
{
    public class ModelEmpleadosDepartamento
    {
        public Departamento Departamento { get; set; }
        public List<Empleado> Empleado { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
