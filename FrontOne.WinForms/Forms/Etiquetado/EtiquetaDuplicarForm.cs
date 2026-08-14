using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Etiquetado;

public partial class EtiquetaDuplicarForm : XtraForm
{
    private readonly EtiquetaService _etiquetaService = null!;
    private readonly int _idOrigen;

    public EtiquetaDuplicarForm()
    {
        InitializeComponent();
    }

    public EtiquetaDuplicarForm(EtiquetaService etiquetaService, int idOrigen)
        : this()
    {
        _etiquetaService = etiquetaService;
        _idOrigen = idOrigen;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNombreNuevo.Text))
        {
            XtraMessageBox.Show(this, "Captura el nombre de la nueva etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            await _etiquetaService.DuplicarAsync(_idOrigen, _txtNombreNuevo.Text.Trim());
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidationException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
