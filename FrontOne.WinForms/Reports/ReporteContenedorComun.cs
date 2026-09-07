using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Reports;

// Encabezado de una sola fila (Contenedor + Empresa) compartido por los 7 reportes de Carga de
// Contenedor — mismo membrete y mismo bloque "Orden de Empaque/Fecha/Pedido/Cliente" en los 7,
// solo cambia el cuerpo (agrupado por pallet/lote/huerta o resumen ya agregado).
public sealed record VistaEncabezadoContenedor(ContenedorDto Encabezado, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo);

// Piezas de layout reutilizadas por los Designer.cs de ReporteContenedorCarga/DetallePorLote/
// DetallePorHuerta/ResumenCalibre/ResumenLote/ResumenHuerta/ResumenHuertaSinKg — evita repetir la
// construcción del membrete y el bloque de datos del contenedor en cada archivo.
public static class ReporteContenedorComun
{
    public static VistaEncabezadoContenedor ArmarVista(ContenedorDto encabezado, EmpresaConfiguracionDto empresa) => new(
        encabezado,
        empresa,
        string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
        string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))));

    public static XRLabel CrearEtiqueta(string texto, bool negrita = true) => new()
    {
        Text = texto,
        Font = new DXFont("Arial", 9, negrita ? DXFontStyle.Bold : DXFontStyle.Regular),
    };

    // Logo + razón social/domicilio/RFC/teléfono, mismo layout que ReporteRecepcionFruta/ReportePallet.
    public static XRControl[] CrearMembrete(string tituloTexto)
    {
        var logo = new XRPictureBox { LocationFloat = new PointFloat(0, 0), SizeF = new System.Drawing.SizeF(140, 60), Sizing = ImageSizeMode.ZoomImage };
        var razonSocial = new XRLabel { LocationFloat = new PointFloat(400, 0), SizeF = new System.Drawing.SizeF(372, 16), Font = new DXFont("Arial", 9, DXFontStyle.Bold) };
        var domicilio = new XRLabel { LocationFloat = new PointFloat(400, 16), SizeF = new System.Drawing.SizeF(372, 14) };
        var rfc = new XRLabel { LocationFloat = new PointFloat(400, 30), SizeF = new System.Drawing.SizeF(372, 14) };
        var telefonoCorreo = new XRLabel { LocationFloat = new PointFloat(400, 44), SizeF = new System.Drawing.SizeF(372, 14) };
        var titulo = new XRLabel
        {
            LocationFloat = new PointFloat(0, 68),
            SizeF = new System.Drawing.SizeF(772, 20),
            Font = new DXFont("Arial", 13, DXFontStyle.Bold),
            TextAlignment = TextAlignment.MiddleCenter,
            Text = tituloTexto,
        };
        var linea = new XRLine { LocationFloat = new PointFloat(0, 92), SizeF = new System.Drawing.SizeF(772, 6) };

        razonSocial.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.RazonSocial]"));
        domicilio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.Domicilio]"));
        rfc.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Rfc]"));
        telefonoCorreo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TelefonoCorreo]"));
        logo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "ImageSource", "Iif(IsNullOrEmpty([Empresa.Logo]), Null, [Empresa.Logo])"));

        return new XRControl[] { logo, razonSocial, domicilio, rfc, telefonoCorreo, titulo, linea };
    }

    // Bloque "Orden de Empaque / Fecha de Empaque / No. de Pedido / Folio del Pedido / Cliente",
    // idéntico en los 7 reportes, a partir de la coordenada Y indicada.
    public static XRControl[] CrearBloqueEncabezadoContenedor(float y)
    {
        var etqOrden = CrearEtiqueta("Orden de Empaque:");
        var valOrden = new XRLabel();
        var etqFecha = CrearEtiqueta("Fecha de Empaque:");
        var valFecha = new XRLabel { TextFormatString = "{0:dd/MM/yyyy}" };
        var etqPedido = CrearEtiqueta("No. de Pedido:");
        var valPedido = new XRLabel();
        var etqFolio = CrearEtiqueta("Folio del Pedido:");
        var valFolio = new XRLabel();
        var etqCliente = CrearEtiqueta("Cliente:");
        var valCliente = new XRLabel();

        UbicarPar(etqOrden, valOrden, 0, y, 110, 150);
        UbicarPar(etqFecha, valFecha, 400, y, 110, 150);
        UbicarPar(etqPedido, valPedido, 0, y + 18, 110, 150);
        UbicarPar(etqFolio, valFolio, 400, y + 18, 110, 150);
        UbicarPar(etqCliente, valCliente, 0, y + 36, 110, 400);

        valOrden.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Encabezado.Folio]"));
        valFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Encabezado.Fecha]"));
        valPedido.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Encabezado.SapDocNum]"));
        valFolio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Encabezado.FolioFronterra]"));
        valCliente.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Encabezado.CardName]"));

        return new XRControl[] { etqOrden, valOrden, etqFecha, valFecha, etqPedido, valPedido, etqFolio, valFolio, etqCliente, valCliente };
    }

    public static void UbicarPar(XRLabel etiqueta, XRLabel valor, float x, float y, float anchoEtiqueta, float anchoValor)
    {
        etiqueta.LocationFloat = new PointFloat(x, y);
        etiqueta.SizeF = new System.Drawing.SizeF(anchoEtiqueta, 16);
        valor.LocationFloat = new PointFloat(x + anchoEtiqueta + 5, y);
        valor.SizeF = new System.Drawing.SizeF(anchoValor, 16);
    }

    public static XRLabel CrearColumna(string texto, float x, float y, float ancho) => new()
    {
        Text = texto,
        Font = new DXFont("Arial", 8, DXFontStyle.Bold),
        LocationFloat = new PointFloat(x, y),
        SizeF = new System.Drawing.SizeF(ancho, 16),
    };

    public static XRLabel CrearCelda(float x, float y, float ancho, bool alinearDerecha = false)
    {
        var lbl = new XRLabel
        {
            Font = new DXFont("Arial", 8),
            LocationFloat = new PointFloat(x, y),
            SizeF = new System.Drawing.SizeF(ancho, 16),
        };
        if (alinearDerecha)
        {
            lbl.TextAlignment = TextAlignment.MiddleRight;
        }

        return lbl;
    }
}
