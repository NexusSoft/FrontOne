package com.frontone.android.data.sqlserver

import com.frontone.android.domain.model.CombinacionMateriaPrima
import com.frontone.android.domain.model.PrecioCombinacion
import com.frontone.android.domain.model.VigenciaListaPrecioFruta
import com.frontone.android.domain.port.ListaPrecioFrutaPort
import java.math.BigDecimal
import java.time.LocalDate

/**
 * Adaptador contra `Acopio.sp_ListaPrecioFruta_*` — mismos SPs que ya consumen
 * `ListaPrecioFrutaForm`/`SimuladorBandasForm` de FrontOne.WinForms. Ver
 * `Database/Acopio/036_Alter_ListaPrecioFruta_Combinaciones.sql` para la firma vigente
 * ("última palabra") de los 3 SPs usados aquí.
 */
class ListaPrecioFrutaSqlServerAdapter(
    connectionFactory: ConnectionFactory
) : SqlRepositoryBase(connectionFactory), ListaPrecioFrutaPort {

    override suspend fun obtenerCombinacionesMateriaPrima(): List<CombinacionMateriaPrima> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Acopio.sp_ListaPrecioFruta_ObtenerCombinacionesMateriaPrima",
            cantidadParametros = 0,
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList {
                        while (resultado.next()) {
                            add(
                                CombinacionMateriaPrima(
                                    categoriaId = resultado.getInt("CategoriaId"),
                                    categoriaNombre = resultado.getString("CategoriaNombre") ?: "",
                                    calibreApeamId = resultado.getInt("CalibreApeamId"),
                                    calibreApeamNombre = resultado.getString("CalibreApeamNombre") ?: ""
                                )
                            )
                        }
                    }
                }
            }
        )

    override suspend fun obtenerVigencias(): List<VigenciaListaPrecioFruta> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Acopio.sp_ListaPrecioFruta_ObtenerFechas",
            cantidadParametros = 0,
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList {
                        while (resultado.next()) {
                            add(
                                VigenciaListaPrecioFruta(
                                    fecha = resultado.getDate("FechaInicio").aLocalDate(),
                                    productorId = resultado.getNullableInt("ProductorId"),
                                    productorNombre = resultado.getString("ProductorNombre")
                                )
                            )
                        }
                    }
                }
            }
        )

    override suspend fun obtenerPorFecha(fecha: LocalDate, productorId: Int?): List<PrecioCombinacion> =
        ejecutarProcedimiento(
            nombreProcedimiento = "Acopio.sp_ListaPrecioFruta_ObtenerPorFecha",
            cantidadParametros = 2,
            asignarParametros = { statement ->
                // java.sql.Date.valueOf(LocalDate) no existe en el core-oj.jar recortado de
                // Android (NoSuchMethodError en runtime aunque compile sin avisos) — mismo tipo
                // de recorte de API que motivó el parseo manual de toString() al leer fechas en
                // PalletSqlServerAdapter.kt. Se usa el overload String, disponible desde
                // siempre: LocalDate.toString() ya regresa el formato ISO "yyyy-MM-dd" que esa
                // sobrecarga espera.
                statement.setDate(1, java.sql.Date.valueOf(fecha.toString()))
                statement.setNullableInt(2, productorId)
            },
            leerResultado = { statement ->
                statement.executeQuery().use { resultado ->
                    buildList {
                        while (resultado.next()) {
                            add(
                                PrecioCombinacion(
                                    categoriaId = resultado.getInt("CategoriaId"),
                                    calibreApeamId = resultado.getInt("CalibreApeamId"),
                                    convencional = resultado.getBigDecimal("Convencional") ?: BigDecimal.ZERO,
                                    organico = resultado.getBigDecimal("Organico") ?: BigDecimal.ZERO,
                                    nacional = resultado.getBigDecimal("Nacional") ?: BigDecimal.ZERO
                                )
                            )
                        }
                    }
                }
            }
        )
}
