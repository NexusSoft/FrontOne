pluginManagement {
    repositories {
        google()
        mavenCentral()
        gradlePluginPortal()
    }
}

dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.FAIL_ON_PROJECT_REPOS)
    repositories {
        google()
        mavenCentral()
    }
}

rootProject.name = "FrontOne.Android"

// Espejo de las capas hexagonales de FrontOne (C#):
//   domain = FrontOne.Domain      (entidades, puertos, casos de uso)
//   data   = FrontOne.Infrastructure.SqlServer  (adaptador contra SQL Server)
//   app    = FrontOne.WinForms    (UI Compose + composition root / DI)
include(":domain")
include(":data")
include(":app")
