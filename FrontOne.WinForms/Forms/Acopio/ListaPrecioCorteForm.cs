using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Shared.Exceptions;

namespace FrontOne.WinForms.Forms.Acopio;

// Captura tipo Excel: el grid es editable directo (celda por celda) solo en los precios y Kg
// Mínimos — CardCode/CardName son de solo lectura, los trae "Actualizar de SAP" desde el grupo
// de proveedores Cosecha. El catálogo lo controla SAP por completo: no hay alta/baja manual
// desde este form, solo captura de precios sobre lo que ya existe en SAP.
public partial class ListaPrecioCorteForm : XtraForm
{
    private readonly ListaPrecioCorteService _listaPrecioCorteService = null!;
    private BindingList<ListaPrecioCorteFila> _filas = new();

    public ListaPrecioCorteForm()
    {
        InitializeComponent();
    }

    public ListaPrecioCorteForm(ListaPrecioCorteService listaPrecioCorteService)
        : this()
    {
        _listaPrecioCorteService = listaPrecioCorteService;
    }

    // Siempre carga primero lo ya capturado en FrontOne; solo si no hay nada capturado todavía
    // dispara la sincronización con SAP automáticamente, para que el grid no se vea vacío la
    // primera vez que se usa el módulo.
    private async void ListaPrecioCorteForm_Load(object? sender, EventArgs e)
    {
        await CargarDatosAsync();

        if (_filas.Count == 0)
        {
            await SincronizarConSapAsync(mostrarResultado: false);
        }
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var lista = await _listaPrecioCorteService.ObtenerAsync();
            _filas = new BindingList<ListaPrecioCorteFila>(lista.Select(l => new ListaPrecioCorteFila
            {
                Id = l.Id,
                CardCode = l.CardCode,
                CardName = l.CardName,
                PrecioKg = l.PrecioKg,
                PrecioDia = l.PrecioDia,
                CuadrillaApoyo = l.CuadrillaApoyo,
            }).ToList());
            _grid.DataSource = _filas;
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e) => await SincronizarConSapAsync(mostrarResultado: true);

    // Muestra el wait form estándar de DevExpress mientras dura la consulta a SAP, igual que
    // ListaPrecioFrutaForm, para que no parezca que la app se congeló.
    private async Task SincronizarConSapAsync(bool mostrarResultado)
    {
        _btnActualizar.Enabled = false;
        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: true, useFadeOut: true, "FrontOne", "Consultando SAP...");

        SincronizacionListaPrecioCorteResultado? resultado = null;
        SapException? error = null;
        try
        {
            resultado = await _listaPrecioCorteService.SincronizarConSapAsync();
        }
        catch (SapException ex)
        {
            error = ex;
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
            _btnActualizar.Enabled = true;
        }

        if (error is not null)
        {
            XtraMessageBox.Show(this, $"No se pudo consultar SAP Business One.\n\n{error.Message}", "FrontOne",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        await CargarDatosAsync();

        if (mostrarResultado && resultado is not null)
        {
            XtraMessageBox.Show(this,
                $"Sincronización con SAP completada: {resultado.Nuevos} proveedor(es) nuevo(s), " +
                $"{resultado.Actualizados} nombre(s) actualizado(s), {resultado.Reactivados} reactivado(s), " +
                $"{resultado.Desactivados} deshabilitado(s).",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        _gridView.CloseEditor();
        _gridView.UpdateCurrentRow();

        foreach (var fila in _filas.ToList())
        {
            try
            {
                await _listaPrecioCorteService.ActualizarPreciosAsync(fila.Id, fila.PrecioKg, fila.PrecioDia, fila.CuadrillaApoyo);
            }
            catch (ValidationException ex)
            {
                XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (SqlRepositoryException ex)
            {
                XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        await CargarDatosAsync();
        XtraMessageBox.Show(this, "Cambios guardados.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
