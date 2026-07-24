using DevExpress.XtraGrid.Localization;

namespace FrontOne.WinForms.Configuration;

// Textos en español para los grids de DevExpress (panel de búsqueda Find, etc.).
// Se registra una sola vez en Program.cs y aplica a todos los GridView del proyecto.
public class GridLocalizerEspanol : GridLocalizer
{
    public override string Language => "Español";

    public override string GetLocalizedString(GridStringId id) => id switch
    {
        GridStringId.FindControlFindButton => "Buscar",
        GridStringId.FindControlClearButton => "Limpiar",
        GridStringId.FindControlNextButton => "Siguiente",
        GridStringId.FindControlPrevButton => "Anterior",
        GridStringId.FindNullPrompt => "Escribe el texto a buscar...",
        _ => base.GetLocalizedString(id),
    };
}
