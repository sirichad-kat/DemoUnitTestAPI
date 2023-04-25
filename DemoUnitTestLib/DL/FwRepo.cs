using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DemoUnitTestLib.DL
{
    public interface IFwRepo
    {
        DTO.FwInit FindFwInitInternal(string Connection, string Keyname);
        void InsertFwInit(string Connection, DTO.FwInit dat);
        void UpdateFwInit(string Connection, DTO.FwInit dat);
        bool IsFwInitExist(string Connection, string Keyname);
    }
    public class FwRepo : IFwRepo
    {
        IConfiguration configuration;
        public FwRepo(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public OracleConnection GetConnection(string Connection)
        {
            try
            {
                string connectionstring = configuration.GetSection("Connectionstrings").GetSection(Connection).Value;
                OracleConnection conn = new OracleConnection(connectionstring);
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                return conn;
            }
            catch (Exception ex)
            {
                throw;
            }


        }



        public DTO.FwInit FindFwInitInternal(string Connection, string Keyname)
        {
            DTO.FwInit ret = null;

            using (OracleConnection con = GetConnection(Connection))
            {
                try
                {
                    OracleCommand cmd = con.CreateCommand();
                    cmd.BindByName = true;
                    cmd.CommandText = "SELECT * FROM FW_INIT WHERE KEY_NAME =:KEYNAME";
                    cmd.Parameters.Add("KEYNAME", Keyname);

                    OracleDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        DTO.FwInit m = new DTO.FwInit();
                        m.RetrieveFromDataReader(rdr);
                        ret = m;
                    }
                    rdr.Close();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
            return ret;
        }



        public void InsertFwInit(string Connection, DTO.FwInit dat)
        {

            using (OracleConnection con = GetConnection(Connection))
            {
                OracleTransaction trn = con.BeginTransaction();
                try
                {
                    OracleCommand cmd = dat.InsertCommand(con);
                    cmd.Transaction = trn;
                    cmd.ExecuteNonQuery();
                    trn.Commit();
                }
                catch (Exception)
                {
                    trn.Rollback();
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void UpdateFwInit(string Connection, DTO.FwInit dat)
        {

            using (OracleConnection con = GetConnection(Connection))
            {
                OracleTransaction trn = con.BeginTransaction();
                try
                {
                    OracleCommand cmd = dat.UpdateCommand(con, "ProgramId,KeyName");
                    cmd.Transaction = trn;
                    cmd.ExecuteNonQuery();
                    trn.Commit();
                }
                catch (Exception)
                {
                    trn.Rollback();
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool IsFwInitExist(string Connection, string Keyname)
        {
            bool ret = true;
            using (OracleConnection con = GetConnection(Connection))
            {
                try
                {
                    OracleCommand cmd = con.CreateCommand();
                    cmd.BindByName = true;
                    cmd.CommandText = "SELECT count(*) FROM FW_INIT WHERE KEY_NAME =:KEYNAME";
                    cmd.Parameters.Add("KEYNAME", Keyname);

                    var cnt = cmd.ExecuteScalar();
                    if (cnt != DBNull.Value && Convert.ToInt32(cnt) > 0)
                    {
                        ret = false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
            return ret;
        }


    }
}
