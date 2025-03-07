using MvcNetCorePrcaticaEmpleadosRegistro.Data;
using MvcNetCorePrcaticaEmpleadosRegistro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.Data.SqlClient;
using System.Data;

#region
//CREATE PROCEDURE SP_GRUPO_EMPLEADOS_DEPARTAMENTO_OUT
//(@posicion int, @iddepartamento int, @registros int out)
//as
//	select @registros = count(EMP_NO) from EMP where DEPT_NO = @iddepartamento

//	select EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from 
//	(select ROW_NUMBER() OVER (ORDER BY APELLIDO) as POSICION, EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//		from EMP
//		where DEPT_NO = @iddepartamento) QUERY
//	where POSICION = @posicion
//go
#endregion

namespace MvcNetCorePrcaticaEmpleadosRegistro.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task<Departamento> FindDepartamentoAsync(int idDepartmento)
        {
            var consulta = from datos in this.context.Departamentos where datos.IdDepartamento == idDepartmento select datos;
            return await consulta.FirstOrDefaultAsync();
        }



        public async Task<ModelEmpleadosDepartamento> GetEmpleadosDepartamentoOutAsync(int posicion, int idDepartamento)
        {
            string sql = "SP_GRUPO_EMPLEADOS_DEPARTAMENTO_OUT @posicion, @iddepartamento, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamIdDepartamento = new SqlParameter("@iddepartamento", idDepartamento);
            SqlParameter pamRegistros = new SqlParameter("@registros", 0);
            pamRegistros.Direction = ParameterDirection.Output;

            // Realizar la consulta y obtener la lista de empleados directamente de la base de datos
            var empleados = await this.context.Empleados
                                              .FromSqlRaw(sql, pamPosicion, pamIdDepartamento, pamRegistros)
                                              .ToListAsync();  // Ahora se usa ToListAsync() sobre un IQueryable

            Departamento departamento = await this.context.Departamentos
                                                          .Where(d => d.IdDepartamento == idDepartamento)
                                                          .FirstOrDefaultAsync();

            int registros = int.Parse(pamRegistros.Value.ToString());


            return new ModelEmpleadosDepartamento
            {
                Departamento = departamento,
                Empleado = empleados,  // Ahora Empleado es una lista de empleados
                NumeroRegistros = registros,
                Posicion = posicion
            };
        }


    }
}
