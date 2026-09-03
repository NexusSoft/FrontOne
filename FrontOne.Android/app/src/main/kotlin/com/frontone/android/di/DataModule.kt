package com.frontone.android.di

import com.frontone.android.config.ConfiguracionConexionStore
import com.frontone.android.data.sqlserver.ConexionSqlServerAdapter
import com.frontone.android.data.sqlserver.ConnectionFactory
import com.frontone.android.data.sqlserver.EmpresaSqlServerAdapter
import com.frontone.android.data.sqlserver.LineaProduccionSqlServerAdapter
import com.frontone.android.data.sqlserver.ListaPrecioFrutaSqlServerAdapter
import com.frontone.android.data.sqlserver.PalletSqlServerAdapter
import com.frontone.android.data.sqlserver.ProductoTerminadoSqlServerAdapter
import com.frontone.android.data.sqlserver.UsuarioSqlServerAdapter
import com.frontone.android.domain.port.ConexionSqlServerPort
import com.frontone.android.domain.port.EmpresaPort
import com.frontone.android.domain.port.LineaProduccionPort
import com.frontone.android.domain.port.ListaPrecioFrutaPort
import com.frontone.android.domain.port.PalletPort
import com.frontone.android.domain.port.ProductoTerminadoPort
import com.frontone.android.domain.port.UsuarioPort
import com.frontone.android.domain.usecase.ActualizarEncabezadoPalletUseCase
import com.frontone.android.domain.usecase.ActualizarLineaPalletUseCase
import com.frontone.android.domain.usecase.AgregarLineaPalletUseCase
import com.frontone.android.domain.usecase.CrearPalletUseCase
import com.frontone.android.domain.usecase.EliminarLineaPalletUseCase
import com.frontone.android.domain.usecase.EliminarPalletUseCase
import com.frontone.android.domain.usecase.LoginUseCase
import com.frontone.android.domain.usecase.ObtenerCombinacionesMateriaPrimaUseCase
import com.frontone.android.domain.usecase.ObtenerLineasProduccionUseCase
import com.frontone.android.domain.usecase.ObtenerLogoEmpresaUseCase
import com.frontone.android.domain.usecase.ObtenerLotesEnProcesoUseCase
import com.frontone.android.domain.usecase.ObtenerPalletDetalleUseCase
import com.frontone.android.domain.usecase.ObtenerPalletsUseCase
import com.frontone.android.domain.usecase.ObtenerPreciosPorFechaUseCase
import com.frontone.android.domain.usecase.ObtenerProductosTerminadosUseCase
import com.frontone.android.domain.usecase.ObtenerVigenciasListaPrecioFrutaUseCase
import com.frontone.android.domain.usecase.ProbarConexionUseCase
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.components.SingletonComponent

/**
 * Composition root de la capa de datos — conecta los puertos de :domain con los
 * adaptadores concretos de :data. Equivalente a
 * ServiceCollectionExtensions.AddSqlServerInfrastructure() en FrontOne (C#).
 */
@Module
@InstallIn(SingletonComponent::class)
object DataModule {

    /**
     * ConnectionFactory recibe un proveedor (no un valor fijo) que consulta
     * ConfiguracionConexionStore en cada llamada — así la pantalla de Configuración
     * de Conexión puede guardar credenciales nuevas y la siguiente operación las usa
     * de inmediato, sin reiniciar la app. Ver CLAUDE.md, sección "Credenciales de
     * conexión": el login sigue siendo "sa" hasta que se defina el login dedicado.
     */
    @Provides
    fun proveerConnectionFactory(store: ConfiguracionConexionStore): ConnectionFactory =
        ConnectionFactory(obtenerConfiguracion = store::obtenerActual)

    @Provides
    fun proveerConexionSqlServerPort(connectionFactory: ConnectionFactory): ConexionSqlServerPort =
        ConexionSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerProbarConexionUseCase(port: ConexionSqlServerPort): ProbarConexionUseCase =
        ProbarConexionUseCase(port)

