using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public static class CamposTabelaSAP
    {
        public static Coluna grupoAprovador = new ColunaVarchar("GrupoAprov", "Grupo Aprovador", 2, false, "V", new List<ValorValido>() {
                            new ValorValido(GrupoAprovador.Planejador, "Planejador"),
                            new ValorValido(GrupoAprovador.Executor, "Executor"),
                            new ValorValido(GrupoAprovador.Autorizador, "Autorizador"),
                            new ValorValido(GrupoAprovador.Gestor, "Gestor"),
                            new ValorValido(GrupoAprovador.Visualizador, "Visualizador")
                        });
    }
}
