using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraWizard;
using FrontOne.Application.Services;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Etiquetado;

public partial class EtiquetaAsistenteForm : XtraForm
{
    private readonly EtiquetaService _etiquetaService = null!;

    public int? IdCreado { get; private set; }

    public EtiquetaAsistenteForm()
    {
        InitializeComponent();
    }

    public EtiquetaAsistenteForm(EtiquetaService etiquetaService)
        : this()
    {
        _etiquetaService = etiquetaService;

        _rdgTipo.Properties.Items.AddRange(new RadioGroupItem[]
        {
            new(TipoEtiqueta.Caja, "Caja"),
            new(TipoEtiqueta.Pallet, "Pallet"),
            new(TipoEtiqueta.RegistroSagarpa, "Registro Sagarpa"),
        });
        _rdgTipo.EditValue = TipoEtiqueta.Caja;
    }

    private void PageNombre_PageValidating(object? sender, WizardPageValidatingEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNombre.Text))
        {
            e.Valid = false;
            e.ErrorText = "Captura el nombre de la etiqueta.";
            return;
        }

        // La validación de unicidad real (contra otras etiquetas activas) se hace en el
        // servicio al finalizar el asistente — aquí solo se valida que no venga vacío, para no
        // bloquear la navegación del wizard con una llamada async por cada tecla.
        e.Valid = true;
    }

    private void PageTamano_PageValidating(object? sender, WizardPageValidatingEventArgs e)
    {
        var ancho = _spnAncho.Value;
        var alto = _spnAlto.Value;

        if (ancho <= 0 || alto <= 0)
        {
            e.Valid = false;
            e.ErrorText = "El ancho y el alto deben ser mayores a cero.";
            return;
        }

        e.Valid = true;
    }

    // Crea la etiqueta AQUÍ (síncrono, bloqueante) en vez de en Wizard_FinishClick: el
    // WizardControl cierra el Form host en cuanto se hace clic en Finish sin esperar a que
    // termine un handler async void — confirmado con logs, la etiqueta se llegaba a crear en BD
    // pero medio minuto después de que ShowDialog ya había regresado con IdCreado en null (el
    // caller ya había leído/descartado el resultado). PageValidating sí es síncrono y bloquea la
    // navegación del wizard hasta que retorna, así que aquí la creación SIEMPRE termina antes de
    // que WizardControl pueda avanzar a Finish/cerrar. Task.Run + GetAwaiter().GetResult() es
    // seguro (no hay deadlock) porque EtiquetaService/EtiquetaRepository no necesitan volver al
    // hilo de UI en ningún punto de su cadena de await.
    private void PageTipo_PageValidating(object? sender, WizardPageValidatingEventArgs e)
    {
        if (_rdgTipo.EditValue is not TipoEtiqueta tipo)
        {
            e.Valid = false;
            e.ErrorText = "Selecciona el tipo de etiqueta.";
            return;
        }

        var nombre = _txtNombre.Text.Trim();
        var ancho = _spnAncho.Value;
        var alto = _spnAlto.Value;

        try
        {
            IdCreado = Task.Run(() => _etiquetaService.CrearAsync(nombre, ancho, alto, tipo)).GetAwaiter().GetResult();
            e.Valid = true;
        }
        catch (ValidationException ex)
        {
            e.Valid = false;
            e.ErrorText = ex.Message;
        }
    }

    private void Wizard_FinishClick(object? sender, EventArgs e)
    {
        // La etiqueta ya se creó en PageTipo_PageValidating (WizardControl no deja avanzar a
        // Finish si esa validación no pasó) — aquí solo queda cerrar con el resultado correcto.
        DialogResult = DialogResult.OK;
        Close();
    }

    private void Wizard_CancelClick(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
