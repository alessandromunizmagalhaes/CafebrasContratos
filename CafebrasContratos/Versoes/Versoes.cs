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

    public class Versao_Zero_Tres : Versionamento
    {
        public override double Versao { get => 0.3; }

        public override void Aplicar(Database db)
        {
            var tabelaPreContrato = new TabelaPreContrato();

            var colunas = new List<Coluna>() {
                tabelaPreContrato.Peneira01,
                tabelaPreContrato.Peneira02,
                tabelaPreContrato.Peneira03,
                tabelaPreContrato.Peneira04,
                tabelaPreContrato.Peneira05,
                tabelaPreContrato.Peneira06,
                tabelaPreContrato.Peneira07,
                tabelaPreContrato.Peneira08,
                tabelaPreContrato.Peneira09,
                tabelaPreContrato.Peneira10,
                tabelaPreContrato.Peneira11,
                tabelaPreContrato.Peneira12,
                tabelaPreContrato.Peneira13,
                tabelaPreContrato.Peneira14,
                tabelaPreContrato.Peneira15,

                tabelaPreContrato.Diferencial01,
                tabelaPreContrato.Diferencial02,
                tabelaPreContrato.Diferencial03,
                tabelaPreContrato.Diferencial04,
                tabelaPreContrato.Diferencial05,
                tabelaPreContrato.Diferencial06,
                tabelaPreContrato.Diferencial07,
                tabelaPreContrato.Diferencial08,
                tabelaPreContrato.Diferencial09,
                tabelaPreContrato.Diferencial10,
                tabelaPreContrato.Diferencial11,
                tabelaPreContrato.Diferencial12,
                tabelaPreContrato.Diferencial13,
                tabelaPreContrato.Diferencial14,
                tabelaPreContrato.Diferencial15,
            };

            foreach (var coluna in colunas)
            {
                db.CriarCampo(tabelaPreContrato.NomeComArroba, coluna);
            }

            var tabelaConfigPeneira = new TabelaConfiguracaoPeneira();
            db.CriarTabela(tabelaConfigPeneira);
        }
    }

    public class Versao_Zero_Quatro : Versionamento
    {
        public override double Versao { get => 0.4; }

        public override void Aplicar(Database db)
        {
            var tabelaPreContrato = new TabelaPreContrato();

            var colunas = new List<Coluna>() {
                tabelaPreContrato.Peneira01,
                tabelaPreContrato.Peneira02,
                tabelaPreContrato.Peneira03,
                tabelaPreContrato.Peneira04,
                tabelaPreContrato.Peneira05,
                tabelaPreContrato.Peneira06,
                tabelaPreContrato.Peneira07,
                tabelaPreContrato.Peneira08,
                tabelaPreContrato.Peneira09,
                tabelaPreContrato.Peneira10,
                tabelaPreContrato.Peneira11,
                tabelaPreContrato.Peneira12,
                tabelaPreContrato.Peneira13,
                tabelaPreContrato.Peneira14,
                tabelaPreContrato.Peneira15,
            };

            foreach (var coluna in colunas)
            {
                db.ExcluirColuna(tabelaPreContrato.NomeComArroba, coluna.Nome);
                db.CriarCampo(tabelaPreContrato.NomeComArroba, coluna);
            }
        }
    }

    public class Versao_Zero_Cinco : Versionamento
    {
        public override double Versao { get => 0.5; }

        public override void Aplicar(Database db)
        {
            db.CriarTabela(new TabelaContratoFinal());
        }
    }
}
