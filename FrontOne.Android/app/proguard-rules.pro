# Reglas ProGuard/R8 del módulo app.
# El driver jTDS (net.sourceforge.jtds) usa reflection/ServiceLoader para registrar
# su Driver — si en el futuro se activa minifyEnabled=true y aparecen errores de
# clase faltante en tiempo de ejecución, agregar aquí:
#   -keep class net.sourceforge.jtds.jdbc.** { *; }
