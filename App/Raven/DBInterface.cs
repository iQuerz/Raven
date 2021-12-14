using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.SQLite;
using App.Models.Base;
using System.Data;
using App.Models;
using App.Business;

namespace App
{
    public static class DBInterface
    {
        public static List<Transaction> LoadData()
        {
            // Lista koja prosledjuje vrednosti iz Data.db 
            List<Transaction> list = new List<Transaction>();

            // Loading data into list
            using (IDbConnection dbConnection = new SQLiteConnection("Data Source=.\\Data.db;Version=3;"))
            {
                var output = dbConnection.Query<dynamic>("Select * from Transactions;", new DynamicParameters()).AsList<dynamic>();
                foreach (dynamic element in output)
                {
                    string date = element.Date;
                    string description = element.Description;
                    int id = (int)element.ID;
                    double value = element.Value;
                    string child = element.Child;

                    switch(child)
                    {
                        case "IncomeTransaction":
                            var type = dbConnection.Query<dynamic>($"Select IncomeTransactionType from IncomeTransaction where TransactionID={id};").AsList<dynamic>();
                            IncomeTransaction incomeTransaction = new IncomeTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _incomeTransactionType = type[0].IncomeTransactionType,
                            };
                            list.Add(incomeTransaction);
                            break;
                        case "LoanTransaction":
                            type = dbConnection.Query<dynamic>($"Select Resolved,PayBack from LoanTransaction where TransactionID={id};").AsList<dynamic>();
                            LoanTransaction loanTransaction = new LoanTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _Status = type[0].Resolved,
                                _PayBack = type[0].PayBack,
                            };
                            list.Add(loanTransaction);
                            break;
                        case "HealthTransaction":
                            type = dbConnection.Query<dynamic>($"Select HealthTransactionType from HealthTransaction where TransactionID={id};").AsList<dynamic>();
                            HealthTransaction healthTransaction = new HealthTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _HealthTransactionType = type[0].HealthTransactionType
                            };
                            list.Add(healthTransaction);
                            break;
                        case "GroceryTransaction":
                            GroceryTransaction groceryTransaction = new GroceryTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                            };
                            list.Add(groceryTransaction);
                            break;
                        case "SavingsTransaction":
                            SavingsTransaction savingsTransaction = new SavingsTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                            };
                            list.Add(savingsTransaction);
                            break;
                        case "EntertainmentTransaction":
                            type = dbConnection.Query<dynamic>($"Select EntertainmentType from EntertainmentTransaction where TransactionID={id};").AsList<dynamic>();
                            EntertainmentTransaction entertainmentTransaction = new EntertainmentTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _EntertainmentType = type[0].EntertainmentType,
                            };
                            list.Add(entertainmentTransaction);
                            break;
                        case "DebtTransaction":
                            type = dbConnection.Query<dynamic>($"Select Resolved,PaidBack from DebtTransaction where TransactionID={id};").AsList<dynamic>();
                            DebtTransaction debtTransaction = new DebtTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _Status = type[0].Resolved,
                                _PaidBack = type[0].PaidBack,
                            };
                            list.Add(debtTransaction);
                            break;
                        case "Bills":
                            type = dbConnection.Query<dynamic>($"Select BillType from Bills where TransactionID={id};").AsList<dynamic>();
                            Bills bill = new Bills
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _BillType = type[0].BillType,
                            };
                            list.Add(bill);
                            break;
                        case "BeautyAndFashionTransaction":
                            type = dbConnection.Query<dynamic>($"Select BeautyAndFashionType from BeautyAndFashionTransaction where TransactionID={id};").AsList<dynamic>();
                            BeautyAndFashionTransaction beautyAndFashionTransaction = new BeautyAndFashionTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _BeautyAndFashionType = type[0].BeautyAndFashionType,
                            };
                            list.Add(beautyAndFashionTransaction);
                            break;

                    

                        default:
                            // TODO: treba se dodati error poruka c:(ili code)
                            throw new RavenException("El si misleo il si zeleo");
                    }
                }
               
            }

            return list;
        }
        public static void SaveData()
        {
        
        
        }
    }
}
