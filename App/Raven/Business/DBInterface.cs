using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

using App.Models.Base;
using App.Models.Types;
using App.Models;

namespace App.Business
{
    public static class DBInterface
    {
        private static readonly string connectionString = "Data Source=.\\Data.db;Version=3;";

        /// <summary>
        /// Loads the data from database onto a list.
        /// </summary>
        /// <returns>A List of transactions from the database.</returns>
        public static List<Transaction> LoadData()
        {
            // List that we will be returning
            List<Transaction> outputList = new List<Transaction>();

            // Loading data into the list using a database connection
            #region Database Interaction
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                // Get all the transactions from "Transactions" table into a "transactionData" list.
                var transactionsData = dbConnection.Query<dynamic>("Select * from Transactions;", new DynamicParameters()).AsList<dynamic>();
                foreach (dynamic transaction in transactionsData)
                {
                    // Extract all the columns into variables
                    string date = transaction.Date;
                    string description = transaction.Description;
                    int id = (int)transaction.ID;
                    double value = transaction.Value;
                    string child = transaction.Child;

                    // Create a different transaction based on the value
                    // stored in "child" column and add it to the "outputList"
                    switch (child)
                    {
                        #region IncomeTransaction
                        case "IncomeTransaction":
                            var categoryData = dbConnection.Query<dynamic>($"Select IncomeTransactionType from IncomeTransaction where TransactionID={id};").AsList<dynamic>();
                            IncomeTransaction incomeTransaction = new IncomeTransaction
                            {
                                //_Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _IncomeTransactionType = (IncomeTransactionType)Enum.Parse(typeof(IncomeTransactionType), categoryData[0].IncomeTransactionType)
                            };
                            outputList.Add(incomeTransaction);
                            break;
                        #endregion

                        #region LoanTransaction
                        case "LoanTransaction":
                            categoryData = dbConnection.Query<dynamic>($"Select Resolved,PayBack from LoanTransaction where TransactionID={id};").AsList<dynamic>();
                            LoanTransaction loanTransaction = new LoanTransaction
                            {
                                //_Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _Status = Convert.ToBoolean(categoryData[0].Resolved),
                                _PayBack = categoryData[0].PayBack
                            };
                            outputList.Add(loanTransaction);
                            break;
                        #endregion

                        #region HealthTransaction
                        case "HealthTransaction":
                            categoryData = dbConnection.Query<dynamic>($"Select HealthTransactionType from HealthTransaction where TransactionID={id};").AsList<dynamic>();
                            HealthTransaction healthTransaction = new HealthTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _HealthTransactionType = (HealthTransactionType)Enum.Parse(typeof(IncomeTransactionType), categoryData[0].HealthTransactionType)
                            };
                            outputList.Add(healthTransaction);
                            break;
                        #endregion

                        #region GroceryTransaction
                        case "GroceryTransaction":
                            GroceryTransaction groceryTransaction = new GroceryTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value
                            };
                            outputList.Add(groceryTransaction);
                            break;
                        #endregion

                        #region SavingsTransaction
                        case "SavingsTransaction":
                            SavingsTransaction savingsTransaction = new SavingsTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value
                            };
                            outputList.Add(savingsTransaction);
                            break;
                        #endregion

                        #region EntertainmentTransaction
                        case "EntertainmentTransaction":
                            categoryData = dbConnection.Query<dynamic>($"Select EntertainmentType from EntertainmentTransaction where TransactionID={id};").AsList<dynamic>();
                            EntertainmentTransaction entertainmentTransaction = new EntertainmentTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _EntertainmentType = (EntertainmentType)Enum.Parse(typeof(EntertainmentType), categoryData[0].EntertainmentType)
                            };
                            outputList.Add(entertainmentTransaction);
                            break;
                        #endregion

