// Alterna mostrar/ocultar la contraseña en Login.razor. JS puro, sin @onclick de Blazor a
// propósito: alternar el atributo type vía Blazor recreaba el <input> en cada render del
// circuito y borraba lo ya escrito (bug real, confirmado probando) — tocar .type directo en el
// elemento vivo por JS no afecta su .value, el navegador lo conserva tal cual.
(function () {
    const boton = document.getElementById('fo-toggle-password');
    if (!boton) {
        return;
    }

    boton.addEventListener('click', function () {
        const input = document.getElementById('password');
        const iconoOjo = document.getElementById('fo-icon-eye');
        const iconoOjoTachado = document.getElementById('fo-icon-eye-off');
        const mostrar = input.type === 'password';

        input.type = mostrar ? 'text' : 'password';
        iconoOjo.style.display = mostrar ? 'none' : '';
        iconoOjoTachado.style.display = mostrar ? '' : 'none';

        const etiqueta = mostrar ? 'Ocultar contraseña' : 'Mostrar contraseña';
        boton.setAttribute('aria-label', etiqueta);
        boton.setAttribute('title', etiqueta);
    });
})();
