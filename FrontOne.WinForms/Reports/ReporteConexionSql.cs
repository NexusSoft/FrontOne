using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.Utils;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Arma un SqlDataSource "pelado" (mismo servidor/BD que ya usa Dapper vía SqlOptions, nunca una
// conexión nueva pedida a mano) para que el Diseñador de Reportes de DevExpress muestre un Field
// List real. Regla dura de este helper: el SqlDataSource que regresa NUNCA debe quedar pegado al
// reporte al momento de guardar el layout (SaveLayoutToXml serializa cualquier componente
// adjunto, incluida la contraseña de conexión) — quien lo use debe desconectarlo antes de
// guardar y reconectarlo fresco cada vez que hace falta (ver ReporteRecepcionFruta.ConectarOrigenDatos/DesconectarOrigenDatos).
public static class ReporteConexionSql
{
    public static SqlDataSource CrearOrigenDatos(SqlOptions sqlOptions, string nombreConsulta, string storedProcedure, params QueryParameter[] parametros)
    {
        var tipoAutorizacion = sqlOptions.IntegratedSecurity
            ? MsSqlAuthorizationType.Windows
            : MsSqlAuthorizationType.SqlServer;

        var conexion = new MsSqlConnectionParameters(
            sqlOptions.Server,
            sqlOptions.Database,
            sqlOptions.UserId ?? string.Empty,
            sqlOptions.Password ?? string.Empty,
            tipoAutorizacion)
        {
            // Mismo criterio que ConnectionFactory (Dapper): el servidor no tiene certificado
            // confiable, así que hay que aceptar la conexión igual sin dejar de cifrarla.
            Encrypt = sqlOptions.TrustServerCertificate ? DefaultBoolean.True : DefaultBoolean.Default,
            TrustServerCertificate = sqlOptions.TrustServerCertificate ? DefaultBoolean.True : DefaultBoolean.Default,
        };

        var origenDatos = new SqlDataSource("FrontOne", conexion);
        var consulta = new StoredProcQuery(nombreConsulta, storedProcedure, parametros);
        origenDatos.Queries.Add(consulta);
        origenDatos.RebuildResultSchema();

        return origenDatos;
    }
}