    @Provides
    fun proveerEmpresaPort(connectionFactory: ConnectionFactory): EmpresaPort =
        EmpresaSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerObtenerLogoEmpresaUseCase(port: EmpresaPort): ObtenerLogoEmpresaUseCase =
        ObtenerLogoEmpresaUseCase(port)

    @Provides
    fun proveerUsuarioPort(connectionFactory: ConnectionFactory): UsuarioPort =
        UsuarioSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerLoginUseCase(port: UsuarioPort): LoginUseCase =
        LoginUseCase(port)

    @Provides
    fun proveerPalletPort(connectionFactory: ConnectionFactory): PalletPort =
        PalletSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerProductoTerminadoPort(connectionFactory: ConnectionFactory): ProductoTerminadoPort =
        ProductoTerminadoSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerLineaProduccionPort(connectionFactory: ConnectionFactory): LineaProduccionPort =
        LineaProduccionSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerObtenerPalletsUseCase(port: PalletPort): ObtenerPalletsUseCase =
        ObtenerPalletsUseCase(port)

    @Provides
    fun proveerObtenerPalletDetalleUseCase(port: PalletPort): ObtenerPalletDetalleUseCase =
        ObtenerPalletDetalleUseCase(port)

    @Provides
    fun proveerObtenerLotesEnProcesoUseCase(port: PalletPort): ObtenerLotesEnProcesoUseCase =
        ObtenerLotesEnProcesoUseCase(port)

    @Provides
    fun proveerCrearPalletUseCase(port: PalletPort): CrearPalletUseCase =
        CrearPalletUseCase(port)

    @Provides
    fun proveerActualizarEncabezadoPalletUseCase(port: PalletPort): ActualizarEncabezadoPalletUseCase =
        ActualizarEncabezadoPalletUseCase(port)

    @Provides
    fun proveerEliminarPalletUseCase(port: PalletPort): EliminarPalletUseCase =
        EliminarPalletUseCase(port)

    @Provides
    fun proveerAgregarLineaPalletUseCase(port: PalletPort): AgregarLineaPalletUseCase =
        AgregarLineaPalletUseCase(port)

    @Provides
    fun proveerActualizarLineaPalletUseCase(port: PalletPort): ActualizarLineaPalletUseCase =
        ActualizarLineaPalletUseCase(port)

    @Provides
    fun proveerEliminarLineaPalletUseCase(port: PalletPort): EliminarLineaPalletUseCase =
        EliminarLineaPalletUseCase(port)

    @Provides
    fun proveerObtenerProductosTerminadosUseCase(port: ProductoTerminadoPort): ObtenerProductosTerminadosUseCase =
        ObtenerProductosTerminadosUseCase(port)

    @Provides
    fun proveerObtenerLineasProduccionUseCase(port: LineaProduccionPort): ObtenerLineasProduccionUseCase =
        ObtenerLineasProduccionUseCase(port)

    @Provides
    fun proveerListaPrecioFrutaPort(connectionFactory: ConnectionFactory): ListaPrecioFrutaPort =
        ListaPrecioFrutaSqlServerAdapter(connectionFactory)

    @Provides
    fun proveerObtenerCombinacionesMateriaPrimaUseCase(port: ListaPrecioFrutaPort): ObtenerCombinacionesMateriaPrimaUseCase =
        ObtenerCombinacionesMateriaPrimaUseCase(port)

    @Provides
    fun proveerObtenerVigenciasListaPrecioFrutaUseCase(port: ListaPrecioFrutaPort): ObtenerVigenciasListaPrecioFrutaUseCase =
        ObtenerVigenciasListaPrecioFrutaUseCase(port)

    @Provides
    fun proveerObtenerPreciosPorFechaUseCase(port: ListaPrecioFrutaPort): ObtenerPreciosPorFechaUseCase =
        ObtenerPreciosPorFechaUseCase(port)
}
