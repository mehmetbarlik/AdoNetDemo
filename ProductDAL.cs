﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace AdoNetDemo
{
    public class ProductDAL
    {
        SqlConnection _connection = new SqlConnection(@"server =(localdb)\mssqllocaldb;initial catalog=ETrade;integrated security=true");
        public List<Product> GetAll()
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("Select * from Products", _connection);

            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    StockAmount = Convert.ToInt32(reader["StockAmount"]),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
                };
                products.Add(product);

            }
            reader.Close();
            _connection.Close();
            return products;

        }

        private void ConnectionControl()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        //public DataTable GetAll2()
        //{
        //    SqlConnection connection = new SqlConnection(@"server =(localdb)\mssqllocaldb;initial catalog=ETrade;integrated security=true");
        //    if (connection.State == ConnectionState.Closed)
        //    {
        //        connection.Open();
        //    }
        //    SqlCommand command = new SqlCommand("Select * from Products", connection);

        //    SqlDataReader reader = command.ExecuteReader();
        //    DataTable dataTable = new DataTable();
        //    dataTable.Load(reader);
        //    reader.Close();
        //    connection.Close();
        //    return dataTable;

        //}

        public void Add(Product product)
        {
             ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Insert into Products values(@name,@UnitPrice,@StockAmount)",_connection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@StockAmount", product.StockAmount);
            command.ExecuteNonQuery();

            _connection.Close();

        }

        public void Update(Product product)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Update Products set Name=@name, UnitPrice =@UnitPrice, StockAmount = @StockAmount where Id=@id", _connection);
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@StockAmount", product.StockAmount);
            command.Parameters.AddWithValue("@Id", product.Id);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void Delete(int id)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Delete from Products where Id=@id", _connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
