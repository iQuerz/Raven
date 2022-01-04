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

        #region Insert, Load & Save

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
                var transactionsData = dbConnection.Query<dynamic>("Select * from Transactions;", new DynamicParameters()).AsList();
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
                            var categoryData = dbConnection.Query<dynamic>($"Select IncomeTransactionType from IncomeTransaction where TransactionID={id};").AsList();
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
                            categoryData = dbConnection.Query<dynamic>($"Select Resolved,PayBack from LoanTransaction where TransactionID={id};").AsList();
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
                            categoryData = dbConnection.Query<dynamic>($"Select HealthTransactionType from HealthTransaction where TransactionID={id};").AsList();
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
                            categoryData = dbConnection.Query<dynamic>($"Select EntertainmentType from EntertainmentTransaction where TransactionID={id};").AsList();
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
                            categoryData = dbConnection.Query<dynamic>($"Select Resolved,PaidBack from DebtTransaction where TransactionID={id};").AsList();
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
                            categoryData = dbConnection.Query<dynamic>($"Select BillType from Bills where TransactionID={id};").AsList();
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
                            categoryData = dbConnection.Query<dynamic>($"Select BeautyAndFashionType from BeautyAndFashionTransaction where TransactionID={id};").AsList();
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

                        #region UncategorizedTransaction
                        case "UncategorizedTransaction":
                            UncategorizedTransaction uncategorizedTransaction = new UncategorizedTransaction
                            {
                                _Date = DateTime.Parse(date),
                                _Description = description,
                                _ID = id,
                                _Value = value,
                            };
                            outputList.Add(uncategorizedTransaction);
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
                        dbConnection.Execute($"UPDATE Transactions" +
                            $" SET Value = {inputList[i]._Value}, Date = '{inputList[i]._Date}', Description = '{inputList[i]._Description}'" +
                            $" WHERE ID={inputList[i]._ID};");

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

                            #region UncategorizedTransaction
                            case "UncategorizedTransaction":
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

        /// <summary>
        /// Inserts a new transaction into the database.
        /// </summary>
        /// <param name="t">Transaction to be inserted.</param>
        /// <returns>Passed transaction, with ID field populated.</returns>
        public static Transaction InsertTransaction(Transaction t)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var ID = Convert.ToInt32(dbConnection.
                    Query<dynamic>("Select seq From sqlite_sequence Where name = 'Transactions'", new DynamicParameters())
                    .AsList()[0].seq) + 1;
                t._ID = ID;

                dbConnection.Execute("Insert into Transactions (Value, Date, Description, Child)" +
                    $"Values ({t._Value}, {t._Date}, {t._Description}, {t.GetType().Name})");

                switch (t.GetType().Name)
                {
                    #region IncomeTransaction
                    case "IncomeTransaction":
                        dbConnection.Execute("Insert Into IncomeTransaction (IncomeTransactionType, TransactionID)" +
                            $" Values ('{t.GetTransactionType()}', {t._ID});");
                        break;
                    #endregion

                    #region LoanTransaction
                    case "LoanTransaction":
                        dbConnection.Execute("Insert Into LoanTransaction (Resolved, PayBack, TransactionID)" +
                            $" Values ({t.GetTransactionStatus()}, {t.GetTransactionPB()}, {t._ID});");
                        break;
                    #endregion

                    #region HealthTransaction
                    case "HealthTransaction":
                        dbConnection.Execute("Insert Into HealthTransaction (HealthTransactionType, TransactionID)" +
                            $" Values ('{t.GetTransactionType()}', {t._ID});");
                        break;
                    #endregion

                    #region GroceryTransaction
                    case "GroceryTransaction":
                        dbConnection.Execute("Insert Into GroceryTransaction (TransactionID)" +
                            $" Values ({t._ID});");
                        break;
                    #endregion

                    #region SavingsTransaction
                    case "SavingsTransaction":
                        dbConnection.Execute("Insert Into SavingsTransaction (TransactionID)" +
                            $" Values ({t._ID});");
                        break;
                    #endregion

                    #region EntertainmentTransaction
                    case "EntertainmentTransaction":
                        dbConnection.Execute("Insert Into EntertainmentTransaction (EntertainmentType, TransactionID)" +
                            $" Values ('{t.GetTransactionType()}', {t._ID});");
                        break;
                    #endregion

                    #region DebtTransaction
                    case "DebtTransaction":
                        dbConnection.Execute("Insert Into DebtTransaction (Resolved, PaidBack, TransactionID)" +
                            $" Values ({t.GetTransactionStatus()}, {t.GetTransactionPB()}, {t._ID});");
                        break;
                    #endregion

                    #region Bills
                    case "Bills":
                        dbConnection.Execute("Insert Into Bills (BillType, TransactionID)" +
                            $" Values ('{t.GetTransactionType()}', {t._ID});");
                        break;
                    #endregion

                    #region Beauty&Fashion
                    case "BeautyAndFashionTransaction":
                        dbConnection.Execute("Insert Into BeautyAndFashionTransaction (BeautyAndFashionType, TransactionID)" +
                            $" Values ('{t.GetTransactionType()}', {t._ID});");
                        break;
                    #endregion

                    #region UncategorizedTransaction
                    case "UncategorizedTransaction":
                        break;
                    #endregion

                    default:
                        // TODO: Create error messages and codes for exceptions.
                        throw new RavenException("Transaction insertion failed.");
                }
            }

            return t;
        }

        #endregion

        #region AppSettings
        // TODO: test if all the AppSettings methods are working as intended.

        #region FirstBoot
        public static bool GetFirstBoot()
        {
            using(IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select FirstBoot from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .FirstBoot;

                // SQLite doesn't support booleans. We use integer 0's and 1's.
                return Convert.ToBoolean(Convert.ToInt16(settingValue));
            }
        }
        public static void SetFirstBoot(bool newValue)
        {
            string updatedSetting;
            updatedSetting = newValue ? "1" : "0";

            using(IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set FirstBoot = {updatedSetting};");
            }
        }
        #endregion

        #region DefaultCurrency
        public static string GetDefaultCurrency()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select DefaultCurrency from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .DefaultCurrency;

                return settingValue;
            }
        }
        public static void SetDefaultCurrency(string newValue)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set DefaultCurrency = '{newValue}';");
            }
        }
        #endregion

        #region DefaultLanguage
        public static string GetDefaultLanguage()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select DefaultLanguage from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .DefaultLanguage;

                return settingValue;
            }
        }
        public static void SetDefaultLanguage(string newValue)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set DefaultLanguage = '{newValue}';");
            }
        }
        #endregion

        #region AutoSavePeriod
        public static int GetAutoSavePeriod()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select AutoSavePeriod from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .AutoSavePeriod;

                return Convert.ToInt32(settingValue);
            }
        }
        public static void SetAutoSavePeriod(int newValue)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set AutoSavePeriod = {newValue};");
            }
        }
        #endregion

        #region DarkMode
        public static bool GetDarkMode()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select DarkMode from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .DarkMode;

                // SQLite doesn't support booleans. We use integer 0's and 1's.
                return Convert.ToBoolean(Convert.ToInt16(settingValue));
            }
        }
        public static void SetDarkMode(bool newValue)
        {
            string updatedSetting;
            updatedSetting = newValue ? "1" : "0";

            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set DarkMode = {updatedSetting};");
            }
        }
        #endregion

        #region FontSize
        public static double GetFontSize()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select FontSize from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .FontSize;

                return Convert.ToInt32(settingValue);
            }
        }
        public static void SetFontSize(double newValue)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set FontSize = {newValue};");
            }
        }
        #endregion

        #region DateFormat
        public static string GetDateFormat()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select DateFormat from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .DateFormat;

                return settingValue;
            }
        }
        public static void SetDateFormat(string newValue)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set DateFormat = '{newValue}';");
            }
        }
        #endregion

        #endregion

        #region Other

        public static void Purge()
        {
            // TODO: Temporary backup + Deletion of all data.
        }

        /// <summary>
        /// DANGEROUS. Use at your own risk and test your query before executing it programmatically.
        /// </summary>
        /// <param name="query">Query string to be executed</param>
        public static List<dynamic> ExecuteCustomQuery(string query)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                return dbConnection
                    .Query<dynamic>(query, new DynamicParameters())
                    .AsList();
            }
        }

        #endregion

    }
}