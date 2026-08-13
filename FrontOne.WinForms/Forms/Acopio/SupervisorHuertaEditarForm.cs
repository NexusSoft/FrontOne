using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

public partial class SupervisorHuertaEditarForm : XtraForm
{
    private readonly SupervisorHuertaService _supervisorHuertaService = null!;
    private readonly SupervisorHuertaDto? _supervisorHuertaExistente;

    public SupervisorHuertaEditarForm()
    {
        InitializeComponent();
    }

    public SupervisorHuertaEditarForm(SupervisorHuertaService supervisorHuertaService, SupervisorHuertaDto? supervisorHuertaExistente)
        : this()
    {
        _supervisorHuertaService = supervisorHuertaService;
        _supervisorHuertaExistente = supervisorHuertaExistente;

        Text = supervisorHuertaExistente is null ? "FrontOne - Nuevo supervisor de huerta" : "FrontOne - Editar supervisor de huerta";

        if (supervisorHuertaExistente is not null)
        {
            _txtNombre.Text = supervisorHuertaExistente.Nombre;
            _chkActivo.Checked = supervisorHuertaExistente.Activo;
        }
        else
        {
            _chkActivo.Checked = true;
        }
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_supervisorHuertaExistente is null)
            {
                await _supervisorHuertaService.CrearAsync(_txtNombre.Text);
            }
            else
            {
                await _supervisorHuertaService.ActualizarAsync(_supervisorHuertaExistente.Id, _txtNombre.Text, _chkActivo.Checked);
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
