using DevExpress.DataAccess.Sql;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using FrontOne.Application.Services;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Exceptions;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Etiquetado;

public partial class EtiquetasForm : XtraForm
{
    private const string Modulo = "Etiquetado";
    private const string Pantalla = "Etiquetas";
    private const string AccionConsultar = "Consultar";
    private const string AccionCrear = "Crear";
    private const string AccionModificar = "Modificar";
    private const string AccionEliminar = "Eliminar";
    private const string AccionRecuperar = "Recuperar";

    private readonly EtiquetaService _etiquetaService = null!;
    private readonly PalletService _palletService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly SessionContext _sessionContext = null!;
    private readonly SqlOptions _sqlOptions = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;

    private sealed record FilaEtiqueta(EtiquetaDto Etiqueta, int Id, string Nombre, string Tamano, string Tipo);

    public EtiquetasForm()
    {
        InitializeComponent();
    }

    public EtiquetasForm(
        EtiquetaService etiquetaService,
        PalletService palletService,
        EmpresaConfiguracionService empresaConfiguracionService,
        SessionContext sessionContext,
        SqlOptions sqlOptions,
        LicenciaTecitService licenciaTecitService)
        : this()
    {
        _etiquetaService = etiquetaService;
        _palletService = palletService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _sessionContext = sessionContext;
        _sqlOptions = sqlOptions;
        _licenciaTecitService = licenciaTecitService;

        _btnRecuperarEliminadas.Visible = _sessionContext.TienePermiso(Modulo, Pantalla, AccionRecuperar);
        _btnNuevo.Enabled = _sessionContext.TienePermiso(Modulo, Pantalla, AccionCrear);
        _btnDuplicar.Enabled = _sessionContext.TienePermiso(Modulo, Pantalla, AccionCrear);
        _btnEliminar.Enabled = _sessionContext.TienePermiso(Modulo, Pantalla, AccionEliminar);

        Load += async (_, _) => await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        var etiquetas = await _etiquetaService.ObtenerTodosAsync();
        _grid.DataSource = null;
        _grid.DataSource = etiquetas
            .Select(e => new FilaEtiqueta(e, e.Id, e.Nombre, $"{e.AnchoPulgadas}\" x {e.AltoPulgadas}\"", TextoTipo(e.Tipo)))
            .ToList();
        ConfigurarColumnas();
        _grid.RefreshDataSource();
    }

    private void ConfigurarColumnas()
    {
        if (_gridView.Columns["Etiqueta"] is { } colEtiqueta)
        {
            colEtiqueta.Visible = false;
        }

        if (_gridView.Columns["Id"] is { } colId)
        {
            colId.Caption = "IdEtiqueta";
            colId.VisibleIndex = 0;
        }

        _gridView.BestFitColumns();
    }

    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        using var asistente = new EtiquetaAsistenteForm(_etiquetaService);

        // IdCreado se crea de forma síncrona dentro del wizard (ver
        // EtiquetaAsistenteForm.PageTipo_PageValidating) antes de que WizardControl deje avanzar
        // a Finish, así que para cuando ShowDialog regresa aquí, ya es un valor confiable.
        asistente.ShowDialog(this);
        if (asistente.IdCreado is not { } idCreado)
        {
            return;
        }

