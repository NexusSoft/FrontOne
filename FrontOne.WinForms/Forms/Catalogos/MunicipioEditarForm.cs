using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class MunicipioEditarForm : XtraForm
{
    private readonly MunicipioService _municipioService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioDto? _municipioExistente;

    public MunicipioEditarForm()
    {
        InitializeComponent();
    }

    public MunicipioEditarForm(MunicipioService municipioService, PaisService paisService, EstadoService estadoService, MunicipioDto? municipioExistente)
        : this()
    {
        _municipioService = municipioService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioExistente = municipioExistente;

        Text = municipioExistente is null ? "FrontOne - Nuevo municipio" : "FrontOne - Editar municipio";

        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPais.ButtonClick += CmbPais_ButtonClick;

        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbEstado.ButtonClick += CmbEstado_ButtonClick;

        _cmbPais.EditValueChanged += async (_, _) => await CargarEstadosAsync(seleccionarPrimero: true);

        Load += async (_, _) =>
        {
            await CargarPaisesAsync();

            if (_municipioExistente is not null)
            {
                var estados = await _estadoService.ObtenerAsync();
                var estado = estados.FirstOrDefault(e => e.Id == _municipioExistente.EstadoId);

                _cmbPais.EditValue = estado?.PaisId;
                await CargarEstadosAsync(seleccionarPrimero: false);
                _cmbEstado.EditValue = _municipioExistente.EstadoId;

                _txtNombre.Text = _municipioExistente.Nombre;
                _chkActivo.Checked = _municipioExistente.Activo;
            }
        };
    }

    private async Task CargarPaisesAsync()
    {
        var paisSeleccionado = _cmbPais.EditValue;

        var paises = await _paisService.ObtenerAsync();

        _cmbPais.Properties.DataSource = paises.ToList();
        _cmbPais.Properties.ValueMember = "Id";
        _cmbPais.Properties.DisplayMember = "Nombre";
        _cmbPais.Properties.Columns.Clear();
        _cmbPais.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbPais.Properties.PopupWidth = 250;
        _cmbPais.EditValue = paisSeleccionado;
    }

    private async Task CargarEstadosAsync(bool seleccionarPrimero)
    {
        var estadoSeleccionado = _cmbEstado.EditValue;

        if (_cmbPais.EditValue is not int paisId)
        {
            _cmbEstado.Properties.DataSource = null;
            _cmbEstado.EditValue = null;
            return;
        }

        var estados = await _estadoService.ObtenerAsync(paisId);
        _cmbEstado.Properties.DataSource = estados.ToList();
        _cmbEstado.Properties.ValueMember = "Id";
        _cmbEstado.Properties.DisplayMember = "Nombre";
        _cmbEstado.Properties.Columns.Clear();
        _cmbEstado.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Estado"));
        _cmbEstado.Properties.PopupWidth = 250;

        if (seleccionarPrimero)
        {
            _cmbEstado.EditValue = estados.Count > 0 ? estados[0].Id : null;
        }
        else
        {
            _cmbEstado.EditValue = estadoSeleccionado;
        }
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

    private async void CmbEstado_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        using var form = new EstadosForm(_estadoService, _paisService);
        form.ShowDialog(this);
        await CargarEstadosAsync(seleccionarPrimero: false);
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbEstado.EditValue is not int estadoId)
        {
            XtraMessageBox.Show(this, "Selecciona un estado.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (_municipioExistente is null)
            {
                await _municipioService.CrearAsync(estadoId, _txtNombre.Text);
            }
            else
            {
                await _municipioService.ActualizarAsync(_municipioExistente.Id, estadoId, _txtNombre.Text, _chkActivo.Checked);
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
