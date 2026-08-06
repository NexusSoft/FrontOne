# QA de FrontOne — seguimiento de pruebas

Tracker publicado (Artifact): pídele el link a quien lo generó la última vez, o pide que se vuelva a desplegar.

## Cómo usarlo
1. Abre el tracker y recorre los bloques en orden (0 → 13) — cada uno cubre un módulo del ERP, en el mismo orden en que se construyeron.
2. Por cada caso, márcalo **OK** o **Falla** según lo que observes probando la app real (no adivines).
3. Si un caso falla: escribe la nota (qué hiciste, qué esperabas, qué pasó — folio/pantalla/mensaje exacto) y da clic en **"Reportar en GitHub"** para abrir el Issue ya prellenado. Publícalo tal cual o edítalo antes de enviarlo.
4. Al terminar tu sesión de revisión, da clic en **"Exportar avance"** y sube ese `.json` reemplazando `docs/qa/estado-qa-frontone.json` (commit normal, o pásaselo a quien mantiene el tracker). La próxima vez que se publique el Artifact, arranca desde ese avance — así el equipo completo ve el mismo corte, no el de tu navegador nada más.

## Por qué así
El tracker mismo no tiene backend: vive como una sola página. La colaboración real pasa por GitHub de dos formas — cada hallazgo se vuelve un Issue de verdad (asignable, con historial, visible para todos), y el avance del checklist se versiona como archivo en este repo (diffable en cualquier PR) en vez de quedar atrapado en el `localStorage` de una sola máquina.
