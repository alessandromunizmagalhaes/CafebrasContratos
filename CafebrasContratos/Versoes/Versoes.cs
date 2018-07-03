using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class Versao_Zero_Um : Versionamento
    {
        public override double Versao { get => 0.1; }

        public override void Aplicar(Database db)
        {
            var tabelas = new List<Tabela>(){
                new TabelaModalidade(),
                new TabelaUnidadeComercial(),
                new TabelaTipoOperacao(),
                new TabelaMetodoFinanceiro(),
                new TabelaSafra(),
                new TabelaCertificado(),
                new TabelaParticipante(),
                new TabelaGrupoCafe(),
                new TabelaConfiguracaoPeneira(),
                new TabelaPreContrato(),
                new TabelaContratoFinal()
            };

            for (int i = 0; i < tabelas.Count; i++)
            {
                Dialogs.Info($"Criando tabelas... {i + 1} de {tabelas.Count}... Aguarde...", SAPbouiCOM.BoMessageTime.bmt_Long);

                db.CriarTabela(tabelas[i]);
            }

            db.CriarCampo("OUSR", CamposTabelaSAP.grupoAprovador);
            db.CriarCampo("OPOR", CamposTabelaSAP.numeroContratoFilho);
        }
    }
}
