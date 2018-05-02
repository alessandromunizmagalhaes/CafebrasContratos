using SAPHelper;
using System.Collections.Generic;
using System.Linq;

namespace CafebrasContratos
{
    public static class Versoes
    {
        public static void Aplicar(Database db, List<Versionamento> versoes)
        {
            var dbVersao = db.Versao();
            var versoesAtrasadas = versoes.OrderBy(v => v.Versao).Where(v => v.Versao < dbVersao);
            foreach (var versao in versoesAtrasadas)
            {
                versao.Aplicar(db);
            }
        }
    }

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
        }
    }

    public abstract class Versionamento
    {
        public abstract double Versao { get; }

        public abstract void Aplicar(Database db);
    }
}
