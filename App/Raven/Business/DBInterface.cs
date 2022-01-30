using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

using App.Models.Base;
using App.Models.Types;
using App.Models;

namespace App.Business
{
    public interface DBInterface
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
        // TODO: add default values if there's no data inside the table.

        #region Username
        public static string GetUsername()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                var settingValue = dbConnection
                    .Query<dynamic>("Select Username from AppSettings;", new DynamicParameters())
                    .AsList()[0]
                    .Username;

                return settingValue;
            }
        }
        public static void SetUsername(string newValue)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Execute($"Update AppSettings Set Username = '{newValue}';");
            }
        }
        #endregion

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
                    .AsList();

                if (settingValue.Count == 0)
                {
                    AppSettings.AutoSavePeriod = 15;
                    return 15;
                }

                return Convert.ToInt32(settingValue[0].AutoSavePeriod);
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

        #region Import & Export

        /// <summary>
        /// Exports(Backups) the whole Database to a single file.
        /// </summary>
        /// <param name="filepath">Absolute path to the JSON file to which export will be saved.</param>
        public static async Task ExportDB(string filepath)
        {
            // start of a file
            string saveFile = "{\n  \"Tables\": [";
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                #region Create json strings from table contents

                #region Transactions
                var tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from Transactions;", new DynamicParameters());
                string transactions = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from IncomeTransaction;", new DynamicParameters());
                string incomeTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from LoanTransaction;", new DynamicParameters());
                string loanTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from DebtTransaction;", new DynamicParameters());
                string debtTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from HealthTransaction;", new DynamicParameters());
                string healthTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from GroceryTransaction;", new DynamicParameters());
                string groceryTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from EntertainmentTransaction;", new DynamicParameters());
                string entertainmentTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from Bills;", new DynamicParameters());
                string bills = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from BeautyAndFashionTransaction;", new DynamicParameters());
                string bnfTransaction = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);
                #endregion

                #region Settings
                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from AppSettings;", new DynamicParameters());
                string appSettings = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);

                tempTableContent = await dbConnection.QueryAsync<dynamic>("Select * from sqlite_sequence;", new DynamicParameters());
                string sequence = JsonConvert.SerializeObject(tempTableContent, Formatting.Indented);
                #endregion

                #endregion

                #region Making the JSON

                #region Transactions
                saveFile += "\n    {\n      \"Table\":\"Transactions\",\n      \"Content\":\n";
                saveFile += indent(transactions, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"IncomeTransaction\",\n      \"Content\":\n";
                saveFile += indent(incomeTransaction, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"LoanTransaction\",\n      \"Content\":\n";
                saveFile += indent(loanTransaction, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"DebtTransaction\",\n      \"Content\":\n";
                saveFile += indent(debtTransaction, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"HealthTransaction\",\n      \"Content\":\n";
                saveFile += indent(healthTransaction, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"GroceryTransaction\",\n      \"Content\":\n";
                saveFile += indent(groceryTransaction, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"Bills\",\n      \"Content\":\n";
                saveFile += indent(bills, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"BeautyAndFashionTransaction\",\n      \"Content\":\n";
                saveFile += indent(bnfTransaction, 6);
                saveFile += "    },";
                #endregion

                #region Settings
                saveFile += "\n    {\n      \"Table\":\"AppSettings\",\n      \"Content\":\n";
                saveFile += indent(appSettings, 6);
                saveFile += "    },";

                saveFile += "\n    {\n      \"Table\":\"sqlite_sequence\",\n      \"Content\":\n";
                saveFile += indent(sequence, 6);
                saveFile += "    }"; //The end of the array. No need for ','.
                #endregion

                saveFile += "\n  ]\n}";

                #endregion
            }
            await File.WriteAllTextAsync(filepath, saveFile);
        }


        /// <summary>
        /// Imports(Restores) the whole Database from a single file.
        /// </summary>
        /// <param name="filepath">Absolute path to the JSON file which holds the Database info.</param>
        /// <returns></returns>
        public static async Task ImportDB(string filepath)
        {
            // deserialize the file.
            string file = await File.ReadAllTextAsync(filepath);
            dynamic data = JsonConvert.DeserializeObject(file);

            // go through each table
            foreach(dynamic table in data.Tables)
            {
                // add rows to db according to tablename
                string tableName = table.Table;
                switch (tableName)
                {
                    #region Transactions
                    case "Transactions":
                        foreach(dynamic row in table.Content)
                        {
                            int ID = Convert.ToInt32(row.ID);
                            double Value = Convert.ToDouble(row.Value);
                            string Date = row.Date;
                            string Description = row.Description;
                            string Child = row.Child;

                            using(IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (ID,Value,Date,Description,Child) " +
                                    $"Values ({ID},{Value},'{Date}','{Description}','{Child}');");
                            }
                        }
                        break;
                    #endregion

                    #region Income
                    case "IncomeTransaction":
                        foreach(dynamic row in table.Content)
                        {
                            string IncomeTransactionType = row.IncomeTransactionType;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (IncomeTransactionType, TransactionID) " +
                                    $"Values ('{IncomeTransactionType}',{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region BeautyAndFashion
                    case "BeautyAndFashionTransaction":
                        foreach(dynamic row in table.Content)
                        {
                            string BeautyAndFashionType = row.BeautyAndFashionType;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (BeautyAndFashionType, TransactionID) " +
                                    $"Values ('{BeautyAndFashionType}',{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region Bills
                    case "Bills":
                        foreach(dynamic row in table.Content)
                        {
                            string BillType = row.BillType;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (BillType, TransactionID) " +
                                    $"Values ('{BillType}',{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region Debt
                    case "DebtTransaction":
                        foreach(dynamic row in table.Content)
                        {
                            string Resolved = row.Resolved;
                            string PaidBack = row.PaidBack;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (Resolved, PaidBack, TransactionID) " +
                                    $"Values ({Resolved},{PaidBack},{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region "Loan"
                    case "LoanTransaction":
                        foreach (dynamic row in table.Content)
                        {
                            string Resolved = row.Resolved;
                            string PayBack = row.PayBack;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (Resolved, PayBack, TransactionID) " +
                                    $"Values ({Resolved},{PayBack},{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region Entertainment
                    case "EntertainmentTransaction":
                        foreach(dynamic row in table.Content)
                        {
                            string EntertainmentType = row.EntertainmentType;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (EntertainmentType, TransactionID) " +
                                    $"Values ('{EntertainmentType}',{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region Health
                    case "HealthTransaction":
                        foreach(dynamic row in table.Content)
                        {
                            string HealthTransactionType = row.HealthTransactionType;
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (HealthTransactionType, TransactionID) " +
                                    $"Values ('{HealthTransactionType}',{TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region Grocery
                    case "GroceryTransaction":
                        foreach(dynamic row in table.Content)
                        {
                            string TransactionID = row.TransactionID;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (TransactionID) " +
                                    $"Values ({TransactionID});");
                            }
                        }
                        break;
                    #endregion

                    #region Settings
                    //case "AppSettings":
                    //    foreach(dynamic row in table.Content)
                    //    {
                    //        string Username = row.Username;
                    //        string FirstBoot = row.FirstBoot;
                    //        string DefaultCurrency = row.DefaultCurrency;
                    //        string DefaultLanguage = row.DefaultLanguage;
                    //        string AutoSavePeriod = row.AutoSavePeriod;
                    //        string DarkMode = row.DarkMode;
                    //        string FontSize = row.FontSize;
                    //        string DateFormat = row.DateFormat;

                    //        using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                    //        {
                    //            dbConnection.Query($"Insert into {tableName} (Username, FirstBoot, DefaultCurrency, DefaultLanguage, AutoSavePeriod," +
                    //                $"DarkMode, FontSize, DateFormat) " +
                    //                $"Values ('{Username}',{FirstBoot},'{DefaultCurrency}','{DefaultLanguage}',{AutoSavePeriod}," +
                    //                $"{DarkMode},{FontSize},'{DateFormat}');");
                    //        }
                    //    }
                    //    break;
                    #endregion

                    #region sequence
                    case "sqlite_sequence":
                        foreach(dynamic row in table.Content)
                        {
                            string name = row.name;
                            string seq = row.seq;

                            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
                            {
                                dbConnection.Query($"Insert into {tableName} (name, seq) " +
                                    $"Values ('{name}',{seq});");
                            }
                        }
                        break;
                    #endregion

                    default:
                        throw new RavenException("Error while reading the file.");
                }
            }
        }


        /// <summary>
        /// Add a <paramref name="indentNum"/> number of spaces in front of each line in a string
        /// </summary>
        /// <param name="s">Input string</param>
        /// <param name="indentNum">Number of spaces to add</param>
        /// <returns>A new string with indented lines.</returns>
        private static string indent(string s, int indentNum)
        {
            string result = "";
            using (var reader = new StringReader(s))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    for(int i = 0; i < indentNum; i++)
                        result += " ";
                    result += line;
                    result += "\n";
                }
            }
            return result;
        }

        #endregion

        #region Other

        /// <summary>
        /// Delete Every single entry from the Database.
        /// </summary>
        public static async Task Purge()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                await dbConnection.QueryAsync("Delete from Transactions");
                await dbConnection.QueryAsync("Delete from IncomeTransaction");
                await dbConnection.QueryAsync("Delete from LoanTransaction");
                await dbConnection.QueryAsync("Delete from DebtTransaction");
                await dbConnection.QueryAsync("Delete from HealthTransaction");
                await dbConnection.QueryAsync("Delete from EntertainmentTransaction");
                await dbConnection.QueryAsync("Delete from GroceryTransaction");
                await dbConnection.QueryAsync("Delete from BeautyAndFashionTransaction");
                await dbConnection.QueryAsync("Delete from Bills");

                await dbConnection.QueryAsync("Delete from sqlite_sequence");
                //await dbConnection.QueryAsync("Delete from AppSettings");
            }
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