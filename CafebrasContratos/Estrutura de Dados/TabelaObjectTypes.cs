using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class TabelaObjectTypes : Tabela, ITabelaPopulavel
    {
        public Coluna ObjectType { get { return new ColunaVarchar("ObjType", "Tipo de Objeto", 30); } }
        public Coluna Tabela { get { return new ColunaVarchar("Tabela", "Tabela", 30); } }
        public Dictionary<string, string> data = new Dictionary<string, string>() {
            {((int)BoObjectTypes.oPurchaseOrders).ToString(), "OPOR"},
            {((int)BoObjectTypes.oPurchaseInvoices).ToString(), "OPCH"},
            {((int)BoObjectTypes.oPurchaseDeliveryNotes).ToString(), "OPDN"},
            {((int)BoObjectTypes.oPurchaseDownPayments).ToString(), "ODPO"},
            {((int)BoObjectTypes.oPurchaseReturns).ToString(), "ORPD"},
            {((int)BoObjectTypes.oPurchaseCreditNotes).ToString(), "ORPC"},
        };

        public TabelaObjectTypes() : base("UPD_OBJ_TYPES", "Tipos de Objetos", BoUTBTableType.bott_NoObject)
        {
        }

        public void Popular()
        {
            using (var recordset = new RecordSet())
            {
                var insert =
                $@"INSERT INTO [{NomeComArroba}] 
                    (Code, Name, {ObjectType.NomeComU_NaFrente}, {Tabela.NomeComU_NaFrente})
                    VALUES ";

                var values = string.Empty;
                foreach (var item in data)
                {
                    values += $@",('{item.Key}', '{item.Key}', '{item.Key}', '{item.Value}')";
                }

                values = values.Remove(0, 1);
                recordset.DoQuery(insert + values);
            }
        }
    }
}
