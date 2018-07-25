using Digisystem.Calibracao.Model;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digisystem.Calibracao.Data
{
    public static class Clientes
    {
        public static int InsertOrUpdate(Cliente cliente)
        {
            try
            {
                using (var db = Common.CertDb)
                {
                    var col = db.GetCollection<Cliente>("clientes");

                    var existente = col.FindOne(x => x.Cnpj == cliente.Cnpj);

                    if (existente != null)
                    {
                        if (existente.Endereco == cliente.Endereco || existente.RazaoSocial == cliente.RazaoSocial)
                        {
                            return -1;
                        }
                        existente.Endereco = cliente.Endereco;
                        existente.RazaoSocial = cliente.RazaoSocial;
                        col.Update(existente);
                        return 0;
                    }
                    else
                    {
                        col.Insert(cliente);
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static List<Cliente> GetAll()
        {
            try
            {
                using (var db = Common.CertDb)
                {
                    var col = db.GetCollection<Cliente>("clientes");

                    return col.FindAll().ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<Cliente>();
            }
        }

        public static bool RemoveByCnpj(string cnpj)
        {
            try
            {
                using (var db = Common.CertDb)
                {
                    var col = db.GetCollection<Cliente>("clientes");

                    col.Delete(x => x.Cnpj == cnpj);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
