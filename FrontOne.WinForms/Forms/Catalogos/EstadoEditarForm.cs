using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class EstadoEditarForm : XtraForm
{
    private readonly EstadoService _estadoService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoDto? _estadoExistente;

    public EstadoEditarForm()
    {
        InitializeComponent();
    }

    public EstadoEditarForm(EstadoService estadoService, PaisService paisService, EstadoDto? estadoExistente)
        : this()
    {
        _estadoService = estadoService;
        _paisService = paisService;
        _estadoExistente = estadoExistente;

        Text = estadoExistente is null ? "FrontOne - Nuevo estado" : "FrontOne - Editar estado";

        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPais.ButtonClick += CmbPais_ButtonClick;

        Load += async (_, _) =>
        {
            await CargarPaisesAsync();

            if (_estadoExistente is not null)
            {
                _cmbPais.EditValue = _estadoExistente.PaisId;
                _txtClave.Text = _estadoExistente.Clave;
                _txtNombre.Text = _estadoExistente.Nombre;
                _chkActivo.Checked = _estadoExistente.Activo;
            }
        };
    }

    private async Task CargarPaisesAsync()
    {
        var paises = await _paisService.ObtenerAsync();

        _cmbPais.Properties.DataSource = paises.ToList();
        _cmbPais.Properties.ValueMember = "Id";
        _cmbPais.Properties.DisplayMember = "Nombre";
        _cmbPais.Properties.Columns.Clear();
        _cmbPais.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbPais.Properties.PopupWidth = 250;
    }

    private async void CmbPais_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);
        await CargarPaisesAsync();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbPais.EditValue is not int paisId)
        {
            XtraMessageBox.Show(this, "Selecciona un país.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (_estadoExistente is null)
            {
                await _estadoService.CrearAsync(paisId, _txtClave.Text, _txtNombre.Text);
            }
            else
            {
                await _estadoService.ActualizarAsync(_estadoExistente.Id, paisId, _txtClave.Text, _txtNombre.Text, _chkActivo.Checked);
            }

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
}
