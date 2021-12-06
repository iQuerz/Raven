using App.Models.Base;
using System;
using System.Collections.Generic;

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

        public double GetBalanceByDate(string mode)//???
        {
            // TODO: Talk about implementation of GetBalanceByDate
            return 0;
        }
        #endregion

        #region Transaction methods
        /// <summary>
        /// Returns a List of recent <paramref name="numberOfTransactions"/> transactions.
        /// </summary>
        /// <param name="numberOfTransactions">Number of transactions to return. Send a non-positive number to return the whole list.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactions(int numberOfTransactions)
        {
            if (numberOfTransactions <= 0)
                return _Transactions;

            return _Transactions.GetRange(_Transactions.Count - numberOfTransactions, numberOfTransactions);
        }

        /// <summary>
        /// Saves a new transaction.
        /// </summary>
        /// <param name="transaction">Transaction object to be added.</param>
        public void InsertTransaction(Transaction transaction)
        {
            //TODO: Add a transaction through DBInterface.
        }
        #endregion

        #region Other
        /// <summary>
        /// Deletes the database.
        /// </summary>
        public void Purge()
        {
            //TODO: Call the purge() method from DBInterface.
        }
        #endregion
    }
}
