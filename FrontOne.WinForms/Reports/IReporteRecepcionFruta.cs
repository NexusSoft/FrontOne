using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Reports;

// Cualquier reporte registrado en CatalogoReportes para la pantalla "RecepcionesFruta" debe
// implementar esto — así RecepcionesFrutaForm puede cargarle los datos sin importar cuál de los
// varios diseños posibles haya elegido el usuario en el selector de reportes.
public interface IReporteRecepcionFruta
{
    void CargarDatos(RecepcionFrutaReporteDto datos, EmpresaConfiguracionDto empresa);
}
