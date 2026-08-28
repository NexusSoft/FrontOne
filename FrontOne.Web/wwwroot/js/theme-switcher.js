// Aplica un conjunto de hojas de estilo DevExpress + el modo claro/oscuro del CHROME propio del
// sitio (topbar/drawer/panel, ver tokens --fo-* en site.css) + el color de acento del borde
// superior del topbar + un color de acento personalizado opcional (overlay de variables
// --dxds-primary-*, solo tiene efecto en el grupo Fluent). Todo se recuerda en localStorage —
// mismo mecanismo que App.razor usa para aplicarlo antes del primer paint. Ver ThemeSwitcher.razor.
//
// Carga las hojas NUEVAS y espera a que terminen antes de quitar las viejas (nunca las quita
// primero) — si se quitan antes, hay una ventana sin ningún tema DX activo donde las variables
// --dxds-*/--dxbl-* no existen y los íconos se ven gigantes/rotos durante la transición.
const FRONTONE_TEMA_DEFAULT = ['_content/DevExpress.Blazor.Themes/blazing-berry.bs5.min.css'];

function frontOneCargarHojas(hrefs) {
    return Promise.all(hrefs.map(href => new Promise(resolve => {
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = href;
        link.setAttribute('data-fo-theme', '');
        link.onload = resolve;
        link.onerror = resolve; // una hoja que falla no debe dejar la transición colgada
        document.head.appendChild(link);
    })));
}

// Ponytail: aplica el mismo hex a los 17 pasos --dxds-primary-10..170 (sin degradado propio de
// claro/oscuro por paso, a diferencia de los accents/*.min.css oficiales). Si algún día hace
// falta el degradado real, ahí es donde generar la escala completa (algoritmo de DevExpress).
function frontOneAplicarColorPersonalizado(hex) {
    let estilo = document.getElementById('fo-custom-accent');
    if (!hex) {
        estilo?.remove();
        return;
    }
    if (!estilo) {
        estilo = document.createElement('style');
        estilo.id = 'fo-custom-accent';
        document.head.appendChild(estilo);
    }
    const pasos = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170];
    estilo.textContent = ':root{' + pasos.map(p => `--dxds-primary-${p}:${hex};`).join('') + '}';
}

window.frontOneTheme = {
    async aplicar(hrefs, oscuro, colorAcento, colorPersonalizado) {
        const linkDefault = document.getElementById('dx-theme-default');
        if (linkDefault) {
            linkDefault.disabled = true; // desde el primer cambio, todo pasa por hojas dinámicas
        }

        const efectivos = hrefs && hrefs.length > 0 ? hrefs : FRONTONE_TEMA_DEFAULT;
        const anteriores = Array.from(document.querySelectorAll('link[data-fo-theme]'));
        await frontOneCargarHojas(efectivos);
        anteriores.forEach(link => link.remove());

        frontOneAplicarColorPersonalizado(colorPersonalizado);
        document.documentElement.setAttribute('data-theme', oscuro ? 'dark' : 'light');
        document.documentElement.style.setProperty('--fo-accent', colorPersonalizado || colorAcento || '#1F3864');

        try {
            localStorage.setItem('fo-tema', JSON.stringify({ hrefs, oscuro, colorAcento, colorPersonalizado }));
        } catch {
            // localStorage puede no estar disponible (modo privado) — el tema no persiste, no truena.
        }
    },
    obtener() {
        try {
            const raw = localStorage.getItem('fo-tema');
            return raw ? JSON.parse(raw) : null;
        } catch {
            return null;
        }
    },
};
