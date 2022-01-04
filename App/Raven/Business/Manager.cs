using System;
using System.Collections.Generic;

using App.Business.Types;
using App.Models.Base;

namespace App.Business
{
    class Manager
    {
        private List<Transaction> _Transactions;

        public Manager()
        {
            _Transactions = new List<Transaction>();
        }

        #region Balance methods

        /// <summary>
        /// Gets the balance sum of all transactions. This field is read only.
        /// </summary>
        public double Balance
        {
            get
            {
                double res = 0;
                foreach (var transaction in _Transactions)
                    res += transaction._Value;
                return res;
            }
        }

        /// <summary>
        /// Returns total balance of each transaction in a given category, for the last <paramref name="timeSpan"/> days.
        /// </summary>
        /// <param name="category">String representing the transaction category.</param>
        /// <param name="timeSpan">Transactions older than <paramref name="timeSpan"/> days will be ignored.</param>
        /// <returns></returns>
        public double GetBalanceByCategory(string category, int timeSpan)
        {
            double balance = 0;

            foreach (var transaction in _Transactions)
            {
                // category match
                if (transaction.GetType().ToString() == category)

                    // not older than <timeSpan> days
                    if ((DateTime.Now - transaction._Date).Days <= timeSpan)

                        balance += transaction._Value;
            }
            return balance;
        }

        /// <summary>
        /// Gets balance sum of incoming, outgoing or all transactions for a given period, matching(or not) a category.
        /// </summary>
        /// <param name="mode">Choose either incoming, outgoing or all transactions.</param>
        /// <param name="type">Transaction category to match. If left out, all transactions will be matched.</param>
        /// <param name="startDate">Starting date to match. If left out, we will look from the oldest transaction.</param>
        /// <param name="endDate">Ending date to match. If left out, current date will be taken.</param>
        /// <returns>Balance sum of transactions that match the limitations.</returns>
        public double GetBalanceByPeriod(TransactionType mode, Type type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // set defaults
            if (type == null)
                type = typeof(Transaction);

            if (startDate == null)
                startDate = DateTime.MinValue;

            if (endDate == null)
                endDate = DateTime.Now;

            // double result to return.
            double balanceOut = 0;

            // iterate through each transaction and check if it fits.
            foreach(Transaction t in _Transactions)
            {
                if(type == typeof(Transaction) || t.GetType() == type)
                {
                    if (t._Date < endDate && t._Date > startDate)
                    {
                        if(mode == TransactionType.Incoming && t._Value > 0)
                        {
                            balanceOut += t._Value;
                            continue;
                        }
                        if(mode == TransactionType.Outgoing && t._Value < 0)
                        {
                            balanceOut += t._Value;
                            continue;
                        }
                        balanceOut += t._Value;
                    }
                }
            }

            return balanceOut;
        }

        #endregion

        #region Transaction methods

        /// <summary>
        /// Saves a new transaction inside internal list.
        /// </summary>
        /// <param name="transaction">Transaction object to be added.</param>
        public void InsertTransaction(Transaction transaction)
        {
            _Transactions.Add(DBInterface.InsertTransaction(transaction));
        }

        /// <summary>
        /// Gets recent transactions.
        /// </summary>
        /// <param name="numberOfTransactions">Number of transactions to return.</param>
        /// <returns>Returns a List of recent <paramref name="numberOfTransactions"/> transactions. Send a non-positive number to return the whole list.</returns>
        public List<Transaction> GetTransactions(int numberOfTransactions)
        {
            if (numberOfTransactions <= 0)
                return _Transactions;

            return _Transactions.GetRange(_Transactions.Count - numberOfTransactions, numberOfTransactions);
        }

        /// <summary>
        /// Gets list of incoming, outgoing or all transactions for a given period, matching(or not) a category.
        /// </summary>
        /// <param name="mode">Choose either incoming, outgoing or all transactions.</param>
        /// <param name="type">Transaction category to match. If left out, all transactions will be matched.</param>
        /// <param name="startDate">Starting date to match. If left out, we will look from the oldest transaction.</param>
        /// <param name="endDate">Ending date to match. If left out, current date will be taken.</param>
        /// <returns>List of transactions that match the limitations.</returns>
        public List<Transaction> GetTransactionsByPeriod(TransactionType mode, Type type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // set default values
            if (type == null)
                type = typeof(Transaction);

            if (startDate == null)
                startDate = DateTime.MinValue;

            if (endDate == null)
                endDate = DateTime.Now;

            // list to be returned.
            List<Transaction> transactionsOut = new List<Transaction>();

            // iterate through each transaction and check if it fits.
            foreach (Transaction transaction in _Transactions)
            {
                if (type == typeof(Transaction) || transaction.GetType() == type)
                {
                    if (transaction._Date <= endDate && transaction._Date >= startDate)
                    {
                        if (mode == TransactionType.Incoming && transaction._Value > 0)
                        {
                            transactionsOut.Add(transaction);
                            continue;
                        }
                        if (mode == TransactionType.Outgoing && transaction._Value < 0)
                        {
                            transactionsOut.Add(transaction);
                            continue;
                        }
                        transactionsOut.Add(transaction);
                    }
                }
            }

            return transactionsOut;
        }

        #endregion

        #region Load & Save
        public void loadTransactions()
        {
            _Transactions = DBInterface.LoadData();
        }

        public void saveTransactions()
        {
            DBInterface.SaveData(ref _Transactions);
        }
        #endregion

        #region Other
        /// <summary>
        /// Creates a temporary backup and deletes the database.
        /// </summary>
        public void Purge()
        {
            DBInterface.Purge();
        }
        #endregion

    }
}