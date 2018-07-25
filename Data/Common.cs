using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digisystem.Calibracao.Data
{
    public static class Common
    {
        public static LiteDB.LiteDatabase CertDb => new LiteDB.LiteDatabase(ConfigurationManager.ConnectionStrings["DbConfig"].ConnectionString);
    }
}