                        #region DebtTransaction
                        case "DebtTransaction":
                            categoryData = dbConnection.Query<dynamic>($"Select Resolved,PaidBack from DebtTransaction where TransactionID={id};").AsList<dynamic>();
                            DebtTransaction debtTransaction = new DebtTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _Status = Convert.ToBoolean(categoryData[0].Resolved),
                                _PaidBack = categoryData[0].PaidBack,
                            };
                            outputList.Add(debtTransaction);
                            break;
                        #endregion

                        #region Bills
                        case "Bills":
                            categoryData = dbConnection.Query<dynamic>($"Select BillType from Bills where TransactionID={id};").AsList<dynamic>();
                            Bills bill = new Bills
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _BillType = (BillType)Enum.Parse(typeof(BillType), categoryData[0].BillType)
                            };
                            outputList.Add(bill);
                            break;
                        #endregion

                        #region Beauty&Fashion
                        case "BeautyAndFashionTransaction":
                            categoryData = dbConnection.Query<dynamic>($"Select BeautyAndFashionType from BeautyAndFashionTransaction where TransactionID={id};").AsList<dynamic>();
                            BeautyAndFashionTransaction beautyAndFashionTransaction = new BeautyAndFashionTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                                _BeautyAndFashionType =  (BeautyAndFashionType)Enum.Parse(typeof(BeautyAndFashionType), categoryData[0].BeautyAndFashionType)
                            };
                            outputList.Add(beautyAndFashionTransaction);
                            break;
                        #endregion

                        default:
                            // TODO: Create error messages and codes for exceptions.
                            throw new RavenException("Database read failure.");
                    }
                }
            }
            #endregion

            //return the list
            return outputList;
        }

        /// <summary>
        /// Saves differences in the database with the <paramref name="inputList"/>.
        /// </summary>
        /// <param name="inputList">Reference to the list that's to be saved to the database.</param>
        public static void SaveData(ref List<Transaction> inputList)
        {   
            // Filling transactionData list with data from DB 
            var transactionData = LoadData();
            // Checking for differences with DB and syncing data
            #region DB Sync
            int count = inputList.Count;
            for(int i = 0; i < count; i++)
            {
                if (!inputList[i].Equals(transactionData[i]))
                {
                    // Overwriting all DB values with values from inputList in element
                    using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                    {
                        dbConnection.Execute($"Update Transactions" +
                            $" set Value = {inputList[i]._Value}, Date = '{inputList[i]._Date}', Description = '{inputList[i]._Description}'" +
                            $" where ID={inputList[i]._ID};");
                        switch (inputList[i].GetType().Name)
                        {
                            #region IncomeTransaction
                            case "IncomeTransaction":
                                dbConnection.Execute("Update IncomeTransaction" +
                                    $" set IncomeTransactionType = '{inputList[i].GetTransactionType()}'" +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            #region LoanTransaction
                            case "LoanTransaction":
                                dbConnection.Execute("Update LoanTransaction" +
                                    $" set Resolved = {inputList[i].GetTransactionStatus()}, PayBack = {inputList[i].GetTransactionPB()}" +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            #region HealthTransaction
                            case "HealthTransaction":
                                dbConnection.Execute("Update HealthTransaction" +
                                    $" set HealthTransactionType = '{inputList[i].GetTransactionType()}'" +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            #region GroceryTransaction
                            case "GroceryTransaction":
                                break;
                            #endregion

                            #region SavingsTransaction
                            case "SavingsTransaction":
                                break;
                            #endregion

                            #region EntertainmentTransaction
                            case "EntertainmentTransaction":
                                dbConnection.Execute("Update EntertainmentTransaction" +
                                    $" set EntertainmentType = '{inputList[i].GetTransactionType()}' " +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            #region DebtTransaction
                            case "DebtTransaction":
                                dbConnection.Execute("Update DebtTransaction" +
                                    $" set Resolved = {inputList[i].GetTransactionStatus()}, PaidBack = {inputList[i].GetTransactionPB()}" +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            #region Bills
                            case "Bills":
                                dbConnection.Execute("Update Bills" +
                                    $" set BillType = '{inputList[i].GetTransactionType()}'" +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            #region Beauty&Fashion
                            case "BeautyAndFashionTransaction":
                                dbConnection.Execute("Update BeautyAndFashionTransaction" +
                                    $" set BeautyAndFashionType = '{inputList[i].GetTransactionType()}'" +
                                    $" where TransactionID = {inputList[i]._ID};");
                                break;
                            #endregion

                            default:
                                // TODO: Create error messages and codes for exceptions.
                                throw new RavenException("Database sync failure.");
                        }
                    }
                }
            }
            #endregion
        }
    }
}
