// Exportación a PDF (vía impresión del navegador) e Imagen (PNG dibujado a mano en un canvas) del
// Simulador de Bandas — DxGrid no trae export nativo a estos dos formatos (solo Xlsx/Csv), a
// diferencia de WinForms que usa PrintableComponentLink/ExportToPdf. Ver
// FrontOne.Web/Components/Pages/Acopio/SimuladorBandas.razor.

export function imprimir() {
    window.print();
}

// Dibuja la tabla completa (encabezado + filas + totales) en un <canvas> y descarga el resultado
// como PNG vía toDataURL — se usa una URL "data:" a propósito porque la CSP del sitio
// (SecurityHeadersMiddleware.cs) permite "img-src 'self' data:" pero no "blob:".
export function exportarImagen(filas, sumaPorcentaje, sumaBanda, nombreArchivo) {
    const columnas = [
        { titulo: "Calibre APEAM", campo: "calibreApeamNombre", ancho: 180 },
        { titulo: "Categoría", campo: "categoriaNombre", ancho: 140 },
        { titulo: "Precio x Kg", campo: "precio", ancho: 140 },
        { titulo: "% de la Curva", campo: "porcentaje", ancho: 140 },
        { titulo: "Banda", campo: "banda", ancho: 140 },
    ];

    const colorPorClase = {
        "fo-banda-cat1": "#E8DAEF",
        "fo-banda-cat2": "#FCF3CF",
        "fo-banda-nal": "#FADBD8",
        "": "#FFFFFF",
    };

    const altoFila = 28;
    const altoEncabezado = 32;
    const altoTotales = 28;
    const anchoTotal = columnas.reduce((suma, c) => suma + c.ancho, 0);
    const altoTotal = altoEncabezado + (filas.length * altoFila) + altoTotales + 1;

    const canvas = document.createElement("canvas");
    const escala = 2; // más nítido en pantallas de alta densidad
    canvas.width = anchoTotal * escala;
    canvas.height = altoTotal * escala;
    const ctx = canvas.getContext("2d");
    ctx.scale(escala, escala);

    ctx.fillStyle = "#FFFFFF";
    ctx.fillRect(0, 0, anchoTotal, altoTotal);
    ctx.font = "13px Segoe UI, Arial, sans-serif";
    ctx.textBaseline = "middle";

    // Encabezado
    let x = 0;
    ctx.fillStyle = "#2b2b2b";
    columnas.forEach((columna) => {
        ctx.fillStyle = "#EDEDED";
        ctx.fillRect(x, 0, columna.ancho, altoEncabezado);
        ctx.strokeStyle = "#C9C9C9";
        ctx.strokeRect(x, 0, columna.ancho, altoEncabezado);
        ctx.fillStyle = "#2b2b2b";
        ctx.font = "bold 13px Segoe UI, Arial, sans-serif";
        ctx.fillText(columna.titulo, x + 8, altoEncabezado / 2);
        x += columna.ancho;
    });

    // Filas
    ctx.font = "13px Segoe UI, Arial, sans-serif";
    filas.forEach((fila, indice) => {
        const y = altoEncabezado + (indice * altoFila);
        x = 0;
        columnas.forEach((columna) => {
            const esIdentidad = columna.campo === "calibreApeamNombre" || columna.campo === "categoriaNombre";
            ctx.fillStyle = esIdentidad ? (colorPorClase[fila.clase] || "#FFFFFF") : "#FFFFFF";
            ctx.fillRect(x, y, columna.ancho, altoFila);
            ctx.strokeStyle = "#DDDDDD";
            ctx.strokeRect(x, y, columna.ancho, altoFila);
            ctx.fillStyle = "#1a1a1a";
            ctx.fillText(String(fila[columna.campo] ?? ""), x + 8, y + (altoFila / 2));
            x += columna.ancho;
        });
    });

    // Totales
    const yTotales = altoEncabezado + (filas.length * altoFila);
    x = 0;
    columnas.forEach((columna) => {
        ctx.fillStyle = "#F5F5F5";
        ctx.fillRect(x, yTotales, columna.ancho, altoTotales);
        ctx.strokeStyle = "#C9C9C9";
        ctx.strokeRect(x, yTotales, columna.ancho, altoTotales);
        ctx.fillStyle = "#1a1a1a";
        ctx.font = "bold 13px Segoe UI, Arial, sans-serif";
        let texto = "";
        if (columna.campo === "porcentaje") texto = sumaPorcentaje;
        if (columna.campo === "banda") texto = sumaBanda;
        ctx.fillText(texto, x + 8, yTotales + (altoTotales / 2));
        x += columna.ancho;
    });

    const enlace = document.createElement("a");
    enlace.href = canvas.toDataURL("image/png");
    enlace.download = nombreArchivo;
    document.body.appendChild(enlace);
    enlace.click();
    document.body.removeChild(enlace);
}
