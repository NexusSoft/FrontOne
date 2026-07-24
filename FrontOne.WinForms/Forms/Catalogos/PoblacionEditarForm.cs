using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Catalogos;

public partial class PoblacionEditarForm : XtraForm
{
    private readonly PoblacionService _poblacionService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly PoblacionDto? _poblacionExistente;
    private readonly int? _municipioIdPreseleccionado;

    public PoblacionEditarForm()
    {
        InitializeComponent();
    }

    public PoblacionEditarForm(
        PoblacionService poblacionService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        PoblacionDto? poblacionExistente,
        int? municipioIdPreseleccionado = null)
        : this()
    {
        _poblacionService = poblacionService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _poblacionExistente = poblacionExistente;
        _municipioIdPreseleccionado = municipioIdPreseleccionado;

        Text = poblacionExistente is null ? "FrontOne - Nueva población" : "FrontOne - Editar población";

        if (poblacionExistente is not null)
        {
            _txtNombre.Text = poblacionExistente.Nombre;
            _chkActivo.Checked = poblacionExistente.Activo;
        }
        else
        {
            _chkActivo.Checked = true;
        }

        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPais.ButtonClick += CmbPais_ButtonClick;

        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbEstado.ButtonClick += CmbEstado_ButtonClick;

        _cmbMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbMunicipio.ButtonClick += CmbMunicipio_ButtonClick;

        Load += async (_, _) => await CargarDatosIniciales();
    }

    private async Task CargarDatosIniciales()
    {
        var paises = await _paisService.ObtenerAsync();
        _cmbPais.Properties.DataSource = paises.ToList();
        _cmbPais.Properties.ValueMember = "Id";
        _cmbPais.Properties.DisplayMember = "Nombre";
        _cmbPais.Properties.Columns.Clear();
        _cmbPais.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "País"));
        _cmbPais.Properties.PopupWidth = 250;

        int? municipioId = _poblacionExistente?.MunicipioId ?? _municipioIdPreseleccionado;

        if (municipioId is not null)
        {
            var municipios = await _municipioService.ObtenerAsync();
            var municipio = municipios.FirstOrDefault(m => m.Id == municipioId);
            if (municipio is not null)
            {
                var estados = await _estadoService.ObtenerAsync();
                var estado = estados.FirstOrDefault(e => e.Id == municipio.EstadoId);
                if (estado is not null)
                {
                    _cmbPais.EditValue = estado.PaisId;
                    await CargarEstadosDelPaisAsync();
                    _cmbEstado.EditValue = estado.Id;
                    await CargarMunicipiosDelEstadoAsync();
                    _cmbMunicipio.EditValue = municipio.Id;
                }
            }
        }
    }

    private async void CmbPais_EditValueChanged(object? sender, EventArgs e) => await CargarEstadosDelPaisAsync();

    private async void CmbEstado_EditValueChanged(object? sender, EventArgs e) => await CargarMunicipiosDelEstadoAsync();

    private async Task CargarEstadosDelPaisAsync()
    {
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
    }

    private async Task CargarMunicipiosDelEstadoAsync()
    {
        if (_cmbEstado.EditValue is not int estadoId)
        {
            _cmbMunicipio.Properties.DataSource = null;
            _cmbMunicipio.EditValue = null;
            return;
        }

        var municipios = await _municipioService.ObtenerAsync(estadoId);
        _cmbMunicipio.Properties.DataSource = municipios.ToList();
        _cmbMunicipio.Properties.ValueMember = "Id";
        _cmbMunicipio.Properties.DisplayMember = "Nombre";
        _cmbMunicipio.Properties.Columns.Clear();
        _cmbMunicipio.Properties.Columns.Add(new LookUpColumnInfo("Nombre", 220, "Municipio"));
        _cmbMunicipio.Properties.PopupWidth = 250;
    }

    private async void CmbPais_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        var paisSeleccionado = _cmbPais.EditValue;
        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);

        var paises = await _paisService.ObtenerAsync();
        _cmbPais.Properties.DataSource = paises.ToList();
        _cmbPais.EditValue = paisSeleccionado;
    }

    private async void CmbEstado_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        var estadoSeleccionado = _cmbEstado.EditValue;
        using var form = new EstadosForm(_estadoService, _paisService);
        form.ShowDialog(this);
        await CargarEstadosDelPaisAsync();
        _cmbEstado.EditValue = estadoSeleccionado;
    }

    private async void CmbMunicipio_ButtonClick(object? sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind != ButtonPredefines.Plus)
        {
            return;
        }

        var municipioSeleccionado = _cmbMunicipio.EditValue;
        using var form = new MunicipiosForm(_municipioService, _paisService, _estadoService);
        form.ShowDialog(this);
        await CargarMunicipiosDelEstadoAsync();
        _cmbMunicipio.EditValue = municipioSeleccionado;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_cmbMunicipio.EditValue is not int municipioId)
        {
            XtraMessageBox.Show(this, "Selecciona un municipio.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (_poblacionExistente is null)
            {
                await _poblacionService.CrearAsync(_txtNombre.Text, municipioId);
            }
            else
            {
                await _poblacionService.ActualizarAsync(_poblacionExistente.Id, _txtNombre.Text, municipioId, _chkActivo.Checked);
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
