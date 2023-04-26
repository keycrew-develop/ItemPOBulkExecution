using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class DBManager
    {
        // SQLコネクション
        public SqlConnection sqlConnection;
        // SQLトランザクション
        public SqlTransaction sqlTransaction;

        /// <summary>
        /// DB接続を行う
        /// </summary>
        public void DbConect(string constr)
        {
            try
            {
                this.sqlConnection = new SqlConnection(constr);

                // データベース接続を開く
                this.sqlConnection.Open();

            }
            catch (SqlException ex)
            {
                this.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// DB切断
        /// </summary>
        public void Close()
        {
            if (this.sqlConnection != null)
            {
                this.sqlConnection.Close();
                this.sqlConnection.Dispose();
            }
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTran()
        {
            try
            {
                this.sqlTransaction = this.sqlConnection.BeginTransaction();
            }
            catch (SqlException ex)
            {
                this.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// トランザクション　コミット
        /// </summary>
        public void CommitTran()
        {
            try
            {
                if (this.sqlTransaction != null)
                {
                    this.sqlTransaction.Commit();
                    this.sqlTransaction.Dispose();
                }
            }
            catch (SqlException ex)
            {
                this.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// トランザクション　ロールバック
        /// </summary>
        public void RollBack()
        {
            try
            {
                if (this.sqlTransaction != null)
                {
                    this.sqlTransaction.Rollback();
                    this.sqlTransaction.Dispose();
                }
            }
            catch (SqlException ex)
            {
                this.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// クエリー実行(SqlDataReaderの戻り値あり)
        /// <para name="query">SQL文</para>
        /// <para name="paramDict">SQLパラメータ</para>
        /// </summary>
        public SqlDataReader ExecuteQuery(string query, Dictionary<string, Object> paramDict)
        {
            try
            {
                SqlCommand sqlCom = new SqlCommand();

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.sqlConnection;
                sqlCom.Transaction = this.sqlTransaction;

                sqlCom.CommandText = query;
                foreach (KeyValuePair<string, Object> item in paramDict)
                {
                    sqlCom.Parameters.Add(new SqlParameter(item.Key, item.Value));
                }

                // SQLを実行
                SqlDataReader reader = sqlCom.ExecuteReader();

                return reader;
            }
            catch (SqlException ex)
            {
                this.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// クエリー実行(SqlDataReaderの戻り値あり)
        /// <para name="query">SQL文</para>
        /// </summary>
        public SqlDataReader ExecuteQuery(string query)
        {
            return this.ExecuteQuery(query, new Dictionary<string, Object>());
        }

        /// <summary>
        /// クエリー実行(戻り値なし)
        /// <para name="query">SQL文</para>
        /// <para name="paramDict">SQLパラメータ</para>
        /// </summary>
        public void ExecuteNonQuery(string query, Dictionary<string, Object> paramDict)
        {
            try
            {
                SqlCommand sqlCom = new SqlCommand();

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.sqlConnection;
                sqlCom.Transaction = this.sqlTransaction;

                sqlCom.CommandText = query;
                if (paramDict != null)
                {
                    foreach (KeyValuePair<string, Object> item in paramDict)
                    {
                        sqlCom.Parameters.Add(new SqlParameter(item.Key, item.Value));
                    }
                }

                // SQLを実行
                sqlCom.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                this.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// プロシージャ実行
        /// </summary>
        /// <param name="query">プロシージャ名</param>
        /// <param name="inParamDict">INPUTパラメータ</param>
        /// <param name="outParamDict">OUTPUTパラメータ</param> 
        /// <param name="result">実行可否</param> 
        /// <returns></returns>
        public SqlCommand ExecuteProcedure(string query,
            Dictionary<string, Object> inParamDict,
            Dictionary<string, Tuple<SqlDbType, string>> outParamDict,
            out int result)
        {
            SqlCommand sqlCom = new SqlCommand(query);
            try
            {

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.sqlConnection;
                sqlCom.Transaction = this.sqlTransaction;

                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandTimeout = 180;
                // INPUTパラメータをセット
                foreach (KeyValuePair<string, Object> inItem in inParamDict)
                {
                    var param = sqlCom.Parameters.AddWithValue(inItem.Key, inItem.Value);

                    if (inItem.Value != null)
                    {
                        if (inItem.Value.GetType().Name.Equals("DataTable"))
                        {
                            param.SqlDbType = SqlDbType.Structured;
                        }

                        sqlCom.Parameters[inItem.Key].Direction = ParameterDirection.Input;
                        sqlCom.Parameters[inItem.Key].Value = inItem.Value;
                    }
                }

                // OUTPUTパラメータをセット
                foreach (KeyValuePair<string, Tuple<SqlDbType, string>> outItem in outParamDict)
                {
                    // 桁数が設定されているか
                    if (outItem.Value.Item2 != null)
                    {
                        sqlCom.Parameters.Add(outItem.Key, outItem.Value.Item1, int.Parse(outItem.Value.Item2));
                    }
                    else
                    {
                        sqlCom.Parameters.Add(outItem.Key, outItem.Value.Item1);
                    }

                    sqlCom.Parameters[outItem.Key].Direction = ParameterDirection.Output;
                }

                // RETURN
                sqlCom.Parameters.Add("return_value", System.Data.SqlDbType.Int);
                sqlCom.Parameters["return_value"].Direction = System.Data.ParameterDirection.ReturnValue;
                // SQLを実行

                sqlCom.ExecuteNonQuery();

                result = Convert.ToInt32(sqlCom.Parameters["return_value"].Value);

                return sqlCom;
            }
            catch (SqlException ex)
            {
                this.Close();
                result = 1;
                if (sqlCom.Parameters["@Msg"].Value == null)
                {
                    sqlCom.Parameters["@Msg"].Value = ex.Message.ToString();
                }
                return sqlCom;
            }
            catch (Exception ex)
            {
                this.Close();
                result = 1;
                sqlCom.Parameters["@Msg"].Value = ex.Message.ToString();
                return sqlCom;
            }
        }

        /// <summary>
        /// プロシージャ実行
        /// </summary>
        /// <param name="query">プロシージャ名</param>
        /// <param name="inParamDict">INPUTパラメータ</param>
        /// <param name="outParamDict">OUTPUTパラメータ</param> 
        /// <param name="result">実行可否</param> 
        /// <returns></returns>
        public SqlCommand ExecuteProcedure(string query,
            Dictionary<string, Object> inParamDict,
            Dictionary<string, Tuple<SqlDbType, string>> outParamDict,
            out int result,
            out SqlDataReader dtr)
        {
            SqlCommand sqlCom = new SqlCommand(query);
            try
            {

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.sqlConnection;
                sqlCom.Transaction = this.sqlTransaction;

                sqlCom.CommandType = CommandType.StoredProcedure;

                // INPUTパラメータをセット
                foreach (KeyValuePair<string, Object> inItem in inParamDict)
                {
                    var param = sqlCom.Parameters.AddWithValue(inItem.Key, inItem.Value);
                    if (inItem.Value != null)
                    {
                        if (inItem.Value.GetType().Name.Equals("DataTable"))
                        {
                            param.SqlDbType = SqlDbType.Structured;
                        }

                        sqlCom.Parameters[inItem.Key].Direction = ParameterDirection.Input;
                        sqlCom.Parameters[inItem.Key].Value = inItem.Value;
                    }

                }

                // OUTPUTパラメータをセット
                foreach (KeyValuePair<string, Tuple<SqlDbType, string>> outItem in outParamDict)
                {
                    // 桁数が設定されているか
                    if (outItem.Value.Item2 != null)
                    {
                        sqlCom.Parameters.Add(outItem.Key, outItem.Value.Item1, int.Parse(outItem.Value.Item2));
                    }
                    else
                    {
                        sqlCom.Parameters.Add(outItem.Key, outItem.Value.Item1);
                    }

                    sqlCom.Parameters[outItem.Key].Direction = ParameterDirection.Output;
                }

                // RETURN
                sqlCom.Parameters.Add("ReturnValue", System.Data.SqlDbType.Int);
                sqlCom.Parameters["ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;

                // SQLを実行
                dtr = sqlCom.ExecuteReader();

                result = Convert.ToInt32(sqlCom.Parameters["ReturnValue"].Value);

                return sqlCom;
            }
            catch (SqlException ex)
            {
                this.Close();
                result = 1;
                if (sqlCom.Parameters["@Msg"].Value == null)
                {
                    sqlCom.Parameters["@Msg"].Value = ex.Message.ToString();
                }
                dtr = null;
                return sqlCom;
            }
            catch (Exception ex)
            {
                this.Close();
                result = 1;
                sqlCom.Parameters["@Msg"].Value = ex.Message.ToString();
                dtr = null;
                return sqlCom;
            }
        }

        /// <summary>
        /// 複数結果セットを返すプロシージャの実行
        /// </summary>
        /// <param name="query"></param>
        /// <param name="inParamDict"></param>
        /// <param name="outParamDict"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public SqlCommand ExecuteProcedureMultiResult(string query,
            Dictionary<string, Object> inParamDict,
            Dictionary<string, Tuple<SqlDbType, string>> outParamDict,
            out int result,
            out DataSet ds)
        {
            SqlCommand sqlCom = new SqlCommand(query);
            DataSet dataSet = new DataSet();
            try
            {

                //クエリー送信先、トランザクションの指定
                sqlCom.Connection = this.sqlConnection;
                sqlCom.Transaction = this.sqlTransaction;

                sqlCom.CommandType = CommandType.StoredProcedure;

                // INPUTパラメータをセット
                foreach (KeyValuePair<string, Object> inItem in inParamDict)
                {
                    var param = sqlCom.Parameters.AddWithValue(inItem.Key, inItem.Value);
                    if (inItem.Value != null)
                    {
                        if (inItem.Value.GetType().Name.Equals("DataTable"))
                        {
                            param.SqlDbType = SqlDbType.Structured;
                        }

                        sqlCom.Parameters[inItem.Key].Direction = ParameterDirection.Input;
                        sqlCom.Parameters[inItem.Key].Value = inItem.Value;
                    }
                }

                // OUTPUTパラメータをセット
                foreach (KeyValuePair<string, Tuple<SqlDbType, string>> outItem in outParamDict)
                {
                    // 桁数が設定されているか
                    if (outItem.Value.Item2 != null)
                    {
                        sqlCom.Parameters.Add(outItem.Key, outItem.Value.Item1, int.Parse(outItem.Value.Item2));
                    }
                    else
                    {
                        sqlCom.Parameters.Add(outItem.Key, outItem.Value.Item1);
                    }

                    sqlCom.Parameters[outItem.Key].Direction = ParameterDirection.Output;
                }

                // RETURN
                sqlCom.Parameters.Add("ReturnValue", System.Data.SqlDbType.Int);
                sqlCom.Parameters["ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;

                // SQLを実行
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCom))
                {
                    adapter.Fill(dataSet);
                }

                result = Convert.ToInt32(sqlCom.Parameters["ReturnValue"].Value);

            }
            catch (SqlException ex)
            {
                this.Close();
                result = 1;
                if (sqlCom.Parameters["@Msg"].Value == null)
                {
                    sqlCom.Parameters["@Msg"].Value = ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                this.Close();
                result = 1;
                sqlCom.Parameters["@Msg"].Value = ex.Message.ToString();
            }
            ds = dataSet;

            return sqlCom;
        }
    }
}
