package com.frontone.android.di

import com.frontone.android.config.ConfiguracionConexionStore
import com.frontone.android.data.sqlserver.ConexionSqlServerAdapter
import com.frontone.android.data.sqlserver.ConnectionFactory
import com.frontone.android.data.sqlserver.EmpresaSqlServerAdapter
import com.frontone.android.data.sqlserver.UsuarioSqlServerAdapter
import com.frontone.android.domain.port.ConexionSqlServerPort
import com.frontone.android.domain.port.EmpresaPort
import com.frontone.android.domain.port.UsuarioPort
import com.frontone.android.domain.usecase.LoginUseCase
import com.frontone.android.domain.usecase.ObtenerLogoEmpresaUseCase
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
}
