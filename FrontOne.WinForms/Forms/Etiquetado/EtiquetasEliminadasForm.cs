using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;

namespace FrontOne.WinForms.Forms.Etiquetado;

public partial class EtiquetasEliminadasForm : XtraForm
{
    private readonly EtiquetaService _etiquetaService = null!;

    private sealed record FilaEtiqueta(int Id, string Nombre, string Tamano, string Tipo, DateTime FechaModificacion);

    public EtiquetasEliminadasForm()
    {
        InitializeComponent();
    }

    public EtiquetasEliminadasForm(EtiquetaService etiquetaService)
        : this()
    {
        _etiquetaService = etiquetaService;

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var etiquetas = await _etiquetaService.ObtenerEliminadosAsync();
        _grid.DataSource = etiquetas
            .Select(e => new FilaEtiqueta(e.Id, e.Nombre, $"{e.AnchoPulgadas}\" x {e.AltoPulgadas}\"", TextoTipo(e.Tipo), e.FechaModificacion))
            .ToList();
        _gridView.BestFitColumns();
    }

    private async void BtnRecuperar_Click(object? sender, EventArgs e)
    {
        var seleccionado = _gridView.GetFocusedRow() as FilaEtiqueta;
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Recuperar la etiqueta '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        await _etiquetaService.RecuperarAsync(seleccionado.Id);
        await CargarDatosAsync();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private static string TextoTipo(TipoEtiqueta tipo) => tipo switch
    {
        TipoEtiqueta.Caja => "Caja",
        TipoEtiqueta.Pallet => "Pallet",
        TipoEtiqueta.RegistroSagarpa => "Registro Sagarpa",
        _ => tipo.ToString(),
    };
}
