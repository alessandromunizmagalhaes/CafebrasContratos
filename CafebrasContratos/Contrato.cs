using System.Collections.Generic;

namespace CafebrasContratos
{
    public class Contrato
    {
        public static readonly Dictionary<string, string> _status = new Dictionary<string, string>()
        {
            // chave em ingles
            // O -> Open
            // A -> Authorized
            // C -> Canceled
            { "O","Aberto" },
            { "A","Autorizado" },
            { "C","Cancelado" }
        };
    }
}
