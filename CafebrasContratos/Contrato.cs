using System.Collections.Generic;

namespace CafebrasContratos
{
    public class Contrato
    {
        public static readonly Dictionary<string, string> _status = new Dictionary<string, string>()
        {
            { "A","Aberto" },
            { "F","Fechado" },
            { "C","Cancelado" }
        };
    }
}