        // Abrir un segundo diálogo modal (el Diseñador) en la MISMA pasada de mensajes en que se
        // acaba de cerrar el primero (el asistente) no lo muestra — reentrancia de modales
        // encadenados de Windows/WinForms (confirmado: "Editar formato", que abre el mismo
        // Diseñador pero SIN un modal recién cerrado justo antes, sí funciona). BeginInvoke lo
        // agenda para la siguiente vuelta del message loop, con el modal anterior ya desmontado
        // por completo.
        BeginInvoke(async () => await CrearYAbrirDisenadorAsync(idCreado));
    }

    private async Task CrearYAbrirDisenadorAsync(int idCreado)
    {
        try
        {
            var etiqueta = await _etiquetaService.ObtenerPorIdAsync(idCreado);
            if (etiqueta is not null)
            {
                await AbrirDisenadorAsync(etiqueta);
            }
        }
        catch (Exception ex)
        {
            // Nunca dejar que un error en este flujo desaparezca en silencio — antes se perdía
            // (la etiqueta se creaba en BD pero la pantalla se quedaba igual, sin aviso).
            XtraMessageBox.Show(this, $"Ocurrió un error creando la etiqueta.\n\n{ex.Message}",
                "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // Garantizado siempre — si el asistente llegó a crear el registro, debe verse en el
            // listado sin importar qué pase después (aunque el Diseñador falle al abrir).
            await CargarDatosAsync();
        }
    }

    private async void BtnDuplicar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new EtiquetaDuplicarForm(_etiquetaService, seleccionado.Id);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEditarFormato_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!_sessionContext.TienePermisoReporte(CodigoTipo(seleccionado.Etiqueta.Tipo), AccionReporte.Diseno))
        {
            XtraMessageBox.Show(this, "No tienes permiso para diseñar este tipo de etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        await AbrirDisenadorAsync(seleccionado.Etiqueta);
        await CargarDatosAsync();
    }

    private async Task AbrirDisenadorAsync(EtiquetaDto etiqueta)
    {
        var reporte = EtiquetaReporte.Crear(etiqueta);
        var origenes = new List<SqlDataSource>();

        // Armar el XRDesignForm + el reporte de referencia tarda varios segundos — el splash
        // cubre ese hueco y se cierra justo antes de que la ventana del Diseñador se haga
        // visible (nunca después: cerrarlo en el finally de abajo es solo un respaldo por si
        // algo truena antes de llegar ahí, useFadeIn:false evita la carrera de DevExpress donde
        // CloseDefaultWaitForm avienta "Splash Form is not displayed" si ya se cerró).
        var splashCerrado = false;
        void CerrarSplash()
        {
            if (splashCerrado)
            {
                return;
            }

            splashCerrado = true;
            SplashScreenManager.CloseDefaultWaitForm();
        }

        SplashScreenManager.ShowDefaultWaitForm(this, useFadeIn: false, useFadeOut: true, "FrontOne", "Generando Etiqueta...");
        try
        {
            await DisenadorReporteForm.MostrarAsync(
                this,
                async () => (await _etiquetaService.ObtenerPorIdAsync(etiqueta.Id))?.DefinicionXml,
                xml => _etiquetaService.GuardarLayoutAsync(etiqueta.Id, xml),
                reporte,
                r => ConectarOrigenDatosDiseno(r, etiqueta.Tipo, origenes),
                r => DesconectarOrigenDatosDiseno(r, origenes),
                _licenciaTecitService,
                antesDeMostrar: CerrarSplash);
        }
        finally
        {
            CerrarSplash();
        }
    }

    // Field List del Diseñador: se conecta contra el/los SP(s) reales con @PalletId=0 (solo para
    // que arme el esquema de columnas, nunca datos reales) — mismo criterio que ConectarOrigenDatos
    // en ReportesForm. Además de dejar los orígenes disponibles en ComponentStorage, el reporte se
    // enlaza con DataSource/DataMember a la consulta "principal" de cada tipo — sin esto DevExpress
    // antepone el nombre de la consulta a cualquier campo que el usuario arrastre del Field List
    // ([EtiquetaCaja].[Campo] en vez de [Campo]), y esa ruta con prefijo nunca resuelve en runtime
    // (PalletImprimirEtiquetaForm/EtiquetaVistaPreviaForm siempre asignan un DataSource plano, sin
    // DataMember) — bug real ya corregido a mano una vez en la etiqueta "Trazabilidad Mission".
    private void ConectarOrigenDatosDiseno(XtraReport reporte, TipoEtiqueta tipo, List<SqlDataSource> origenes)
    {
        SqlDataSource origenPrincipal;
        string dataMemberPrincipal;

        switch (tipo)
        {
            case TipoEtiqueta.Caja:
                origenPrincipal = ReporteConexionSql.CrearOrigenDatos(_sqlOptions, "EtiquetaCaja",
                    "Produccion.sp_Pallet_ObtenerEtiquetaCaja", new QueryParameter("@PalletId", typeof(int), 0));
                dataMemberPrincipal = "EtiquetaCaja";
                origenes.Add(origenPrincipal);
                break;
            case TipoEtiqueta.Pallet:
                // Un solo SP de diseño (sp_Pallet_ObtenerEtiquetaPalletEncabezadoDiseno) que une
                // encabezado + membrete de empresa en una sola fila, con los mismos alias que
                // PalletImprimirEtiquetaForm.VistaPalletEncabezado — así TODOS esos campos salen
                // planos al arrastrarlos, no solo los de encabezado. El SP real que usa runtime
                // (sin Empresa) no se toca, este es exclusivo del Diseñador.
                origenPrincipal = ReporteConexionSql.CrearOrigenDatos(_sqlOptions, "EtiquetaPalletEncabezado",
                    "Produccion.sp_Pallet_ObtenerEtiquetaPalletEncabezadoDiseno", new QueryParameter("@PalletId", typeof(int), 0));
                dataMemberPrincipal = "EtiquetaPalletEncabezado";
                origenes.Add(origenPrincipal);
                origenes.Add(ReporteConexionSql.CrearOrigenDatos(_sqlOptions, "EtiquetaPalletDetalle",
                    "Produccion.sp_Pallet_ObtenerEtiquetaPalletDetalle", new QueryParameter("@PalletId", typeof(int), 0)));
                break;
            case TipoEtiqueta.RegistroSagarpa:
                origenPrincipal = ReporteConexionSql.CrearOrigenDatos(_sqlOptions, "EtiquetaSagarpa",
                    "Produccion.sp_Pallet_ObtenerEtiquetaSagarpa", new QueryParameter("@PalletId", typeof(int), 0));
                dataMemberPrincipal = "EtiquetaSagarpa";
                origenes.Add(origenPrincipal);
                break;
            default:
                throw new InvalidOperationException($"Tipo de etiqueta no soportado: {tipo}");
        }

        foreach (var origen in origenes)
        {
            reporte.ComponentStorage.Add(origen);
        }

        reporte.DataSource = origenPrincipal;
        reporte.DataMember = dataMemberPrincipal;
    }

    private static void DesconectarOrigenDatosDiseno(XtraReport reporte, List<SqlDataSource> origenes)
    {
        // Se limpia ANTES de remover/disponer los orígenes — el SqlDataSource (con la contraseña
        // de conexión) nunca debe quedar serializado en el DefinicionXml que se guarda
        // (SaveLayoutToXml se llama justo después de esto, ver DisenadorReporteForm).
        reporte.DataSource = null;
        reporte.DataMember = null;

        foreach (var origen in origenes)
        {
            reporte.ComponentStorage.Remove(origen);
            origen.Dispose();
        }

        origenes.Clear();
    }

    private async void BtnVistaPrevia_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new EtiquetaVistaPreviaForm(
            seleccionado.Etiqueta, _palletService, _empresaConfiguracionService, _licenciaTecitService, _sessionContext);
        form.ShowDialog(this);
        await Task.CompletedTask;
    }

    private async void BtnEliminar_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null)
        {
            XtraMessageBox.Show(this, "Selecciona una etiqueta.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = XtraMessageBox.Show(this, $"¿Eliminar la etiqueta '{seleccionado.Nombre}'?", "FrontOne",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _etiquetaService.EliminarAsync(seleccionado.Id);
            await CargarDatosAsync();
        }
        catch (SqlRepositoryException ex)
        {
            XtraMessageBox.Show(this, ex.Message, "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnRecuperarEliminadas_Click(object? sender, EventArgs e)
    {
        using var form = new EtiquetasEliminadasForm(_etiquetaService);
        form.ShowDialog(this);
        await CargarDatosAsync();
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();

    private FilaEtiqueta? ObtenerSeleccionado()
        => _gridView.GetFocusedRow() as FilaEtiqueta;

    private static string TextoTipo(TipoEtiqueta tipo) => tipo switch
    {
        TipoEtiqueta.Caja => "Caja",
        TipoEtiqueta.Pallet => "Pallet",
        TipoEtiqueta.RegistroSagarpa => "Registro Sagarpa",
        _ => tipo.ToString(),
    };

    // Código pseudo-fijo usado solo para el permiso de reporte (Seguridad.ReportePermiso) y para
    // VisorReporteForm — ver Database/Seguridad/041_Seed_Modulo_Etiquetado.sql.
    internal static string CodigoTipo(TipoEtiqueta tipo) => tipo switch
    {
        TipoEtiqueta.Caja => "EtiquetaCaja",
        TipoEtiqueta.Pallet => "EtiquetaPallet",
        TipoEtiqueta.RegistroSagarpa => "EtiquetaSagarpa",
        _ => throw new InvalidOperationException($"Tipo de etiqueta no soportado: {tipo}"),
    };
}
