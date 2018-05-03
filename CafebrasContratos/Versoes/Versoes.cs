using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class Versao_Zero_Um : Versionamento
    {
        public override double Versao { get => 0.1; }

        public override void Aplicar(Database db)
        {
            db.CriarTabela(new TabelaModalidade());
            db.CriarTabela(new TabelaUnidadeComercial());
            db.CriarTabela(new TabelaTipoOperacao());
            db.CriarTabela(new TabelaMetodoFinanceiro());
            db.CriarTabela(new TabelaSafra());
            db.CriarTabela(new TabelaCertificado());
            db.CriarTabela(new TabelaParticipante());
            db.CriarTabela(new TabelaGrupoCafe());

            db.CriarTabela(new TabelaPreContrato());

            db.CriarCampo("OUSR", CamposTabelaSAP.grupoAprovador);
        }
    }

    public class Versao_Zero_Dois : Versionamento
    {
        public override double Versao { get => 0.2; }

        public override void Aplicar(Database db)
        {
            var tabelaPreContrato = new TabelaPreContrato();

            db.CriarCampo(tabelaPreContrato.NomeComArroba, tabelaPreContrato.Transportadora);
            db.CriarCampo(tabelaPreContrato.NomeComArroba, tabelaPreContrato.ValorSeguro);
            db.CriarCampo(tabelaPreContrato.NomeComArroba, tabelaPreContrato.LocalRetirada);
            db.CriarCampo(tabelaPreContrato.NomeComArroba, tabelaPreContrato.NomeEstrangeiro);

            UserObjectsMD uDO = Global.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            if (uDO.GetByKey(tabelaPreContrato.NomeSemArroba))
            {
                db.DefinirColunasComoUDO(uDO, new List<Coluna>() {
                    tabelaPreContrato.Transportadora,
                    tabelaPreContrato.ValorSeguro,
                    tabelaPreContrato.LocalRetirada,
                    tabelaPreContrato.NomeEstrangeiro
                });
            }
        }
    }
}
