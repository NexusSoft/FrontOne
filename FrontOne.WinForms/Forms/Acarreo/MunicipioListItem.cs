namespace FrontOne.WinForms.Forms.Acarreo;

// Fuente del LookUpEdit de Municipio: combina Municipio+Estado solo para mostrar la columna
// Estado en el desplegable — el valor seleccionado sigue siendo el Id del Municipio.
public record MunicipioListItem(int Id, string EstadoNombre, string MunicipioNombre);
